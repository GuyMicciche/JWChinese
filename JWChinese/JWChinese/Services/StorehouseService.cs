using JWChinese;
using SQLite;
//using SQLite.Net.Async;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(StorehouseService))]
namespace JWChinese
{
    public class StorehouseService: IStorehouseService
    {
        // private SQLiteAsyncConnection Store { get; } = DependencyService.Get<ISQLiteService>().GetAsyncConnection();

        private static readonly AsyncLock AsyncLock = new AsyncLock();

        private static readonly Lazy<StorehouseService> Lazy = new Lazy<StorehouseService>(() => new StorehouseService());

        public static IStorehouseService Instance => Lazy.Value;

        private SQLiteAsyncConnection _store;
        public SQLiteAsyncConnection Store
        {
            get
            {
                if (_store == null)
                {
                    LazyInitializer.EnsureInitialized(ref _store, DependencyService.Get<ISQLiteService>().GetAsyncConnection);
                }

                return _store;
            }
            set
            {
                _store = value;
            }
        }

        public void CloseStore()
        {
            //DependencyService.Get<ISQLiteService>().CloseConnection();

            SQLiteAsyncConnection.ResetPool();

            //DependencyService.Get<ISQLiteService>().DeleteDatabase();
        }

        public async Task<IEnumerable<Article>> GetArticlesAsync()
        {
            using (await AsyncLock.LockAsync())
            {
                return await Store.Table<Article>().ToListAsync();
            }
        }

        public async Task<IEnumerable<Article>> GetArticlesAsync(string language, string symbol)
        {
            using (await AsyncLock.LockAsync())
            {
                return await Store.Table<Article>().Where(a => a.Library == language && a.Symbol == symbol).ToListAsync();
            }
        }

        public async Task<IEnumerable<Article>> GetArticlesAsync(string language, string symbol, string group)
        {
            group = group + ".";

            using (await AsyncLock.LockAsync())
            {
                return await Store.Table<Article>().Where(a => a.Library == language && a.Symbol == symbol && a.MepsID.StartsWith(group)).ToListAsync();
            }
        }

        public async Task<IEnumerable<Article>> GetArticlesAsync(string symbol)
        {
            using (await AsyncLock.LockAsync())
            {
                return await Store.Table<Article>().Where(a => a.Symbol == symbol).ToListAsync();
            }
        }

        public async Task<Article> GetArticleAsync(int id)
        {
            using (await AsyncLock.LockAsync())
            {
                return await Store.Table<Article>().Where(t => t.ID == id).FirstOrDefaultAsync();
            }
        }

        public async Task<Article> GetArticleAsync(Article article)
        {
            using (await AsyncLock.LockAsync())
            {
                return await Store.Table<Article>().Where(a => a == article).FirstOrDefaultAsync();
            }
        }

        public async Task<Article> GetArticleAsync(string mepsId)
        {
            using (await AsyncLock.LockAsync())
            {
                return await Store.Table<Article>().Where(a => a.MepsID == mepsId).FirstOrDefaultAsync();
            }
        }

        public async Task DeleteArticleAsync(Article article)
        {
            using (await AsyncLock.LockAsync())
            {
                await Store.DeleteAsync(article);
            }
        }

        public async Task<int> AddArticleAsync(Article article)
        {
            using (await AsyncLock.LockAsync())
            {
                if (article.ID != 0)
                {
                    return await Store.UpdateAsync(article);
                }
                else
                {
                    return await Store.InsertAsync(article);
                }
            }
        }


        public async Task<IEnumerable<Word>> GetWordsAsync()
        {
            using (await AsyncLock.LockAsync())
            {
                return await Store.Table<Word>().ToListAsync();
            }
        }

        public async Task<Word> GetWordAsync(Word word)
        {
            using (await AsyncLock.LockAsync())
            {
                return await Store.Table<Word>().Where(w => w == word).FirstOrDefaultAsync();
            }
        }

        public async Task<int> AddWordAsync(Word word)
        {
            using (await AsyncLock.LockAsync())
            {
                if (word.ID != 0)
                {
                    return await Store.UpdateAsync(word);
                }
                else
                {
                    App.CurrentArticleWords.Add(word);

                    return await Store.InsertAsync(word);
                }
            }
        }

        public async Task DeleteWordAsync(Word word)
        {
            using (await AsyncLock.LockAsync())
            {
                App.CurrentArticleWords.Remove(word);

                await Store.DeleteAsync(word);
            }
        }
    }
}