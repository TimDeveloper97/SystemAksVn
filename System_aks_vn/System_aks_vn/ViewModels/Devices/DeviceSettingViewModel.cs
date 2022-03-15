using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System_aks_vn.Controls;
using System_aks_vn.Domain;
using System_aks_vn.Models.Response;
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
        private DeviceSetting _setting;

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

            ExecuteLoadDeviceSetting();
        });


        public ICommand SmsConfigCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync($"{nameof(DeviceSettingSmsPage)}" +
               $"?{nameof(DeviceSettingSmsViewModel.ParameterDeviceId)}={ParameterDeviceId}" + 
               $"&{nameof(DeviceSettingSmsViewModel.ParameterSms)}={_setting?.Smss}");
        });

        public ICommand CallConfigCommand => new Command(async() =>
        {
            await Shell.Current.GoToAsync($"{nameof(DeviceSettingCallPage)}" +
               $"?{nameof(DeviceSettingCallViewModel.ParameterDeviceId)}={ParameterDeviceId}" +
               $"&{nameof(DeviceSettingCallViewModel.ParameterCall)}={_setting?.Calls}");
        });

        public ICommand ScheduleConfigCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync($"{nameof(DeviceSettingSchedulePage)}" +
               $"?{nameof(DeviceSettingScheduleViewModel.ParameterDeviceId)}={ParameterDeviceId}" +
               $"&{nameof(DeviceSettingScheduleViewModel.ParameterPlan)}={_setting?.PLan}");
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

        void ExecuteLoadDeviceSetting()
        {
            IsBusy = true;

            try
            {
                Mqtt.ClearEvent();
                Mqtt.Publish(Topic, Api.DeviceList, new { token = Token });

                Mqtt.MessageReceived += async (s, e) =>
                {
                    var res = (s as Mqtt).Response;
                    if (res.Code == 100)
                        await TimeoutSession(res.Message);

                    if (res.Value == null) return;

                    var ldevice = JsonConvert.DeserializeObject<List<DeviceModel>>(res.Value.ToString());
                    var device = ldevice.Find(x => x.Id == ParameterDeviceId);

                    _setting = device.Setting;
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
        #endregion
    }
}
