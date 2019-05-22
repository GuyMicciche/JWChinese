using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tidy.Core;

namespace WolDownloader
{
    public class WolDownloader
    {
        private const string BASE = "http://wol.jw.org/";
        private const string WOL = BASE + "en/wol/";

        private const string ENGLISH = "r1/lp-e/";
        private const string CHINESE = "r23/lp-chs/";

        private const string BIBLE_EN = WOL + "b/" + ENGLISH + "nwt/E/2013/";
        private const string BIBLE_CHS = WOL + "b/" + CHINESE + "bi12/CHS/2001/";

        private const string DAILYTEXT_EN = WOL + "dt/" + ENGLISH;
        private const string DAILYTEXT_CHS = WOL + "dt/" + CHINESE;

        private const string PUBLICATION_EN = WOL + "d/" + ENGLISH;
        private const string PUBLICATION_CHS = WOL + "d/" + CHINESE;

        private const string MONTHLY_EN = WOL + "lv/" + ENGLISH;
        private const string MONTHLY_CHS = WOL + "lv/" + CHINESE;

        /// <summary>
        /// 2019 MONTHLY PUBLICATIONS - THESE VALUES MUST CHANGE FOR EVERY UPDATED DOWNLOAD
        /// </summary>
        private const string YEAR = "2019";
        private const string W_EN = MONTHLY_EN + "0/" + "21378";
        private const string W_CHS = MONTHLY_CHS + "0/" + "12292";
        private const string WP_EN = MONTHLY_EN + "0/" + "21356";
        private const string WP_CHS = MONTHLY_CHS + "0/" + "12270";
        private const string G_EN = MONTHLY_EN + "0/" + "41454";
        private const string G_CHS = MONTHLY_CHS + "0/" + "23494";
        private const string MWB_EN = MONTHLY_EN + "0/" + "56439";
        private const string MWB_CHS = MONTHLY_CHS + "0/" + "32045";

        private const string LIBRARY_PUBLICATIONS = "WolDownloader.Assets.Library.Publications.";

        public static string CurrentMepsId = string.Empty;
        public static int CurrentIndex = 0;
        public static string PublicationTitle = string.Empty;

        /// <summary>
        /// Generates Insight Book articles from Watchtower Online Library
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="type"></param>
        /// <returns>Insight Book Watchtower Online Library articles</returns>
        public static async Task<List<Article>> GenerateInsightAsync(string symbol, ArticleType type)
        {
            List<Article> articles = new List<Article>();

            /////////////////////////////////////////////////////
            // English
            /////////////////////////////////////////////////////
            var assembly = typeof(WolDownloader).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(LIBRARY_PUBLICATIONS + symbol + "-en.xml");
            using (StreamReader reader = new StreamReader(stream))
            {
                string xml = reader.ReadToEnd();

                XDocument doc = XDocument.Parse(xml);
                foreach (XElement a in doc.Descendants("p").Descendants("a"))
                {
                    CurrentMepsId = a.Value;

                    articles.Add(await GenerateArticleAsync(PUBLICATION_EN + CurrentMepsId.Split('.')[1], type, Language.English));

                    Debug.WriteLine(PUBLICATION_EN + CurrentMepsId.Split('.')[1]);
                }
            }

            /////////////////////////////////////////////////////
            // Chinese
            /////////////////////////////////////////////////////
            assembly = typeof(WolDownloader).GetTypeInfo().Assembly;
            stream = assembly.GetManifestResourceStream(LIBRARY_PUBLICATIONS + symbol + "-chs.xml");
            using (StreamReader reader = new StreamReader(stream))
            {
                string xml = reader.ReadToEnd();

                XDocument doc = XDocument.Parse(xml);
                foreach (XElement a in doc.Descendants("p").Descendants("a"))
                {
                    CurrentMepsId = a.Value;

                    articles.Add(await GenerateArticleAsync(PUBLICATION_CHS + CurrentMepsId.Split('.')[1], type, Language.Chinese));

                    Debug.WriteLine(PUBLICATION_EN + CurrentMepsId.Split('.')[1]);
                }
            }

            return articles;
        }

        public static async Task<List<Article>> GenerateMonthlyPublicationAsync(string symbol, ArticleType type, List<string> monthlyMeps = null)
        {
            List<Article> articles = new List<Article>();

            string url = string.Empty;

            /////////////////////////////////////////////////////
            // The Watchtower (Study)
            /////////////////////////////////////////////////////
            if (symbol == "w")
            {
                url = W_EN;
            }
            /////////////////////////////////////////////////////
            // The Watchtower (Public)
            /////////////////////////////////////////////////////
            else if (symbol == "wp")
            {
                url = WP_EN;
            }
            /////////////////////////////////////////////////////
            // Awake!
            /////////////////////////////////////////////////////
            else if (symbol == "g")
            {
                url = G_EN;
            }
            /////////////////////////////////////////////////////
            // Life and Ministry Meeting Workbook
            /////////////////////////////////////////////////////
            else if (symbol == "mwb")
            {
                url = MWB_EN;
            }

            Debug.WriteLine("URL GenerateMonthlyPublicationAsync => "+ url);

            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Publications
            List<HtmlNode> availablePublications = doc.DocumentNode.Descendants("li").Where(li => li.GetAttributeValue("role", string.Empty) == "presentation").ToList();

            Debug.WriteLine(availablePublications.Count + " publications in " + type.ToString() + " available.");

            for (var i = 0; i < availablePublications.Count; i++)
            {
                string pub = (symbol + "-" + i);

                //if (monthlyMeps != null)
                //{
                //    if(monthlyMeps.Any(m => m.StartsWith(pub)))
                //    {
                //        continue;
                //    }
                //}

                //Debug.WriteLine(pub);

                string toc = availablePublications[i].Descendants("a").First().GetAttributeValue("href", string.Empty).Trim().Split('/').Last();

                httpClient = new HttpClient();

                Debug.WriteLine("TOC => " + MONTHLY_EN + "0/" + toc);
                html = await httpClient.GetStringAsync(MONTHLY_EN + "0/" + toc);

                doc = new HtmlDocument();
                doc.LoadHtml(html);

                List<HtmlNode> availableArticles = doc.DocumentNode.Descendants("li").Where(li => li.GetAttributeValue("role", string.Empty) == "presentation").ToList();
                for (var j = 0; j < availableArticles.Count; j++)
                {
                    string link = availableArticles[j].Descendants("a").First().GetAttributeValue("href", string.Empty).Trim().Split('/').Last();

                    try
                    {
                        CurrentMepsId = pub + "." + link + "." + j;

                        // CHECK IF ARTICLE IS DOWNLOADED
                        if (monthlyMeps != null)
                        {
                            if (monthlyMeps.Any(m => m == CurrentMepsId))
                            {
                                continue;
                            }
                        }

                        Debug.WriteLine("Chinese Article Link => " + PUBLICATION_CHS + link);
                        Debug.WriteLine("English Article Link => " + PUBLICATION_EN + link);

                        articles.Add(await GenerateArticleAsync(PUBLICATION_CHS + link, type, Language.Chinese));
                        articles.Add(await GenerateArticleAsync(PUBLICATION_EN + link, type, Language.English));
                    }
                    catch(Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }
            }

            return articles;
        }

        /// <summary>
        /// Generates Publication articles from Watchtower Online Library
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="type"></param>
        /// <returns>Insight Book Watchtower Online Library articles</returns>
        public static async Task<List<Article>> GeneratePublicationAsync(string symbol, ArticleType type)
        {
            if (type == ArticleType.Insight)
            {
                return await GenerateInsightAsync(symbol, type);
            }
            else if (type == ArticleType.Monthly)
            {
                return await GenerateMonthlyPublicationAsync(symbol, type);
            }

            List<Article> articles = new List<Article>();

            var assembly = typeof(WolDownloader).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(LIBRARY_PUBLICATIONS + symbol + ".xml");

            string xml = string.Empty;
            using (StreamReader reader = new StreamReader(stream))
            {
                xml = reader.ReadToEnd();
            }

            Debug.WriteLine("XML => " + xml);

            // Reset counter
            CurrentIndex = 0;

            XDocument doc = XDocument.Parse(xml);
            foreach (XElement a in doc.Descendants("p").Descendants("a"))
            {
                CurrentMepsId = a.Value;

                string en = string.Empty;
                string ch = string.Empty;

                if (type == ArticleType.Bible)
                {
                    en = BIBLE_EN;
                    ch = BIBLE_CHS;
                }
                else if (type == ArticleType.DailyText)
                {
                    en = DAILYTEXT_EN;
                    ch = DAILYTEXT_CHS;
                }
                else
                {
                    en = PUBLICATION_EN;
                    ch = PUBLICATION_CHS;
                }

                Debug.WriteLine("English GeneratePublicationAsync => " + en + CurrentMepsId);
                Debug.WriteLine("English GeneratePublicationAsync => " + ch + CurrentMepsId);

                articles.Add(await GenerateArticleAsync(en + CurrentMepsId, type, Language.English));
                articles.Add(await GenerateArticleAsync(ch + CurrentMepsId, type, Language.Chinese));

                CurrentIndex++;
            }

            return articles;
        }

        /// <summary>
        /// Generates single article from Watchtower Online Library
        /// </summary>
        /// <param name="url"></param>
        /// <param name="type"></param>
        /// <param name="library"></param>
        /// <returns>Single article from Watchtower Online Library</returns>
        public static async Task<Article> GenerateArticleAsync(string url, ArticleType type, Language library)
        {
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            Article article = GetArticle(doc, type);
            article.Library = (library == Language.English) ? "lp-e" : "lp-chs";
            article.URL = url;

            return article;
        }

        /// <summary>
        /// Get Watchtower Online Library article with formatting
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="type"></param>
        /// <returns>Watchtower Online Library article</returns>
        private static Article GetArticle(HtmlDocument doc, ArticleType type)
        {
            Article article = new Article();

            HtmlNode symbol = null;
            HtmlNode publication = null;
            HtmlNode title = null;
            HtmlNode location = null;
            HtmlNode content = null;
            HtmlNode bookNo = null;
            HtmlNode chapterNo = null;

            // Content
            content = doc.DocumentNode.Descendants("div").Where(div => div.GetAttributeValue("id", string.Empty) == "content").FirstOrDefault();

            // Library
            string library = doc.DocumentNode.Descendants("input")
                .Where(input => input.GetAttributeValue("id", string.Empty) == "lib")
                .FirstOrDefault()
                .GetAttributeValue("value", string.Empty).Trim();

            if (type == ArticleType.Bible)
            {
                // Publication
                publication = doc.DocumentNode.Descendants("li").Where(li => li.GetAttributeValue("class", string.Empty).Contains("resultDocumentPubTitle")).ElementAt(1);
                // Title
                title = doc.DocumentNode.Descendants("li").Where(li => li.GetAttributeValue("class", string.Empty).Contains("resultsNavigationSelected")).FirstOrDefault();
                // Book Number
                bookNo = doc.DocumentNode.Descendants("input").Where(input => input.GetAttributeValue("id", string.Empty) == "bookNo").FirstOrDefault();
                // Chapter Number
                chapterNo = doc.DocumentNode.Descendants("input").Where(input => input.GetAttributeValue("id", string.Empty) == "chapNo").FirstOrDefault();

                string b = bookNo.GetAttributeValue("value", string.Empty).Trim();
                string c = chapterNo.GetAttributeValue("value", string.Empty).Trim();

                article.Symbol = "nwt";
                article.Publication = publication.InnerText.Trim();
                article.Location = title.InnerText.Trim();
                article.MepsID = "nwt" + "." + b + "." + c;
            }
            else if (type == ArticleType.DailyText)
            {
                // Title
                title = doc.DocumentNode.Descendants("header").FirstOrDefault();
                // Location, doesn't work anymore, need to get es17 pp. 7-17, etc.
                //location = doc.DocumentNode.Descendants("span").Where(span => span.GetAttributeValue("class", string.Empty) == "ref").FirstOrDefault();

                //article.Symbol = location.InnerText.Trim().Substring(0, 4);
                article.Symbol = "es";
                //article.Publication = location.InnerText.Trim().Split(new[] { " - " }, StringSplitOptions.RemoveEmptyEntries)[1];
                article.Publication = doc.DocumentNode.Descendants("span").Where(span => span.GetAttributeValue("class", string.Empty) == "pubTitle").FirstOrDefault().InnerText.Trim();
                //article.Location = location.InnerText.Trim().Split(new[] { " - " }, StringSplitOptions.RemoveEmptyEntries)[0];
                article.Location = doc.DocumentNode.Descendants("span").Where(span => span.GetAttributeValue("class", string.Empty) == "docTitle").FirstOrDefault().InnerText.Trim();
                article.MepsID = CurrentMepsId.Replace(DateTime.Now.Year.ToString(), article.Symbol).Replace('/', '.');

                // Removes Navigation Bar
                HtmlNode node = content.Descendants("div").Where(div => div.GetAttributeValue("id", string.Empty) == "todayNav").FirstOrDefault();
                node.ParentNode.RemoveChild(node);
            }
            else if (type == ArticleType.Publication || type == ArticleType.Insight)
            {
                // Code
                symbol = doc.DocumentNode.Descendants("input").Where(input => input.GetAttributeValue("id", string.Empty) == "englishSym").FirstOrDefault();
                // Publication
                publication = doc.DocumentNode.Descendants("li").Where(li => li.GetAttributeValue("class", string.Empty).Contains("resultDocumentPubTitle")).ElementAt(1);
                // Title
                title = doc.DocumentNode.Descendants("li").Where(li => li.GetAttributeValue("class", string.Empty).Contains("resultDocumentPubTitle")).FirstOrDefault();
                // Location
                location = doc.DocumentNode.Descendants("li").Where(li => li.GetAttributeValue("class", string.Empty).Contains("documentLocation")).FirstOrDefault();
                // English Symbol
                string englishSym = doc.DocumentNode.Descendants("input")
                    .Where(input => input.GetAttributeValue("id", string.Empty) == "englishSym")
                    .FirstOrDefault()
                    .GetAttributeValue("value", string.Empty).Trim();

                article.Symbol = (type == ArticleType.Publication) ? englishSym : "it";
                article.Publication = publication.InnerText.Trim();
                article.Location = location.InnerText.Trim();
                article.MepsID = (type == ArticleType.Publication) ? englishSym + "." + CurrentMepsId + "." + CurrentIndex : CurrentMepsId;
            }
            else if (type == ArticleType.Monthly)
            {
                // Title
                title = doc.DocumentNode.Descendants("li").Where(li => li.GetAttributeValue("class", string.Empty).Contains("resultDocumentPubTitle")).FirstOrDefault();
                // Location
                location = doc.DocumentNode.Descendants("li").Where(li => li.GetAttributeValue("class", string.Empty).Contains("documentLocation")).FirstOrDefault();
                // Document Description
                string documentDescription = doc.DocumentNode.Descendants("input")
                    .Where(input => input.GetAttributeValue("id", string.Empty) == "documentDescription")
                    .FirstOrDefault()
                    .GetAttributeValue("value", string.Empty).Trim();
                // English Symbol
                string englishSym = doc.DocumentNode.Descendants("input")
                    .Where(input => input.GetAttributeValue("id", string.Empty) == "englishSym")
                    .FirstOrDefault()
                    .GetAttributeValue("value", string.Empty).Trim();

                article.Symbol = englishSym;
                article.Publication = BuildMonthlyPublicationTitle(englishSym, documentDescription, library);
                article.Location = location.InnerText.Trim();
                article.MepsID = CurrentMepsId;
            }

            article.Title = title.InnerText.Trim();
            article.Content = GEMFormat(content.InnerHtml.Trim());

            return article;
        }

        /// <summary>
        /// Formats Watchtower Online Library HTML with my style
        /// </summary>
        /// <param name="input">HTML string</param>
        /// <returns>Formatted HTML</returns>
        public static string GEMFormat(string input)
        {
            string output = input.Replace(@"href=""/", @"href=""" + BASE).Replace(@"src=""/", @"src=""" + BASE);
            output = Regex.Replace(output, @"(?s)(<div class=""bannerContent"">)(.*?)(</div>)", "", RegexOptions.Singleline); // Get rid of pinyin banner
            output = Regex.Replace(output, @"(data-no)(\=""\d+"")", "");
            output = Regex.Replace(output, @" (data-pid)(\=""\d+"")", "");
            output = Regex.Replace(output, @" (data-rel-pid)(\=""\[\d+\]"")", "");
            output = Regex.Replace(output, @" (data-bid)(\=""\d+)(-\d+)("")", "");
            output = Regex.Replace(output, @"(<!-- Root element of lightbox -->)(.|\n)+?(</article>)", "</article>", RegexOptions.Singleline);

            // MAKE Chinese links pinyin
            output = Regex.Replace(output, @"lp-chs", "lp-chs-rb");

            return output;
        }

        public static string BuildMonthlyPublicationTitle(string symbol, string documentDescription, string library)
        {
            string title = string.Empty;

            /////////////////////////////////////////////////////
            // The Watchtower (Study)
            /////////////////////////////////////////////////////
            if (symbol == "w")
            {
                documentDescription = documentDescription.Substring(4).Trim();
                title = (library == "lp-e") ? "The Watchtower, " + documentDescription + " " + YEAR : "守望台" + YEAR + "年" + documentDescription + "刊";
            }
            /////////////////////////////////////////////////////
            // The Watchtower (Public)
            /////////////////////////////////////////////////////
            else if (symbol == "wp")
            {
                documentDescription = documentDescription.Substring(5).Trim();
                title = (library == "lp-e") ? "The Watchtower, " + documentDescription + " " + YEAR : "守望台" + YEAR + "年" + documentDescription;
            }
            /////////////////////////////////////////////////////
            // Awake!
            /////////////////////////////////////////////////////
            else if (symbol == "g")
            {
                documentDescription = documentDescription.Substring(4).Trim();
                title = (library == "lp-e") ? "Awake!, " + documentDescription + " " + YEAR : "警醒！" + YEAR + "年" + documentDescription;
            }
            /////////////////////////////////////////////////////
            // Life and Ministry Meeting Workbook
            /////////////////////////////////////////////////////
            else if (symbol.Contains("mwb"))
            {
                documentDescription = documentDescription.Substring(6).Trim();
                title = (library == "lp-e") ? "Life and Ministry Meeting Workbook, " + documentDescription + " " + YEAR : "聚会手册" + YEAR + "年" + documentDescription + "刊";
            }

            return title;
        }

        public static void DebugArticleOutput(Article article)
        {
            Debug.WriteLine("Library => " + article.Library);
            Debug.WriteLine("Symbol => " + article.Symbol);
            Debug.WriteLine("Publication => " + article.Publication);
            Debug.WriteLine("Title => " + article.Title);
            Debug.WriteLine("MepsID => " + article.MepsID);
            Debug.WriteLine("Location => " + article.Location);
            Debug.WriteLine("Group => " + article.Group);
            Debug.WriteLine("URL => " + article.URL);
        }

        public string DoTidy(string html)
        {
            Tidy.Core.Tidy document = new Tidy.Core.Tidy();
            TidyMessageCollection messageCollection = new TidyMessageCollection();
            document.Options.DocType = DocType.Omit;
            document.Options.Xhtml = true;
            document.Options.CharEncoding = CharEncoding.Utf8;
            document.Options.LogicalEmphasis = true;

            document.Options.MakeClean = false;
            document.Options.QuoteNbsp = false;
            document.Options.SmartIndent = false;
            document.Options.IndentContent = false;
            document.Options.TidyMark = false;

            document.Options.DropFontTags = false;
            document.Options.QuoteAmpersand = true;
            document.Options.DropEmptyParas = true;

            MemoryStream input = new MemoryStream();
            MemoryStream output = new MemoryStream();
            byte[] array = Encoding.UTF8.GetBytes(html);
            input.Write(array, 0, array.Length);
            input.Position = 0;

            document.Parse(input, output, messageCollection);

            string tidyXhtml = Encoding.UTF8.GetString(output.ToArray(), 0, output.ToArray().Length);

            return XElement.Parse(tidyXhtml).ToString();
        }
    }
}