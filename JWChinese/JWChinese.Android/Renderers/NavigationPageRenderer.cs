using System.ComponentModel;
using JWChinese.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using NavigationRenderer = Xamarin.Forms.Platform.Android.AppCompat.NavigationPageRenderer;
using Support = Android.Support.V7.Widget;
using System;
using JWChinese;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationPageRenderer))]

namespace JWChinese.Droid
{
    public class NavigationPageRenderer : NavigationRenderer
    {
        private Support.Toolbar toolbar;
        private string Title { get; set; }
        private string Subtitle { get; set; }

        protected override void OnElementChanged(ElementChangedEventArgs<NavigationPage> e)
        {
            base.OnElementChanged(e);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName.Equals("CurrentPage"))
            {
                if (toolbar != null)
                {
                    var page = Element.CurrentPage;
                    if (page is CustomPage)
                    {
                        CustomPage p = page as CustomPage;
                        toolbar.Title = p.Title;
                        toolbar.Subtitle = p.Subtitle ?? null;
                    }
                    else
                    {
                        toolbar.Title = page.Title;
                        toolbar.Subtitle = null;
                    }
                }
            }
        }

        public override void OnViewAdded(global::Android.Views.View child)
        {
            base.OnViewAdded(child);

            if (child.GetType() == typeof(Support.Toolbar))
            {
                toolbar = (Support.Toolbar)child;
            }
        }
    }
}



//using System.ComponentModel;
//using JWChinese.Droid;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.Android;
//using NavigationRenderer = Xamarin.Forms.Platform.Android.AppCompat.NavigationPageRenderer;
//using Support = Android.Support.V7.Widget;
//using System;
//using JWChinese;

//[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationPageRenderer))]

//namespace JWChinese.Droid
//{
//    public class NavigationPageRenderer : NavigationRenderer
//    {
//        private Support.Toolbar toolbar;
//        private string Title { get; set; }
//        private string Subtitle { get; set; }

//        protected override void OnElementChanged(ElementChangedEventArgs<NavigationPage> e)
//        {
//            base.OnElementChanged(e);

//            Unbind(e.OldElement);
//            Bind(e.NewElement);
//        }

//        private void Unbind(NavigationPage oldElement)
//        {
//            if (oldElement != null)
//            {
//                oldElement.PropertyChanging -= ElementPropertyChanging;
//                oldElement.PropertyChanged -= ElementPropertyChanged;
//            }
//        }

//        private void Bind(NavigationPage newElement)
//        {
//            if (newElement != null)
//            {
//                newElement.PropertyChanging += ElementPropertyChanging;
//                newElement.PropertyChanged += ElementPropertyChanged;
//            }
//        }
//        private void ElementPropertyChanging(object sender, Xamarin.Forms.PropertyChangingEventArgs e)
//        {
//            if (e.PropertyName.Equals("CurrentPage") && Element != null)
//            {
//                Element.CurrentPage.Appearing -= Page_Appearing;
//            }
//        }

//        private void ElementPropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            if (e.PropertyName.Equals("CurrentPage") && Element != null)
//            {
//                if (toolbar != null)
//                {
//                    var page = Element.CurrentPage;
//                    page.Appearing += Page_Appearing;
//                    SetTitle(page);
//                }
//            }
//        }

//        private void Page_Appearing(object sender, EventArgs e)
//        {
//            SetTitle(sender);
//        }

//        private void SetTitle(object page)
//        {
//            try
//            {
//                if (toolbar != null)
//                {
//                    if (page is CustomPage)
//                    {
//                        CustomPage p = page as CustomPage;
//                        if (!string.IsNullOrEmpty(p.Subtitle))
//                        {
//                            toolbar.Subtitle = p.Subtitle;
//                            toolbar.Title = p.Title;
//                        }
//                    }
//                    else
//                    {
//                        toolbar.Title = ((Page)page).Title;
//                        toolbar.Subtitle = null;
//                    }
//                }
//            }
//            catch (Exception ex) { }
//        }

//        public override void OnViewAdded(global::Android.Views.View child)
//        {
//            base.OnViewAdded(child);

//            if (child.GetType() == typeof(Support.Toolbar))
//            {
//                toolbar = (Support.Toolbar)child;
//            }
//        }
//    }
//}














//using System.ComponentModel;
//using JWChinese.Droid;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.Android;
//using NavigationRenderer = Xamarin.Forms.Platform.Android.AppCompat.NavigationPageRenderer;
//using Support = Android.Support.V7.Widget;
//using System;
//using System.Linq;

//[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationPageRenderer))]

//namespace JWChinese.Droid
//{
//    public class NavigationPageRenderer : NavigationRenderer
//    {
//        private Support.Toolbar toolbar;
//        private string Title { get; set; }
//        private string Subtitle { get; set; }

//        protected override void OnElementChanged(ElementChangedEventArgs<NavigationPage> e)
//        {
//            base.OnElementChanged(e);
//            //SetCustomView(e.NewElement.CurrentPage.GetType().Name);

//            if (e.OldElement != null)
//            {
//                if (Element != null)
//                {
//                    // Unsubscribe from event handlers and cleanup any resources
//                    //Element.CurrentPage.Appearing -= CurrentPage_Appearing;
//                    //Element.CurrentPage.PropertyChanged += CurrentPage_PropertyChanged;
//                }
//            }

//            if (e.NewElement != null)
//            {
//                //SetTitleSubtitle(Element.CurrentPage);

//                // Configure the control and subscribe to event handlers
//                //Element.CurrentPage.Appearing += CurrentPage_Appearing;
//                //Element.CurrentPage.PropertyChanged -= CurrentPage_PropertyChanged;
//            }
//        }

//        private void CurrentPage_PropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            Console.WriteLine("NavigationPageRenderer CurrentPage_PropertyChanged => " + e.PropertyName.ToString());
//            if (e.PropertyName.Equals("Renderer") || e.PropertyName.Equals("Title"))
//            {
//                if (Element != null)
//                {
//                    SetTitleSubtitle(Element.CurrentPage);
//                }
//            }
//        }

//        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            Console.WriteLine("NavigationPageRenderer OnElementPropertyChanged => " + e.PropertyName.ToString());

//            base.OnElementPropertyChanged(sender, e);
//            if (e.PropertyName.Equals("CurrentPage"))
//            {
//                toolbar.Subtitle = null;
//                toolbar.Title = "";

//                SetTitleSubtitle(Element.CurrentPage);

//                if (Element != null)
//                {
//                    // All pages except first one
//                    var pages = Element.Navigation.NavigationStack.Skip(1);
//                    foreach (var p in pages)
//                    {
//                        p.Appearing -= CurrentPage_Appearing;
//                        p.Appearing += CurrentPage_Appearing;

//                        p.PropertyChanged -= CurrentPage_PropertyChanged;
//                        p.PropertyChanged += CurrentPage_PropertyChanged;
//                    }
//                }

//                ////SetCustomView(((NavigationPage)sender).CurrentPage.GetType().Name);
//                //SetTitleSubtitle(Element.CurrentPage);


//                //    //Element.CurrentPage.PropertyChanged += CurrentPage_PropertyChanged;
//                //    Element.CurrentPage.Appearing += CurrentPage_Appearing;
//                //    Element.CurrentPage.Disappearing += CurrentPage_Disappearing;
//                //}
//            }
//        }

//        private void SetTitleSubtitle(Page page)
//        {
//            try
//            {
//                if (toolbar != null)
//                {
//                    Title = page.Title;
//                    Subtitle = null;

//                    // Define the variables heres
//                    string[] title = page.Title.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
//                    if (title.Length > 0)
//                    {
//                        Title = title[0]?.Trim() ?? page.Title;
//                        if (title[0] != title[1])
//                        {
//                            Subtitle = title[1]?.Trim() ?? Subtitle;
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//            }
//        }

//        private void CurrentPage_Appearing(object sender, EventArgs e)
//        {
//            try
//            {
//                if (toolbar != null)
//                {
//                    // Acually set the titles here
//                    toolbar.Title = Title;
//                    toolbar.Subtitle = Subtitle;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//            }
//        }

//        public override void OnViewAdded(global::Android.Views.View child)
//        {
//            base.OnViewAdded(child);

//            if (child.GetType() == typeof(Support.Toolbar))
//            {
//                toolbar = (Support.Toolbar)child;
//            }
//        }
//    }
//}