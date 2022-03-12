using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System_aks_vn.Controls;
using System_aks_vn.iOS.CustomRenderer;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(TimerRenderer))]
namespace System_aks_vn.iOS.CustomRenderer
{
    public class TimerRenderer : ITimer
    {
        public void StartTimer(TimeSpan interval, Func<bool> callback)
        {
            NSTimer timer = NSTimer.CreateRepeatingTimer(interval, t =>
            {
                if (!callback())
                    t.Invalidate();
            });
            NSRunLoop.Main.AddTimer(timer, NSRunLoopMode.Common);
        }
    }
}