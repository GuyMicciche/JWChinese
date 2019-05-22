using JWChinese.UWP.Renderers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform;
using Xamarin.Forms.Platform.UWP;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(Xamarin.Forms.CarouselView), typeof(CarouselViewRender))]
namespace JWChinese.UWP.Renderers
{
    public class CarouselViewRender : CarouselViewRenderer
    {
        public CarouselViewRender()
        {
            this.Loaded += CarouselViewRender_Loaded;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.CarouselView> e)
        {
            base.OnElementChanged(e);

            if (this.Control != null)
            {
                this.Control.SizeChanged += Control_SizeChanged;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            //if (this.Control != null)
            //{
            //    var list2 = new List<Windows.UI.Xaml.Controls.Button>();
            //    FindChildren(list2, this);

            //    foreach (var item in list2)
            //    {
            //        if (item is Windows.UI.Xaml.Controls.Button)
            //        {
            //            if (item.Name.Contains("Horizontal"))
            //            {
            //                item.Opacity = 1.0;
            //                item.IsHitTestVisible = true;
            //            }
            //        }
            //    }
            //}
        }

        private void Control_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Element.Layout(new Rectangle(0, 0, e.NewSize.Width, e.NewSize.Height));
        }

        private void CarouselViewRender_Loaded(object sender, RoutedEventArgs e)
        {
            // THIS SWEET BIT OF CODE PREVENTS MOUSE SCROLLING TO FLIP THROUGH FLIPVIEW
            var list = new List<Windows.UI.Xaml.Controls.ContentPresenter>();
            FindChildren(list, this);

            foreach (var item in list)
            {
                item.PointerWheelChanged += Item_PointerWheelChanged;
            }
        }

        //private void ButtonShow(FlipView f, string name)
        //{
        //    Windows.UI.Xaml.Controls.Button b;
        //    b = Helper.FindVisualChild<Windows.UI.Xaml.Controls.Button>(f, name);
        //    b.Opacity = 1.0;
        //    b.IsHitTestVisible = true;
        //}

        private void Item_PointerWheelChanged(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            // PREVENT MOUSE SCROLLING TO FLIP THROUGH FLIPVIEW
            e.Handled = true;
        }

        public static void FindChildren<T>(List<T> results, DependencyObject startNode) where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(startNode);
            for (int i = 0; i < count; i++)
            {
                DependencyObject current = VisualTreeHelper.GetChild(startNode, i);
                if ((current.GetType()).Equals(typeof(T)) || (current.GetType().GetTypeInfo().IsSubclassOf(typeof(T))))
                {
                    T asType = (T)current;
                    results.Add(asType);
                }
                FindChildren<T>(results, current);
            }
        }
    }
}