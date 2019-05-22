using JWChinese.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationRender))]
namespace JWChinese.iOS
{
    public class NavigationRender : NavigationRenderer
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //NavigationController.NavigationBar.BarStyle = UIBarStyle.Black;
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (Element == null) return;

            (Element as NavigationPage).BarTextColor = Color.White;

            //// Create the material drop shadow
            //NavigationBar.Layer.ShadowColor = UIColor.Black.CGColor;
            //NavigationBar.Layer.ShadowOffset = new CGSize(0, 0);
            //NavigationBar.Layer.ShadowRadius = 3;
            //NavigationBar.Layer.ShadowOpacity = 1;

            //// Create the back arrow icon image
            //var arrowImage = UIImage.FromBundle("Icons/ic_arrow_back_white.png");
            //NavigationBar.BackIndicatorImage = arrowImage;
            //NavigationBar.BackIndicatorTransitionMaskImage = arrowImage;


            // Set the back button title
            //if (NavigationItem.BackBarButtonItem != null)
            //{
            //    NavigationItem.BackBarButtonItem.Title = "Back";
            //}
            //if (NavigationBar.BackItem != null)
            //{
            //    NavigationBar.BackItem.Title = "Back";
            //}
        }
    }
}
