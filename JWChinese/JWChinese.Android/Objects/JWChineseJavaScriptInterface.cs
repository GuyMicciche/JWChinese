using System;
using Java.Interop;
using Android.Content;
using Android.Runtime;
using Android.Webkit;
using Android.App;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Android.Widget;
using System.Diagnostics;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;

namespace JWChinese.Droid
{
    public class JWChineseJavaScriptInterface : Java.Lang.Object
    {
        private Context context;
        private WebView webview;
        Dialog dialog;

        //private IEnumerable<Word> words;

        public JWChineseJavaScriptInterface(Context context, WebView webview)
        {
            this.context = context;
            this.webview = webview;

            // THIS WAY 
            //dialog = new ProgressDialog(context, Resource.Style.MyGravity);
            //dialog.Window.SetBackgroundDrawableResource(global::Android.Resource.Color.Transparent);
            ////dialog.Window.AddFlags(global::Android.Views.WindowManagerFlags.DimBehind);
            ////dialog.Window.SetDimAmount(0.4f);
            ////dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.ParseColor("#66ffffff")));\
            //dialog.SetCancelable(false);

            // OR THIS WAY
            dialog = new Dialog(context, global::Android.Resource.Style.ThemeBlack);
            View view = LayoutInflater.From(context).Inflate(Resource.Layout.ProgressDialog, null);
            dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
            dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.ParseColor("#66ffffff")));
            dialog.SetContentView(view);

            try
            {
                //App.GemWriteLine(webview.Url);
                //App.GemWriteLine((int)webview.Tag);

                // if loading pages from database, then uri will be null
                // if pinyin view
                if ((webview.Url == "about:blank" || webview.Url == null) && (bool)webview.Tag)
                {
                    dialog.Show();
                }
            }
            catch (Exception ex)
            {
                App.GemWriteLine("ProgressDialog.Show() ERROR!!", ex.Message);
            }
        }

        public JWChineseJavaScriptInterface(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {

        }

        [JavascriptInterface]
        [Export("annotate")]
        public string annotate(Java.Lang.String text)
        {
            string result = new Annotator((string)text).Result();
            result = result.Replace("<ruby title=\"", "<ruby onclick=\"annotPopAll(this)\" title=\"");

            return result;
        }

        [JavascriptInterface]
        [Export("notify")]
        public async void notify(Java.Lang.String result)
        {
            string text = (string)result;

            if (text.Contains("DICTIONARY"))
            {
                string p = text.Split(':').Last();

                Word word = new Word() { Pinyin = p };
                JWChinese.App.CurrentArticleWords.Add(word);

                Debug.WriteLine(JWChinese.App.CurrentArticleWords.Count());
            }

            if (text.Contains("COMPLETE"))
            {
                if((bool)webview.Tag)
                {
                    try
                    {
                        dialog.Dismiss();
                    }
                    catch (Exception ex)
                    {
                        App.GemWriteLine("ProgressDialog.Dismiss() ERROR!!", ex.Message);
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
                string action = (words.Any(w => w.Pinyin == word.Pinyin && w.Chinese == word.Chinese)) ? "Delete" : "Add";

                AlertDialog.Builder alert = new AlertDialog.Builder(context);
                //alert.SetTitle(word.Pinyin);
                alert.SetMessage(word.Pinyin + "\n" + word.Chinese + " \n\n" + "‌• " + word.English.Replace("/", "\n‌• "));
                alert.SetPositiveButton(action, async (sender, args) =>
                {
                    if(action == "Add")
                    {
                        AlertDialog.Builder e = new AlertDialog.Builder(context); 

                        LinearLayout layout = new LinearLayout(context);
                        LinearLayout.LayoutParams parms = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                        layout.Orientation = Orientation.Vertical;
                        layout.LayoutParameters = parms;
                        layout.SetPadding(16, 16, 16, 16);

                        EditText et = new EditText(context);
                        et.Text = word.English;
                        //et.SelectAll(); //Select all text
                        layout.AddView(et, new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent));

                        // HIDE PINYIN CHECKBOX
                        CheckBox ch = new CheckBox(context) { Text = "Hide Pinyin", Checked = IsPinyinHidden(word.English) };
                        layout.AddView(ch, new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent) { Gravity = GravityFlags.Right });

                        e.SetTitle(word.Pinyin + " " + word.Chinese);
                        e.SetView(layout);
                        e.SetPositiveButton("OK", async (s, a) =>
                        {
                            word.English = ToggleShowPinyin(et.Text, ch.Checked); // IF CHECKED, HIDE PINYIN
                            await StorehouseService.Instance.AddWordAsync(word);
                            Debug.WriteLine(word.English);

                            string js = "javascript:englishToggle('" + word.English + "');";
                            webview.LoadUrl(js);
                        });
                        e.SetNegativeButton("Cancel", (s, a) =>
                        {

                        });

                        ((Activity)context).RunOnUiThread(() =>
                        {
                            Dialog d = e.Create();
                            d.Show();
                        });
                    }
                    else if(action == "Delete")
                    {
                        word = words.Where(w => w.Pinyin == word.Pinyin && w.Chinese == word.Chinese).SingleOrDefault();
                        await StorehouseService.Instance.DeleteWordAsync(word);

                        string js = "javascript:englishToggle('" + word.English + "');";
                        webview.LoadUrl(js);
                    }
                });
                if (action == "Delete")
                {
                    alert.SetNeutralButton("Edit", (sender, args) =>
                    {
                        AlertDialog.Builder e = new AlertDialog.Builder(context);

                        LinearLayout layout = new LinearLayout(context);
                        LinearLayout.LayoutParams parms = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                        layout.Orientation = Orientation.Vertical;
                        layout.LayoutParameters = parms;
                        layout.SetPadding(16, 16, 16, 16);

                        EditText et = new EditText(context);
                        et.Text = word.English;
                        //et.SelectAll();
                        layout.AddView(et, new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent));

                        // HIDE PINYIN CHECKBOX
                        CheckBox ch = new CheckBox(context) { Text = "Hide Pinyin", Checked = IsPinyinHidden(word.English) };
                        layout.AddView(ch, new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent) { Gravity = GravityFlags.Right });

                        e.SetTitle(word.Chinese + " " + word.Pinyin);
                        e.SetView(layout);
                        e.SetPositiveButton("OK", async (s, a) =>
                        {
                            // Delete and hide old English word
                            word = words.Where(w => w.Pinyin == word.Pinyin && w.Chinese == word.Chinese).SingleOrDefault();
                            await StorehouseService.Instance.DeleteWordAsync(word);
                            webview.LoadUrl("javascript:englishToggle('" + word.English + "');");

                            word.English = ToggleShowPinyin(et.Text, ch.Checked); // IF CHECKED, HIDE PINYIN

                            // Show and save new edited word
                            await StorehouseService.Instance.AddWordAsync(word);
                            webview.LoadUrl("javascript:englishToggle('" + word.English + "');");
                        });
                        e.SetNegativeButton("Cancel", (s, a) =>
                        {

                        });

                        ((Activity)context).RunOnUiThread(() =>
                        {
                            Dialog d = e.Create();
                            d.Show();
                        });
                    });
                }
                alert.SetNegativeButton("Cancel", (sender, args) =>
                {

                });

                ((Activity)context).RunOnUiThread(() =>
                {
                    Dialog dialog = alert.Create();
                    dialog.Show();
                });

                //Console.WriteLine("CLICKED => " + chinese + " : " + english + " : " + pinyin);
            }
        }

        [JavascriptInterface]
        [Export("writeLine")]
        public void writeLine(string s)
        {
            App.GemWriteLine("JavascriptInterface.test()", s);
        }

        public string ToggleShowPinyin(string text, bool? isChecked = null)
        {
            if (isChecked == null)
            {
                if (text.StartsWith("||"))
                {
                    text = text.Remove(0, 1);
                }
                else
                {
                    text = "||" + text;
                }
            }
            else if(isChecked == false)
            {
                if (text.StartsWith("||"))
                {
                    text = text.Remove(0, 1);
                }
            }
            else if (isChecked == true)
            {
                if (!text.StartsWith("||"))
                {
                    text = "||" + text;
                }
            }

            return text;
        }

        public bool IsPinyinHidden (string text)
        {
            if (text.StartsWith("*"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}