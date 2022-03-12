using System;
using System.Collections.Generic;
using System.Text;

namespace System_aks_vn.Models.View
{
    public class DeviceDetailModel : Vst.Context
    {
        public string DeviceId { get { return GetString("#deviceId"); } set => Push("#deviceId", value); }
    }
}
