using JWChinese.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace JWChinese
{
    public static class Extensions
    {
        public static Article ToArticle(this WolDownloader.Article a)
        {
            Article article = new Article
            {
                Library = a.Library,
                Symbol = a.Symbol,
                Content = a.Content,
                Group = a.Group,
                Location = a.Location,
                MepsID = a.MepsID,
                Publication = a.Publication,
                Title = a.Title,
                URL = a.URL
            };

            return article;
        }

        public static Publication ToPublication(this WolDownloader.Publication p)
        {
            Publication publication = new Publication
            {
                PublicationImage = p.ImageAsset,
                Title = p.Title,
                Description = p.ShortTitle,
                DueDate = DateTime.Now,
                Type = p.Category,
                //StatusMessageFileSource = p.Symbol,
                //StatusMessage = p.Content,
                //ActionMessageFileSource = p.Group,
                //ActionMessage = p.Location,
                //DirationInMinutes = p.Title,
            };

            return publication;
        }

        //public class Publication
        //{
        //    public FileImageSource PublicationImage { get; set; }
        //    public FileImageSource StatusMessageFileSource { get; set; }
        //    public string StatusMessage { get; set; }
        //    public FileImageSource ActionMessageFileSource { get; set; }
        //    public string ActionMessage { get; set; }
        //    public string Title { get; set; }
        //    public string Description { get; set; }
        //    public ContentView MessageView { get; set; }
        //    public ContentView ActionView { get; set; }
        //    public DateTime DueDate { get; set; }
        //    public int DirationInMinutes { get; set; }
        //}

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            return new ObservableCollection<T>(enumerable.Cast<T>());
        }

        public static string GetName(this LPLanguage input)
        {
            switch (input)
            {
                case LPLanguage.English:
                    return "lp-e";
                case LPLanguage.Chinese:
                    return "lp-chs";
            }

            return string.Empty;
        }
    }
}