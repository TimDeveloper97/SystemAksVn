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
    public class DeviceSettingSmsViewModel : BaseViewModel
    {
        #region Property
        private string parameterDeviceId;
        private DeviceSettingNumberModel sms;

        public string ParameterDeviceId
        {
            get => parameterDeviceId;
            set
            {
                parameterDeviceId = Uri.UnescapeDataString(value ?? string.Empty);
                SetProperty(ref parameterDeviceId, value);
            }
        }
        public DeviceSettingNumberModel Sms { get => sms; set => SetProperty(ref sms, value); }

        #endregion

        #region Command 
        public ICommand PageAppearingCommand => new Command(async () =>
        {
            Init();
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

        #endregion

        public DeviceSettingSmsViewModel()
        {
        }

        #region Method
        void Init()
        {
            DependencyService.Get<IStatusBar>().SetColoredStatusBar("#007bff");
            Sms = new DeviceSettingNumberModel();
            IsBusy = true;
        }
        #endregion
    }
}
