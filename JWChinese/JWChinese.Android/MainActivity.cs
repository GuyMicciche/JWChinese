using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using CarouselView.Android;
using System;
using Xamarin.Forms.Platform.Android;

namespace JWChinese.Droid
{
    [Activity(Label = "JW Chinese", Icon = "@drawable/ic_launcher", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            //System.Threading.Thread.Sleep(2000);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            Window.RequestFeature(WindowFeatures.ActionBar);
            Window.AddFlags(WindowManagerFlags.KeepScreenOn);
            //SetTheme(Resource.Style.MainTheme);

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            CarouselViewRenderer.Init();

            LoadApplication(new JWChinese.App());

            UpdateCurrentRotation();

            // Super important for converting dpi to px
            // Both these do the exact same thing, (please adjust App.cs)
            //App.AndroidDPI = Resources.DisplayMetrics.Density / 160;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                App.AndroidDPI = new Size((int)Resources.DisplayMetrics.DensityDpi, (int)Resources.DisplayMetrics.DensityDpi).Height / 160;
            }
            else
            {
                App.AndroidDPI = (int)Resources.DisplayMetrics.DensityDpi / 160;
            }

            // SET STATUS BAR COLOR
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                // clear FLAG_TRANSLUCENT_STATUS flag:
                Window.ClearFlags(WindowManagerFlags.TranslucentStatus);

                //Window.ClearFlags(WindowManager.Pa WindowManager.LayoutParams.FLAG_TRANSLUCENT_STATUS);
                Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);

                // finally change the color
                Window.SetStatusBarColor(new Color(ContextCompat.GetColor(this, Resource.Color.statusBarColor)));
            }
        }
        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            Console.WriteLine("Orientation => " + (newConfig.ScreenWidthDp > newConfig.ScreenHeightDp ? "LANDSCAPE" : "PORTRAIE"));
            //JWChinese.Objects.Orientation.Landscape = newConfig.ScreenWidthDp > newConfig.ScreenHeightDp;
            //JWChinese.Objects.Orientation.Width = newConfig.ScreenWidthDp;
            //JWChinese.Objects.Orientation.Height = newConfig.ScreenHeightDp;

            UpdateCurrentRotation();
        }

        private void UpdateCurrentRotation()
        {
            DisplayMetrics dm = Resources.DisplayMetrics;
            float w = dm.WidthPixels / dm.Density;
            float h = dm.HeightPixels / dm.Density;

            JWChinese.Objects.Orientation.Landscape = w > h;
            JWChinese.Objects.Orientation.Width = (int)w;
            JWChinese.Objects.Orientation.Height = (int)h;

            switch (WindowManager.DefaultDisplay.Rotation)
            {
                case SurfaceOrientation.Rotation0:
                    JWChinese.Objects.Orientation.Rotation = JWChinese.Objects.Rotation.Rotation0;
                    break;
                case SurfaceOrientation.Rotation90:
                    JWChinese.Objects.Orientation.Rotation = JWChinese.Objects.Rotation.Rotation90;
                    break;
                case SurfaceOrientation.Rotation180:
                    JWChinese.Objects.Orientation.Rotation = JWChinese.Objects.Rotation.Rotation180;
                    break;
                case SurfaceOrientation.Rotation270:
                    JWChinese.Objects.Orientation.Rotation = JWChinese.Objects.Rotation.Rotation270;
                    break;
            }
            Console.WriteLine("WindowManager.DefaultDisplay.Rotation => " + WindowManager.DefaultDisplay.Rotation);
            GemWriteLine(WindowManager.DefaultDisplay.Rotation);
        }

        private void GemWriteLine(object obj)
        {
            Console.WriteLine(obj.GetType().ToString() + " => " + obj.ToString());
        }
    }
}

