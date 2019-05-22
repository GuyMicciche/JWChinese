using Android.Content;
using Android.Webkit;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace JWChinese.Droid
{
    /// <summary>
    /// Custom JW Chinese WebViewClient
    /// </summary>
    public class JWChineseWebViewClient : WebViewClient
    {
        public JWChineseWebViewClient()
        {

        }

        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            App.GemWriteLine("JWChineseWebViewClient.ShouldOverrideUrlLoading", url);

            if (url.Contains("wol.jw.org"))
            {
                view.Context.StartActivity(new Intent(Intent.ActionView, global::Android.Net.Uri.Parse(url)));

                return true;
            }

            return base.ShouldOverrideUrlLoading(view, url);
        }

        public override void OnLoadResource(WebView view, string url)
        {
            // DO NOT LOAD ANYTHING
        }

        public override void OnReceivedError(WebView view, IWebResourceRequest request, WebResourceError error)
        {
            base.OnReceivedError(view, request, error);

            App.GemWriteLine("JWChineseWebViewClient.OnReceivedError", error.Description);
        }

        public async override void OnPageFinished(WebView view, string url)
        {
            base.OnPageFinished(view, url);

            JWChinese.App.CurrentArticleWords = new ObservableCollection<Word>();

            App.GemWriteLine(view.Tag + " => PAGE FINNISHED! SHOULD START ANNOTATION!!");

            RunJs(view, "annotScan()");

            var words = await StorehouseService.Instance.GetWordsAsync();

            string english = string.Join(",", words.Select(w => w.English).ToArray());
            string pinyin = string.Join(",", words.Select(w => w.Pinyin).ToArray());

            RunJs(view, "injectWords('" + english + "', '" + pinyin + "')");
        }

        public void RunJs(WebView webview, string js)
        {
            try
            {
                webview.LoadUrl("javascript:" + js);
            }
            catch(Exception e)
            {
                App.GemWriteLine("RunJs ERROR", e.Message);
            }
        }
    }
}