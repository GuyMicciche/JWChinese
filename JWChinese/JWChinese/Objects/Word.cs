using SQLite;

namespace JWChinese
{
    public class Word
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string English { get; set; }
        public string Chinese { get; set; }
        public string Pinyin { get; set; }
        public string EnglishCustom { get; set; }

        public Word()
        {

        }
    }
}