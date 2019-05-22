using Xamarin.Forms;

namespace JWChinese
{
    public class JWWebView : WebView
    {
        public static readonly BindableProperty JWIdProperty = BindableProperty.Create("JWId", typeof(int), typeof(JWWebView), 0);
        public int JWId
        {
            get { return (int)GetValue(JWIdProperty); }
            set { SetValue(JWIdProperty, value); }
        }

        public static readonly BindableProperty IsChineseProperty = BindableProperty.Create("IsChinese", typeof(bool), typeof(JWWebView), false);
        public bool IsChinese
        {
            get { return (bool)GetValue(IsChineseProperty); }
            set { SetValue(IsChineseProperty, value); }
        }

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create("FontSize", typeof(int), typeof(JWWebView), 11, BindingMode.TwoWay);
        public int FontSize
        {
            get { return (int)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }
    }
}
