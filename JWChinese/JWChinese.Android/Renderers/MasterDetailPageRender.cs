using Android.Content;
using Android.Content.Res;
using Android.Util;
using Android.Views;
using JWChinese.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(MasterDetailPage), typeof(MasterDetailPageRender))]
namespace JWChinese.Droid
{
    public class MasterDetailPageRender : MasterDetailPageRenderer
    {
        bool firstDone;
        static int width = 320;

        public override void AddView(global::Android.Views.View child)
        {
            if (firstDone)
            {
                LayoutParams p = (LayoutParams)child.LayoutParameters;
                p.Width = dp(child.Context, width);
                base.AddView(child, p);
            }
            else
            {
                firstDone = true;
                base.AddView(child);
            }
        }

        public static int dp(Context context, int px)
        {
            return (int)(px * context.Resources.DisplayMetrics.Density);
        }
    }
}