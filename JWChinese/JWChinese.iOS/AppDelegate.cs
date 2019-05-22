using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using CarouselView.iOS;
using System.Diagnostics;
using Social;

namespace JWChinese.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //

        //static AppDelegate()
        //{
        //    global::Xamarin.Forms.Forms.Init();
        //}

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            var x = typeof(JWChinese.iOS.GridViewRender);

            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
            UIApplication.SharedApplication.SetStatusBarHidden(false, false);

            var tint = UIColor.FromRGB(47, 100, 168);

            //UINavigationBar.Appearance.TitleTextAttributes = new UIStringAttributes() { ForegroundColor = UIColor.White }; 
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes { TextColor = UIColor.White }); //title text color

            UINavigationBar.Appearance.BarTintColor = tint; //bar background
            //UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(232, 234, 237); //bar background
            UINavigationBar.Appearance.TintColor = UIColor.White; //Tint color of button items

            //UIBarButtonItem.Appearance.TintColor = tint; //Tint color of bar button items

            UITabBar.Appearance.TintColor = tint;

            UISwitch.Appearance.OnTintColor = tint;

            UIAlertView.Appearance.TintColor = tint;

            UIView.AppearanceWhenContainedIn(typeof(UIAlertController)).TintColor = tint;
            UIView.AppearanceWhenContainedIn(typeof(UIActivityViewController)).TintColor = tint;
            UIView.AppearanceWhenContainedIn(typeof(SLComposeViewController)).TintColor = tint;

            // Another way to do statusbar background color to white
            var statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;
            if (statusBar.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")))
            {
                statusBar.BackgroundColor = tint;
            }

            CarouselViewRenderer.Init();

            Corcav.Behaviors.Infrastructure.Init();

            // Super important for ios size type to px
            if (DeviceHardware.Version.Equals(DeviceHardware.IOSHardware.iPhone3G)
                || DeviceHardware.Version.Equals(DeviceHardware.IOSHardware.iPhone3GS))
            {
                App.IOSSizeProportion = 1;
            }
            else if (DeviceHardware.Version.Equals(DeviceHardware.IOSHardware.iPhone4)
                || DeviceHardware.Version.Equals(DeviceHardware.IOSHardware.iPhone4CDMA)
                || DeviceHardware.Version.Equals(DeviceHardware.IOSHardware.iPhone4RevA)
                || DeviceHardware.Version.Equals(DeviceHardware.IOSHardware.iPhone4S)
                || DeviceHardware.Version.Equals(DeviceHardware.IOSHardware.iPhone5CDMAGSM)
                || DeviceHardware.Version.Equals(DeviceHardware.IOSHardware.iPhone5GSM)
                || DeviceHardware.Version.Equals(DeviceHardware.IOSHardware.İPhone6)
                || DeviceHardware.Version.Equals(DeviceHardware.IOSHardware.İPhone6S)
                || DeviceHardware.Version.Equals(DeviceHardware.IOSHardware.İPhoneSE))
            {
                App.IOSSizeProportion = 2;
            }
            else
            {
                App.IOSSizeProportion = 3;
            }

            LoadApplication(new App());

            UpdateRotation(UIApplication.SharedApplication);

            //UIDevice.Notifications.ObserveOrientationDidChange((s, e) =>
            //{
            //    UpdateOrientationInformationSoThatICanCorrectTheGridViewLayoutParadigmFunction();

            //});
            return base.FinishedLaunching(app, options);
        }

        public override void DidChangeStatusBarOrientation(UIApplication application, UIInterfaceOrientation oldStatusBarOrientation)
        {
            if (!Xamarin.Forms.Forms.IsInitialized)
            {
                global::Xamarin.Forms.Forms.Init();
            }

            UpdateRotation(application);
        }


        private static void UpdateRotation(UIApplication application)
        {
            UIInterfaceOrientation orientation = application.StatusBarOrientation;
            nfloat w = UIScreen.MainScreen.Bounds.Width;
            nfloat h = UIScreen.MainScreen.Bounds.Height;

            JWChinese.Objects.Orientation.Width = (int)w;
            JWChinese.Objects.Orientation.Height = (int)h;

            Debug.WriteLine(w + " X " + h);

            JWChinese.Objects.Orientation.Landscape = (w > h);

            var o = UIDevice.CurrentDevice.Orientation;
            if (o == UIDeviceOrientation.Portrait)
                JWChinese.Objects.Orientation.Rotation = JWChinese.Objects.Rotation.Rotation0;
            else if (o == UIDeviceOrientation.PortraitUpsideDown)
                JWChinese.Objects.Orientation.Rotation = JWChinese.Objects.Rotation.Rotation180;
            else if (o == UIDeviceOrientation.LandscapeLeft)
                JWChinese.Objects.Orientation.Rotation = JWChinese.Objects.Rotation.Rotation90;
            else if (o == UIDeviceOrientation.LandscapeRight)
                JWChinese.Objects.Orientation.Rotation = JWChinese.Objects.Rotation.Rotation270;
        }
    }
}
