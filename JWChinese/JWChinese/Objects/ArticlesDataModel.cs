using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using PropertyChanged;

namespace JWChinese
{
    [AddINotifyPropertyChangedInterface]
    public class ArticlesDataModel
    {
        private static HtmlWebViewSource primary;
        private static HtmlWebViewSource secondary;

        public HtmlWebViewSource Primary
        {
            get
            {
                return primary;
            }
            set
            {
                if (value.Html.Contains("<title>Chinese</title>"))
                {
                    IsPrimaryChinese = true;
                    IsSecondaryChinese = false;
                }
                primary = value;
            }
        }

        public HtmlWebViewSource Secondary
        {
            get
            {
                return secondary;
            }
            set
            {
                if (value.Html.Contains("<title>Chinese</title>"))
                {
                    IsPrimaryChinese = false;
                    IsSecondaryChinese = true;
                }
                secondary = value;
            }
        }

        public bool IsPrimaryChinese { get; set; }
        public bool IsSecondaryChinese { get; set; }

        private Command<WebNavigatingEventArgs> navigatingCommand;
        public Command<WebNavigatingEventArgs> NavigatingCommand
        {
            get
            {
                return navigatingCommand ?? (navigatingCommand = new Command<WebNavigatingEventArgs>(
                    (param) =>
                    {
                        //if (param != null && -1 < Array.IndexOf(_uris, param.Url))
                        //{
                        //Debug.WriteLine(param.Url);
                        Device.OpenUri(new Uri(param.Url));
                        param.Cancel = true;
                        //}
                    },
                     (param) => true
                     ));
            }
        }

        private Command sizeChangedCommand;
        public Command SizeChangedCommand
        {
            get
            {
                return sizeChangedCommand ?? (sizeChangedCommand = new Command(
                    (param) =>
                    {
                        var view = (ContentView)param;

                        var perimeter = view.FindByName<Grid>("perimeter");
                        var primary = view.FindByName<StackLayout>("primary");
                        var secondary = view.FindByName<StackLayout>("secondary");

                        if (view.Width < view.Height)
                        {
                            perimeter.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                            perimeter.ColumnDefinitions[1].Width = new GridLength(0);

                            perimeter.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
                            perimeter.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);

                            Grid.SetRow(secondary, 0);
                            Grid.SetColumn(secondary, 0);

                            Grid.SetRow(primary, 1);
                            Grid.SetColumn(primary, 0);

                            perimeter.RowSpacing = 3;
                            perimeter.ColumnSpacing = 0;
                        }
                        else
                        {
                            perimeter.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                            perimeter.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);

                            perimeter.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
                            perimeter.RowDefinitions[1].Height = new GridLength(0);

                            Grid.SetRow(primary, 0);
                            Grid.SetColumn(primary, 1);

                            Grid.SetRow(secondary, 0);
                            Grid.SetColumn(secondary, 0);

                            perimeter.RowSpacing = 0;
                            perimeter.ColumnSpacing = 3;
                        }
                    },
                    (param) => true
                    ));
            }
        }
    }
}