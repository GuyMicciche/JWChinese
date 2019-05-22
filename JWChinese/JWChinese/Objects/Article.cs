using SQLite;

namespace JWChinese
{
    public class Article
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Library { get; set; }
        public string Symbol { get; set; }
        public string Publication { get; set; }
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