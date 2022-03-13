using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System_aks_vn.Controls;
using System_aks_vn.Domain;
using System_aks_vn.Models.View;
using Xamarin.Forms;

namespace System_aks_vn.ViewModels.Devices
{
    [QueryProperty(nameof(ParameterDeviceId), nameof(ParameterDeviceId))]
    public class DeviceSettingViewModel : BaseViewModel
    {
        #region Property
        private string parameterDeviceId;
        private DeviceSettingNumberModel sms, call;

        public string ParameterDeviceId
        {
            get => parameterDeviceId;
            set
            {
                parameterDeviceId = Uri.UnescapeDataString(value ?? string.Empty);
                Title = $"Setting [{parameterDeviceId}]";
                SetProperty(ref parameterDeviceId, value);
            }
        }
        public DeviceSettingNumberModel Sms { get => sms; set => SetProperty(ref sms, value); }
        public DeviceSettingNumberModel Call { get => call; set => SetProperty(ref call, value); }
        #endregion

        #region Command 
        public ICommand PageAppearingCommand => new Command(async () =>
        {
            Init();

            await ExecuteLoadDeviceSettingCommand();
        });

        public ICommand SubmitSmsCommand => new Command(() =>
        {
            Mqtt.ClearEvent();
            Mqtt.Publish(Topic, new DeviceContext
            { 
                Args = new List<string>(5) { Sms.Number1, Sms.Number2, Sms.Number3, Sms.Number4, Sms.Number5 },
                DeviceId = ParameterDeviceId,
                Token = Token,
                Func = "SMS",
                Url = Api.SettingSms
            });
        });

        public ICommand SubmitCallCommand => new Command(() =>
        {
            Mqtt.ClearEvent();
            Mqtt.Publish(Topic, new DeviceContext
            {
                Args = new List<string>(5) { Call.Number1, Call.Number2, Call.Number3, Call.Number4, Call.Number5 },
                DeviceId = ParameterDeviceId,
                Token = Token,
                Func = "CALL",
                Url = Api.SettingCall
            });
        });

        public ICommand SubmitScheduleCommand => new Command(() =>
        {
            Mqtt.ClearEvent();
            Mqtt.Publish(Topic, new DeviceContext
            {
                
            });
        });
        #endregion

        public DeviceSettingViewModel()
        {
        }

        #region Method
        void Init()
        {
            DependencyService.Get<IStatusBar>().SetColoredStatusBar("#007bff");
            Sms = new DeviceSettingNumberModel();
            Call = new DeviceSettingNumberModel();
            IsBusy = true;
        }

        async Task ExecuteLoadDeviceSettingCommand()
        {
            IsBusy = true;

            try
            {
                
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
