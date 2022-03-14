using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System_aks_vn.Controls;
using System_aks_vn.Domain;
using System_aks_vn.Models.View;
using System_aks_vn.ViewModels.Devices.Settings;
using System_aks_vn.Views.Devices.Settings;
using Xamarin.Forms;

namespace System_aks_vn.ViewModels.Devices
{
    [QueryProperty(nameof(ParameterDeviceId), nameof(ParameterDeviceId))]
    public class DeviceSettingViewModel : BaseViewModel
    {
        #region Property
        private string parameterDeviceId;

        public string ParameterDeviceId
        {
            get => parameterDeviceId;
            set
            {
                parameterDeviceId = Uri.UnescapeDataString(value ?? string.Empty);
                Title = $"{parameterDeviceId}";
                SetProperty(ref parameterDeviceId, value);
            }
        }
        #endregion

        #region Command 
        public ICommand PageAppearingCommand => new Command(() =>
        {
            Init();
        });

        public ICommand SmsConfigCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync($"{nameof(DeviceSettingSmsPage)}" +
               $"?{nameof(DeviceSettingSmsViewModel.ParameterDeviceId)}={ParameterDeviceId}");
        });

        public ICommand CallConfigCommand => new Command(async() =>
        {
            await Shell.Current.GoToAsync($"{nameof(DeviceSettingCallPage)}" +
               $"?{nameof(DeviceSettingCallViewModel.ParameterDeviceId)}={ParameterDeviceId}");
        });

        public ICommand ScheduleConfigCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync($"{nameof(DeviceSettingSchedulePage)}" +
               $"?{nameof(DeviceSettingScheduleViewModel.ParameterDeviceId)}={ParameterDeviceId}");
        });
        #endregion

        public DeviceSettingViewModel()
        {
        }

        #region Method
        void Init()
        {
            DependencyService.Get<IStatusBar>().SetColoredStatusBar("#007bff");
            IsBusy = true;
        }
        #endregion
    }
}
