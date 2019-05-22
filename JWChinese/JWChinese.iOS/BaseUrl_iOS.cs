using Xamarin.Forms;
using Foundation;
using JWChinese.iOS;

[assembly: Dependency(typeof(BaseUrl_iOS))]
namespace JWChinese.iOS
{
    public class BaseUrl_iOS : IBaseUrl
    {
        public string Get()
        {
            return NSBundle.MainBundle.BundlePath + "/www/";
        }
    }
}