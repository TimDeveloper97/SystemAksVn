using System;
using System.Collections.Generic;
using System.Text;

namespace System_aks_vn.Controls
{
    public interface ITimer
    {
        void StartTimer(TimeSpan interval, Func<bool> callback);
    }
}
