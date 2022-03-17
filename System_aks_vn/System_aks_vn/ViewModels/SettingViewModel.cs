using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System_aks_vn.Controls;
using System_aks_vn.Domain;
using System_aks_vn.Models.View;
using System_aks_vn.Views;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace System_aks_vn.ViewModels
{
    public class SettingViewModel: BaseViewModel
    {
        #region Property
        private string oldPassword, newPassword, confirmPassword;
        private string errorOldPassword, errorNewPassword, errorConfirmPassword;

        public string OldPassword { get => oldPassword; set => SetProperty(ref oldPassword, value); }
        public string NewPassword { get => newPassword; set => SetProperty(ref newPassword, value); }
        public string ConfirmPassword { get => confirmPassword; set => SetProperty(ref confirmPassword, value); }
        public string ErrorOldPassword { get => errorOldPassword; set => SetProperty(ref errorOldPassword, value); }
        public string ErrorNewPassword { get => errorNewPassword; set => SetProperty(ref errorNewPassword, value); }
        public string ErrorConfirmPassword { get => errorConfirmPassword; set => SetProperty(ref errorConfirmPassword, value); }

        #endregion

        #region Command  
        public ICommand ChangePasswordCommand => new Command(async () =>
        {
            if (CheckChangePassword())
            {
                Reset();
                await ExecuteLoadChangePasswordCommand();
            }
        });

        public ICommand LogoutCommand => new Command(async () =>
        {
            Mqtt.Disconnect();
            Topic = null;
            Token = null;

            await Shell.Current.GoToAsync("//LoginPage");
        });

        public ICommand PageAppearingCommand => new Command(() =>
        {
            Init();
        });
        #endregion

        public SettingViewModel()
        {
        }

        #region Method
        void Init()
        {
            Title = "Settings";
            DependencyService.Get<IStatusBar>().SetColoredStatusBar("#007bff");
            Reset();
        }

        void Reset()
        {
            ErrorOldPassword = null;
            ErrorConfirmPassword = null;
            ErrorNewPassword = null;
        }

        bool CheckChangePassword()
        {
            if (string.IsNullOrEmpty(OldPassword))
            {
                ErrorOldPassword = "Old password is required.";
                return false;
            }

            if (string.IsNullOrEmpty(NewPassword))
            {
                ErrorNewPassword = "New password is required.";
                return false;
            }

            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                ErrorConfirmPassword = "Confirm password is required.";
                return false;
            }

            return true;
        }

        async Task ExecuteLoadChangePasswordCommand()
        {
            IsBusy = true;

            try
            {
                Mqtt.ClearEvent();
                Mqtt.Publish(Topic, new AccountContext
                {
                    Token = Token,
                    OldPass = OldPassword,
                    NewPass = NewPassword,
                    Confirm = ConfirmPassword,
                });

                Mqtt.MessageReceived += async (s, e) =>
                {
                    var res = (s as Mqtt).Response;
                    if (res.Code == 100)
                        await TimeoutSession(res.Message);

                    if(res.Code != 0)
                    {
                        if(res.Code == -2)
                            ErrorConfirmPassword = res.Message;
                        else
                            ErrorOldPassword = res.Message;

                        await MaterialDialog.Instance.SnackbarAsync(message: res.Message,
                              msDuration: MaterialSnackbar.DurationLong);
                    }    
                    else
                        await MaterialDialog.Instance.SnackbarAsync(message: "Success",
                              msDuration: MaterialSnackbar.DurationLong);
                };
                
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion
    }
}
