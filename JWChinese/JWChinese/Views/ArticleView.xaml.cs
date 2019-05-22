using System;
using JWChinese.Helpers;
using PropertyChanged;

using Xamarin.Forms;
using JWChinese.Objects;
using System.Diagnostics;

namespace JWChinese
{
    [AddINotifyPropertyChangedInterface]
    public partial class ArticleView : ContentView
    {
        public static readonly BindableProperty PrimaryProperty = BindableProperty.Create(nameof(Primary), typeof(HtmlWebViewSource), typeof(ArticleView));
        public HtmlWebViewSource Primary
        {
            get { return (HtmlWebViewSource)GetValue(PrimaryProperty); }
            set { SetValue(PrimaryProperty, value); }
        }

        public static readonly BindableProperty SecondaryProperty = BindableProperty.Create(nameof(Secondary), typeof(HtmlWebViewSource), typeof(ArticleView));
        public HtmlWebViewSource Secondary
        {
            get { return (HtmlWebViewSource)GetValue(SecondaryProperty); }
            set { SetValue(SecondaryProperty, value); }
        }

        private double width;
        private double height;

        public ArticleView()
        {
            InitializeComponent();

            width = Objects.Orientation.Width;
            height = Objects.Orientation.Height;

            ExpandParadigm(Settings.ExpandSetting);

            Orientation.RotationChanged += CurrentOrientation_Changed;
        }

        private void CurrentOrientation_Changed(object sender, EventArgs e)
        {
            width = Objects.Orientation.Width;
            height = Objects.Orientation.Height;

            UpdateUI(Orientation.Width > Orientation.Height);
        }

        public void ExpandParadigm(int jwid)
        {
            // primary
            if (jwid == 1)
            {
                SecondaryWebView.IsVisible = !SecondaryWebView.IsVisible;
            }
            // secondary
            else if (jwid == 2)
            {
                PrimaryWebView.IsVisible = !PrimaryWebView.IsVisible;
            }

            // Store Expand Setting
            if(SecondaryWebView.IsVisible == PrimaryWebView.IsVisible == true)
            {
                Settings.ExpandSetting = (int)ExpandScreen.Dual;
            }
            else
            {
                Settings.ExpandSetting = jwid;
            }

            UpdateUI(Orientation.Width > Orientation.Height);
        }

        private bool Expanded()
        {
            return Settings.ExpandSetting != (int)ExpandScreen.Dual;
        }

        private void UpdateUI(bool landscape)
        {
            if (landscape)
            {
                HorizontalMainGrid();
                //Perimeter.Orientation = StackOrientation.Horizontal;
            }
            else
            {
                VerticalMainGrid();
                //Perimeter.Orientation = StackOrientation.Vertical;
            }            

            if (Settings.ExpandSetting == (int)ExpandScreen.Dual)
            {
                FixWebViewLayout();
            }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;

                UpdateUI(width > height);
            }
        }

        private void HorizontalMainGrid()
        {
            MainGrid.ColumnDefinitions.Clear();
            MainGrid.RowDefinitions.Clear();

            MainGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
            MainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0) });

            if (Expanded())
            {
                MainGrid.ColumnSpacing = 0;
                MainGrid.RowSpacing = 0;

                MainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0) });

                if (Settings.ExpandSetting == (int)ExpandScreen.Primary)
                {
                    Grid.SetRow(PrimaryWebView, 0);
                    Grid.SetColumn(PrimaryWebView, 0);
                }
                else if (Settings.ExpandSetting == (int)ExpandScreen.Secondary)
                {
                    Grid.SetRow(SecondaryWebView, 0);
                    Grid.SetColumn(SecondaryWebView, 0);
                }
            }
            else
            {
                MainGrid.ColumnSpacing = 3;
                MainGrid.RowSpacing = 0;

                MainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });

                Grid.SetRow(PrimaryWebView, 0);
                Grid.SetColumn(PrimaryWebView, 0);

                Grid.SetRow(SecondaryWebView, 0);
                Grid.SetColumn(SecondaryWebView, 1);
            }
        }

        private void VerticalMainGrid()
        {
            MainGrid.ColumnDefinitions.Clear();
            MainGrid.RowDefinitions.Clear();

            MainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0) });

            if (Expanded())
            {
                MainGrid.RowSpacing = 0;
                MainGrid.ColumnSpacing = 0;

                MainGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
                MainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0) });

                if (Settings.ExpandSetting == (int)ExpandScreen.Primary)
                {
                    Grid.SetRow(PrimaryWebView, 0);
                    Grid.SetColumn(PrimaryWebView, 0);
                }
                else if (Settings.ExpandSetting == (int)ExpandScreen.Secondary)
                {
                    Grid.SetRow(SecondaryWebView, 0);
                    Grid.SetColumn(SecondaryWebView, 0);
                }
            }
            else
            {
                MainGrid.RowSpacing = 3;
                MainGrid.ColumnSpacing = 0;

                MainGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
                MainGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });

                Grid.SetRow(PrimaryWebView, 0);
                Grid.SetColumn(PrimaryWebView, 0);

                Grid.SetRow(SecondaryWebView, 1);
                Grid.SetColumn(SecondaryWebView, 0);
            }
        }

        private void FixWebViewLayout()
        {
            // THIS HACK FIXED UNEVEN WEBVIEWS WHEN BOTH ON SCREEN
            MainGrid.HeightRequest = 100;
            MainGrid.WidthRequest = 100;

            PrimaryWebView.HeightRequest = 100;
            PrimaryWebView.WidthRequest = 100;

            SecondaryWebView.HeightRequest = 100;
            SecondaryWebView.WidthRequest = 100;
        }
    }

    public class ArticleTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate mainArticle;

        public ArticleTemplateSelector()
        {
            this.mainArticle = new DataTemplate(typeof(ArticleView));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return mainArticle;
        }
    }
}