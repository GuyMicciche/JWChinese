using System;
using FreshMvvm;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using JWChinese.Helpers;
using System.Text.RegularExpressions;

namespace JWChinese
{
    [AddINotifyPropertyChangedInterface]
    public class DailyTextPageModel : FreshBasePageModel
    {
        // US THIS
        //IStorehouseService _storehouseService;
        //public DailyTextPageModel(IStorehouseService storehouseService)
        //{
        //    _storehouseService = storehouseService;
        //}

        // OR THIS
        //private static IStorehouseService StorehouseService { get; } = DependencyService.Get<IStorehouseService>();

        //public ArticleTemplateSelector ArticleTemplate { get; set; }

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
        public string Url { get; set; }
        public string FontSize { get; set; }

        public override void Init(object initData)
        {
            UpdateArticle();

            MessagingCenter.Subscribe<SettingsPage>(this, "WebViewRefresh", (sender) =>
            {
                if (ArticleWebViewSources != null)
                {
                    UpdateArticle();
                }
            });

            MessagingCenter.Subscribe<DictionaryPage>(this, "DictionaryPageRefresh", (sender) =>
            {
                UpdateArticle();
            });
        }

        private async void UpdateArticle()
        {
            ArticleWebViewSource = null;
            ArticleWebViewSources?.Clear();
            ArticleWebViewSources = null;

            FontSize = Settings.TextSettings.ToString();

            //ArticleTemplate = new ArticleTemplateSelector();

            string date = "es." + DateTime.Now.ToString(@"M.d");
            NavStruct nav = NavStruct.Parse(date);

            ArticleWebViewSources = new ObservableCollection<ArticlesDataModel>();

            //ObservableCollection<Article> articles = new ObservableCollection<Article>(await StorehouseService.Instance.GetArticlesAsync("es")).ToObservableCollection();
            ObservableCollection<Article> articles = new ObservableCollection<Article>(await StorehouseService.Instance.GetArticlesAsync("es")).Where(a => a.MepsID == nav.ToString()).ToObservableCollection();

            App.GemWriteLine(articles.Count());

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
                string secondaryHtml = TEMPLATE.Replace("%|%", articles.Where(a => a.MepsID == meps[index] && a.Library == Settings.SecondaryLanguage).SingleOrDefault().Content);
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

                ArticlesDataModel dayArticleModel = new ArticlesDataModel()
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

                ArticleWebViewSources.Add(dayArticleModel);

                if (NavStruct.Parse(meps[index]).ToString() == nav.ToString())
                {
                    ArticleIndex = index;
                }
            }

            ArticleWebViewSource = ArticleWebViewSources.First();
        }
    }
}