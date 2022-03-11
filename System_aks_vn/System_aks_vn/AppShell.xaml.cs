using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System_aks_vn.Services.Temp;
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
            //Routing.RegisterRoute(nameof(NotificationPage), typeof(NotificationPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
