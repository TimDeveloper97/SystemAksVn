using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System_aks_vn.Controls;
using System_aks_vn.Domain;
using System_aks_vn.Models.Response;
using System_aks_vn.Services.Temp;
using Xamarin.Forms;

namespace System_aks_vn.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        #region Property
        private ObservableCollection<DeviceModel> devices;
        private double widthCard;

        public ObservableCollection<DeviceModel> Devices { get => devices; set => SetProperty(ref devices, value); }
        public double WidthCard { get => widthCard; set => SetProperty(ref widthCard, value); }

        #endregion

        #region Command 
        public ICommand PageAppearingCommand => new Command(() =>
        {
            Init();
        });
        public ICommand DetailDeviceCommand => new Command<DeviceModel>(device =>
        {
            var x = device;
        });
        public ICommand LoadDeviceCommand { get; set; }
        #endregion

        public HomeViewModel()
        {
            LoadDeviceCommand = new Command(() => ExecuteLoadDeviceCommand());
        }

        #region Method
        void Init()
        {
            Title = "Home";
            Devices = new ObservableCollection<DeviceModel>();
            DependencyService.Get<IStatusBar>().SetColoredStatusBar("#007bff");
            WidthCard = Application.Current.MainPage.Width * 0.35;
            IsBusy = true;
        }

        void ExecuteLoadDeviceCommand()
        {
            IsBusy = true;

            try
            {
                Devices?.Clear();
                Mqtt.ClearEvent();
                Mqtt.Publish(Topic, Api.DeviceList, new { token = Token });

                Mqtt.MessageReceived += async (s, e) =>
                {
                    var mqtt = (s as Mqtt);
                    var ldevice = JsonConvert.DeserializeObject<List<DeviceModel>>(mqtt.Response.Value.ToString());

                    await Device.InvokeOnMainThreadAsync(() =>
                    {
                        foreach (var device in ldevice)
                        {
                            Devices.Add(device);
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
                IsBusy = false;
            }

        }
        #endregion
    }
}
