using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolDownloader
{
    public class BibleBook
    {
        public int PublicationRootKeyId { get; set; }
        public int BookNumber { get; set; }
        public int Chapters { get; set; }
        public int MepsLanguageId { get; set; }
        public string LanguageName { get; set; }
        public string OfficialBookAbbreviation { get; set; }
        public string StandardBookAbbreviation { get; set; }
        public string StandardBookName { get; set; }
        public BibleBookGroup Group { get; set; }
    }
}
