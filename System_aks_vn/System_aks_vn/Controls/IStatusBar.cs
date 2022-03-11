using System;
using System.Collections.Generic;
using System.Text;

namespace System_aks_vn.Controls
{
    public interface IStatusBar
    {
        void SetColoredStatusBar(string hexColor);
        void SetWhiteStatusBar();
        void SetBlackStatusBar();
    }
}
