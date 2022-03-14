using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System_aks_vn.Controls;
using System_aks_vn.Domain;
using System_aks_vn.Models.View;
using Xamarin.Forms;

namespace System_aks_vn.ViewModels.Devices.Settings
{
    public class DeviceSettingScheduleViewModel : BaseViewModel
    {
        #region Property
        private string parameterDeviceId;
        private ObservableCollection<string> hexStatus;
        private int day;

        public string ParameterDeviceId
        {
            get => parameterDeviceId;
            set
            {
                parameterDeviceId = Uri.UnescapeDataString(value ?? string.Empty);
                SetProperty(ref parameterDeviceId, value);
            }
        }
        public ObservableCollection<string> HexStatus { get => hexStatus; set => SetProperty(ref hexStatus, value); }
        public int Day { get => day; set => SetProperty(ref day, value); }

        #endregion

        #region Command 
        public ICommand PageAppearingCommand => new Command(() =>
        {
            Init();
        });

        public ICommand SubmitScheduleCommand => new Command(async () =>
        {
            var day = Day;
            var x = HexStatus;

            Mqtt.ClearEvent();
            Mqtt.Publish(Topic, new DeviceContext
            {
                Args = new List<string>(2) { Day.ToString(), ObservableToString(HexStatus) },
                DeviceId = ParameterDeviceId,
                Token = Token,
                Func = "CALL",
                Url = Api.SettingCall
            });
        });

        #endregion

        public DeviceSettingScheduleViewModel()
        {
            HexStatus = new ObservableCollection<string>();
            DeviceScheduleView._tmpSource = new List<string>();

            for (int i = 0; i < 48; i++)
            {
                HexStatus.Add("0");
                DeviceScheduleView._tmpSource.Add("0");
            }
        }

        #region Method
        void Init()
        {
            DependencyService.Get<IStatusBar>().SetColoredStatusBar("#007bff");
            IsBusy = true;
        }

        string ObservableToString(ObservableCollection<string> l)
        {
            var output = "";
            foreach (var i in l)
            {
                output += i;
            }
            return output;
        }
        #endregion
    }
}
