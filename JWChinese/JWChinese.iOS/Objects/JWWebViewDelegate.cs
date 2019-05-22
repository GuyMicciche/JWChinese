using CoreText;
using Foundation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;

namespace JWChinese.iOS
{
    public class JWWebViewDelegate : UIWebViewDelegate
    {
        JWWebView jwWebView;
        ArticleView articleView;
        UIViewController controller;
        public event EventHandler<WebNavigatedEventArgs> Navigated;
        WebNavigationEvent lastEvent;

        public JWWebViewDelegate(UIViewController controller, JWWebView jwWebView)
        {
            this.jwWebView = jwWebView;
            this.controller = controller;

            articleView = (ArticleView)jwWebView.Parent.Parent;
            Navigated += JWWebViewDelegate_Navigated;
        }

        public void ExpandParadigm()
        {
            articleView.ExpandParadigm(jwWebView.JWId);
        }

        private void InjectJS(UIWebView webView, string js)
        {
            //InvokeOnMainThread(() => webView.EvaluateJavascript(new NSString(js)));
            webView.EvaluateJavascript(new NSString(js));
        }

        private void JWWebViewDelegate_Navigated(object sender, WebNavigatedEventArgs e)
        {
            //Console.WriteLine("[Forms WebView] {0}", e.Url.ToString());
        }

        public string Annotate(string text)
        {
            string result = new Annotator(text).Result();
            result = result.Replace("<ruby title=\"", "<ruby onclick=\"annotPopAll(this)\" title=\"");

            return result;
        }

        public override bool ShouldStartLoad(UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
        {
            Debug.WriteLine("ShouldStartLoad");

            string text = request.ToString();

            if (text.Contains("http://"))
            {
                if (UIApplication.SharedApplication.CanOpenUrl(request.Url))
                {
                    UIApplication.SharedApplication.OpenUrl(request.Url);
                }
                return false;
            }

            //Console.WriteLine("[Custom Delegate] Url: {0}", request.Url);
            //Console.WriteLine("[Custom Delegate] MainDocUrl: {0}", request.MainDocumentURL);

            if(text.Contains("COMPLETE"))
            {
                Console.WriteLine("COMPLETE!");

                return false;
            }
            else if(text.Contains("EXPAND"))
            {
                ExpandParadigm();

                return false;
            }
            else if (text.Contains("PRINT"))
            {
                Console.WriteLine(text);

                return false;
            }
            else if (text.Contains("ANNOTATION"))
            {
                text = WebUtility.UrlDecode(text);

                string english = text.Split('|')[3];
                string chinese = text.Split('|')[1];
                string pinyin = text.Split('|')[2];

                Console.WriteLine("english => " + english);
                Console.WriteLine("chinese => " + chinese);
                Console.WriteLine("pinyin => " + pinyin);

                Word word = new Word()
                {
                    Chinese = chinese,
                    Pinyin = pinyin,
                    English = english
                };
                DisplayDialogParadigm(webView, word);

                return false;
            }
            else if (text.Contains("GET_WORD"))
            {
                string word = text.Split(new[] { "word=" }, StringSplitOptions.RemoveEmptyEntries).Last();
                string anno = Annotate(word);

                return false;
            }

            WebNavigationEvent navEvent = WebNavigationEvent.NewPage;
            switch (navigationType)
            {
                case UIWebViewNavigationType.LinkClicked:
                    navEvent = WebNavigationEvent.NewPage;
                    break;
                case UIWebViewNavigationType.FormSubmitted:
                    navEvent = WebNavigationEvent.NewPage;
                    break;
                case UIWebViewNavigationType.Reload:
                    navEvent = WebNavigationEvent.Refresh;
                    break;
                case UIWebViewNavigationType.FormResubmitted:
                    navEvent = WebNavigationEvent.NewPage;
                    break;
                case UIWebViewNavigationType.Other:
                    navEvent = WebNavigationEvent.NewPage;
                    break;
            }

            lastEvent = navEvent;
            
            return true;
        }

        private async void DisplayDialogParadigm(UIWebView webView, Word word)
        {
            var controller = UIApplication.SharedApplication.KeyWindow.RootViewController;

            var words = await StorehouseService.Instance.GetWordsAsync();
            string action = (words.Any(w => w.Pinyin == word.Pinyin && w.Chinese == word.Chinese)) ? "Edit" : "Add";

            NSMutableAttributedString eng = new NSMutableAttributedString("‌• " + word.English.Replace("/", "\n‌• "), new UIStringAttributes
            {
                StrikethroughStyle = NSUnderlineStyle.Single,
                ParagraphStyle = new NSMutableParagraphStyle() { Alignment = UITextAlignment.Left }
            });

            UIAlertController alert = UIAlertController.Create(word.Pinyin + " \n" + word.Chinese, "‌• " + word.English.Replace("/", "\n‌• "), UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create(action, UIAlertActionStyle.Default, okAction =>
            {
                if(action == "Add")
                {
                    var alertController = UIAlertController.Create(null, word.Chinese + " \n" + word.Pinyin, UIAlertControllerStyle.Alert);

                    UITextField alertTextField = null;
                    alertController.AddTextField((textField) => 
                    {
                        //textField.Placeholder = word.English;
                        textField.EditingDidBegin += delegate {
                            BeginInvokeOnMainThread(delegate {
                                // Select the text
                                //textField.SelectAll(this);
                            });
                        };
                        textField.Text = word.English;
                        alertTextField = textField;
                    });

                    alertController.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, async ok =>
                    {
                        word.English = alertTextField.Text;
                        await StorehouseService.Instance.AddWordAsync(word);
                        InjectJS(webView, "englishToggle('" + word.English + "')");
                    }));

                    alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Default, cancelAction =>
                    {
                        // Do absolutely nothing
                    }));

                    controller?.PresentViewController(alertController, true, null);
                }
                else if(action == "Edit")
                {
                    var alertController = UIAlertController.Create(null, word.Chinese + " \n" + word.Pinyin, UIAlertControllerStyle.Alert);

                    UITextField alertTextField = null;
                    alertController.AddTextField((textField) => 
                    {
                        //textField.Placeholder = word.English;
                        textField.EditingDidBegin += delegate {
                            BeginInvokeOnMainThread(delegate {
                                //textField.SelectAll(this);
                            });
                        };
                        textField.Text = word.English;
                        alertTextField = textField;
                    });

                    alertController.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, async ok =>
                    {
                        word = words.Where(w => w.Pinyin == word.Pinyin && w.Chinese == word.Chinese).SingleOrDefault();
                        await StorehouseService.Instance.DeleteWordAsync(word);
                        InjectJS(webView, "englishToggle('" + word.English + "')");

                        // Show and save new edited word
                        await StorehouseService.Instance.AddWordAsync(new Word { Chinese = word.Chinese, English = alertTextField.Text, Pinyin = word.Pinyin });
                        InjectJS(webView, "englishToggle('" + alertTextField.Text + "')");

                    }));

                    alertController.AddAction(UIAlertAction.Create("Delete", UIAlertActionStyle.Default, async cancelAction => 
                    {
                        word = words.Where(w => w.Pinyin == word.Pinyin && w.Chinese == word.Chinese).SingleOrDefault();
                        await StorehouseService.Instance.DeleteWordAsync(word);
                        InjectJS(webView, "englishToggle('" + word.English + "')");
                    }));

                    controller?.PresentViewController(alertController, true, null);
                }
            }));
            alert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Default, cancelAction =>
            {
                // Do absolutely nothing
            }));

            controller?.PresentViewController(alert, true, null);
        }

        public override void LoadFailed(UIWebView webView, NSError error)
        {
            Debug.WriteLine("LoadFailed => " + error.ToString());
        }


        public override async void LoadingFinished(UIWebView webView)
        {
            try
            {
                Debug.WriteLine("LoadingFinished");

                // Sometimes pinyin doesn't work, so delay 500ms before starting
                await Task.Delay(500);

                InjectJS(webView, "annotScan()");

                var words = await StorehouseService.Instance.GetWordsAsync();

                string english = string.Join(",", words.Select(w => w.English).ToArray());
                string pinyin = string.Join(",", words.Select(w => w.Pinyin).ToArray());

                InjectJS(webView, string.Format("injectWords('{0}', '{1}')", english, pinyin));

                Debug.WriteLine("ENGLISH => " + english);
                Debug.WriteLine("PINYIN => " + pinyin);

                var url = webView.Request.Url.AbsoluteUrl.ToString();
                var args = new WebNavigatedEventArgs(lastEvent, jwWebView.Source, url, WebNavigationResult.Success);

                Navigated(jwWebView, args);
            }
            catch(Exception e)
            {
                Debug.WriteLine("LoadingFinished Excption => " + e.Message);
            }
        }
        
        public override void LoadStarted(UIWebView webView)
        {
            //Debug.WriteLine("LoadStarted");
        }
    }
}
