using FreshMvvm;
using ICSharpCode.SharpZipLib.Zip;
using JWChinese.Helpers;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WolDownloader;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;

// INSTALL DIRECTORY FOR FILES AND DATABASES
//C:\Users\Guy\AppData\Local\Packages\e52633c9-9b18-4b99-b5f0-c7ced58cfd21_9kp3639my2q66\LocalState

namespace JWChinese
{
    public partial class App : Application
    {
        //public static IStorehouseService StorehouseService { get; } = DependencyService.Get<IStorehouseService>();

        public static double AndroidDPI { get; set; }
        public static int IOSSizeProportion { get; set; }

        public static double TextSettingsBase = 80;
        public static string PrimaryLanguageBase = "lp-e";
        public static string SecondaryLanguageBase = "lp-chs";

        public const string DatabaseName = "storehouse";

        public static string LIBRARY = Path.Combine(FileSystem.Current.LocalStorage.Path, "Library.xml");

        public static ObservableCollection<Word> CurrentArticleWords = new ObservableCollection<Word>();

        public static void GemWriteLine(object obj)
        {
            Debug.WriteLine(obj.GetType().ToString() + " => " + obj.ToString());
        }

        public static void GemWriteLine(string str, object obj)
        {
            Debug.WriteLine(str + " => " + obj.ToString());
        }

        public static double GetTextSettings(double value)
        {
            return Math.Floor((value * 100) + TextSettingsBase);
        }

        public static string GetLanguageValue(string e, string ch)
        {
            return (Settings.PrimaryLanguage == LPLanguage.English.GetName()) ? e : ch;
        }

        public static void GemWriteLine(string str)
        {
            Debug.WriteLine(str);
        }

        public ActivityIndicator LoadingActivityIndicator = new ActivityIndicator();
        public ProgressBar DownloadProgress = new ProgressBar();

        private Label StatusLabel = new Label();

        public App()
        {
            InitializeComponent();

            // TEMPORARY UNTIL YOU MAKE THE DOWNLOADING DATABASE PAGE
            DisplayLoadingPage();

            CheckDatabase();

            //DownloadArticlesToStorehouse(pubs:new List<string>(){ "lf","gf","we","ypq","rj"});
            //DownloadArticlesToStorehouse(types:new List<ArticleType>(){ ArticleType.DailyText});
            //DownloadArticlesToStorehouse(true, null, null);

            DownloadPublicationImages();

            // Counts the articles
            //Test();
        }

        private void DisplayLoadingPage()
        {
            StatusLabel = new Label()
            {
                Text = "Loading...",
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Center
            };

            LoadingActivityIndicator = new ActivityIndicator() { IsRunning = true };
            LoadingActivityIndicator.IsVisible = true;

            DownloadProgress = new ProgressBar
            {
                IsVisible = false,
            };

            if(Device.RuntimePlatform == Device.iOS)
            {
                DownloadProgress.BackgroundColor = Color.FromHex("#2f64a8");
            }

            var viewWithStack = new ContentView
            {
                BackgroundColor = Color.FromHex("#662f64a8"),
                Content = new StackLayout()
                {
                    Orientation = StackOrientation.Vertical,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Children = { StatusLabel, LoadingActivityIndicator, DownloadProgress }
                }
            };

            MainPage = new ContentPage()
            {
                BackgroundColor = Color.FromHex("#000000"),
                Content = viewWithStack
            };
        }

        // This is called after database is downloaded and extracted
        private void DisplayMainPage()
        {
            FreshIOC.Container.Register<IStorehouseService, StorehouseService>();

            Device.BeginInvokeOnMainThread(() =>
            {
                if(Device.RuntimePlatform != Device.iOS)
                {
                    MainPage = new SplitViewPage();
                    MainPage.On<Windows>().SetToolbarPlacement(ToolbarPlacement.Top);
                }
                else
                {
                    var tabbedNavigation = new FreshTabbedNavigationContainer(Guid.NewGuid().ToString());
                    tabbedNavigation.AddTab<BiblePageModel>(GetLanguageValue("Bible", "圣经"), "TabBible.png", null);
                    tabbedNavigation.AddTab<SongBookPageModel>(GetLanguageValue("“Sing Out Joyfully”", "高声欢唱"), "TabSongbook.png", null);
                    tabbedNavigation.AddTab<PublicationsPageModel>(GetLanguageValue("Publications", "出版物"), "TabPublications.png", null);
                    tabbedNavigation.AddTab<DictionaryPageModel>(GetLanguageValue("Dictionary", "字典"), "TabDictionary.png", null);
                    tabbedNavigation.AddTab<SettingsPageModel>(GetLanguageValue("Settings", "设置"), "TabSettings.png", null);
                    MainPage = tabbedNavigation;
                }
            });
        }

        private async void Test()
        {
            IEnumerable<Article> articles = await StorehouseService.Instance.GetArticlesAsync();
            GemWriteLine(articles.Count());
        }

        private async void CheckDatabase()
        {
            // IFolder interface comes from PCLStorage, check if database file is there
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            ExistenceCheckResult exists = await rootFolder.CheckExistsAsync(DatabaseName + ".db3");
            if (exists == ExistenceCheckResult.FileExists)
            {
                DisplayMainPage();

                return;
            }            

            byte[] bytes;

            // DOWNLOADING THE DATABASE OR...
            //bytes = await GetRemoteDatabase(DatabaseName);

            // SHIPPING THE DATABASE ORIGINAL
            bytes = GetShippedDatabase(DatabaseName);

            await UnpackZipFile(bytes).ContinueWith(async (antecedent) =>
            {
                await CreateStorehouseAsync();

                DisplayMainPage();
            });
        }

        public async void UpdateDatabase()
        {
            ObservableCollection<Word> Words = new ObservableCollection<Word>(await StorehouseService.Instance.GetWordsAsync()).ToObservableCollection();

            Device.BeginInvokeOnMainThread(() =>
            {
                DisplayLoadingPage();
            });

            StorehouseService.Instance.CloseStore();

            // Delete unwanted files, including the database amd library xml
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            string[] filesToDelete = { DatabaseName + ".zip", DatabaseName + ".db3", "Library.xml" };
            foreach (var f in filesToDelete)
            {
                IFile file = await rootFolder.CreateFileAsync(f, CreationCollisionOption.OpenIfExists);
                await file.DeleteAsync();
            }

            var fileName ="storehouse.zip";
            var downloadDirectory = "http://jwguy.com/storehouse.zip";
            var destinationFolder = FileSystem.Current.LocalStorage;

            using (var client = new HttpClientDownloadWithProgress(downloadDirectory, fileName))
            {
                client.ProgressChanged += (totalFileSize, totalBytesDownloaded, progressPercentage) => {
                    DownloadProgress.IsVisible = true;
                    LoadingActivityIndicator.IsVisible = false;
                    DownloadProgress.Progress = progressPercentage.Value/100;
                    StatusLabel.Text = Math.Floor(progressPercentage.Value).ToString() + " %";
                    Debug.WriteLine($"{progressPercentage}% ({totalBytesDownloaded}/{totalFileSize})");
                };

                await client.StartDownload();
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                DisplayLoadingPage();
            });

            await UnpackZipFile(destinationFolder, fileName).ContinueWith(async (antecedent) =>
            {
                await CreateStorehouseAsync();

                foreach (var word in Words)
                {
                    await StorehouseService.Instance.AddWordAsync(word);
                }

                DisplayMainPage();
            });
        }

        private byte[] GetShippedDatabase(string database)
        {
            byte[] bytes;
            var assembly = typeof(App).GetTypeInfo().Assembly;
            using (Stream s = assembly.GetManifestResourceStream("JWChinese." + database + ".zip"))
            {
                long length = s.Length;
                bytes = new byte[length];
                s.Read(bytes, 0, (int)length);
            }

            return bytes;
        }

        private async Task<byte[]> GetRemoteDatabase(string database)
        {
            string databaseUrl = "http://jwguy.com/" + database + ".zip";

            Uri url = new Uri(databaseUrl, UriKind.Absolute);
            var client = new HttpClient();

            var response = await client.GetAsync(url);

            return await response.Content.ReadAsByteArrayAsync();
        }

        public static async Task<string> GetLibraryXML()
        {
            string xml = "";

            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFile file = await rootFolder.CreateFileAsync("Library.xml", CreationCollisionOption.OpenIfExists);
            using (Stream stream = await file.OpenAsync(FileAccess.ReadAndWrite))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    xml = reader.ReadToEnd();
                }
            }

            return xml;
        }

        private async Task UnpackZipFile(byte[] zipFileBytes)
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFile file = await rootFolder.CreateFileAsync("storehouse.zip", CreationCollisionOption.ReplaceExisting);
            using (Stream stream = await file.OpenAsync(FileAccess.ReadAndWrite))
            {
                await stream.WriteAsync(zipFileBytes, 0, zipFileBytes.Length);
                using (var zf = new ZipFile(stream))
                {
                    foreach (ZipEntry zipEntry in zf)
                    {
                        // Gete Entry Stream.
                        Stream zipEntryStream = zf.GetInputStream(zipEntry);

                        // Create the file in filesystem and copy entry stream to it.
                        IFile zipEntryFile = await rootFolder.CreateFileAsync(zipEntry.Name, CreationCollisionOption.ReplaceExisting);
                        using (Stream outPutFileStream = await zipEntryFile.OpenAsync(FileAccess.ReadAndWrite))
                        {
                            await zipEntryStream.CopyToAsync(outPutFileStream);
                        }
                    }
                }
            }
        }

        private async Task UnpackZipFile(IFolder directory, string zipFile)
        {
            IFile file = await directory.GetFileAsync(zipFile);
            using (Stream stream = await file.OpenAsync(FileAccess.ReadAndWrite))
            {
                using (var zf = new ZipFile(stream))
                {
                    foreach (ZipEntry zipEntry in zf)
                    {
                        // Gete Entry Stream.
                        Stream zipEntryStream = zf.GetInputStream(zipEntry);

                        // Create the file in filesystem and copy entry stream to it.
                        IFile zipEntryFile = await directory.CreateFileAsync(zipEntry.Name, CreationCollisionOption.ReplaceExisting);
                        using (Stream outPutFileStream = await zipEntryFile.OpenAsync(FileAccess.ReadAndWrite))
                        {
                            await zipEntryStream.CopyToAsync(outPutFileStream);
                        }
                    }
                }
            }
        }

        private async void DownloadPublicationImages()
        {
            foreach (var pub in WolLibrary.GetAllPublications(await GetLibraryXML()).Where(p => !p.ImageAsset.Contains("null")))
            {
                //GemWriteLine(pub.Title);
                await DownloadImage(pub.ImageAsset, pub.ImageAsset.Split('/').Last());
            }
        }

        public static async Task DownloadImage(string url, string name)
        {
            try
            {
                IFolder rootfolder = FileSystem.Current.LocalStorage;
                ExistenceCheckResult exist = await rootfolder.CheckExistsAsync(name);

                if (exist == ExistenceCheckResult.NotFound)
                {
                    GemWriteLine("DOWNLOADING... " + url);

                    Uri uri = new Uri(url);
                    var client = new HttpClient();

                    IFile file = await rootfolder.CreateFileAsync(name, CreationCollisionOption.OpenIfExists);
                    using (var fileHandler = await file.OpenAsync(FileAccess.ReadAndWrite))
                    {
                        var httpResponse = await client.GetAsync(uri);
                        byte[] dataBuffer = await httpResponse.Content.ReadAsByteArrayAsync();
                        await fileHandler.WriteAsync(dataBuffer, 0, dataBuffer.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                GemWriteLine("DownloadImage", ex.Message);
            }
        }

        private async void DownloadArticlesToStorehouse(bool updateMonthly = false, List<ArticleType> types = null, List<string> pubs = null)
        {
            Debug.WriteLine("START => " + DateTime.Now);

            if (updateMonthly)
            {
                await UpdateMonthlyPublications();
            }

            if (pubs != null)
            {
                foreach(string pub in pubs)
                {
                    await DownloadPublication(pub, ArticleType.Publication);
                }
            }

            if (types != null)
            {
                foreach (ArticleType code in types)
                {
                    if (code == ArticleType.Publication)
                    {
                        // Download Books, Brochures, and Tracts
                        foreach (var pub in WolLibrary.GetAllPublications(await GetLibraryXML())
                            .Where(p => p.Language == Language.English)
                            .Where(p => (p.KeySymbol != "es") && (p.KeySymbol != "it") && !p.KeySymbol.StartsWith("mwb-") && !p.KeySymbol.StartsWith("w-") && !p.KeySymbol.StartsWith("wp-") && !p.KeySymbol.StartsWith("g-")))
                        {
                            await DownloadPublication(pub.KeySymbol, ArticleType.Publication);
                            GemWriteLine(pub.KeySymbol);
                        }
                    }

                    if (code == ArticleType.DailyText)
                    {
                        // Download Daily Text
                        await DownloadPublication("es", ArticleType.DailyText);
                    }

                    if (code == ArticleType.Insight)
                    {
                        // Download Insight Book
                        await DownloadPublication("it", ArticleType.Insight);
                    }

                    if (code == ArticleType.Bible)
                    {
                        // Download Bible
                        await DownloadPublication("nwt", ArticleType.Bible);
                    }

                    if (code == ArticleType.Monthly)
                    {
                        // Download meeting Workbook
                        await DownloadPublication("mwb", ArticleType.Monthly);

                        // Download Watchtower Study Edition
                        await DownloadPublication("w", ArticleType.Monthly);

                        // Download Watchtower Public
                        await DownloadPublication("wp", ArticleType.Monthly);

                        // Download Awake!
                        await DownloadPublication("g", ArticleType.Monthly);
                    }
                }
            }

            Debug.WriteLine("END => " + DateTime.Now);
        }

        private async Task DownloadPublication(string symbol, ArticleType type)
        {
            //IEnumerable<Article> articles = await StorehouseService.Instance.GetArticlesAsync();

            foreach (var article in await WolDownloader.WolDownloader.GeneratePublicationAsync(symbol, type))
            {
                await StorehouseService.Instance.AddArticleAsync(article.ToArticle());

                WolDownloader.WolDownloader.DebugArticleOutput(article);
            }
        }

        private async Task UpdateMonthlyPublications()
        {
            List<string> meps = new List<string>((await StorehouseService.Instance.GetArticlesAsync())
                .Where(a => NavStruct.Parse(a.MepsID).Publication.StartsWith("mwb-") || NavStruct.Parse(a.MepsID).Publication.StartsWith("w-") || NavStruct.Parse(a.MepsID).Publication.StartsWith("wp-") || NavStruct.Parse(a.MepsID).Publication.StartsWith("g-"))
                .Select(a => a.MepsID));

            GemWriteLine("MEPS Count", meps.Count);

            // Meeting Workbook
            foreach (var article in await WolDownloader.WolDownloader.GenerateMonthlyPublicationAsync("mwb", ArticleType.Monthly, meps))
            {
                try
                {
                    await StorehouseService.Instance.AddArticleAsync(article.ToArticle());

                    WolDownloader.WolDownloader.DebugArticleOutput(article);
                }
                catch (Exception e) { Debug.WriteLine(e.Message); }
            }
            // Watchtower Study Edition
            foreach (var article in await WolDownloader.WolDownloader.GenerateMonthlyPublicationAsync("w", ArticleType.Monthly, meps))
            {
                try
                {
                    await StorehouseService.Instance.AddArticleAsync(article.ToArticle());

                    //WolDownloader.WolDownloader.DebugArticleOutput(article);
                }
                catch (Exception e) { Debug.WriteLine(e.Message); }
            }
            // Watchtower Public
            foreach (var article in await WolDownloader.WolDownloader.GenerateMonthlyPublicationAsync("wp", ArticleType.Monthly, meps))
            {
                try
                {
                    await StorehouseService.Instance.AddArticleAsync(article.ToArticle());

                    //WolDownloader.WolDownloader.DebugArticleOutput(article);
                }
                catch (Exception e) { Debug.WriteLine(e.Message); }
            }
            // Awake!
            foreach (var article in await WolDownloader.WolDownloader.GenerateMonthlyPublicationAsync("g", ArticleType.Monthly, meps))
            {
                try
                {
                    await StorehouseService.Instance.AddArticleAsync(article.ToArticle());

                    //WolDownloader.WolDownloader.DebugArticleOutput(article);
                }
                catch (Exception e) { Debug.WriteLine(e.Message); }
            }
        }

        public void CreateStorehouse()
        {
            var database = DependencyService.Get<ISQLiteService>().GetConnection();
            database.CreateTable<Article>();
            database.CreateTable<Word>();
        }

        public async Task CreateStorehouseAsync()
        {
            var database = DependencyService.Get<ISQLiteService>().GetAsyncConnection();
            await database.CreateTableAsync<Article>().ConfigureAwait(false);
            await database.CreateTableAsync<Word>().ConfigureAwait(false);
        }

        /// <summary>
        /// Dropping tables ONLY works using the synchronous DB connection
        /// For some reason, dropping asynchronously fails miserably
        /// </summary>
        public void DropAllTables()
        {
            var database = DependencyService.Get<ISQLiteService>().GetConnection();
            database.DropTable<Article>();
            database.DropTable<Word>();
        }

        public static double GetRealSize(double size)
        {
            if(Device.RuntimePlatform == Device.iOS)
            {
                return IOSSizeProportion * size;
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                // Both these do the exact same thing, (please adjust MainActivity.cs)
                return size * AndroidDPI;
                //return size * (AndroidDPI);
            }
            else
            {
                return size;
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        
    }

    public class DeviceInfo
    {
        protected static DeviceInfo _instance;
        double width;
        double height;

        static DeviceInfo()
        {
            _instance = new DeviceInfo();
        }
        protected DeviceInfo()
        {
        }

        public static bool IsOrientationPortrait()
        {
            return _instance.height > _instance.width;
        }

        public static void SetSize(double width, double height)
        {
            _instance.width = width;
            _instance.height = height;
        }
    }
}