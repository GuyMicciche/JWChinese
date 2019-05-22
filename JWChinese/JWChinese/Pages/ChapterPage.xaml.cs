using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JWChinese
{
    public partial class ChapterPage : CustomPage
    {
        private double width;
        private double height;

        public ChapterPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.Android)
            {
                ControlTemplate = (ControlTemplate)Resources["AndroidNumbersGridView"];
            }

            DoLayout(Objects.Orientation.Width);

            //if(Device.RuntimePlatform == Device.Android)
            //{
            //    chapterGrid.ButtonHeight = 68;
            //    chapterGrid.ButtonWidth = 68;
            //}

            if (Device.RuntimePlatform == Device.iOS)
            {
                if (Device.Idiom == TargetIdiom.Phone)
                {
                    chapterGrid.ButtonHeight = 52;
                    chapterGrid.ButtonWidth = 52;
                }
                else
                {
                    chapterGrid.ButtonHeight = 62;
                    chapterGrid.ButtonWidth = 62;
                }
            }
        }

        public void DoLayout(int w)
        {
            try
            {
                // ADJUST PADDING
                if(Device.RuntimePlatform == Device.Windows)
                {
                    if (w > 600)
                    {
                        ChapterGrid.Padding = new Thickness(40);
                    }
                    else
                    {
                        ChapterGrid.Padding = new Thickness(15);
                    }
                }
                else if (Device.RuntimePlatform == Device.iOS)
                {
                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        var p = App.GetRealSize(5);
                        ChapterGrid.Padding = new Thickness(p);
                    }
                    else
                    {
                        var p = App.GetRealSize(10);
                        ChapterGrid.Padding = new Thickness(p);
                    }
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    //Grid grid = XFVisualTreeHelper.FindTemplateElementByName<Grid>(this, "ChapterGrid");

                    //if (w >= App.GetRealSize(500))
                    //{
                    //    var p = App.GetRealSize(32) / 2;
                    //    grid.Padding = new Thickness(p);
                    //}
                    //else
                    //{
                    //    var p = App.GetRealSize(8) / 2;
                    //    grid.Padding = new Thickness(p);
                    //}
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;

                DoLayout((int)width);
            }
        }
    }
}