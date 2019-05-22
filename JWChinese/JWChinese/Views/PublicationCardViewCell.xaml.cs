using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JWChinese
{
    public partial class PublicationCardViewCell : ViewCell
    {
        public PublicationCardViewCell()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
            {
                MainFrame.HasShadow = false;
                MainFrame.OutlineColor = Color.Transparent;
                MainFrame.BackgroundColor = Color.Transparent;
            }

            DoLayout(JWChinese.Objects.Orientation.Width);

            if (Device.RuntimePlatform == Device.Windows)
            {
                View.SizeChanged += View_SizeChanged;
            }
            else
            {
                JWChinese.Objects.Orientation.RotationChanged += CurrentOrientation_Changed;
            }
        }
        private void View_SizeChanged(object sender, EventArgs e)
        {
            DoLayout(JWChinese.Objects.Orientation.Width);
        }

        private void CurrentOrientation_Changed(object sender, EventArgs e)
        {
            DoLayout(JWChinese.Objects.Orientation.Width);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            DoLayout(JWChinese.Objects.Orientation.Width);
        }

        private void RefreshGrid(int h)
        {
            if (Device.RuntimePlatform == Device.Windows)
            {
                Height = h;
                View.HeightRequest = h;
            }

            MainGrid.ColumnDefinitions[0].Width = new GridLength(h, GridUnitType.Absolute);
            MainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);

            if (h == 100)
            {
                TitleLabel.FontSize = 11;
                DetailsLabel.FontSize = 16;

            }
            else if (h == 80)
            {
                TitleLabel.FontSize = 10;
                DetailsLabel.FontSize = 14;
            }
        }

        private void DoLayout(double w)
        {
            try
            {
                if (w == -1)
                {
                    return;
                }
                else if (w >= 800 && (View.HeightRequest == 100 || Height == 100))
                {
                    return;
                }
                else if (w < 800 && (View.HeightRequest == 80 || Height == 80))
                {
                    return;
                }

                if (Device.RuntimePlatform == Device.Windows || Device.RuntimePlatform == Device.Android)
                {
                    if (w >= 800)
                    {
                        RefreshGrid(100);
                    }
                    else
                    {
                        RefreshGrid(80);
                    }
                }
                else if (Device.RuntimePlatform == Device.iOS)
                {
                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        RefreshGrid(80);
                    }
                    else
                    {
                        RefreshGrid(100);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PublicationCardViewCell.DoLayout ERROR => " + ex.Message);
            }
        }
    }

    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !string.IsNullOrEmpty((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}