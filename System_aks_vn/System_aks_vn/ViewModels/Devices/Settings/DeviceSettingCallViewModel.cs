using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System_aks_vn.Controls;
using System_aks_vn.Domain;
using System_aks_vn.Models.View;
using Xamarin.Forms;

namespace System_aks_vn.ViewModels.Devices.Settings
{
    public class DeviceSettingCallViewModel: BaseViewModel
    {
        #region Property
        private string parameterDeviceId;
        private DeviceSettingNumberModel call;

        public string ParameterDeviceId
        {
            get => parameterDeviceId;
            set
            {
                parameterDeviceId = Uri.UnescapeDataString(value ?? string.Empty);
                SetProperty(ref parameterDeviceId, value);
            }
        }
        public DeviceSettingNumberModel Call { get => call; set => SetProperty(ref call, value); }
        #endregion

        #region Command 
        public ICommand PageAppearingCommand => new Command(async () =>
        {
            Init();
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
        #endregion

        public DeviceSettingCallViewModel()
        {
        }

        #region Method
        void Init()
        {
            DependencyService.Get<IStatusBar>().SetColoredStatusBar("#007bff");
            Call = new DeviceSettingNumberModel();
            IsBusy = true;
        }
        #endregion
    }
}
