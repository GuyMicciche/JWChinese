namespace WolDownloader
{
    public class Article
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Library Language
        /// </summary>
        public string Library { get; set; }

        /// <summary>
        /// Publication Symbol
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Publication Name
        /// </summary>
        public string Publication { get; set; }

        /// <summary>
        /// Article Title
        /// </summary>
        public string Title { get; set; }


        public string MepsID { get; set; }


        public string Location { get; set; }


        public string Content { get; set; }


        public string Group { get; set; }


        public string URL { get; set; }

        public Article()
        {

        }
    }
}