using JWChinese.Helpers;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JWChinese
{
    [AddINotifyPropertyChangedInterface]
    public partial class SettingsPage : ContentPage
    {
        private double text_settings = Settings.TextSettings;
        private bool reference_symbols = Settings.ReferenceSymbols;
        private bool primary_language_english = (Settings.PrimaryLanguage == LPLanguage.English.GetName());

        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            text_settings = Settings.TextSettings;
            reference_symbols = Settings.ReferenceSymbols;
            primary_language_english = (Settings.PrimaryLanguage == LPLanguage.English.GetName());


            ReferenceSymbolsSwitch.On = reference_symbols;
            PrimaryEnglishSwitch.On = primary_language_english;
            TextSettingsSlider.Value = (text_settings - App.TextSettingsBase) / 100;

            UpdateUI();

            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            bool refresh = false;

            if (ReferenceSymbolsSwitch.On != reference_symbols)
            {
                refresh = true;
                Settings.ReferenceSymbols = ReferenceSymbolsSwitch.On;
            }

            if (PrimaryEnglishSwitch.On != primary_language_english)
            {
                refresh = true;
                Settings.PrimaryLanguage = PrimaryEnglishSwitch.On ? LPLanguage.English.GetName() : LPLanguage.Chinese.GetName();
            }

            if (App.GetTextSettings(TextSettingsSlider.Value) != text_settings)
            {
                refresh = true;
                Settings.TextSettings = TextSettingsSlider.Value;
            }


            if (refresh)
            {
                MessagingCenter.Send(this, "WebViewRefresh");
            }

            base.OnDisappearing();
        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            //Settings.TextSettings = e.NewValue;

            //textSize.Text = "Text Size: " + Settings.TextSettings.ToString() + "%";

            //MessagingCenter.Send(this, "WebViewRefresh");
        }

        private async void SwitchCell_OnChanged(object sender, ToggledEventArgs e)
        {
            Settings.PrimaryLanguage = e.Value ? LPLanguage.English.GetName() : LPLanguage.Chinese.GetName();

            if (e.Value != primary_language_english)
            {
                await DisplayAlert("Alert!", "To see the changes, please restart the app.", "OK");
            }

            UpdateUI();

            App.GemWriteLine(e.Value);

            //Settings.ReferenceSymbols = e.Value;

            //MessagingCenter.Send(this, "WebViewRefresh");
        }

        private void UpdateUI()
        {
            if (PrimaryEnglishSwitch.On == true)
            {
                PrimaryEnglishSwitch.Text = "Primary Language: English";
            }
            else
            {
                PrimaryEnglishSwitch.Text = "Primary Language: Chinese";
            }
        }
    }
}