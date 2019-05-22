// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;

namespace JWChinese.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        private const string ExpandKey = "expand_key";
        private static readonly int ExpandDefault = (int)ExpandScreen.Dual;

        private const string ReferenceSymbolsKey = "referencesynbols_key";
        private static readonly bool ReferenceSymbolsDefault = true;

        private const string TextSettingsKey = "textsettings_key";
        private static readonly double TextSettingsDefault = App.TextSettingsBase;

        private const string PrimaryLanguageKey = "primarylanguage_key";
        private static readonly string PrimaryLanguageDefault = App.PrimaryLanguageBase;

        private const string SecondaryLanguageKey = "secondarylanguage_key";
        private static readonly string SecondaryLanguageDefault = App.SecondaryLanguageBase;

        #endregion


        public static string GeneralSettings
        {
            get { return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault); }
            set { AppSettings.AddOrUpdateValue(SettingsKey, value); }
        }

        public static int ExpandSetting
        {
            get { return AppSettings.GetValueOrDefault(ExpandKey, ExpandDefault); }
            set { AppSettings.AddOrUpdateValue(ExpandKey, value); }
        }

        public static bool ReferenceSymbols
        {
            get { return AppSettings.GetValueOrDefault(ReferenceSymbolsKey, ReferenceSymbolsDefault); }
            set { AppSettings.AddOrUpdateValue(ReferenceSymbolsKey, value); }
        }

        public static double TextSettings
        {
            get { return AppSettings.GetValueOrDefault(TextSettingsKey, TextSettingsDefault); }
            set
            {
                double t = App.GetTextSettings(value);
                AppSettings.AddOrUpdateValue(TextSettingsKey, t);
            }
        }

        public static string PrimaryLanguage
        {
            get { return AppSettings.GetValueOrDefault(PrimaryLanguageKey, PrimaryLanguageDefault); }
            set
            {
                if(value == LPLanguage.English.GetName())
                {
                    SecondaryLanguage = LPLanguage.Chinese.GetName();
                }
                else if(value == LPLanguage.Chinese.GetName())
                {
                    SecondaryLanguage = LPLanguage.English.GetName();
                }

                AppSettings.AddOrUpdateValue(PrimaryLanguageKey, value);
            }
        }

        public static string SecondaryLanguage
        {
            get { return AppSettings.GetValueOrDefault(SecondaryLanguageKey, SecondaryLanguageDefault); }
            set { AppSettings.AddOrUpdateValue(SecondaryLanguageKey, value); }
        }
    }

    public enum ExpandScreen
    {
        Dual = 0,
        Primary = 1,
        Secondary = 2
    }

    public enum LPLanguage
    {
        English = 0,
        Chinese = 1,
    }
}
