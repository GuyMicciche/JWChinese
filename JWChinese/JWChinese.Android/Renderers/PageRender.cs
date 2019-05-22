//using System;
//using Android.Animation;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.Android;
//using Android.Support.V7.App;
//using Android.Views;
//using Android.Widget;
//using Toolbar = Android.Support.V7.Widget.Toolbar;
//using JWChinese.Droid;
//using FreshMvvm;
//using System.Linq;
//using Android.App;

//[assembly: ExportRenderer(typeof(Page), typeof(PageRender))]
//namespace JWChinese.Droid
//{
//    public class PageRender : PageRenderer
//    {
//        global::Android.Views.View customActionBarView;

//        AppCompatActivity GetActivity
//        {
//            get
//            {
//                try
//                {
//                    return (AppCompatActivity)Context;
//                }
//                catch
//                {
//                    return null;
//                }
//            }
//        }

//        Toolbar GetToolbar
//        {
//            get
//            {
//                var context = GetActivity;
//                var toolbar = context.FindViewById<Toolbar>(Resource.Id.toolbar);

//                return toolbar;
//            }
//        }

//        public override void OnViewAdded(Android.Views.View child)
//        {
//            base.OnViewAdded(child);
//        }

//        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
//        {
//            base.OnElementChanged(e);

//            var page = Element as Page;
//            var viewModel = page?.BindingContext as FreshBasePageModel;

//            if (GetToolbar != null)
//            {
//                if (page.Title.Contains(" - "))
//                {
//                    string[] title = page.Title.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);

//                    GetToolbar.Title = title[0];
//                    GetToolbar.Subtitle = title[1];
//                }

//                page.Appearing += (object sender, EventArgs ev3) =>
//                {
//                    if (GetToolbar != null)
//                    {
//                        if (page.Title.Contains(" - "))
//                        {
//                            string[] title = viewModel.CurrentPage.Title.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);

//                            GetToolbar.Title = title[0];
//                            GetToolbar.Subtitle = title[1];
//                        }
//                    }
//                };

//                page.Disappearing += (object sender, EventArgs ev3) =>
//                {
//                    if (GetToolbar != null)
//                    {
//                        GetToolbar.Subtitle = null;
//                    }
//                };
//            }
//        }
//    }
//}