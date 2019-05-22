using System;
using Xamarin.Forms;

namespace JWChinese
{
    public partial class DictionaryPage : CustomPage
    {
        private double width;
        private double height;

        public DictionaryPage()
        {
            InitializeComponent();

            DoLayout(JWChinese.Objects.Orientation.Width);

            if (Device.RuntimePlatform != Device.Windows)
            {
                JWChinese.Objects.Orientation.RotationChanged += CurrentOrientation_Changed;
            }
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

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;

                if (Device.RuntimePlatform == Device.Windows)
                {
                    DoLayout(JWChinese.Objects.Orientation.Width);
                }
            }
        }

        private void DoLayout(double w)
        {
            try
            {
                //if (Device.RuntimePlatform == Device.Windows)
                //{
                //    if (w > 600)
                //    {
                //        DictionaryGridView.Padding = App.GetRealSize(40);
                //    }
                //    else
                //    {
                //        DictionaryGridView.Padding = App.GetRealSize(15);
                //    }
                //}
                //else if (Device.RuntimePlatform == Device.iOS)
                //{
                //    if (Device.Idiom == TargetIdiom.Phone)
                //    {
                //        var p = App.GetRealSize(5);
                //        DictionaryGridView.Padding = p;

                //        DictionaryGridView.RowHeight = 80;
                //    }
                //    else
                //    {
                //        var p = App.GetRealSize(10);
                //        DictionaryGridView.Padding = p;

                //        DictionaryGridView.RowHeight = 100;
                //    }
                //}
                //else if (Device.RuntimePlatform == Device.Android)
                //{
                //    if (w >= 800)
                //    {
                //        DictionaryGridView.Padding = App.GetRealSize(32);
                //        DictionaryGridView.RowHeight = 100;
                //    }
                //    else
                //    {
                //        DictionaryGridView.Padding = App.GetRealSize(16);
                //        DictionaryGridView.RowHeight = 80;
                //    }
                //}
            }
            catch (Exception ex)
            {
                App.GemWriteLine("DictionaryPage.DoLayout ERROR", ex.Message);
            }
        }

        private void Grid_SizeChanged(object sender, EventArgs e)
        {
            var grid = sender as Grid;
            if (Device.RuntimePlatform == Device.Windows)
            {
                if (Objects.Orientation.Width > 600)
                {
                    grid.Padding = new Thickness(40, 8, 40, 8);

                    foreach (View child in grid.Children)
                    {
                        if (child is Label && (double)child.GetValue(Label.FontSizeProperty) != Device.GetNamedSize(NamedSize.Micro, typeof(Label)))
                        {
                            ((Label)child).FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                        }
                    }
                }
                else
                {
                    grid.Padding = new Thickness(15, 8, 15, 8);

                    foreach (View child in grid.Children)
                    {
                        if (child is Label && (double)child.GetValue(Label.FontSizeProperty) != Device.GetNamedSize(NamedSize.Micro, typeof(Label)))
                        {
                            ((Label)child).FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
                        }
                    }
                }
            }
        }
    }
}