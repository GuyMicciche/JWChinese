using FreshMvvm;
using JWChinese.Helpers;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace JWChinese
{
    [AddINotifyPropertyChangedInterface]
    public class ArticlePageModel : FreshBasePageModel
    {
        //private static IStorehouseService StorehouseService { get; } = DependencyService.Get<IStorehouseService>();

        private static string TEMPLATE = @"
                <html>
                    <head>
                        <link rel=""stylesheet"" type=""text/css"" href=""css/meps-styles.min.css"">
                        <link rel=""stylesheet"" type=""text/css"" href=""css/wol.min.css"">
                        <link rel=""stylesheet"" type=""text/css"" href=""css/wol.unified.fonts.min.css"">
                        <link rel=""stylesheet"" type=""text/css"" href=""css/pubs/nwt.min.css"">
                        <link rel=""stylesheet"" type=""text/css"" href=""css/today.min.css"">
                        <link rel=""stylesheet"" type=""text/css"" href=""css/print.min.css"" media=""print"">
                        <script src=""js/init.js""></script>
                        <script src=""js/annotator.js""></script>

                        <title></title>
                    </head>
                    <body>
                    <div class=""scalableui"" style=""font-size: %||%%; display: block;"">
                        %|%
                    </div>
                    </body> 
                </html>";

        public ObservableCollection<ArticlesDataModel> ArticleWebViewSources { get; set; }
        public ArticlesDataModel ArticleWebViewSource { get; set; }
        public int ArticleIndex { get; set; }
        public string _articleTitle { get; set; }
        public string ArticleTitle { get; set; }
        public string PublicationTitle { get; set; }
        public string Url { get; set; }
        public string FontSize { get; set; }
        public object initData { get; set; }

        public override void Init(object initData)
        {
            MessagingCenter.Subscribe<SettingsPage>(this, "WebViewRefresh", (sender) =>
            {
                if(ArticleWebViewSources != null)
                {
                    LoadHTML(this.initData);
                }
            });
            MessagingCenter.Subscribe<DictionaryPage>(this, "DictionaryPageRefresh", (sender) =>
            {
                if (ArticleWebViewSources != null)
                {
                    LoadHTML(this.initData);
                }
            });

            this.initData = initData;
            LoadHTML(initData);
        }

        private void LoadHTML(object initData)
        {
            ArticleWebViewSource = null;
            ArticleWebViewSources?.Clear();
            ArticleWebViewSources = null;

            FontSize = Settings.TextSettings.ToString();

            ArticleWebViewSources = new ObservableCollection<ArticlesDataModel>();

            // BIBILE BOOK CHAPTERS
            if (initData is string)
            {
                string nwt = (initData as string).Split('|')[1];

                if (Device.RuntimePlatform == Device.Windows)
                {
                    ArticleTitle = (initData as string).Split('|')[0] + " - " + App.GetLanguageValue("New World Translation", "圣经新世界译本");
                }
                else
                {
                    ArticleTitle = (initData as string).Split('|')[0];
                    PublicationTitle = App.GetLanguageValue("New World Translation", "圣经新世界译本");
                }

                GenerateChapters(nwt);
            }
            // PUBLICATION ARTICLES
            else
            {
                Article article = initData as Article;

                if (Device.RuntimePlatform == Device.Windows)
                {
                    ArticleTitle = article.Title + " - " + article.Publication;
                }
                else
                {
                    ArticleTitle = article.Title;
                    PublicationTitle = article.Publication;
                }

                GenerateArticles(article);
            }
        }

        private async void GenerateArticles(Article article)
        {
            NavStruct nav = NavStruct.Parse(article.MepsID);

            // Loads all articles
            //ObservableCollection<Article> articles = new ObservableCollection<Article>(await StorehouseService.Instance.GetArticlesAsync(article.Symbol)).ToObservableCollection();

            // Loads only 1 article at a time
            //ObservableCollection<Article> articles = new ObservableCollection<Article>(await StorehouseService.Instance.GetArticlesAsync(article.Symbol)).Where(a => a.MepsID == nav.ToString()).ToObservableCollection();
            ObservableCollection<Article> articles = new ObservableCollection<Article>(await StorehouseService.Instance.GetArticlesAsync(article.Symbol)).Where(a => (article.Symbol != "es") ? a.MepsID.Contains("." + nav.MepsID + ".") : a.MepsID == nav.ToString()).ToObservableCollection();

            var root = DependencyService.Get<IBaseUrl>().Get();
            Url = $"{root}index.html";

            string[] meps = articles.Where(a => a.Library == App.PrimaryLanguageBase).Select(a => a.MepsID).ToArray();
            for (int index = 0; index < meps.Count(); index++)
            {
                // PRIMARY
                string primaryHtml = TEMPLATE.Replace("%|%", articles.Where(a => a.MepsID == meps[index] && a.Library == Settings.PrimaryLanguage).SingleOrDefault().Content);
                primaryHtml = primaryHtml.Replace("%||%", FontSize);
                primaryHtml = primaryHtml.Replace(@"href=""/", @"href=""" + "http://wol.jw.org/").Replace(@"src=""/", @"src=""" + "http://wol.jw.org/");

                // SECONDARY
                string secondaryHtml = (article.Symbol != "es") ? TEMPLATE.Replace("%|%", articles.Where(a => a.MepsID.Contains("." + NavStruct.Parse(meps[index]).MepsID + ".") && a.Library == Settings.SecondaryLanguage).SingleOrDefault().Content)
                                                                : TEMPLATE.Replace("%|%", articles.Where(a => a.MepsID == meps[index] && a.Library == Settings.SecondaryLanguage).SingleOrDefault().Content);
                secondaryHtml = secondaryHtml.Replace("%||%", FontSize);
                //string secondaryHtml = template.Replace("|", articles.Where(a => a.MepsID == meps[index] && a.Library == Settings.SecondaryLanguage).SingleOrDefault().Content);
                secondaryHtml = secondaryHtml.Replace(@"href=""/", @"href=""" + "http://wol.jw.org/").Replace(@"src=""/", @"src=""" + "http://wol.jw.org/");

                // Reference Symbols
                if (!Settings.ReferenceSymbols)
                {
                    primaryHtml = primaryHtml.Replace(@">*</a>", @" style=""display: none;"">*</a>")
                        .Replace(@"class=""b"">+</a>", @"class=""b"" style=""display: none;"">*</a>");
                    secondaryHtml = secondaryHtml.Replace(@">*</a>", @" style=""display: none;"">*</a>")
                        .Replace(@"class=""b"">+</a>", @"class=""b"" style=""display: none;"">*</a>");
                }

                // Choose Chinese WebView
                if (Settings.PrimaryLanguage == LPLanguage.Chinese.GetName())
                {
                    primaryHtml = primaryHtml.Replace(@"<title></title>", @"<title>Chinese</title>"); // IMPORTANT FOR ANNOTATIONS TO WORK ONLY ON CHINESE WEBVIEW
                }
                if (Settings.SecondaryLanguage == LPLanguage.Chinese.GetName())
                {
                    secondaryHtml = secondaryHtml.Replace(@"<title></title>", @"<title>Chinese</title>"); // IMPORTANT FOR ANNOTATIONS TO WORK ONLY ON CHINESE WEBVIEW
                }

                ArticlesDataModel publicationArticleModel = new ArticlesDataModel()
                {
                    Primary = new HtmlWebViewSource
                    {
                        Html = primaryHtml,
                        BaseUrl = root
                    },
                    Secondary = new HtmlWebViewSource
                    {
                        Html = secondaryHtml,
                        BaseUrl = root
                    }
                };

                ArticleWebViewSources.Add(publicationArticleModel);

                if (NavStruct.Parse(meps[index]).ToString() == nav.ToString())
                {
                    ArticleIndex = index;
                    //_articleTitle = articles.Where(a => a.MepsID == meps[index] && a.Library == Settings.PrimaryLanguage).SingleOrDefault().Title;
                }
            }

            ArticleWebViewSource = ArticleWebViewSources.First();
        }

        private async void GenerateChapters(string nwt)
        {
            NavStruct nav = NavStruct.Parse(nwt);

            //ObservableCollection<Article> chapters = new ObservableCollection<Article>(await StorehouseService.Instance.GetArticlesAsync("nwt")).Where(c => c.MepsID.Contains("." + nav.MepsID + ".")).ToObservableCollection();
            ObservableCollection<Article> chapters = new ObservableCollection<Article>(await StorehouseService.Instance.GetArticlesAsync("nwt")).Where(a => a.MepsID == nav.ToString()).ToObservableCollection();

            var root = DependencyService.Get<IBaseUrl>().Get();
            Url = $"{root}index.html";

            string[] meps = chapters.Where(a => a.Library == App.PrimaryLanguageBase).Select(a => a.MepsID).ToArray();
            for (int index = 0; index < meps.Count(); index++)
            {
                // PRIMARY
                string primaryHtml = TEMPLATE.Replace("%|%", chapters.Where(a => a.MepsID == meps[index] && a.Library == Settings.PrimaryLanguage).SingleOrDefault().Content);
                primaryHtml = primaryHtml.Replace("%||%", FontSize);
                primaryHtml = primaryHtml.Replace(@"href=""/", @"href=""" + "http://wol.jw.org/").Replace(@"src=""/", @"src=""" + "http://wol.jw.org/");

                // SECONDARY
                string secondaryHtml = TEMPLATE.Replace("%|%", chapters.Where(a => a.MepsID == meps[index] && a.Library == Settings.SecondaryLanguage).SingleOrDefault().Content);
                secondaryHtml = secondaryHtml.Replace("%||%", FontSize);
                secondaryHtml = secondaryHtml.Replace(@"href=""/", @"href=""" + "http://wol.jw.org/").Replace(@"src=""/", @"src=""" + "http://wol.jw.org/");

                // Reference Symbols
                if (!Settings.ReferenceSymbols)
                {
                    primaryHtml = primaryHtml.Replace(@">*</a>", @" style=""display: none;"">*</a>")
                        .Replace(@"class=""b"">+</a>", @"class=""b"" style=""display: none;"">*</a>");
                    secondaryHtml = secondaryHtml.Replace(@">*</a>", @" style=""display: none;"">*</a>")
                        .Replace(@"class=""b"">+</a>", @"class=""b"" style=""display: none;"">*</a>");
                }

                // Choose Chinese WebView
                if (Settings.PrimaryLanguage == LPLanguage.Chinese.GetName())
                {
                    primaryHtml = primaryHtml.Replace(@"<title></title>", @"<title>Chinese</title>"); // IMPORTANT FOR ANNOTATIONS TO WORK ONLY ON CHINESE WEBVIEW
                }
                if (Settings.SecondaryLanguage == LPLanguage.Chinese.GetName())
                {
                    secondaryHtml = secondaryHtml.Replace(@"<title></title>", @"<title>Chinese</title>"); // IMPORTANT FOR ANNOTATIONS TO WORK ONLY ON CHINESE WEBVIEW
                }

                ArticlesDataModel chapterArticleModel = new ArticlesDataModel()
                {
                    Primary = new HtmlWebViewSource
                    {
                        Html = primaryHtml,
                        BaseUrl = root
                    },
                    Secondary = new HtmlWebViewSource
                    {
                        Html = secondaryHtml,
                        BaseUrl = root
                    }
                };

                ArticleWebViewSources.Add(chapterArticleModel);

                if (NavStruct.Parse(meps[index]).ToString() == nwt)
                {
                    ArticleIndex = index;
                    //_articleTitle = chapters.Where(a => a.MepsID == meps[index] && a.Library == Settings.PrimaryLanguage).SingleOrDefault().Title;
                }
            }

            ArticleWebViewSource = ArticleWebViewSources.First();
        }

        public string DictionaryIcon
        {
            get
            {
                return String.Format("{0}{1}", Device.OnPlatform("", "", "Assets/"), "bar_dictionary.png");
            }
        }

        public Command OpenDictionary
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<DictionaryPageModel>(new object[] { false, ArticleTitle }, false, true);
                });
            }
        }
    }
}