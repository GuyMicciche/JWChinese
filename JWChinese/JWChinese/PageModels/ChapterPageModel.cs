using FreshMvvm;
using PropertyChanged;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WolDownloader;
using Xamarin.Forms;

namespace JWChinese
{
    [AddINotifyPropertyChangedInterface]

    public class ChapterPageModel : FreshBasePageModel
    {
        public BibleBook Book { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public ObservableCollection<string> Chapters { get; set; }
        public int NumberOfElements { get; set; }

        public override void Init(object initData)
        {
            Book = initData as BibleBook;

            var chapters = new List<string>();
            for (var i = 1; i <= Book.Chapters; i++)
            {
                chapters.Add(i.ToString());
            }

            Chapters = chapters.ToObservableCollection();
            NumberOfElements = chapters.Count;

            if(Device.RuntimePlatform == Device.Windows)
            {
                Title = Book.StandardBookName + " - " + App.GetLanguageValue("New World Translation", "圣经新世界译本");
            }
            else
            {
                Title = Book.StandardBookName;
                Subtitle = App.GetLanguageValue("New World Translation", "圣经新世界译本");
            }
        }

        public Command ChapterTapped
        {
            get
            {
                return new Command(async (chapter) =>
                {
                    Debug.WriteLine(chapter);
                    Debug.WriteLine((string)chapter);
                    string title = Book.StandardBookName + " " + (string)chapter;
                    string meps = title + "|nwt." + Book.BookNumber + "." + (string)chapter;

                    await CoreMethods.PushPageModel<ArticlePageModel>(meps);
                });
            }
        }
    }
}
