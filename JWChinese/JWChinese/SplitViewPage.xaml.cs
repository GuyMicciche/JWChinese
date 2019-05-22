using FreshMvvm;
using System;
using System.Linq;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using PropertyChanged;
using System.Diagnostics;
using JWChinese.Helpers;
using System.Threading.Tasks;

namespace JWChinese
{
    // required temporarily for iOS, due to BaseUrl bug, used with IBaseUrl
    public class BaseUrlWebView : WebView { }
    
    [AddINotifyPropertyChangedInterface]
    public partial class SplitViewPage : MasterDetailPage, IFreshNavigationService
    {
        private static IStorehouseService StorehouseService { get; } = DependencyService.Get<IStorehouseService>();

        ObservableCollection<MasterMenuItem> TopItems { get; set; } = new ObservableCollection<MasterMenuItem>();
        ObservableCollection<MasterMenuItem> BottomItems { get; set; } = new ObservableCollection<MasterMenuItem>();

        public const string HomePageContainer = "HomePageContainer";
        public const string BiblePageContainer = "BiblePageContainer";
        public const string PublicationsPageContainer = "PublicationsPageContainer";
        public const string DictionaryPageContainer = "DictionaryPageContainer";
        public const string SongBookPageContainer = "SongBookPageContainer";
        public const string SettingsPageContainer = "SettingsPageContainer";
        public const string AnotherPageContainer = "AnotherPageContainer";

        private FreshTabbedFONavigationContainer _tabbedNavigationPage;
        private NavigationPage _homePage;
        private NavigationPage _biblePage;
        private NavigationPage _publicationsPage;
        private NavigationPage _dictionaryPage;
        private NavigationPage _songBookPage;
        private NavigationPage _meetingsPage;
        private NavigationPage _settingsPage;

        private string current = "";

        private MasterMenuItem _selectedItem;
        public MasterMenuItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != null)
                {
                    _selectedItem.Selected = false;
                }
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    _selectedItem.Selected = true;
                }

                if (Device.RuntimePlatform != Device.Windows)
                {
                    _selectedItem.Selected = false;
                }
            }
        }

        public string NavigationServiceName => "SplitViewPage";

        public SplitViewPage()
        {
            InitializeComponent();

            CreateMenuItems();
            CreateMenuPages();

            //Menu_ItemSelected(MenuTop, new SelectedItemChangedEventArgs(TopItems[0]));

            FreshIOC.Container.Register<IFreshNavigationService>(this, NavigationServiceName);

            // Stupid Hack because Android LibraryGridView doesn't load elements, says NULL
            new Action(async () =>
            {
                await Task.Delay(200);
                MenuTop.SelectedItem = TopItems[0];

                if (Device.RuntimePlatform == Device.iOS)
                {
                    Master.Icon = "Menu.png";
                    Master.BackgroundColor = Color.White;
                    ((NavigationPage)Detail).BarTextColor = Color.White;
                }
            }).Invoke();
        }

        private void Menu_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            MasterMenuItem item = (MasterMenuItem)e.Item;

            if (item.Title == current)
            {
                if(item.Title == "Home")
                {
                    //_tabbedNavigationPage.SwitchSelectedRootPageModel<DailyTextPageModel>();
                }

                ((NavigationPage)Detail).PopToRootAsync();
                IsPresented = false;
            }

            current = item.Title;
        }

        private void Menu_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            MasterMenuItem item = (MasterMenuItem)e.SelectedItem;

            if (item == null)
            {
                IsPresented = false;
                return;
            }

            if ((sender as ListView) == MenuTop)
            {
                MenuBottom.SelectedItem = null;
            }
            else if ((sender as ListView) == MenuBottom)
            {
                MenuTop.SelectedItem = null;
            }

            switch (item.Page)
            {
                case PageItem.Home:
                    Detail = _tabbedNavigationPage;
                    //Detail = _homePage;
                    break;
                case PageItem.Bible:
                    Detail = _biblePage;
                    break;
                case PageItem.Publications:
                    Detail = _publicationsPage;
                    break;
                case PageItem.Dictionary:
                    Detail = _dictionaryPage;
                    break;
                case PageItem.SongBook:
                    Detail = _tabbedNavigationPage;
                    break;
                case PageItem.Settings:
                    Detail = _settingsPage;
                    break;
                default:
                    break;
            }

            Detail.Title = item.Title;
            IsPresented = false;

            SelectedItem = item;

            if (Device.RuntimePlatform == Device.iOS)
            {
                ((NavigationPage)Detail).BarTextColor = Color.White;
            }

            if (Device.RuntimePlatform == Device.Android)
            {
                (sender as ListView).SelectedItem = null;
            }
        }

        protected void CreateMenuPages()
        {
            //var page = FreshPageModelResolver.ResolvePageModel<ArticlePageModel>();
            //page.Title = "Home";
            //_homePage = new FreshNavigationContainer(page, HomePageContainer);

            // HOME PAGE
            //_tabbedNavigationPage = new FreshTabbedFONavigationContainer(App.GetLanguageValue("Home", "首页"));
            //_tabbedNavigationPage.BackgroundColor = Color.FromHex("#e8eaed");
            //_tabbedNavigationPage.AddTab<DailyTextPageModel>(App.GetLanguageValue("Daily Text", "每日经文"), null); 
            //_tabbedNavigationPage.AddTab<DictionaryPageModel>(App.GetLanguageValue("My Dictionary", "我的字典"), null);

            //var homePage = FreshPageModelResolver.ResolvePageModel<DailyTextPageModel>();
            //homePage.Title = "Home";
            //_homePage = new FreshNavigationContainer(homePage, HomePageContainer);

            // BIBLE
            var biblePage = FreshPageModelResolver.ResolvePageModel<BiblePageModel>();
            biblePage.Title = App.GetLanguageValue("Bible", "圣经");
            _biblePage = new FreshNavigationContainer(biblePage, BiblePageContainer);

            // PUBLICATIONS
            var publicationsPage = FreshPageModelResolver.ResolvePageModel<PublicationsPageModel>();
            publicationsPage.Title = App.GetLanguageValue("Publications", "出版物");
            _publicationsPage = new FreshNavigationContainer(publicationsPage, PublicationsPageContainer);
           
            // DICTIONARY
            var dictionaryPage = FreshPageModelResolver.ResolvePageModel<DictionaryPageModel>();
            dictionaryPage.Title = App.GetLanguageValue("Dictionary", "字典");
            _dictionaryPage = new FreshNavigationContainer(dictionaryPage, DictionaryPageContainer);

            // SONGBOOK
            //var songBookPage = FreshPageModelResolver.ResolvePageModel<SongBookPageModel>();
            //songBookPage.Title = App.GetLanguageValue("“Sing Out Joyfully”", "高声欢唱");
            //_songBookPage = new FreshNavigationContainer(songBookPage, SongBookPageContainer);

            _tabbedNavigationPage = new FreshTabbedFONavigationContainer(App.GetLanguageValue("“Sing Out Joyfully”", "高声欢唱"));
            _tabbedNavigationPage.BackgroundColor = Color.FromHex("#e8eaed");
            _tabbedNavigationPage.AddTab<SongBookPageModel>(App.GetLanguageValue("NUMBER", "编号"), null);
            _tabbedNavigationPage.AddTab<PublicationContentsPageModel>(App.GetLanguageValue("TITLE", "歌名"), null, new PublicationGroup() { KeySymbol = "sjj", ShortTitle = "" });

            var page = FreshPageModelResolver.ResolvePageModel<TestPageModel>();
            page.Title = "Meetings";
            _meetingsPage = new FreshNavigationContainer(page, AnotherPageContainer + "4");

            // SETTINGS
            var settingsPage = FreshPageModelResolver.ResolvePageModel<SettingsPageModel>();
            settingsPage.Title = App.GetLanguageValue("Settings", "设置");
            _settingsPage = new FreshNavigationContainer(settingsPage, SettingsPageContainer + "5");
        }

        protected void CreateMenuItems()
        {
            //TopItems.Add(new MasterMenuItem
            //{
            //    Title = App.GetLanguageValue("Home", "首页"),
            //    IconSource = String.Format("{0}{1}", Device.OnPlatform("", "", "Assets/"), "nav_home.png"),
            //    Page = PageItem.Home
            //});
            TopItems.Add(new MasterMenuItem
            {
                Title = App.GetLanguageValue("Bible", "圣经"),
                IconSource = String.Format("{0}{1}", Device.OnPlatform("", "", "Assets/"), "nav_bible.png"),
                Page = PageItem.Bible
            });
            TopItems.Add(new MasterMenuItem
            {
                Title = App.GetLanguageValue("“Sing Out Joyfully”", "高声欢唱"),
                IconSource = String.Format("{0}{1}", Device.OnPlatform("", "", "Assets/"), "nav_songbook.png"),
                Page = PageItem.SongBook
            });
            TopItems.Add(new MasterMenuItem
            {
                Title = App.GetLanguageValue("Publications", "出版物"),
                IconSource = String.Format("{0}{1}", Device.OnPlatform("", "", "Assets/"), "nav_publications.png"),
                Page = PageItem.Publications
            });
            TopItems.Add(new MasterMenuItem
            {
                Title = App.GetLanguageValue("Dictionary", "字典"),
                IconSource = String.Format("{0}{1}", Device.OnPlatform("", "", "Assets/"), "nav_dictionary.png"),
                Page = PageItem.Dictionary
            });
            //TopItems.Add(new MasterMenuItem
            //{
            //    Title = "Media",
            //    IconSource = String.Format("{0}{1}", Device.OnPlatform("", "", "Assets/"), "nav_media.png"),
            //});
            //TopItems.Add(new MasterMenuItem
            //{
            //    Title = "Meetings",
            //    IconSource = String.Format("{0}{1}", Device.OnPlatform("", "", "Assets/"), "nav_meetings.png"),
            //});

            var settings = new MasterMenuItem
            {
                Title = App.GetLanguageValue("Settings", "设置"),
                IconSource = String.Format("{0}{1}", Device.OnPlatform("", "", "Assets/"), "nav_settings.png"),
                Page = PageItem.Settings
            };

            if (Device.RuntimePlatform == Device.Windows)
            {
                BottomItems.Add(settings);
            }
            else
            {
                BottomItems.Add(settings);
            }

            MenuTop.ItemsSource = TopItems;
            MenuBottom.ItemsSource = BottomItems;

            if (Device.RuntimePlatform == Device.Windows)
            {
                MenuTop.Header = null;
                MenuTop.HeightRequest = (TopItems.Count() * 48);
                MenuBottom.HeightRequest = (BottomItems.Count() * 48) + 1;
            }
            else
            {
                MenuTop.HeightRequest = ((TopItems.Count() + 1) * 60) + 200;
                MenuTop.SeparatorVisibility = SeparatorVisibility.None;
                MenuBottom.HeightRequest = (BottomItems.Count() * 60) + 1 + 1;
                //MenuBottom.HeightRequest = 1;
            }

            //if (Device.RuntimePlatform == Device.Android)
            //{
            //    var _menuPage = new ContentPage();
            //    _menuPage.Title = "FreshMvvmSampleApp";
            //    _menuPage.Content = SplitViewPane;

            //    Master = new NavigationPage(_menuPage) { Title = "FreshMvvmSampleApp" };
            //}
            //else
            //{

            //}

#if __ANDROID__
#endif

#if __ANDROID__
            
#endif

        }

        public Task PopToRoot(bool animate = true)
        {
            throw new NotImplementedException();
        }

        public Task PushPage(Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
        {
            throw new NotImplementedException();
        }

        public Task PopPage(bool modal = false, bool animate = true)
        {
            throw new NotImplementedException();
        }

        public Task<FreshBasePageModel> SwitchSelectedRootPageModel<T>() where T : FreshBasePageModel
        {
            throw new NotImplementedException();
        }

        public void NotifyChildrenPageWasPopped()
        {
            throw new NotImplementedException();
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class MasterMenuItem
    {
        public PageItem Page { get; set; }
        public string Title { get; set; }
        public string IconSource { get; set; }
        public bool Selected { get; set; }

        public MasterMenuItem()
        {

        }
    }

    public enum PageItem
    {
        Home = 0,
        Bible = 1,
        Publications = 2,
        Dictionary = 3,
        SongBook = 4,
        Settings = 5
    }
}