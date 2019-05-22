using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using WolDownloader;
using PropertyChanged;
using System.IO;
using PCLStorage;
using System.Diagnostics;
using JWChinese.Helpers;

namespace JWChinese
{
    [AddINotifyPropertyChangedInterface]
    public class PublicationsPageModel : FreshBasePageModel
    {
        //public GEMObservableCollection<object> Publications { get; set; }
        //public GEMObservableCollection<object> PublicationGroups { get; set; }

        public ObservableCollection<PublicationGroup> Publications { get; set; }
        public ObservableCollection<IGrouping<string, PublicationGroup>> PublicationGroups { get; set; }
        public string Title { get; set; }

        //public ObservableCollection<Publication> Pubs { get; set; }

        public int GridViewRowHeight
        {
            get
            {
                if (Device.RuntimePlatform == Device.Windows)
                {
                    return -1;
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    if (JWChinese.Objects.Orientation.Width > 800)
                    {
                        return 100;
                    }
                    else
                    {
                        return 80;
                    }
                }
                else if (Device.RuntimePlatform == Device.iOS)
                {
                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        return 80;
                    }
                    else
                    {
                        return 100;
                    }
                }

                return 0;
            }
        }

        public override async void Init(object initData)
        {
            //Publications = new GEMObservableCollection<object>();
            //PublicationGroups = new GEMObservableCollection<object>();

            //Publications.ReplaceRange(WolLibrary.GetAllPublications().Where(p => p.Language == Language.English).Select((p, index) => new PublicationGroup()
            //{
            //    KeySymbol = p.KeySymbol,
            //    PublicationRootKeyId = p.PublicationRootKeyId.ToString(),
            //    PubType = p.Type,
            //    Title = p.Title,
            //    ShortTitle = p.ShortTitle,
            //    Description = p.ShortTitle,
            //    PublicationImage = p.ImageAsset,
            //    Type = p.Type.ToString()
            //}));

            //PublicationGroups.ReplaceRange(Publications.Cast<PublicationGroup>().GroupBy(p => p.Type));

            Publications = new ObservableCollection<PublicationGroup>();
            PublicationGroups = new ObservableCollection<IGrouping<string, PublicationGroup>>();

            Language langid = (Settings.PrimaryLanguage == LPLanguage.English.GetName()) ? Language.English : Language.Chinese;

            Publications = WolLibrary.GetAllPublications(await App.GetLibraryXML()).Where(p => p.Language == langid).Select((p, index) => new PublicationGroup()
            {
                KeySymbol = p.KeySymbol,
                PublicationRootKeyId = p.PublicationRootKeyId.ToString(),
                Category = p.Category,
                Title = p.Title,
                ShortTitle = p.ShortTitle,
                Description = p.ShortTitle,
                PublicationImage = p.ImageAsset,
                CategoryName = p.Category.ToString()
            }).ToObservableCollection();

            PublicationGroups = Publications.GroupBy(u => u.CategoryName).ToObservableCollection();

            Title = App.GetLanguageValue("Publications", "出版物");

            //Pubs = WolLibrary.GetAllPublications().Where(p => p.Language == Language.English).Select(p => p.ToPublication()).ToObservableCollection();
        }

        public Command PublicationTapped
        {
            get
            {
                return new Command(async (item) =>
                {
                    PublicationGroup pub = item as PublicationGroup;

                    //if (pub.KeySymbol == "es")
                    //{
                    //    //TODO
                    //    FreshTabbedFONavigationContainer tabbedNavigation = new FreshTabbedFONavigationContainer("Awesome");
                    //    tabbedNavigation.BackgroundColor = Color.FromHex("#e8eaed");
                    //    tabbedNavigation.AddTab<DailyTextPageModel>(App.GetLanguageValue("TODAY", "今天"), null);
                    //    tabbedNavigation.AddTab<PublicationContentsPageModel>(App.GetLanguageValue("THIS YEAR", "今年"), null, new PublicationGroup() { KeySymbol = "es", ShortTitle = "" });
                    //    tabbedNavigation.Title = "Awesome";

                    //    //await CoreMethods.PushNewNavigationServiceModal(tabbedNavigation);
                    //    //NavigationPage.SetHasBackButton(tabbedNavigation, true);
                    //    //((MasterDetailPage)Application.Current.MainPage).Detail = tabbedNavigation;

                    //    //await ((NavigationPage)((MasterDetailPage)Application.Current.MainPage).Detail).PushAsync(tabbedNavigation, true);
                    //    //await CurrentPage.Navigation.PushAsync(tabbedNavigation, true);
                    //}
                    //else if (pub.KeySymbol == "it")
                    //{
                    //    //TODO
                    //}
                    //else
                    //{
                    //    await CoreMethods.PushPageModel<PublicationContentsPageModel>(pub);
                    //}


                    await CoreMethods.PushPageModel<PublicationContentsPageModel>(pub);
                });
            }
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class PublicationGroup
    {
        public string KeySymbol { get; set; }
        public string PublicationRootKeyId { get; set; }
        public string ShortTitle { get; set; }
        public PublicationCatagory Category { get; set; }

        private string _publicationImage;
        public string PublicationImage
        {
            get { return _publicationImage; }
            set
            {
                SetPublicationImage(value.Split('/').Last());

                //App.GemWriteLine(_publicationImage);
                //App.GemWriteLine(value);
            }
        }

        public async void SetPublicationImage(string img)
        {
            IFolder rootfolder = FileSystem.Current.LocalStorage;
            ExistenceCheckResult exist = await rootfolder.CheckExistsAsync(img);

            if (exist == ExistenceCheckResult.FileExists)
            {
                _publicationImage = Path.Combine(FileSystem.Current.LocalStorage.Path, img);
            }
            else
            {
                _publicationImage = Path.Combine(FileSystem.Current.LocalStorage.Path, "Icon.jpg");
            }
        }

        private string _alternate;
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (Category == PublicationCatagory.Awake 
                    || Category == PublicationCatagory.Meeting_Workbooks 
                    || Category == PublicationCatagory.Watchtower_Public
                    || Category == PublicationCatagory.Watchtower_Study)
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
                if (Category == PublicationCatagory.Awake 
                    || Category == PublicationCatagory.Meeting_Workbooks 
                    || Category == PublicationCatagory.Watchtower_Public 
                    || Category == PublicationCatagory.Watchtower_Study)
                {
                    _description = value;
                }
                else
                {
                    _description = _alternate;
                }
            }
        }

        string _categoryName;
        public string CategoryName
        {
            get { return _categoryName; }
            set
            {
                _categoryName = value.Replace('_', ' ').ToUpper();
            }
        }
    }
}