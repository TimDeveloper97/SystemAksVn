using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
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
    [QueryProperty(nameof(ParameterCall), nameof(ParameterCall))]
    public class DeviceSettingCallViewModel : BaseViewModel
    {
        #region Property
        private string parameterDeviceId, parameterCall;
        private NumberId number;
        private double widthCard;
        private ObservableCollection<NumberId> calls;

        public string ParameterDeviceId
        {
            get => parameterDeviceId;
            set
            {
                parameterDeviceId = Uri.UnescapeDataString(value ?? string.Empty);
                Title = ParameterDeviceId;
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

        public ObservableCollection<NumberId> Calls { get => calls; set => SetProperty(ref calls, value); }
        public double WidthCard { get => widthCard; set => SetProperty(ref widthCard, value); }
        public NumberId Number { get => number; set => SetProperty(ref number, value); }
        #endregion

        #region Command 

        public ICommand LoadCallCommand { get; set; }
        public ICommand PageAppearingCommand => new Command(() =>
        {
            Init();
            IsBusy = true;
        });
        public ICommand SubmitCallCommand => new Command(async () =>
        {
            IsBusy = true;

            try
            {
                if (!Mqtt.IsConnected)
                    Mqtt.Connect();

                Mqtt.ClearEvent();
                Mqtt.Publish(Topic, new DeviceContext
                {
                    Args = GetListString(Calls),
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
                await MaterialDialog.Instance.SnackbarAsync(message: "Success",
                              msDuration: MaterialSnackbar.DurationLong);
            }
        });
        public ICommand ChangeCommand => new Command<NumberId>(async (item) =>
        {
            if (item == null || string.IsNullOrEmpty(item.Number)) return;

            if (item.Id == null)
            {
                var callnull = Calls.FirstOrDefault(i => string.IsNullOrEmpty(i.Number));
                if (callnull == null) return;

                callnull.Number = item.Number;
            }
            else
            {
                var call = Calls.FirstOrDefault(i => i.Id == item.Id);
                if (call == null)
                {
                    var callnull = Calls.FirstOrDefault(i => string.IsNullOrEmpty(i.Number));
                    if (callnull == null) return;

                    callnull.Number = item.Number;
                }
                else
                    call.Number = item.Number;
            }

            Number.Number = null;
            Number.Id = null;
        });
        public ICommand EditCommand => new Command<NumberId>((item) =>
        {
            if (string.IsNullOrEmpty(item.Number))
                return;

            Number = new NumberId();

            Number.Number = item.Number;
            Number.Id = item.Id;
        });
        public ICommand RemoveCommand => new Command<NumberId>((item) =>
        {
            Calls.Remove(item);
            Calls.Add(new NumberId
            {
                Id = "4",
                Number = "",
            });
        });
        #endregion

        public DeviceSettingCallViewModel()
        {
            LoadCallCommand = new Command(() => ExecuteLoadCallCommand());
            Number = new NumberId();
        }

        #region Method
        void Init()
        {
            DependencyService.Get<IStatusBar>().SetColoredStatusBar("#007bff");
            WidthCard = App.ScreenWidth * 0.8;
            Calls = new ObservableCollection<NumberId>();

            for (int i = 0; i < 5; i++)
            {
                Calls.Add(new NumberId
                {
                    Number = "",
                    Id = i.ToString(),
                });
            }
        }
        void ExecuteLoadCallCommand()
        {
            try
            {
                IsBusy = true;
                if (ParameterCall != string.Empty)
                {
                    var lcall = JsonConvert.DeserializeObject<List<string>>(ParameterCall);
                    if (lcall == null && lcall.Count == 0) return;

                    for (int i = 0; i < lcall.Count; i++)
                    {
                        var item = Calls[i];
                        item.Number = lcall[i];
                    }
                }
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

        bool IsValidatePhoneNumber(string number)
        {
            return System.Text.RegularExpressions.Regex.Match(number, @"^(\+[0-9]{9})$").Success;
        }

        List<string> GetListString(IEnumerable<NumberId> Calls)
        {
            var list = new List<string>();
            foreach (var sms in Calls)
            {
                list.Add(sms.Number);
            }
            return list;
        }
        #endregion
    }
}
