using FreshMvvm;
using JWChinese.Helpers;
using PropertyChanged;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WolDownloader;
using Xamarin.Forms;

namespace JWChinese
{
    [AddINotifyPropertyChangedInterface]
    public class BiblePageModel : FreshBasePageModel
    {
        public List<BibleBook> BibleBooks { get; set; }
        public ObservableCollection<BibleBook> HebrewBibleBooks { get; set; }
        public ObservableCollection<BibleBook> GreekBibleBooks { get; set; }
        public int ArticleIndex { get; set; }
        public string Title { get; set; }

        public override async void Init(object initData)
        {
            BibleBooks = WolLibrary.GetAllBibleBooks();
            
            int langid = (Settings.PrimaryLanguage == LPLanguage.English.GetName()) ? (int)Language.English : (int)Language.Chinese;

            HebrewBibleBooks = new ObservableCollection<BibleBook>(BibleBooks.Where(b => b.MepsLanguageId == langid).Take(39));
            GreekBibleBooks = new ObservableCollection<BibleBook>(BibleBooks.Where(b => b.MepsLanguageId == langid).Skip(39));

            Title = App.GetLanguageValue("New World Translation", "新世界译本");
        }

        public Command BookSelected
        {
            get
            {
                return new Command(async (parameter) =>
                {
                    var book = parameter as BibleBook;
                    if (book != null)
                    {
                        await CoreMethods.PushPageModel<ChapterPageModel>(book);

                        //await CoreMethods.DisplayAlert(book.StandardBookName, book.BookNumber.ToString(), "Cancel");
                    }
                });
            }
        }
    }
}
