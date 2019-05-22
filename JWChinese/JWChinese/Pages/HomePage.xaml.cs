using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JWChinese
{
    public partial class HomePage : CarouselPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        //protected override void OnSizeAllocated(double width, double height)
        //{
        //    base.OnSizeAllocated(width, height);

        //    if (width != this.WidthRequest || height != this.HeightRequest)
        //    {
        //        this.WidthRequest = width;
        //        this.HeightRequest = height;

        //        if (Width < Height)
        //        {
        //            perimeter.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
        //            perimeter.ColumnDefinitions[1].Width = new GridLength(0);

        //            perimeter.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
        //            perimeter.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);

        //            Grid.SetRow(secondary, 0);
        //            Grid.SetColumn(secondary, 0);

        //            Grid.SetRow(primary, 1);
        //            Grid.SetColumn(primary, 0);

        //            perimeter.RowSpacing = 3;
        //            perimeter.ColumnSpacing = 0;
        //        }
        //        else
        //        {
        //            perimeter.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
        //            perimeter.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);

        //            perimeter.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
        //            perimeter.RowDefinitions[1].Height = new GridLength(0);

        //            Grid.SetRow(primary, 0);
        //            Grid.SetColumn(primary, 1);

        //            Grid.SetRow(secondary, 0);
        //            Grid.SetColumn(secondary, 0);

        //            perimeter.RowSpacing = 0;
        //            perimeter.ColumnSpacing = 3;
        //        }

        //        // USE THIS IF USING STACKPANELS!
        //        //if (width > height)
        //        //{
        //        //    perimeter.Orientation = StackOrientation.Horizontal;
        //        //}
        //        //else
        //        //{
        //        //    perimeter.Orientation = StackOrientation.Vertical;
        //        //}
        //    }
        //}
    }
}
