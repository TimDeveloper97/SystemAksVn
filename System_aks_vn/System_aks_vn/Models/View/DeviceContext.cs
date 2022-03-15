using System;
using System.Collections.Generic;
using System.Text;

namespace System_aks_vn.Models.View
{
    public class DeviceContext : Vst.Context
    {
        public string DeviceId { get { return GetString("#deviceId"); } set => Push("#deviceId", value); }
        public string Func { get { return GetString("func"); } set => Push("func", value); }
        public object Args { get { return GetString("args"); } set => Push("args", value); }
    }

    public class AccountContext : Vst.Context
    {
        public string OldPass { get { return GetString("Password"); } set => Push("Password", value); }
        public string NewPass { get { return GetString("NewPass"); } set => Push("NewPass", value); }
        public object Confirm { get { return GetString("Confirm"); } set => Push("Confirm", value); }
    }
}
