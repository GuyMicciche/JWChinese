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
using JWChinese.Helpers;

namespace JWChinese
{
    [AddINotifyPropertyChangedInterface]
    public class PublicationContentsPageModel : FreshBasePageModel
    {
        //private static IStorehouseService StorehouseService { get; } = DependencyService.Get<IStorehouseService>();
        public ObservableCollection<Article> Articles { get; set; }
        public string PublicationTitle { get; set; }

        public override async void Init(object initData)
        {
            var pub = initData as PublicationGroup;

            PublicationTitle = pub.ShortTitle;

            string symbol = pub.KeySymbol;

            if(pub.Category == PublicationCatagory.Watchtower_Study)
            {
                Articles = new ObservableCollection<Article>(await StorehouseService.Instance.GetArticlesAsync(Settings.PrimaryLanguage, "w", symbol));
            }
            else if (pub.Category == PublicationCatagory.Meeting_Workbooks)
            {
                Articles = new ObservableCollection<Article>(await StorehouseService.Instance.GetArticlesAsync(Settings.PrimaryLanguage, "mwb", symbol));
            }
            else if (pub.Category == PublicationCatagory.Watchtower_Public)
            {
                Articles = new ObservableCollection<Article>(await StorehouseService.Instance.GetArticlesAsync(Settings.PrimaryLanguage, "wp", symbol));
            }
            else if (pub.Category == PublicationCatagory.Awake)
            {
                Articles = new ObservableCollection<Article>(await StorehouseService.Instance.GetArticlesAsync(Settings.PrimaryLanguage, "g", symbol));
            }
            else
            {
                Articles = new ObservableCollection<Article>(await StorehouseService.Instance.GetArticlesAsync(Settings.PrimaryLanguage, symbol));
            }

            // If only one article, then just display it.
            //if(Articles.Count == 1)
            //{
            //    SelectedArticle = Articles.First();
            //}

            Debug.WriteLine(symbol);
        }

        Article _selectedArticle;
        public Article SelectedArticle
        {
            get
            {
                return _selectedArticle;
            }
            set
            {
                _selectedArticle = value;
                if (value != null)
                {
                    ArticleSelected.Execute(value);
                }
            }
        }

        public Command<Article> ArticleSelected
        {
            get
            {
                return new Command<Article>(async (article) =>
                {
                    await CoreMethods.PushPageModel<ArticlePageModel>(article);
                    SelectedArticle = null;
                });
            }
        }
    }
}