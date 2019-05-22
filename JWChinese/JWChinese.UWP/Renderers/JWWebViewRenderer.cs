using System;
using System.Linq;
using JWChinese.UWP.Renderers;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Popups;
using AnnotatorRuntimeComponent;
using Windows.UI.Xaml;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.System.Profile;
using Windows.UI.Xaml.Input;
using Windows.Devices.Input;
using JWChinese;
using JWChinese.Helpers;
using System.Collections.ObjectModel;

[assembly: ExportRenderer(typeof(JWWebView), typeof(JWWebViewRenderer))]
namespace JWChinese.UWP.Renderers
{
    public class JWWebViewRenderer : WebViewRenderer
    {
        private HtmlCommunicator comm = new HtmlCommunicator();
        private BusyIndicator busyIndicator;
        uint numActiveContacts;
        JWWebView view;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            if (Control != null && e.NewElement != null)
            {
                view = e.NewElement as JWWebView;

                SetupControlSettings();
            }
        }

        private void SetupControlSettings()
        {
            Control.Settings.IsJavaScriptEnabled = true;

            Control.NavigationStarting += WebView_NavigationStarting;
            Control.NavigationCompleted += WebView_NavigationCompleted;
            Control.ScriptNotify += WebView_ScriptNotify;


            Control.PointerPressed += new PointerEventHandler(Control_PointerPressed);
            Control.PointerReleased += new PointerEventHandler(Control_PointerReleased);

            // reset pointer
            Control.PointerExited += new PointerEventHandler(Control_PointerExited);
            Control.PointerCanceled += new PointerEventHandler(Control_PointerExited);
            Control.PointerCaptureLost += new PointerEventHandler(Control_PointerExited);
        }

        public void ExpandParadigm()
        {
            ArticleView a = (ArticleView)view.Parent.Parent;
            a.ExpandParadigm(view.JWId);
        }

        private void Control_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (numActiveContacts == 2)
            {
                ExpandParadigm();
                numActiveContacts = 0;
            }

            e.Handled = true;
        }

        private void Control_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            numActiveContacts = 0;

            e.Handled = true;
        }

        private void Control_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Pointer ptr = e.Pointer;

            if (ptr.PointerDeviceType == PointerDeviceType.Mouse)
            {
                Windows.UI.Input.PointerPoint ptrPt = e.GetCurrentPoint(this);
                if (ptrPt.Properties.IsLeftButtonPressed)
                {
                }
                if (ptrPt.Properties.IsMiddleButtonPressed)
                {
                }
                if (ptrPt.Properties.IsRightButtonPressed)
                {
                    // Right Mouse click
                    numActiveContacts = 2;
                }
            }
            else if (ptr.PointerDeviceType == PointerDeviceType.Touch)
            {
                numActiveContacts++;
            }

            e.Handled = true;
        }

        private void WebView_NavigationStarting(Windows.UI.Xaml.Controls.WebView sender, WebViewNavigationStartingEventArgs args)
        {
            //Debug.Write("args.Uri => " + args.Uri);

            sender.AddWebAllowedObject("JWChinese", comm);

            try
            {
                // if loading pages from database, then uri will be null
                // if pinyin view
                if(args.Uri == null && view.IsChinese)
                {
                    busyIndicator = BusyIndicator.Start(" ");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("busyIndicator ERROR!! => " + ex.Message);
            }
        }

        private async void WebView_NavigationCompleted(Windows.UI.Xaml.Controls.WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            try
            {
                //Debug.Write("args.Uri => " + args.Uri);
                if (args.Uri == null && view.IsChinese)
                {
                    JWChinese.App.CurrentArticleWords = new ObservableCollection<Word>();

                    await sender.InvokeScriptAsync("eval", new[] { "annotScan()" });

                    var words = await StorehouseService.Instance.GetWordsAsync();

                    string english = string.Join(",", words.Select(w => w.English).ToArray());
                    string pinyin = string.Join(",", words.Select(w => w.Pinyin).ToArray());

                    await sender.InvokeScriptAsync("injectWords", new string[] { english, pinyin });
                }
            }
            catch (Exception ex) { Debug.Write("OOPS! CHECK YOUR JAVASCRIPT => " + ex.Message); }
        }

        private async void WebView_ScriptNotify(object sender, NotifyEventArgs e)
        {
            string text = e.Value;

            if (text == "EXPAND")
            {
                ExpandParadigm();
            }

            if(text.Contains("DICTIONARY"))
            {
                string p = text.Split(':').Last();

                Word word = new Word() { Pinyin = p };
                JWChinese.App.CurrentArticleWords.Add(word);

                Debug.WriteLine(JWChinese.App.CurrentArticleWords.Count());
            }

            if (text.Contains("COMPLETE"))
            {
                if (view.IsChinese)
                {
                    try
                    {
                        busyIndicator.Close();
                    }
                    catch (Exception ex)
                    {
                        JWChinese.App.GemWriteLine("COMPLETE busyIndicator ERROR!!", ex.Message);
                    }
                }

                // Prints the entire page html after all javascript is loaded and executed
                //await ((Windows.UI.Xaml.Controls.WebView)sender).InvokeScriptAsync("eval", new[] { "getHtml()" });
            }

            if (text.Contains("ANNOTATION"))
            {
                string english = text.Split('|')[3];
                string chinese = text.Split('|')[1];
                string pinyin = text.Split('|')[2];

                Word word = new Word()
                {
                    Chinese = chinese,
                    Pinyin = pinyin,
                    English = english
                };

                var words = await StorehouseService.Instance.GetWordsAsync();
                string action = (words.Any(w => w.Pinyin == word.Pinyin && w.Chinese == word.Chinese)) ? "Edit" : "Add";

                MessageDialog dialog = new MessageDialog("‌• " + word.English.Replace("/", "\n‌• "), word.Pinyin + " \n" + word.Chinese);
                dialog.Commands.Clear();
                dialog.Commands.Add(new UICommand { Label = action, Id = 0 });
                if (action == "Edit")
                {
                    // STUPID HACK BECAUSE UWP PHONE DOESNT SUPPORT MORE THAN 2 BUTTONS
                    var deviceFamily = AnalyticsInfo.VersionInfo.DeviceFamily;
                    if (deviceFamily.Contains("Desktop"))
                    {
                        dialog.Commands.Add(new UICommand { Label = "Delete", Id = 1 });
                    }
                }
                dialog.Commands.Add(new UICommand { Label = "Cancel", Id = 2 });

                var result = await dialog.ShowAsync();
                if ((int)result.Id == 0)
                {
                    if (action == "Add")
                    {
                        TextBox def = new TextBox() { Text = word.English };
                        //def.SelectAll();
                        ContentDialog d = new ContentDialog
                        {
                            Title = word.Chinese + " \n" + word.Pinyin,
                            Content = def,
                            PrimaryButtonText = "OK",
                            SecondaryButtonText = "Cancel"
                        };

                        ContentDialogResult r = await d.ShowAsync();
                        if (r == ContentDialogResult.Primary)
                        {
                            word.English = def.Text;
                            await StorehouseService.Instance.AddWordAsync(word);
                            await ((Windows.UI.Xaml.Controls.WebView)sender).InvokeScriptAsync("englishToggle", new string[] { word.English });
                        }
                    }
                    else if (action == "Edit")
                    {
                        var deviceFamily = AnalyticsInfo.VersionInfo.DeviceFamily;

                        TextBox def = new TextBox() { Text = word.English };
                        //def.SelectAll();
                        ContentDialog d = new ContentDialog
                        {
                            Title = word.Chinese + " \n" + word.Pinyin,
                            Content = def,
                            PrimaryButtonText = "OK",
                            SecondaryButtonText = deviceFamily.Contains("Desktop") ? "Cancel" : "Delete"
                        };

                        ContentDialogResult r = await d.ShowAsync();
                        if (r == ContentDialogResult.Primary)
                        {
                            // Delete and hide old English word
                            word = words.Where(w => w.Pinyin == word.Pinyin && w.Chinese == word.Chinese).SingleOrDefault();
                            await StorehouseService.Instance.DeleteWordAsync(word);
                            await ((Windows.UI.Xaml.Controls.WebView)sender).InvokeScriptAsync("englishToggle", new string[] { word.English });

                            // Show and save new edited word
                            await StorehouseService.Instance.AddWordAsync(new Word { Chinese = word.Chinese, English = def.Text, Pinyin = word.Pinyin });
                            await ((Windows.UI.Xaml.Controls.WebView)sender).InvokeScriptAsync("englishToggle", new string[] { def.Text });
                        }

                        if (r == ContentDialogResult.Secondary)
                        {
                            if (!deviceFamily.Contains("Desktop"))
                            {
                                word = words.Where(w => w.Pinyin == word.Pinyin && w.Chinese == word.Chinese).SingleOrDefault();
                                await StorehouseService.Instance.DeleteWordAsync(word);
                                await ((Windows.UI.Xaml.Controls.WebView)sender).InvokeScriptAsync("englishToggle", new string[] { word.English });
                            }
                        }
                    }
                }
                else if ((int)result.Id == 1)
                {
                    word = words.Where(w => w.Pinyin == word.Pinyin && w.Chinese == word.Chinese).SingleOrDefault();
                    await StorehouseService.Instance.DeleteWordAsync(word);
                    await ((Windows.UI.Xaml.Controls.WebView)sender).InvokeScriptAsync("englishToggle", new string[] { word.English });
                }
            }
        }
    }
}