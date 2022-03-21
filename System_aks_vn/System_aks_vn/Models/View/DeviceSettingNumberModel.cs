using System;
using System.Collections.Generic;
using System.Text;
using System_aks_vn.Domain;

namespace System_aks_vn.Models.View
{
    public class DeviceSettingNumberModel : BaseBinding
    {
        private string number5;
        private string number1;
        private string number2;
        private string number3;
        private string number4;

        public string Number1 { get => number1; set => SetProperty(ref number1, value ?? ""); }
        public string Number2 { get => number2; set => SetProperty(ref number2, value ?? ""); }
        public string Number3 { get => number3; set => SetProperty(ref number3, value ?? ""); }
        public string Number4 { get => number4; set => SetProperty(ref number4, value ?? ""); }
        public string Number5 { get => number5; set => SetProperty(ref number5, value ?? ""); }
    }

    public class NumberId : BaseBinding
    {
        private string number;
        public string Id { get; set; }
        public string Number { get => number; set => SetProperty(ref number, value); }
    }
}
