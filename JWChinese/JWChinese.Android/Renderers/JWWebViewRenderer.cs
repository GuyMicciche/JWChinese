using Android.Views;
using Android.Webkit;
using JWChinese;
using JWChinese.Droid;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;
using Android.OS;

[assembly: ExportRenderer(typeof(JWWebView), typeof(JWWebViewRenderer))]
namespace JWChinese.Droid
{
    public class JWWebViewRenderer : WebViewRenderer
    {
        private readonly JWGestureListener _listener;
        private readonly GestureDetector _detector;
        JWWebView view;
        int defaultFontSize = 24;

        public JWWebViewRenderer()
        {
            _listener = new JWGestureListener();
            _detector = new GestureDetector(_listener);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            // Basic settings
            if (Control != null && e.NewElement != null)
            {
                view = e.NewElement as JWWebView;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            Console.WriteLine("OnElementPropertyChanged => " + e.PropertyName);

            if (e.PropertyName == JWWebView.FontSizeProperty.PropertyName)
            {
                if (Element != null)
                {
                    Control.Settings.DefaultFontSize = ((JWWebView)sender).FontSize;
                    defaultFontSize = ((JWWebView)sender).FontSize;
                }
            }

            if (e.PropertyName == JWWebView.IsChineseProperty.PropertyName)
            {
                Console.WriteLine("JWWebView => " + ((JWWebView)sender).IsChinese);

                if (Element != null)
                {
                    view.IsChinese = ((JWWebView)sender).IsChinese;

                    SetupControlSettings();
                }
            }
        }

        public override bool DispatchTouchEvent(MotionEvent e)
        {
            switch (e.Action & e.ActionMasked)
            {
                // Two-finger up
                case MotionEventActions.Pointer1Up:
                    //Console.WriteLine("WebViewRender.HandleTouch() => Two-finger up");
                    //Console.WriteLine(view.Parent.Parent.Parent); // ArticleView
                    //((ArticleView)view.Parent.Parent.Parent).TestFunction();  // ArticleView.TestFunction()
                    //Console.WriteLine("StackLayout Uid => " + view.JWId);
                    ExpandParadigm();
                    break;
            }

            _detector.OnTouchEvent(e);

            return base.DispatchTouchEvent(e);
        }

        public void ExpandParadigm()
        {
            ArticleView a = (ArticleView)view.Parent.Parent;
            a.ExpandParadigm(view.JWId);
        }

        private void SetupControlSettings()
        {
            JWChineseWebViewClient client = new JWChineseWebViewClient();

            Control.Settings.JavaScriptEnabled = true;
            Control.Settings.DomStorageEnabled = true;
            Control.Settings.BuiltInZoomControls = false;
            Control.VerticalScrollBarEnabled = true;
            Control.Settings.SetRenderPriority(WebSettings.RenderPriority.High);
            Control.Settings.CacheMode = CacheModes.NoCache;
            Control.Tag = view.IsChinese;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            {
                Control.SetLayerType(LayerType.Hardware, null);
            }
            else
            {
                Control.SetLayerType(LayerType.Software, null);
            }

            Control.SetWebChromeClient(new WebChromeClient());
            Control.SetWebViewClient(client);

            Control.AddJavascriptInterface(new JWChineseJavaScriptInterface(Context, Control), "JWChinese");

            App.GemWriteLine(Control.Settings.DefaultFontSize);
        }
    }
}