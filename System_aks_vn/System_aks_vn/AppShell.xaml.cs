using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System_aks_vn.Services.Temp;
using System_aks_vn.Views;
using System_aks_vn.Views.Devices;
using System_aks_vn.Views.Devices.Settings;
using System_aks_vn.Views.Version;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace System_aks_vn
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            InitDependencyService();
            InitRoute();
        }

        void InitDependencyService()
        {
            DependencyService.Register<SomeService>();
        }

        void InitRoute()
        {
            Routing.RegisterRoute(nameof(DeviceV30Page), typeof(DeviceV30Page));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(SettingPage), typeof(SettingPage));
            Routing.RegisterRoute(nameof(DeviceSettingPage), typeof(DeviceSettingPage));
            Routing.RegisterRoute(nameof(DeviceHistoryPage), typeof(DeviceHistoryPage));
            Routing.RegisterRoute(nameof(DeviceSettingSmsPage), typeof(DeviceSettingSmsPage));
            Routing.RegisterRoute(nameof(DeviceSettingCallPage), typeof(DeviceSettingCallPage));
            Routing.RegisterRoute(nameof(DeviceSettingSchedulePage), typeof(DeviceSettingSchedulePage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
