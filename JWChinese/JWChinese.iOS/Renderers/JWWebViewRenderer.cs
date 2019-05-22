using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using JWChinese;
using JWChinese.iOS;
using Xamarin.Forms.Platform.iOS;
using WebKit;

//[assembly: ExportRenderer(typeof(JWWebView), typeof(JWWebViewRenderer))]
//namespace JWChinese.iOS
//{
//    public class JWWebViewRenderer : ViewRenderer<JWWebView, WKWebView>, IWKScriptMessageHandler
//    {
//        JWWebView view;

//        protected override void OnElementChanged(ElementChangedEventArgs<JWWebView> e)
//        {
//            base.OnElementChanged(e);

//            if (Control == null && e.NewElement != null)
//            {
//                view = e.NewElement as JWWebView;
//                //Delegate = new JWWebViewDelegate(view);

//                Init();
//            }

//            if (e.NewElement == null && Control != null)
//            {
//                //Control.RemoveGestureRecognizer(_leftSwipeGestureRecognizer);
//                //Control.RemoveGestureRecognizer(_rightSwipeGestureRecognizer);
//            }

//            //Unbind(e.OldElement);
//            //Bind();
//        }

//        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
//        {
//            Console.WriteLine("message.Body.ToString() => " + message.Body.ToString());
//        }
//    }
//}

[assembly: ExportRenderer(typeof(JWWebView), typeof(JWWebViewRenderer))]
namespace JWChinese.iOS
{
    public class JWWebViewRenderer : WebViewRenderer
    {
        JWWebView view;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                view = e.NewElement as JWWebView;
                Delegate = new JWWebViewDelegate(ViewController, view);

                Initialize();
            }
        }

        private void Initialize()
        {
            //AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
            //ScalesPageToFit = true;
        }
    }
}