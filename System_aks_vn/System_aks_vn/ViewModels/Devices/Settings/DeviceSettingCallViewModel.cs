using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using System_aks_vn.Controls;
using System_aks_vn.Domain;
using System_aks_vn.Models.View;
using Xamarin.Forms;

namespace System_aks_vn.ViewModels.Devices.Settings
{
    [QueryProperty(nameof(ParameterDeviceId), nameof(ParameterDeviceId))]
    [QueryProperty(nameof(ParameterCall), nameof(ParameterCall))]
    public class DeviceSettingCallViewModel : BaseViewModel
    {
        #region Property
        private string parameterDeviceId, parameterCall;
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
        public string ParameterCall
        {
            get => parameterCall; set
            {
                parameterCall = Uri.UnescapeDataString(value ?? string.Empty);
                SetProperty(ref parameterCall, value);
            }
        }

        public DeviceSettingNumberModel Call { get => call; set => SetProperty(ref call, value); }
        #endregion

        #region Command 
        public ICommand PageAppearingCommand => new Command(() =>
        {
            Init();
            GetData();
        });
        public ICommand SubmitCallCommand => new Command(() =>
        {
            IsBusy = true;

            try
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
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
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
        void GetData()
        {
            if (parameterCall != string.Empty)
            {
                var lcall = JsonConvert.DeserializeObject<List<string>>(parameterCall);
                if (lcall == null && lcall.Count == 0) return;

                Call.Number1 = lcall[0];
                Call.Number2 = lcall[1];
                Call.Number3 = lcall[2];
                Call.Number4 = lcall[3];
                Call.Number5 = lcall[4];
            }
        }
        #endregion
    }
}
