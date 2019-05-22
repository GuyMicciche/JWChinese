using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolDownloader;
using Xamarin.Forms;

namespace JWChinese
{
    public class Publication
    {
        public FileImageSource PublicationImageSource { get; set; }
        public PublicationCatagory Type { get; set; }

        private string _publicationImage;
        public string PublicationImage
        {
            get { return _publicationImage; }
            set
            {
                _publicationImage = Path.Combine(FileSystem.Current.LocalStorage.Path, value.Split('/').Last());
            }
        }

        private string _alternate;
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if(Type == PublicationCatagory.Awake || Type == PublicationCatagory.Meeting_Workbooks || Type == PublicationCatagory.Watchtower_Public || Type == PublicationCatagory.Watchtower_Study)
                {
                    _title = value;
                }
                else
                {
                    _title = string.Empty;
                    _alternate = value;
                }
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                if (Type == PublicationCatagory.Awake || Type == PublicationCatagory.Meeting_Workbooks || Type == PublicationCatagory.Watchtower_Public || Type == PublicationCatagory.Watchtower_Study)
                {
                    _description = value;
                }
                else
                {
                    _description = _alternate;
                }
            }
        }

        public FileImageSource StatusMessageFileSource { get; set; }
        public string StatusMessage { get; set; }
        public FileImageSource ActionMessageFileSource { get; set; }
        public string ActionMessage { get; set; }
        public ContentView MessageView { get; set; }
        public ContentView ActionView { get; set; }
        public DateTime DueDate { get; set; }
        public int DirationInMinutes { get; set; }
    }
}