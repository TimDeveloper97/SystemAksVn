using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System_aks_vn.Controls;
using System_aks_vn.Droid.CustomRenderer;

[assembly: Xamarin.Forms.Dependency(typeof(TimerRenderer))]
namespace System_aks_vn.Droid.CustomRenderer
{
    public class TimerRenderer : ITimer
    {
        public void StartTimer(TimeSpan interval, Func<bool> callback)
        {
            var handler = new Handler(Looper.MainLooper);
            handler.PostDelayed(() =>
            {
                if (callback())
                    StartTimer(interval, callback);

                handler.Dispose();
                handler = null;
            }, (long)interval.TotalMilliseconds);
        }
    }
}