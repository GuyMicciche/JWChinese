using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolDownloader
{
    public class Publication
    {
        public string KeySymbol { get; set; }
        public PublicationCatagory Category { get; set; }
        public int PublicationRootKeyId { get; set; }
        public string ImageAsset { get; set; }
        public string NameFragment { get; set; }
        public int MepsLanguageId { get; set; }
        public string LanguageName { get; set; }
        public Language Language { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }
    }
}
