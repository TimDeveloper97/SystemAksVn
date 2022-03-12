using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System_aks_vn.Controls;
using System_aks_vn.Domain;
using System_aks_vn.Views;
using Xamarin.Forms;

namespace System_aks_vn.ViewModels
{
    public class SettingViewModel: BaseViewModel
    {
        #region Property
        #endregion

        #region Command 
        public ICommand ProfileCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync(nameof(ProfilePage));
        });
        public ICommand SmsConfigCommand => new Command(async () =>
        {

        });
        public ICommand CallConfigCommand => new Command(async () =>
        {

        });
        public ICommand ScheduleConfigCommand => new Command(async () =>
        {

        });
        public ICommand LogoutCommand => new Command(async () =>
        {
            Mqtt.Disconnect();
            Topic = null;
            Token = null;

            await Shell.Current.GoToAsync("//LoginPage");
        });

        public ICommand PageAppearingCommand => new Command(() =>
        {
            Init();
        });
        #endregion

        public SettingViewModel()
        {
        }

        #region Method
        void Init()
        {
            Title = "Setting";
            DependencyService.Get<IStatusBar>().SetWhiteStatusBar();
        }

        #endregion
    }
}
