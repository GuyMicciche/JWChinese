using Xamarin.Forms;

namespace JWChinese
{
    /// <summary>
    /// Extends <see cref="Xamarin.Forms.Button"/>.
    /// </summary>
    public class ExtendedButton : Button
    {
        //public static readonly BindableProperty IndexProperty = BindableProperty.Create("Index", typeof(int), typeof(ExtendedButton));

        /// <summary>
        /// Bindable property for button content vertical alignment.
        /// </summary>
        public static readonly BindableProperty VerticalContentAlignmentProperty = BindableProperty.Create("VerticalContentAlignment", typeof(TextAlignment), typeof(ExtendedButton), TextAlignment.Center);

        /// <summary>
        /// Bindable property for button content horizontal alignment.
        /// </summary>
        public static readonly BindableProperty HorizontalContentAlignmentProperty = BindableProperty.Create("HorizontalContentAlignment", typeof(TextAlignment), typeof(ExtendedButton), TextAlignment.Center);

        public static readonly BindableProperty IsNumberProperty = BindableProperty.Create("IsNumber", typeof(bool), typeof(ExtendedButton), false);

        public bool IsNumber
        {
            get { return (bool)GetValue(IsNumberProperty); }
            set { this.SetValue(IsNumberProperty, value); }
        }

        //public int Index
        //{
        //    get { return (int)GetValue(IndexProperty); }
        //    set { this.SetValue(IndexProperty, value); }
        //}

        /// <summary>
        /// Gets or sets the content vertical alignment.
        /// </summary>
        public TextAlignment VerticalContentAlignment
        {
            get { return (TextAlignment)GetValue(VerticalContentAlignmentProperty); }
            set { this.SetValue(VerticalContentAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content horizontal alignment.
        /// </summary>
        public TextAlignment HorizontalContentAlignment
        {
            get { return (TextAlignment)GetValue(HorizontalContentAlignmentProperty); }
            set { this.SetValue(HorizontalContentAlignmentProperty, value); }
        }

        public ExtendedButton()
        {
            //this.FontFamily = Device.OnPlatform("HelveticaNeue-Light", "Roboto Light", "/Assets/Fonts/SEGOEWP-LIGHT.TTF#Segoe WP");
            this.FontSize = 16;
        }
    }
}