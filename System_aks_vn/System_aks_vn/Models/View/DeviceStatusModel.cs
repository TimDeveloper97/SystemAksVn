using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System_aks_vn.Controls;
using System_aks_vn.Domain;
using Xamarin.Forms;

namespace System_aks_vn.Models.View
{
    public class DeviceStatusModel : BaseBinding
    {
        private bool enableARMHome;
        private bool enableRelay4;
        private bool enableARMAway;
        private bool enableDISARM;
        private bool enableRelay1;
        private bool enableRelay2;
        private bool enableRelay3;
        private bool blinkRelay1;
        private bool blinkRelay2;
        private bool blinkRelay3;
        private bool blinkRelay4;

        [Description("ARM0")]
        public bool EnableARMAway { get => enableARMAway; set => SetProperty(ref enableARMAway, value); }
        [Description("DISARM")]
        public bool EnableDISARM { get => enableDISARM; set => SetProperty(ref enableDISARM, value); }
        [Description("ARM1")]
        public bool EnableARMHome { get => enableARMHome; set => SetProperty(ref enableARMHome, value); }
        [Description("OUTPUT0")]
        public bool EnableRelay1 { get => enableRelay1; set { SetProperty(ref enableRelay1, value); BlinkRelay1 = EnableRelay1; } }
        [Description("OUTPUT1")]
        public bool EnableRelay2 { get => enableRelay2; set { SetProperty(ref enableRelay2, value); BlinkRelay2 = EnableRelay2; } }
        [Description("OUTPUT2")]
        public bool EnableRelay3 { get => enableRelay3; set { SetProperty(ref enableRelay3, value); BlinkRelay3 = EnableRelay3; } }
        [Description("OUTPUT3")]
        public bool EnableRelay4 { get => enableRelay4; set { SetProperty(ref enableRelay4, value); BlinkRelay4 = EnableRelay4; } }

        public bool BlinkRelay1
        {
            get => blinkRelay1; set
            {
                blinkRelay1 = value;
                if (blinkRelay1 == true)
                {
                    DependencyService.Get<ITimer>().StartTimer(TimeSpan.FromSeconds(2), () =>
                    {
                        blinkRelay1 = !blinkRelay1;

                        OnPropertyChanged();
                        return EnableRelay1;
                    });
                }

                //SetProperty(ref blinkRelay1, value);
            }
        }
        public bool BlinkRelay2
        {
            get => blinkRelay2; set
            {
                blinkRelay2 = value;

                if (blinkRelay2 == true)
                {
                    DependencyService.Get<ITimer>().StartTimer(TimeSpan.FromSeconds(2), () =>
                    {
                        blinkRelay2 = !blinkRelay2;
                        OnPropertyChanged();
                        return EnableRelay2;
                    });
                }
            }
        }
        public bool BlinkRelay3
        {
            get => blinkRelay3; set
            {
                blinkRelay3 = value;
                if (blinkRelay3 == true)
                {
                    DependencyService.Get<ITimer>().StartTimer(TimeSpan.FromSeconds(2), () =>
                    {
                        blinkRelay3 = !blinkRelay3;
                        OnPropertyChanged();
                        return EnableRelay3;
                    });
                }
            }
        }
        public bool BlinkRelay4
        {
            get => blinkRelay4; set
            {
                blinkRelay4 = value;
                if (blinkRelay4 == true)
                {
                    DependencyService.Get<ITimer>().StartTimer(TimeSpan.FromSeconds(2), () =>
                    {
                        blinkRelay4 = !blinkRelay4;
                        OnPropertyChanged();
                        return EnableRelay4;
                    });
                }
            }
        }

        public static readonly List<string> Tags = new List<string> { "ARM0", "ARM1", "DISARM", "OUTPUT0", "OUTPUT1", "OUTPUT2", "OUTPUT3" };

        public DeviceStatusModel()
        {
            Reset();
        }

        void Reset()
        {
            EnableARMAway = false;
            EnableDISARM = false;
            EnableARMHome = false;
            EnableRelay1 = false;
            EnableRelay2 = false;
            EnableRelay3 = false;
            EnableRelay4 = false;
        }
    }
}
