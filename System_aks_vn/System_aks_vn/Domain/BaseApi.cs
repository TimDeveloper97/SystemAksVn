using System;
using System.Collections.Generic;
using System.Text;

namespace System_aks_vn.Domain
{
    public class Api
    {
        public const string Login = "account/login";
        public const string DeviceList = "service/getDeviceList";
        public const string DeviceStatus = "remote/getDeviceStatus";
        public const string Control = "remote/control";
        public const string SettingSms = "setting/phone";
        public const string SettingCall = "setting/phone";
        public const string SettingPlan = "setting/plan";
        public const string DeviceHistory = "remote/getHistory";
    }
}
