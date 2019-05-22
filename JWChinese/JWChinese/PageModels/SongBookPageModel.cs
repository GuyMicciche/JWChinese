using FreshMvvm;
using PropertyChanged;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WolDownloader;
using JWChinese.Helpers;
using Xamarin.Forms;

namespace JWChinese
{
    [AddINotifyPropertyChangedInterface]
    public class SongBookPageModel : FreshBasePageModel
    {
        public string Title { get; set; }
        public ObservableCollection<Article> Songs { get; set; }
        public ObservableCollection<string> SongNumbers { get; set; }

        public override async void Init(object initData)
        {
            Songs = new ObservableCollection<Article>(await StorehouseService.Instance.GetArticlesAsync(Settings.PrimaryLanguage, "sjj"));

            var songs = new List<string>();
            for (var i = 1; i <= Songs.Count; i++)
            {
                songs.Add(i.ToString());
            }

            SongNumbers = songs.ToObservableCollection();

            Title = App.GetLanguageValue("“Sing Out Joyfully” to Jehovah", "向耶和华高声欢唱");
        }

        public Command SongTapped
        {
            get
            {
                return new Command(async (songNumber) =>
                {
                    await CoreMethods.PushPageModel<ArticlePageModel>(Songs.ElementAt(int.Parse((string)songNumber) - 1));
                });
            }
        }
    }
}
