
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace System_aks_vn.Domain
{
    public class BaseViewModel : BaseBinding
    {
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        #region Extend
        private static Mqtt mqtt;
        protected static Mqtt Mqtt
        {
            get
            {
                if (mqtt == null)
                {
                    mqtt = new Mqtt();
                    mqtt.Connect();
                }    
                return mqtt;
            }
        }
        protected static string Topic { get; set; }
        protected static string Token { get; set; }
        protected Api Api { get; set; }
        #endregion
    }
}
