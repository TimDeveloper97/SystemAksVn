using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System_aks_vn.Controls;
using System_aks_vn.Domain;
using System_aks_vn.Models.Response;
using System_aks_vn.Services.Temp;
using System_aks_vn.ViewModels.Version;
using System_aks_vn.Views;
using System_aks_vn.Views.Version;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using XF.Material.Forms.UI.Dialogs.Configurations;

namespace System_aks_vn.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        #region Property
        private ObservableCollection<DeviceModel> devices;
        private double widthCard;

        public ObservableCollection<DeviceModel> Devices { get => devices; set => SetProperty(ref devices, value); }
        public double WidthCard { get => widthCard; set => SetProperty(ref widthCard, value); }

        #endregion

        #region Command 
        public ICommand PageAppearingCommand => new Command(() =>
        {
            Init();
        });
        public ICommand DetailDeviceCommand => new Command<DeviceModel>(async (device) =>
        {
            if (device.Version == "V30")
            {
                await Shell.Current.GoToAsync($"{nameof(DeviceV30Page)}" +
                $"?{nameof(DeviceV30ViewModel.ParameterDeviceId)}={device.Id}");
            }
        });
        public ICommand MenuCommand => new Command(async () =>
        {
            var configuration = new MaterialSimpleDialogConfiguration
            {
                TextColor = GetColorTextMenu(),
            };

            var actions = new string[]
            {
                Resources.Languages.LanguageResource.settingTitle,
                Resources.Languages.LanguageResource.settingLogout 
            };
            var result = await MaterialDialog.Instance.SelectActionAsync(
                title: Resources.Languages.LanguageResource.homeMenu, actions: actions, configuration);

            if (result == 0)
                await ExecuteLoadChangePassword();
            else if (result == 1)
                await ExecuteLoadLogout();
        });

        public ICommand LoadDeviceCommand { get; set; }
        #endregion

        public HomeViewModel()
        {
            LoadDeviceCommand = new Command(() => ExecuteLoadDeviceCommand());
        }

        #region Method
        void Init()
        {
            Title = "Home";
            Devices = new ObservableCollection<DeviceModel>();
            DependencyService.Get<IStatusBar>().SetColoredStatusBar("#007bff");
            WidthCard = App.ScreenWidth * 0.8;
            IsBusy = true;
        }

        void ExecuteLoadDeviceCommand()
        {
            IsBusy = true;

            try
            {
                Devices?.Clear();
                Mqtt.ClearEvent();
                Mqtt.Publish(Topic, Api.DeviceList, new { token = Token });

                Mqtt.MessageReceived += async (s, e) =>
                {
                    try
                    {
                        var res = (s as Mqtt).Response;
                        if (res.Code == 100)
                            await TimeoutSession(res.Message);
                        if (res.Value == null)
                        {
                            await MaterialDialog.Instance.SnackbarAsync(message: "Notthing response",
                                  msDuration: MaterialSnackbar.DurationLong);
                            return;
                        }

                        var ldevice = JsonConvert.DeserializeObject<List<DeviceModel>>(res.Value.ToString());

                        await Device.InvokeOnMainThreadAsync(() =>
                        {
                            foreach (var device in ldevice)
                            {
                                Devices.Add(device);
                            }
                        });
                    }
                    catch (Exception) { }
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }

        }

        async Task ExecuteLoadChangePassword()
        {
            await Shell.Current.GoToAsync($"{nameof(SettingPage)}");
        }

        async Task ExecuteLoadLogout()
        {
            Mqtt.Disconnect();
            Topic = null;
            Token = null;

            await Shell.Current.GoToAsync("//LoginPage");
        }
        Color GetColorTextMenu()
        {
            OSAppTheme currentTheme = Application.Current.RequestedTheme;
            return currentTheme == OSAppTheme.Dark ? Color.FromHex("#d9d9d9") : Color.FromHex("#666666");
        }
        #endregion
    }
}
