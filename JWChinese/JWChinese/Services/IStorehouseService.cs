using System.Collections.Generic;
using System.Threading.Tasks;

namespace JWChinese
{
    public interface IStorehouseService
    {
        void CloseStore();

        Task<IEnumerable<Article>> GetArticlesAsync();

        Task<IEnumerable<Article>> GetArticlesAsync(string language, string symbol);

        Task<IEnumerable<Article>> GetArticlesAsync(string language, string symbol, string group);

        Task<IEnumerable<Article>> GetArticlesAsync(string symbol);

        Task<Article> GetArticleAsync(int id);

        Task<Article> GetArticleAsync(string mepsId);

        Task<Article> GetArticleAsync(Article article);

        Task DeleteArticleAsync(Article article);

        Task<int> AddArticleAsync(Article article);

        Task<IEnumerable<Word>> GetWordsAsync();

        Task<Word> GetWordAsync(Word word);

        Task<int> AddWordAsync(Word word);

        Task DeleteWordAsync(Word word);
    }
}