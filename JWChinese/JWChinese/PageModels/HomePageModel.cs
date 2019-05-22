 using System;
using FreshMvvm;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Diagnostics;
using System.Linq;
using JWChinese.Helpers;

namespace JWChinese
{
    [AddINotifyPropertyChangedInterface]
    public class HomePageModel : FreshBasePageModel
    {
        //private static IStorehouseService StorehouseService { get; } = DependencyService.Get<IStorehouseService>();

        public List<Article> Articles { get; set; }

        HtmlWebViewSource _primaryWebViewSource;
        public HtmlWebViewSource PrimaryWebViewSource
        {
            get
            {
                return _primaryWebViewSource;
            }
            set
            {
                if (_primaryWebViewSource != value)
                {
                    _primaryWebViewSource = value;
                }
            }
        }

        HtmlWebViewSource _secondaryWebViewSource;
        public HtmlWebViewSource SecondaryWebViewSource
        {
            get
            {
                return _secondaryWebViewSource;
            }
            set
            {
                if (_secondaryWebViewSource != value)
                {
                    _secondaryWebViewSource = value;
                }
            }
        }

        string _url = null;
        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                if (_url != value)
                {
                    _url = value;
                }
            }
        }

        public async override void Init(object initData)
        {
            Articles = new List<Article>(await StorehouseService.Instance.GetArticlesAsync("es"));

            string template = @"
                <html>
                    <head>
                        <link rel=""stylesheet"" type=""text/css"" href=""css/meps-styles.min.css"">
                        <link rel=""stylesheet"" type=""text/css"" href=""css/wol.min.css"">
                        <link rel=""stylesheet"" type=""text/css"" href=""css/wol.unified.fonts.min.css"">
                        <link rel=""stylesheet"" type=""text/css"" href=""css/pubs/nwt.min.css"">
                        <link rel=""stylesheet"" type=""text/css"" href=""css/today.min.css"">
                        <link rel=""stylesheet"" type=""text/css"" href=""css/print.min.css"" media=""print"">
                    </head>
                    <body>
                        |
                    </body> 
                </html>";

            string date = "es" + DateTime.Now.ToString(@"yy.M.d");
            NavStruct article = NavStruct.Parse(date);
            //Debug.WriteLine(article.ToString());

            string primaryHtml = template.Replace("|", Articles.Where(a => a.MepsID == article.ToString() && a.Library == Settings.PrimaryLanguage).First().Content);
            primaryHtml = primaryHtml.Replace(@"href=""/", @"href=""" + "http://wol.jw.org/").Replace(@"src=""/", @"src=""" + "http://wol.jw.org/");

            string secondaryHtml = template.Replace("|", Articles.Where(a => a.MepsID == article.ToString() && a.Library == Settings.SecondaryLanguage).First().Content);
            secondaryHtml = secondaryHtml.Replace(@"href=""/", @"href=""" + "http://wol.jw.org/").Replace(@"src=""/", @"src=""" + "http://wol.jw.org/");

            var root = DependencyService.Get<IBaseUrl>().Get();
            Url = $"{root}index.html";

            PrimaryWebViewSource = new HtmlWebViewSource
            {
                Html = primaryHtml,
                BaseUrl = root
            };

            SecondaryWebViewSource = new HtmlWebViewSource
            {
                Html = secondaryHtml,
                BaseUrl = root
            };
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
                return new Command<Article>(async (article) => {
                    await CoreMethods.PushPageModel<BiblePageModel>(article);
                });
            }
        }

        public Command<WebNavigatingEventArgs> NavigatingCommand
        {
            get
            {
                return new Command<WebNavigatingEventArgs>(
                    (param) =>
                    {
                        //if (param != null && -1 < Array.IndexOf(_uris, param.Url))
                        //{
                            //Debug.WriteLine(param.Url);
                            Device.OpenUri(new Uri(param.Url));
                            param.Cancel = true;
                        //}
                    },
                    (param) => true
                    );
            }
        }
    }
}