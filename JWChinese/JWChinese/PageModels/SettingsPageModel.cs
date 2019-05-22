using FreshMvvm;
using JWChinese.Helpers;
using PropertyChanged;
using System;
using Xamarin.Forms;

namespace JWChinese
{
    [AddINotifyPropertyChangedInterface]
    public class SettingsPageModel : FreshBasePageModel
    {
        public double TextSettings { get; set; }
        public bool ReferenceSymbols { get; set; }
        public string TextSize { get; set; }
        public string Title { get; set; }

        public override void Init(object initData)
        {
            UpdateControls();
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            UpdateControls();
        }

        private void UpdateControls()
        {
            var size = Settings.TextSettings;

            TextSettings = (size - App.TextSettingsBase) / 100;
            ReferenceSymbols = Settings.ReferenceSymbols;
            TextSize = "Text Size: " + size.ToString() + "%";

            Title = App.GetLanguageValue("Settings", "设置");
        }

        public Command<string> JWLink
        {
            get
            {
                return new Command<string>((id) =>
                {
                    string url = "http://jw.org";

                    switch (int.Parse(id))
                    {
                        case -1:
                            return;
                        case 0:
                            url = "https://wol.jw.org/en/wol/binav/r1/lp-e";
                            break;
                        case 1:
                            url = "https://jw.org";
                            break;
                        case 2:
                            url = "https://wol.jw.org";
                            break;
                        case 3:
                            url = "https://tv.jw.org";
                            break;
                    }

                    Device.OpenUri(new Uri(url));
                });
            }
        }

        public Command UpdateDatabase
        {
            get
            {
                return new Command(() =>
                {
                    ((App)Application.Current).UpdateDatabase();
                });
            }
        }
    }
}