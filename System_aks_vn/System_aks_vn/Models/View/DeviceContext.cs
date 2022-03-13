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
}
