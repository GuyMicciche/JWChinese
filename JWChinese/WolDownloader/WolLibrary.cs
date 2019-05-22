using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WolDownloader
{
    public class WolLibrary
    {
        private const string BASE_PUBLICATION = "http://download-a.akamaihd.net/";
        private const string BASE_IMAGE_ASSET = "http://download-a.akamaihd.net/meps/jwl/current/assets/";

        private const string LIBRARY_XML = "WolDownloader.Assets.Library.Library.xml";
        private const string LIBRARY_JSON = "WolDownloader.Assets.Library.Library.json";

        public static List<BibleBook> GetAllBibleBooks(string libraryXml = null)
        {
            List<BibleBook> bibleBooks = new List<BibleBook>();

            var assembly = typeof(WolLibrary).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(LIBRARY_XML);
            using (StreamReader reader = new StreamReader(stream))
            {
                string xml = string.IsNullOrEmpty(libraryXml) ? reader.ReadToEnd() : libraryXml;

                XDocument doc = XDocument.Parse(xml);
                foreach (XElement bb in doc.Descendants("Bible").Elements())
                {
                    foreach (XElement lang in bb.Elements())
                    {
                        BibleBook bibleBook = new BibleBook()
                        {
                            PublicationRootKeyId = int.Parse(bb.Parent.Attribute("PublicationRootKeyId").Value),
                            BookNumber = int.Parse(bb.Attribute("BookNumber").Value),
                            Chapters = int.Parse(bb.Attribute("Chapters").Value),
                            LanguageName = lang.Name.LocalName,
                            MepsLanguageId = int.Parse(lang.Attribute("MepsLanguageId").Value),
                            OfficialBookAbbreviation = lang.Attribute("OfficialBookAbbreviation").Value,
                            StandardBookAbbreviation = lang.Attribute("StandardBookAbbreviation").Value,
                            StandardBookName = lang.Attribute("StandardBookName").Value,
                            Group = (BibleBookGroup)int.Parse(bb.Attribute("BibleBookGroup").Value)
                        };

                        bibleBooks.Add(bibleBook);
                    }
                }
            }

            return bibleBooks;
        }

        public static List<Publication> GetAllPublications(string libraryXml = null)
        {
            List<Publication> publications = new List<Publication>();

            var assembly = typeof(WolDownloader).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(LIBRARY_XML);
            using (StreamReader reader = new StreamReader(stream))
            {
                string xml = string.IsNullOrEmpty(libraryXml) ? reader.ReadToEnd() : libraryXml; 

                XDocument doc = XDocument.Parse(xml);

                // Insight
                foreach (XElement pub in doc.Descendants("Publications").Elements("Insight"))
                {
                    foreach (XElement lang in pub.Elements())
                    {
                        Publication publication = new Publication()
                        {
                            Category = PublicationCatagory.Insight,
                            KeySymbol = pub.Attribute("KeySymbol").Value,
                            PublicationRootKeyId = int.Parse(pub.Attribute("PublicationRootKeyId").Value),
                            ImageAsset = (pub.Attribute("ImageAsset").Value != "null" ) ? BASE_IMAGE_ASSET + pub.Attribute("ImageAsset").Value : "",
                            LanguageName = lang.Name.LocalName,
                            MepsLanguageId = int.Parse(lang.Attribute("MepsLanguageId").Value),
                            Language = (Language)int.Parse(lang.Attribute("MepsLanguageId").Value),
                            Title = lang.Attribute("Title").Value,
                            ShortTitle = lang.Attribute("ShortTitle").Value,
                            NameFragment = ""

                        };

                        publications.Add(publication);
                    }
                }

                // DailyText
                foreach (XElement pub in doc.Descendants("Publications").Elements("DailyText"))
                {
                    foreach (XElement lang in pub.Elements())
                    {
                        Publication publication = new Publication()
                        {
                            Category = PublicationCatagory.Daily_Text,
                            KeySymbol = pub.Attribute("KeySymbol").Value,
                            PublicationRootKeyId = int.Parse(pub.Attribute("PublicationRootKeyId").Value),
                            ImageAsset = BASE_IMAGE_ASSET + pub.Attribute("ImageAsset").Value,
                            LanguageName = lang.Name.LocalName,
                            MepsLanguageId = int.Parse(lang.Attribute("MepsLanguageId").Value),
                            Language = (Language)int.Parse(lang.Attribute("MepsLanguageId").Value),
                            Title = lang.Attribute("Title").Value,
                            ShortTitle = lang.Attribute("ShortTitle").Value,
                            NameFragment = ""

                        };

                        publications.Add(publication);
                    }
                }

                // Books
                foreach (XElement pub in doc.Descendants("Publications").Elements("Book"))
                {
                    foreach (XElement lang in pub.Elements())
                    {
                        Publication publication = new Publication()
                        {
                            Category = PublicationCatagory.Books,
                            KeySymbol = pub.Attribute("KeySymbol").Value,
                            PublicationRootKeyId = int.Parse(pub.Attribute("PublicationRootKeyId").Value),
                            ImageAsset = BASE_IMAGE_ASSET + pub.Attribute("ImageAsset").Value,
                            LanguageName = lang.Name.LocalName,
                            MepsLanguageId = int.Parse(lang.Attribute("MepsLanguageId").Value),
                            Language = (Language)int.Parse(lang.Attribute("MepsLanguageId").Value),
                            Title = lang.Attribute("Title").Value,
                            ShortTitle = lang.Attribute("ShortTitle").Value,
                            NameFragment = ""

                        };

                        publications.Add(publication);
                    }
                }

                // Brochures
                foreach (XElement pub in doc.Descendants("Publications").Elements("Brochure"))
                {
                    foreach (XElement lang in pub.Elements())
                    {
                        Publication publication = new Publication()
                        {
                            Category = PublicationCatagory.Brochures,
                            KeySymbol = pub.Attribute("KeySymbol").Value,
                            PublicationRootKeyId = int.Parse(pub.Attribute("PublicationRootKeyId").Value),
                            ImageAsset = BASE_IMAGE_ASSET + pub.Attribute("ImageAsset").Value,
                            LanguageName = lang.Name.LocalName,
                            MepsLanguageId = int.Parse(lang.Attribute("MepsLanguageId").Value),
                            Language = (Language)int.Parse(lang.Attribute("MepsLanguageId").Value),
                            Title = lang.Attribute("Title").Value,
                            ShortTitle = lang.Attribute("ShortTitle").Value,
                            NameFragment = ""

                        };

                        publications.Add(publication);
                    }
                }

                // Tracts
                foreach (XElement pub in doc.Descendants("Publications").Elements("Tract"))
                {
                    foreach (XElement lang in pub.Elements())
                    {
                        Publication publication = new Publication()
                        {
                            Category = PublicationCatagory.Tracts,
                            KeySymbol = pub.Attribute("KeySymbol").Value,
                            PublicationRootKeyId = int.Parse(pub.Attribute("PublicationRootKeyId").Value),
                            ImageAsset = BASE_IMAGE_ASSET + pub.Attribute("ImageAsset").Value,
                            LanguageName = lang.Name.LocalName,
                            MepsLanguageId = int.Parse(lang.Attribute("MepsLanguageId").Value),
                            Language = (Language)int.Parse(lang.Attribute("MepsLanguageId").Value),
                            Title = lang.Attribute("Title").Value,
                            ShortTitle = lang.Attribute("ShortTitle").Value,
                            NameFragment = ""

                        };

                        publications.Add(publication);
                    }
                }

                // The Watchtower Study Edition
                foreach (XElement pub in doc.Descendants("Publications").Elements("WatchtowerStudy"))
                {
                    foreach (XElement lang in pub.Elements())
                    {
                        Publication publication = new Publication()
                        {
                            Category = PublicationCatagory.Watchtower_Study,
                            KeySymbol = pub.Attribute("KeySymbol").Value,
                            PublicationRootKeyId = int.Parse(pub.Attribute("PublicationRootKeyId").Value),
                            ImageAsset = BASE_IMAGE_ASSET + pub.Attribute("ImageAsset").Value,
                            LanguageName = lang.Name.LocalName,
                            MepsLanguageId = int.Parse(lang.Attribute("MepsLanguageId").Value),
                            Language = (Language)int.Parse(lang.Attribute("MepsLanguageId").Value),
                            Title = lang.Attribute("Title").Value,
                            ShortTitle = lang.Attribute("ShortTitle").Value,
                            NameFragment = ""

                        };

                        publications.Add(publication);
                    }
                }

                // Meeting Workbook
                foreach (XElement pub in doc.Descendants("Publications").Elements("MeetingWorkbook"))
                {
                    foreach (XElement lang in pub.Elements())
                    {
                        Publication publication = new Publication()
                        {
                            Category = PublicationCatagory.Meeting_Workbooks,
                            KeySymbol = pub.Attribute("KeySymbol").Value,
                            PublicationRootKeyId = int.Parse(pub.Attribute("PublicationRootKeyId").Value),
                            ImageAsset = BASE_IMAGE_ASSET + pub.Attribute("ImageAsset").Value,
                            LanguageName = lang.Name.LocalName,
                            MepsLanguageId = int.Parse(lang.Attribute("MepsLanguageId").Value),
                            Language = (Language)int.Parse(lang.Attribute("MepsLanguageId").Value),
                            Title = lang.Attribute("Title").Value,
                            ShortTitle = lang.Attribute("ShortTitle").Value,
                            NameFragment = ""

                        };

                        publications.Add(publication);
                    }
                }

                // The Watchtower Public Edition
                foreach (XElement pub in doc.Descendants("Publications").Elements("WatchtowerPublic"))
                {
                    foreach (XElement lang in pub.Elements())
                    {
                        Publication publication = new Publication()
                        {
                            Category = PublicationCatagory.Watchtower_Public,
                            KeySymbol = pub.Attribute("KeySymbol").Value,
                            PublicationRootKeyId = int.Parse(pub.Attribute("PublicationRootKeyId").Value),
                            ImageAsset = BASE_IMAGE_ASSET + pub.Attribute("ImageAsset").Value,
                            LanguageName = lang.Name.LocalName,
                            MepsLanguageId = int.Parse(lang.Attribute("MepsLanguageId").Value),
                            Language = (Language)int.Parse(lang.Attribute("MepsLanguageId").Value),
                            Title = lang.Attribute("Title").Value,
                            ShortTitle = lang.Attribute("ShortTitle").Value,
                            NameFragment = ""

                        };

                        publications.Add(publication);
                    }
                }

                // Awake!
                foreach (XElement pub in doc.Descendants("Publications").Elements("Awake"))
                {
                    foreach (XElement lang in pub.Elements())
                    {
                        Publication publication = new Publication()
                        {
                            Category = PublicationCatagory.Awake,
                            KeySymbol = pub.Attribute("KeySymbol").Value,
                            PublicationRootKeyId = int.Parse(pub.Attribute("PublicationRootKeyId").Value),
                            ImageAsset = BASE_IMAGE_ASSET + pub.Attribute("ImageAsset").Value,
                            LanguageName = lang.Name.LocalName,
                            MepsLanguageId = int.Parse(lang.Attribute("MepsLanguageId").Value),
                            Language = (Language)int.Parse(lang.Attribute("MepsLanguageId").Value),
                            Title = lang.Attribute("Title").Value,
                            ShortTitle = lang.Attribute("ShortTitle").Value,
                            NameFragment = ""

                        };

                        publications.Add(publication);
                    }
                }
            }

            return publications;
        }
    }
}
