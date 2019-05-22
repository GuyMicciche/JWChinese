using System;
using Xamarin.Forms;

namespace JWChinese
{
    public partial class PublicationContentsPage : CustomPage
    {
        private double width;
        private double height;

        public PublicationContentsPage()
        {
            InitializeComponent();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;
            }
        }

        private void Grid_SizeChanged(object sender, EventArgs e)
        {
            var grid = sender as Grid;

            if (width > 600)
            {
                grid.Padding = new Thickness(40, 8, 40, 8);

                if (Device.RuntimePlatform == Device.Windows)
                {
                    foreach (View child in grid.Children)
                    {
                        if (child is Label && (double)child.GetValue(Label.FontSizeProperty) != Device.GetNamedSize(NamedSize.Micro, typeof(Label)))
                        {
                            ((Label)child).FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                        }
                    }
                }
            }
            else
            {
                grid.Padding = new Thickness(15, 8, 15, 8);

                if (Device.RuntimePlatform == Device.Windows)
                {
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