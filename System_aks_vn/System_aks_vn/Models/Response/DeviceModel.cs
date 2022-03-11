using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace System_aks_vn.Models.Response
{
    public class DeviceModel
    {
        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }
    }
}
