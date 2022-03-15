using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System_aks_vn.Controls;
using System_aks_vn.Domain;
using System_aks_vn.Models;
using System_aks_vn.Models.View;
using Xamarin.Forms;

namespace System_aks_vn.ViewModels.Devices
{
    [QueryProperty(nameof(ParameterDeviceId), nameof(ParameterDeviceId))]
    public class DeviceHistoryViewModel : BaseViewModel
    {
        #region Property
        private string parameterDeviceId;
        private ObservableCollection<DeviceHistoryModel> historys;

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
        public ObservableCollection<DeviceHistoryModel> Historys { get => historys; set => SetProperty(ref historys, value); }

        #endregion

        #region Command 
        public ICommand PageAppearingCommand => new Command(async () =>
        {
            DependencyService.Get<IStatusBar>().SetColoredStatusBar("#007bff");
            IsBusy = true;
        });

        public ICommand LoadHistoryDeviceCommand { get; set; }
        #endregion

        public DeviceHistoryViewModel()
        {
            Init();
        }

        #region Method
        void Init()
        {
            Historys = new ObservableCollection<DeviceHistoryModel>();
            LoadHistoryDeviceCommand = new Command(async () => await ExecuteLoadHistoryCommand());
        }

        async Task ExecuteLoadHistoryCommand()
        {
            var loadingDialog = await XF.Material.Forms.UI.Dialogs.MaterialDialog.Instance
                    .LoadingDialogAsync(message: $"Loading");
            IsBusy = true;

            try
            {
                Historys?.Clear();
                Mqtt.ClearEvent();
                Mqtt.Publish(Topic, new DeviceContext
                {
                    Url = Api.DeviceHistory,
                    Token = Token,
                    DeviceId = ParameterDeviceId,
                });

                Mqtt.MessageReceived += async (s, e) =>
                {
                    var res = (s as Mqtt).Response;
                    if (res.Code == 100)
                        await TimeoutSession(res.Message);

                    var month = JObject.Parse(res.Value.ToString())[DateTime.Now.ToString("yyyyMM")].ToString();
                    if (month == null) return;

                    var lhistory = JsonConvert.DeserializeObject<List<HistoryResponse>>(month);
                    var ldetail = new List<Details>();

                    // get list command
                    foreach (var item in lhistory)
                    {
                        var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(item.Content.ToString());
                        var content = "";

                        foreach (var kv in dict)
                        {
                            content += kv.Key + ", ";
                        }
                        content = content.Substring(0, content.Length - 2);

                        // add to list temp
                        ldetail.Add(new Details { Name = content, Time = item.Time });
                    }

                    var groups = ldetail.GroupBy(x => x.Time.Date).Select(y => new DeviceHistoryModel
                    {
                        Name = y.Key.ToString("yyyy-MM-dd"),
                        Count = y.Count(),
                        Time = y.Key,
                        Details = new ObservableCollection<Details>(y.ToList()),
                    });

                    await Device.InvokeOnMainThreadAsync(() =>
                    {
                        foreach (var item in groups)
                        {
                            Historys.Add(item);
                        }
                    });
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                await loadingDialog.DismissAsync();
                IsBusy = false;
            }
        }
        #endregion
    }
}
