using Xamarin.Forms;
using JWChinese.Droid;

[assembly: Dependency(typeof(BaseUrl_Android))]
namespace JWChinese.Droid
{
    public class BaseUrl_Android : IBaseUrl
    {
        public string Get()
        {
            return "file:///android_asset/www/";
        }
    }
}