using JWChinese.UWP;
using Xamarin.Forms;

[assembly: Dependency(typeof(BaseUrl))]
namespace JWChinese.UWP
{
    public class BaseUrl : IBaseUrl
    {
        public string Get()
        {
            return "ms-appx-web:///Assets/www/";
        }
    }
}