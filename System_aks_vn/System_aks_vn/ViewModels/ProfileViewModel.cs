using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System_aks_vn.Controls;
using System_aks_vn.Domain;
using Xamarin.Forms;

namespace System_aks_vn.ViewModels
{
    public class ProfileViewModel: BaseViewModel
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

        public ICommand PreviousCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync("..");
        });

        public ICommand PageAppearingCommand => new Command(() =>
        {
            Init();
        });
        #endregion

        public ProfileViewModel()
        {
        }

        #region Method
        void Init()
        {
            Title = "Profile";
            DependencyService.Get<IStatusBar>().SetWhiteStatusBar();
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
            var loadingDialog = await XF.Material.Forms.UI.Dialogs.MaterialDialog.Instance
                    .LoadingDialogAsync(message: $"Waiting to change password");
            IsBusy = true;

            try
            {
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                await loadingDialog.DismissAsync();
                IsBusy = false;
            }
        }
        #endregion
    }
}
