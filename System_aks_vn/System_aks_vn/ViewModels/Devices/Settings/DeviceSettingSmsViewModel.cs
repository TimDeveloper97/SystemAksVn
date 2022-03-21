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
    [QueryProperty(nameof(ParameterSms), nameof(ParameterSms))]
    public class DeviceSettingSmsViewModel : BaseViewModel
    {
        #region Property
        private string parameterDeviceId, parameterSms;
        private NumberId number;
        private double widthCard;
        private ObservableCollection<NumberId> smss;

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
        public string ParameterSms
        {
            get => parameterSms; set
            {
                parameterSms = Uri.UnescapeDataString(value ?? string.Empty);
                SetProperty(ref parameterSms, value);
            }
        }

        public ObservableCollection<NumberId> Smss { get => smss; set => SetProperty(ref smss, value); }
        public double WidthCard { get => widthCard; set => SetProperty(ref widthCard, value); }
        public NumberId Number { get => number; set => SetProperty(ref number, value); }
        #endregion

        #region Command 
        public ICommand LoadSmsCommand { get; set; }
        public ICommand PageAppearingCommand => new Command(() =>
        {
            Init();
            IsBusy = true;
        });
        public ICommand SubmitSmsCommand => new Command(async () =>
        {
            IsBusy = true;

            try
            {
                Mqtt.ClearEvent();
                Mqtt.Publish(Topic, new DeviceContext
                {
                    Args = GetListString(Smss),
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

        public ICommand ChangeCommand => new Command<NumberId>(async (item) =>
        {
            if (item == null) return;

            var sms = Smss.FirstOrDefault(i => i.Id == item.Id);
            sms.Number = item.Number;
        });
        public ICommand EditCommand => new Command<NumberId>((item) =>
        {
            if (Number == null)
                Number = new NumberId();

            Number.Number = item.Number;
            Number.Id = item.Id;
        });
        public ICommand RemoveCommand => new Command<NumberId>((item) =>
        {
            var sms = Smss.FirstOrDefault(i => i.Id == item.Id);
            sms.Number = "";
        });
        #endregion

        public DeviceSettingSmsViewModel()
        {
            LoadSmsCommand = new Command(() => ExecuteLoadSmsCommand());
        }

        #region Method
        void Init()
        {
            DependencyService.Get<IStatusBar>().SetColoredStatusBar("#007bff");
            WidthCard = App.ScreenWidth * 0.8;
            Smss = new ObservableCollection<NumberId>();

            for (int i = 0; i < 5; i++)
            {
                Smss.Add(new NumberId
                {
                    Number = "",
                    Id = i.ToString(),
                });
            }
        }

        void ExecuteLoadSmsCommand()
        {
            try
            {
                IsBusy = true;
                if (parameterSms != string.Empty)
                {
                    var lsms = JsonConvert.DeserializeObject<List<string>>(ParameterSms);
                    if (lsms == null && lsms.Count == 0) return;

                    for (int i = 0; i < lsms.Count; i++)
                    {
                        var item = Smss[i];
                        item.Number = lsms[i];
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
        
        List<string> GetListString(IEnumerable<NumberId> Smss)
        {
            var list = new List<string>();
            foreach (var sms in Smss)
            {
                list.Add(sms.Number);
            }
            return list;
        }
        #endregion
    }
}
