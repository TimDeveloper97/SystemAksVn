using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System_aks_vn.Controls;
using System_aks_vn.iOS.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(StatusBarRenderer))]
namespace System_aks_vn.iOS.CustomRenderer
{
    public class StatusBarRenderer : IStatusBar
    {
        public void SetColoredStatusBar(string hexColor)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                {
                    UIView statusBar = new UIView(UIApplication.SharedApplication.KeyWindow.WindowScene.StatusBarManager.StatusBarFrame);
                    statusBar.BackgroundColor = Color.FromHex(hexColor).ToUIColor();
                    UIApplication.SharedApplication.KeyWindow.AddSubview(statusBar);
                }
                else
                {
                    UIView statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("StatusBarRenderer")) as UIView;
                    if (statusBar.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")))
                    {
                        statusBar.BackgroundColor = Color.FromHex(hexColor).ToUIColor();
                    }
                }
                UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
                GetCurrentViewController().SetNeedsStatusBarAppearanceUpdate();
            });
        }

        public void SetWhiteStatusBar()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                {
                    UIView statusBar = new UIView(UIApplication.SharedApplication.KeyWindow.WindowScene.StatusBarManager.StatusBarFrame);
                    statusBar.BackgroundColor = UIColor.White;
                    UIApplication.SharedApplication.KeyWindow.AddSubview(statusBar);
                }
                else
                {
                    UIView statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("StatusBarRenderer")) as UIView;
                    if (statusBar.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")))
                    {
                        statusBar.BackgroundColor = UIColor.White;
                    }
                }
                UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.DarkContent, false);
                GetCurrentViewController().SetNeedsStatusBarAppearanceUpdate();
            });
        }

        public void SetBlackStatusBar()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                {
                    UIView statusBar = new UIView(UIApplication.SharedApplication.KeyWindow.WindowScene.StatusBarManager.StatusBarFrame);
                    statusBar.BackgroundColor = UIColor.White;
                    UIApplication.SharedApplication.KeyWindow.AddSubview(statusBar);
                }
                else
                {
                    UIView statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("StatusBarRenderer")) as UIView;
                    if (statusBar.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")))
                    {
                        statusBar.BackgroundColor = UIColor.White;
                    }
                }
                UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.DarkContent, false);
                GetCurrentViewController().SetNeedsStatusBarAppearanceUpdate();
            });
        }

        UIViewController GetCurrentViewController()
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
                vc = vc.PresentedViewController;
            return vc;
        }
    }
}