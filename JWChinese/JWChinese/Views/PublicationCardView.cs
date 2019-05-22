using System;
using System.Diagnostics;
using System.Globalization;
using Xamarin.Forms;

namespace JWChinese
{
    public class PublicationCardView : GridViewCell
    {
        public Grid MainGrid = new Grid();
        public Frame MainFrame = new Frame();

        //private double width;
        //private double height;

        private Label title;
        private Label details;

        public PublicationCardView()
        {
            MainGrid = new Grid
            {
                Padding = new Thickness(1, 1, 1, 1),
                RowSpacing = 1,
                ColumnSpacing = 1,
                BackgroundColor = StyleKit.CardBorderColor,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //RowDefinitions =
                //{
                //    new RowDefinition { Height = new GridLength (.7, GridUnitType.Star) },
                //    new RowDefinition { Height = new GridLength (.3, GridUnitType.Star) }
                //},
                //ColumnDefinitions =
                //{
                //    new ColumnDefinition { Width = new GridLength (100, GridUnitType.Absolute) },
                //    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                //    new ColumnDefinition { Width = new GridLength (100, GridUnitType.Absolute) },
                //    new ColumnDefinition { Width = new GridLength (50, GridUnitType.Absolute) }
                //}
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength (1, GridUnitType.Star) }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength (100, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) }
                }
            };

            // IMAGE
            ContentView mainImage = new ContentView();
            Image img = new Image();
            img.SetBinding(Image.SourceProperty, new Binding("PublicationImage"));
            img.VerticalOptions = LayoutOptions.FillAndExpand;
            img.HorizontalOptions = LayoutOptions.FillAndExpand;
            mainImage.Content = img;

            // TITLE AND DETAILS
            ContentView titleDetails = new ContentView();
            titleDetails.BackgroundColor = Color.White;
            title = new Label();
            title.FontSize = 11;
            title.TextColor = StyleKit.PubCategoryTextColor;
            title.SetBinding(Label.FormattedTextProperty, new Binding("Title"));
            title.SetBinding(VisualElement.IsVisibleProperty, "Title", BindingMode.Default, new StringToVisibilityConverter(), null);
            details = new Label();
            details.SetBinding(Label.FormattedTextProperty, new Binding("Description"));
            details.FontSize = 16;
            details.TextColor = StyleKit.PubTitleTextColor;
            StackLayout titleDetailsContainer = new StackLayout();
            titleDetailsContainer.Spacing = 0;
            titleDetailsContainer.Padding = new Thickness(9, 9, 0, 0);
            titleDetailsContainer.VerticalOptions = LayoutOptions.StartAndExpand;
            titleDetailsContainer.Children.Add(title);
            titleDetailsContainer.Children.Add(details);
            titleDetails.Content = titleDetailsContainer;

            //// STATUS
            //ContentView status = new ContentView();
            //status.BackgroundColor = StyleKit.CardFooterBackgroundColor;
            //Label statusLabel = new Label();
            ////statusLabel.SetBinding(Label.TextProperty, new Binding("StatusMessage"));
            //statusLabel.FontSize = 9;
            //statusLabel.FontAttributes = FontAttributes.Bold;
            //statusLabel.TextColor = StyleKit.LightTextColor;
            //Image statusIcon = new Image();
            ////statusIcon.SetBinding(Image.SourceProperty, "StatusMessageFileSource");
            //statusIcon.HeightRequest = 10;
            //statusIcon.WidthRequest = 10;
            //StackLayout statusContainer = new StackLayout();
            //statusContainer.Padding = new Thickness(5);
            //statusContainer.Orientation = StackOrientation.Horizontal;
            //statusContainer.HorizontalOptions = LayoutOptions.StartAndExpand;
            //statusContainer.VerticalOptions = LayoutOptions.Center;
            //statusContainer.Children.Add(statusIcon);
            //statusContainer.Children.Add(statusLabel);
            //status.Content = statusContainer;

            //// ACTION
            //ContentView action = new ContentView();
            //action.BackgroundColor = StyleKit.CardFooterBackgroundColor;
            //Label actionLabel = new Label();
            ////actionLabel.SetBinding(Label.TextProperty, new Binding("ActionMessage"));
            //actionLabel.FontSize = 9;
            //actionLabel.FontAttributes = FontAttributes.Bold;
            //actionLabel.TextColor = StyleKit.LightTextColor;
            //Image actionImage = new Image();
            ////actionImage.SetBinding(Image.SourceProperty, new Binding("ActionMessageFileSource"));
            //actionImage.HeightRequest = 10;
            //actionImage.WidthRequest = 10;
            //StackLayout actionContainer = new StackLayout();
            //actionContainer.Padding = new Thickness(5);
            //actionContainer.Orientation = StackOrientation.Horizontal;
            //actionContainer.HorizontalOptions = LayoutOptions.StartAndExpand;
            //actionContainer.VerticalOptions = LayoutOptions.Center;
            //actionContainer.Children.Add(actionImage);
            //actionContainer.Children.Add(actionLabel);
            //action.Content = actionContainer;

            //// CONFIG
            //ContentView config = new ContentView();
            //config.BackgroundColor = StyleKit.CardFooterBackgroundColor;
            //Image configImage = new Image();
            ////configImage.Source = StyleKit.Icons.Cog;
            //configImage.VerticalOptions = LayoutOptions.Center;
            //configImage.HorizontalOptions = LayoutOptions.Center;
            //configImage.HeightRequest = 10;
            //configImage.WidthRequest = 10;
            //config.Content = configImage;

            //ContentView mainImage = new ContentView();
            //BoxView bg = new BoxView();
            //bg.BackgroundColor = StyleKit.Status.CompletedColor;
            //bg.VerticalOptions = LayoutOptions.Fill;
            //bg.HorizontalOptions = LayoutOptions.Fill;
            //mainImage.Content = bg;

            MainGrid.Children.Add(mainImage, 0, 0);
            MainGrid.Children.Add(titleDetails, 1, 0);

            //MainGrid.Children.Add(mainImage, 0, 1, 0, 2);
            //MainGrid.Children.Add(titleDetails, 1, 4, 0, 1);
            //MainGrid.Children.Add(status, 1, 1);
            //MainGrid.Children.Add(action, 2, 1);
            //MainGrid.Children.Add(config, 3, 1);

            MainFrame.Padding = 0;
            MainFrame.HorizontalOptions = LayoutOptions.FillAndExpand;
            MainFrame.VerticalOptions = LayoutOptions.FillAndExpand;
            MainFrame.Content = MainGrid;
            if (Device.RuntimePlatform == Device.iOS)
            {
                MainFrame.HasShadow = false;
                MainFrame.OutlineColor = Color.Transparent;
                MainFrame.BackgroundColor = Color.Transparent;
            }

            View = new StackLayout()
            {
                Children = { MainFrame }
            };

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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            DoLayout(JWChinese.Objects.Orientation.Width);
        }

        private void CurrentOrientation_Changed(object sender, EventArgs e)
        {
            DoLayout(JWChinese.Objects.Orientation.Width);
        }

        private void View_SizeChanged(object sender, EventArgs e)
        {
            //var grid = ((GridView)Parent);
            //var w = grid.Width;

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

            if(h == 100)
            {
                title.FontSize = 11;
                details.FontSize = 16;

            }
            else if(h == 80)
            {
                title.FontSize = 10;
                details.FontSize = 14;
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
                else if (w >= 800 && View.HeightRequest == 100 && Height == 100)
                {
                    return;
                }
                else if (w < 800 && View.HeightRequest == 80 && Height == 80)
                {
                    return;
                }
                

                if (w >= 800)
                {
                    RefreshGrid(100);
                }
                else
                {
                    RefreshGrid(80);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("AAAGH => " + ex.Message);
            }
        }

        protected override void InitializeCell()
        {
            //InitializeComponent();
        }

        protected override void SetupCell(bool isRecycled)
        {
            //
        }
    }
}