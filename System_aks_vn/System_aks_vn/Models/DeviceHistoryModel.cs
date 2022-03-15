using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace System_aks_vn.Models
{
    public class DeviceHistoryModel
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public int? ExpanderHeight { get; set; }
        public DateTime Time { get; set; }
        public ObservableCollection<Details> Details { get; set; }
    }

    public class Details
    {
        public DateTime Time { get; set; }
        public string Name { get; set; }
    }

    class HistoryResponse
    {
        [JsonProperty("Time")]
        public DateTime Time { get; set; }
        [JsonProperty("Content")]
        public object Content { get; set; }
    }
}
