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
using XF.Material.Forms.UI.Dialogs;

namespace System_aks_vn.ViewModels.Devices.Settings
{
    [QueryProperty(nameof(ParameterDeviceId), nameof(ParameterDeviceId))]
    [QueryProperty(nameof(ParameterSms), nameof(ParameterSms))]
    public class DeviceSettingSmsViewModel : BaseViewModel
    {
        #region Property
        private string parameterDeviceId, parameterSms;
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
        public string ParameterSms
        {
            get => parameterSms; set
            {
                parameterSms = Uri.UnescapeDataString(value ?? string.Empty);
                SetProperty(ref parameterSms, value);
            }
        }

        public DeviceSettingNumberModel Sms { get => sms; set => SetProperty(ref sms, value); }

        #endregion

        #region Command 
        public ICommand PageAppearingCommand => new Command(() =>
        {
            Init();
            GetData();
        });
        public ICommand SubmitSmsCommand => new Command(async () =>
        {
            IsBusy = true;

            try
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
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
                await MaterialDialog.Instance.SnackbarAsync(message: "Success",
                              msDuration: MaterialSnackbar.DurationLong);
            }
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
        }

        void GetData()
        {
            if (parameterSms != string.Empty)
            {
                var lsms = JsonConvert.DeserializeObject<List<string>>(ParameterSms);
                if (lsms == null && lsms.Count == 0) return;

                Sms.Number1 = lsms[0];
                Sms.Number2 = lsms[1];
                Sms.Number3 = lsms[2];
                Sms.Number4 = lsms[3];
                Sms.Number5 = lsms[4];
            }
        }
        #endregion
    }
}
