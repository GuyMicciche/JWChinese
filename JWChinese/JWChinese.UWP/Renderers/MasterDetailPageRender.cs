using JWChinese.UWP.Renderers;
using System.Diagnostics;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(MasterDetailPage), typeof(MasterDetailPageRender))]
namespace JWChinese.UWP.Renderers
{
    public class MasterDetailPageRender : MasterDetailPageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<MasterDetailPage> e)
        {
            base.OnElementChanged(e);

            Control.ToolbarBackground = Helper.GetBrushFromHexa("#e8eaed");

            Control.Master.SizeChanged += Master_SizeChanged;
            Control.Detail.SizeChanged += Detail_SizeChanged;
            Control.DetailTitleVisibility = Visibility.Visible;

            Control.Loaded += Control_Loaded;
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            SetSplitViewStyle();
        }

        private void Detail_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            Element.Detail.Layout(new Rectangle(0, 0, e.NewSize.Width, e.NewSize.Height));
        }

        private void Master_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            // if not do this , master page content not show
            Element.Master.Layout(new Rectangle(0, 0, e.NewSize.Width, e.NewSize.Height));
        }
        public void SetSplitViewStyle()
        {
            Debug.WriteLine("GETTING STYLE!!!");

            // Get listview controls inside the SplitView
            var spv = Helper.FindChild<SplitView>(Control.Parent, "SplitView");
            Debug.WriteLine(spv);

            foreach (Windows.UI.Xaml.Controls.ListView lv in Helper.FindVisualChildren<Windows.UI.Xaml.Controls.ListView>(spv))
            {
                Debug.WriteLine("APPLYING STYLE!!!!!!!!!!!!!!!!!");


                // Add custom style
                lv.ItemContainerStyle = (Windows.UI.Xaml.Style)Windows.UI.Xaml.Application.Current.Resources["SplitViewItemContainerStyle"];

                // Remove listview opening transition
                TransitionCollection tc = lv.ItemContainerTransitions;
                for (int i = tc.Count - 1; i >= 0; i--)
                {
                    if (tc[i] is AddDeleteThemeTransition)
                    {
                        tc.RemoveAt(i);
                    }
                }
            }

            spv.SizeChanged += Spv_SizeChanged;
        }
        private void Spv_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var size = e.NewSize;

            UpdateRotation();
        }

        private void UpdateRotation()
        {
            var display = DisplayInformation.GetForCurrentView();
            var current = display.CurrentOrientation;

            if (current == DisplayOrientations.Portrait)
                JWChinese.Objects.Orientation.Rotation = JWChinese.Objects.Rotation.Rotation0;
            else if (current == DisplayOrientations.PortraitFlipped)
                JWChinese.Objects.Orientation.Rotation = JWChinese.Objects.Rotation.Rotation180;
            else if (current == DisplayOrientations.Landscape)
                JWChinese.Objects.Orientation.Rotation = JWChinese.Objects.Rotation.Rotation90;
            else
                JWChinese.Objects.Orientation.Rotation = JWChinese.Objects.Rotation.Rotation270;
        }
    }
}