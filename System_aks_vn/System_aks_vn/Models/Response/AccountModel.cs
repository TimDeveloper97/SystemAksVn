using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace System_aks_vn.Models.Response
{
    public class AccountModel
    {
        [JsonProperty("UserName")]
        public string UserName { get; set; }

        [JsonProperty("#token")]
        public string Token { get; set; }

        [JsonProperty("Role")]
        public string Role { get; set; }
    }
}
