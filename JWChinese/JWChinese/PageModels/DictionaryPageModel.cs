using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using WolDownloader;
using FreshMvvm;
using Xamarin.Forms;
using PropertyChanged;
using System.Diagnostics;
using System.Linq;
using System.Collections;

namespace JWChinese
{
    [AddINotifyPropertyChangedInterface]
    public class DictionaryPageModel : FreshBasePageModel
    {
        //private static IStorehouseService StorehouseService { get; } = DependencyService.Get<IStorehouseService>();
        
        public ObservableCollection<Word> Words { get; set; }
        public bool UseGlobalDictionary = true;
        public string Title { get; set; }
        public string Subtitle { get; set; }

        public double ViewCellHeight
        {
            get
            {
                if (Objects.Orientation.Width > 600)
                {
                    return 100;
                }
                else
                {
                    return 80;
                }
            }
        }

        public int GridViewRowHeight
        {
            get
            {
                if (Device.RuntimePlatform == Device.Windows)
                {
                    return -1;
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    if (Objects.Orientation.Width > 800)
                    {
                        return 100;
                    }
                    else
                    {
                        return 80;
                    }
                }
                else if (Device.RuntimePlatform == Device.iOS)
                {
                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        return 80;
                    }
                    else
                    {
                        return 100;
                    }
                }

                return 0;
            }
        }

        public override void Init(object initData)
        {
            if (initData != null)
            {
                var data = (object[])initData;

                UseGlobalDictionary = (bool)data.ElementAt(0);
                if (UseGlobalDictionary)
                {
                    Title = App.GetLanguageValue("Dictionary", "字典");
                }
                else
                {
                    if (Device.RuntimePlatform == Device.Windows)
                    {
                        Title = App.GetLanguageValue("Dictionary - ", "字典 - ") + (string)data.ElementAt(1);
                    }
                    else
                    {
                        Title = App.GetLanguageValue("Dictionary", "字典");
                        Subtitle = (string)data.ElementAt(1);
                    }
                }
            }
            else
            {
                Title = App.GetLanguageValue("Dictionary", "字典");
            }

            Words = new ObservableCollection<Word>();

        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            UpdateUI();

            base.ViewIsAppearing(sender, e);
        }

        private async void UpdateUI()
        {
            var dummy = new Word()
            {
                Chinese = "0 words in your dictionary.",
                English = "View articles and add some words!",
                Pinyin = ""
            };

            var dict = await StorehouseService.Instance.GetWordsAsync();
            ObservableCollection<Word> words = new ObservableCollection<Word>();

            if (dict.Count() == 0)
            {
                words.Add(dummy);
                Words = new ObservableCollection<Word>(words.Reverse().ToObservableCollection());
            }
            else
            {
                if (UseGlobalDictionary)
                {
                    Words = new ObservableCollection<Word>(await StorehouseService.Instance.GetWordsAsync()).Reverse().ToObservableCollection();
                }
                else
                {

                    foreach (var w in dict)
                    {
                        App.GemWriteLine(w.Pinyin);
                        App.GemWriteLine("App.CurrentArticleWords.Count()", App.CurrentArticleWords.Count());
                        if (App.CurrentArticleWords.Any(v => v.Pinyin.Contains(w.Pinyin)))
                        {
                            words.Add(w);
                        }
                    }

                    Words = new ObservableCollection<Word>(words.Reverse().ToObservableCollection());
                }
            }
        }

        public Command<Word> DeleteWord
        {
            get
            {
                return new Command<Word>(async (word) =>
                {
                    SelectedWord = null;

                    MessagingCenter.Send((DictionaryPage)CurrentPage, "DictionaryPageRefresh");

                    await StorehouseService.Instance.DeleteWordAsync(word);

                    UpdateUI();
                });
            }
        }

        Word _selectedWord;
        public Word SelectedWord
        {
            get
            {
                return _selectedWord;
            }
            set
            {
                _selectedWord = value;
                if (value != null)
                {
                    WordSelected.Execute(value);
                }
            }
        }

        public Command<Word> WordSelected
        {
            get
            {
                SelectedWord = null;

                return new Command<Word>(async (word) =>
                {
                    string title = (Device.RuntimePlatform == Device.Android) ? "" : word.Pinyin + " \n" + word.Chinese;
                    string message = (Device.RuntimePlatform == Device.Android) ? word.Pinyin + "\n" + word.Chinese + " \n\n" + "‌• " + word.English.Replace("/", "\n‌• ") : "‌• " + word.English.Replace("/", "\n‌• ");

                    await CoreMethods.DisplayAlert(title, message, "OK");
                });
            }
        }
    }
}