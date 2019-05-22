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
    public partial class PublicationsPage : ContentPage
    {
        private double width;
        private double height;

        public PublicationsPage()
        {
            InitializeComponent();

            DoLayout(JWChinese.Objects.Orientation.Width);

            if (Device.RuntimePlatform != Device.Windows)
            {
                JWChinese.Objects.Orientation.RotationChanged += CurrentOrientation_Changed;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            DoLayout(JWChinese.Objects.Orientation.Width);
        }

        private void CurrentOrientation_Changed(object sender, EventArgs e)
        {
            //Debug.WriteLine("CurrentOrientation_Changed => " + JWChinese.Objects.Orientation.Width);
            DoLayout(JWChinese.Objects.Orientation.Width);
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;

                if(Device.RuntimePlatform == Device.Windows)
                {
                    DoLayout(JWChinese.Objects.Orientation.Width);
                }
            }
        }

        private void DoLayout(double w)
        {
            try
            {
                if(Device.RuntimePlatform == Device.Windows)
                {
                    if (w > 600)
                    {
                        PublicationGridView.Padding = App.GetRealSize(40);
                    }
                    else
                    {
                        PublicationGridView.Padding = App.GetRealSize(15);
                    }
                }
                else if (Device.RuntimePlatform == Device.iOS)
                {
                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        var p = App.GetRealSize(5);
                        PublicationGridView.Padding = p;

                        PublicationGridView.RowHeight = 80;
                    }
                    else
                    {
                        var p = App.GetRealSize(10);
                        PublicationGridView.Padding = p;

                        PublicationGridView.RowHeight = 100;
                    }
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    if (w >= 800)
                    {
                        PublicationGridView.Padding = App.GetRealSize(32);
                        PublicationGridView.RowHeight = 100;
                    }
                    else
                    {
                        PublicationGridView.Padding = App.GetRealSize(16);
                        PublicationGridView.RowHeight = 80;
                    }
                }
            }
            catch (Exception ex)
            {
                App.GemWriteLine("PublicationsPage.DoLayout ERROR", ex.Message);
            }
        }
    }
}