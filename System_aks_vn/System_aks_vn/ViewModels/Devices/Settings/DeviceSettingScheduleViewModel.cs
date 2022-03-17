using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    [QueryProperty(nameof(ParameterPlan), nameof(ParameterPlan))]
    public class DeviceSettingScheduleViewModel : BaseViewModel
    {
        #region Property
        private string parameterDeviceId, parameterPlan;
        private ObservableCollection<string> hexStatus;
        private int day;
        private Dictionary<int, List<string>> _mapHexStatus;

        public string ParameterDeviceId
        {
            get => parameterDeviceId;
            set
            {
                parameterDeviceId = Uri.UnescapeDataString(value ?? string.Empty);
                SetProperty(ref parameterDeviceId, value);
            }
        }
        public string ParameterPlan
        {
            get => parameterPlan; set
            {
                parameterPlan = Uri.UnescapeDataString(value ?? string.Empty);
                SetProperty(ref parameterPlan, value);
            }
        }

        public ObservableCollection<string> HexStatus { get => hexStatus; set => SetProperty(ref hexStatus, value); }
        public int Day
        {
            get => day; set
            {
                SetProperty(ref day, value);
                if(_mapHexStatus.Count != 0)
                    DeviceScheduleView.UpdateViewWhenChangeDay(_mapHexStatus[day]);
            }
        }

        #endregion

        #region Command 
        public ICommand PageAppearingCommand => new Command(() =>
        {
            InitView();
            GetData();
            InitDrawData();
        });

        public ICommand SubmitScheduleCommand => new Command(async () =>
        {
            IsBusy = true;
            
            try
            {
                var day = Day;
                var x = HexStatus;

                Mqtt.ClearEvent();
                Mqtt.Publish(Topic, new DeviceContext
                {
                    Args = new List<string>(2) { Day.ToString(), ObservableToString(HexStatus) },
                    DeviceId = ParameterDeviceId,
                    Token = Token,
                    Func = "PLAN",
                    Url = Api.SettingPlan
                });

                _mapHexStatus[day] = new List<string>(HexStatus);
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

        public ICommand OnSwiped => new Command<string>((type) =>
        {
            var x = type;
        });
        #endregion

        public DeviceSettingScheduleViewModel()
        {
            Init();
        }

        #region Method
        void InitView()
        {
            DependencyService.Get<IStatusBar>().SetColoredStatusBar("#007bff");
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

        void Init()
        {
            HexStatus = new ObservableCollection<string>();
            _mapHexStatus = new Dictionary<int, List<string>>();
        }

        void GetData()
        {
            _mapHexStatus?.Clear();

            if (ParameterPlan == string.Empty)
            {
                for (int i = 0; i <= 7; i++)
                {
                    var lhex = new List<string>();
                    for (int j = 0; j < 48; j++)
                        lhex.Add("-1");

                    _mapHexStatus.Add(i, lhex);
                }
            }
            else
            {
                var lplan = JsonConvert.DeserializeObject<Dictionary<string, string>>(ParameterPlan);
                string all = null;
                string dayinweek = "";

                foreach (var kv in lplan)
                {
                    if (kv.Key == "7")
                        all = kv.Key;

                    // set cac ngay trong tuan + ca ngay all
                    dayinweek += kv.Key; // ngay da set
                    var lhex = new List<string>();

                    foreach (var c in kv.Value)
                        lhex.Add(c.ToString());

                    _mapHexStatus.Add(int.Parse(kv.Key), lhex);
                }

                // set tat ca ngay con lai
                for (int i = 0; i <= 7; i++)
                {
                    var day = i.ToString();

                    if (!dayinweek.Contains(day) && all != null)
                        _mapHexStatus.Add(i, _mapHexStatus[int.Parse(all)]);
                    else if (!dayinweek.Contains(day))
                    {
                        var lhex = new List<string>();
                        for (int j = 0; j < 48; j++)
                            lhex.Add("-1");

                        _mapHexStatus.Add(i, lhex);
                    }
                }
            } 
        }

        void InitDrawData()
        {
            var hex = _mapHexStatus[Day];
            HexStatus?.Clear();

            foreach (var code in hex)
            {
                HexStatus.Add(code);
            }
            DeviceScheduleView.UpdateViewWhenChangeDay(_mapHexStatus[0]);
        }
        #endregion
    }
}
