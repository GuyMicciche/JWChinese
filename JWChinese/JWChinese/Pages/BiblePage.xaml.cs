using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JWChinese
{
    public partial class BiblePage : ContentPage, IPageController
    {
        private double width;
        private double height;

        public BiblePage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.Android)
            {
                ControlTemplate = (ControlTemplate)Resources["AndroidLibraryGridView"];
            }

            width = Objects.Orientation.Width;
            height = Objects.Orientation.Height;

            DoLayout((int)width);

            if (Device.RuntimePlatform != Device.Windows)
            {
                Objects.Orientation.RotationChanged += CurrentOrientation_Changed;
            }
        }

        private void CurrentOrientation_Changed(object sender, EventArgs e)
        {
            Debug.WriteLine("CurrentOrientation_Changed => " + Objects.Orientation.Width);
            DoLayout(Objects.Orientation.Width);
        }

        public void DoLayout(int w)
        {
            try
            {
                if (Device.RuntimePlatform == Device.Windows)
                {
                    FlowLayout_SizeChanged(HebrewFlowLayout, null);
                    FlowLayout_SizeChanged(GreekFlowLayout, null);

                    //Debug.WriteLine("REAL SIZE => " + App.GetRealSize(width));
                    //Debug.WriteLine("SIZE => " + width);

                    // ADJUST PADDING
                    if (w > 600)
                    {
                        LibraryGrid.Padding = new Thickness(40, 0, 40, 40);
                    }
                    else
                    {
                        LibraryGrid.Padding = new Thickness(15, 0, 15, 15);
                    }

                    // LAYOUT OF THE FLOWLAYOUTS
                    // STACK VERTICALLY
                    if (w < 1160)
                    {
                        VerticalLibraryGrid();
                    }
                    // STACK HORIZONTALLY
                    else if (w >= 1160 && w < 1565)
                    {
                        HorizontalLibraryGrid();
                    }
                    //STATIC COLUMN WIDTHS
                    else
                    {
                        LibraryGrid.ColumnDefinitions[0].Width = new GridLength(806);
                        LibraryGrid.ColumnDefinitions[1].Width = new GridLength(605);
                    }
                }
                else if (Device.RuntimePlatform == Device.iOS)
                {
                    FlowLayout_SizeChanged(HebrewFlowLayout, null);
                    FlowLayout_SizeChanged(GreekFlowLayout, null);

                    // ADJUST PADDING
                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        var p = App.GetRealSize(5);
                        LibraryGrid.Padding = new Thickness(p, 0, p, p);
                    }
                    else
                    {
                        var p = App.GetRealSize(10);
                        LibraryGrid.Padding = new Thickness(p, 0, p, p);
                    }

                    // LAYOUT OF THE FLOWLAYOUTS
                    // STACK HORIZONTALLY
                    if (Objects.Orientation.Landscape)
                    { 
                        VerticalLibraryGrid();
                    }
                    // STACK VERTICALLY
                    else
                    {
                        VerticalLibraryGrid();
                    }
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    // XFVisualTreeHelper.FindTemplateElementByName will not work with custom controls
                    ScrollView mainScrollView = XFVisualTreeHelper.FindTemplateElementByName<ScrollView>(this, "MainScrollView");
                    Grid libraryGrid = XFVisualTreeHelper.FindTemplateElementByName<Grid>(this, "LibraryGrid");
                    LibraryGridView hebrewFlowLayout = (LibraryGridView)XFVisualTreeHelper.FindTemplateElementByName<View>(this, "HebrewGridLayout");
                    LibraryGridView greekFlowLayout = (LibraryGridView)XFVisualTreeHelper.FindTemplateElementByName<View>(this, "GreekGridLayout");
                    StackLayout primary = XFVisualTreeHelper.FindTemplateElementByName<StackLayout>(this, "Primary");
                    StackLayout secondary = XFVisualTreeHelper.FindTemplateElementByName<StackLayout>(this, "Secondary");

                    primary.HorizontalOptions = LayoutOptions.CenterAndExpand;
                    secondary.HorizontalOptions = LayoutOptions.CenterAndExpand;

                    // ADJUST PADDING
                    if (w >= App.GetRealSize(500))
                    {
                        var p = App.GetRealSize(32) / 2;
                        mainScrollView.Padding = new Thickness(p, 0, p, p);
                    }
                    else
                    {
                        var p = App.GetRealSize(8) / 2;
                        mainScrollView.Padding = new Thickness(p, 0, p, p);
                    }

                    // LAYOUT OF THE FLOWLAYOUTS
                    // STACK VERTICALLY
                    if (w < 1160)
                    {
                        libraryGrid.ColumnDefinitions[0].Width = GridLength.Auto;
                        libraryGrid.ColumnDefinitions[1].Width = new GridLength(0);

                        libraryGrid.RowDefinitions[0].Height = GridLength.Auto;
                        libraryGrid.RowDefinitions[1].Height = GridLength.Auto;

                        Grid.SetRow(primary, 0);
                        Grid.SetColumn(primary, 0);

                        Grid.SetRow(secondary, 1);
                        Grid.SetColumn(secondary, 0);

                        libraryGrid.ColumnSpacing = 0;
                        libraryGrid.RowSpacing = 0;
                    }
                    // STACK HORIZONTALLY
                    else
                    {
                        libraryGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                        libraryGrid.ColumnDefinitions[1].Width = new GridLength(.75, GridUnitType.Star);

                        libraryGrid.RowDefinitions[0].Height = GridLength.Auto;
                        libraryGrid.RowDefinitions[1].Height = new GridLength(0);

                        Grid.SetRow(primary, 0);
                        Grid.SetColumn(primary, 0);

                        Grid.SetRow(secondary, 0);
                        Grid.SetColumn(secondary, 1);

                        libraryGrid.ColumnSpacing = 16;
                        libraryGrid.RowSpacing = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("EXCEPTION => " + ex.Message);
            }
        }

        private void HorizontalLibraryGrid()
        {
            LibraryGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
            LibraryGrid.ColumnDefinitions[1].Width = new GridLength(.75, GridUnitType.Star);

            LibraryGrid.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
            LibraryGrid.RowDefinitions[1].Height = new GridLength(0);

            Grid.SetRow(Primary, 0);
            Grid.SetColumn(Primary, 0);

            Grid.SetRow(Secondary, 0);
            Grid.SetColumn(Secondary, 1);

            LibraryGrid.ColumnSpacing = 26;
            LibraryGrid.RowSpacing = 0;
        }

        private void VerticalLibraryGrid()
        {
            LibraryGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
            LibraryGrid.ColumnDefinitions[1].Width = new GridLength(0);

            LibraryGrid.RowDefinitions[0].Height = GridLength.Auto;
            LibraryGrid.RowDefinitions[1].Height = GridLength.Auto;

            Grid.SetRow(Primary, 0);
            Grid.SetColumn(Primary, 0);

            Grid.SetRow(Secondary, 1);
            Grid.SetColumn(Secondary, 0);

            LibraryGrid.ColumnSpacing = 0;
            LibraryGrid.RowSpacing = 0;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width != this.width || height != this.height)
            {
                //Debug.WriteLine("OnSizeAllocated");
                this.width = width;
                this.height = height;

                if (Device.RuntimePlatform == Device.Windows)
                {
                    DoLayout(JWChinese.Objects.Orientation.Width);
                }
            }
        }

        private void ExtendedButton_SizeChanged(object sender, EventArgs e)
        {
            //Debug.WriteLine("ExtendedButton_SizeChanged");

            var w = Objects.Orientation.Width;
            var h = Objects.Orientation.Height;

            ExtendedButton button = ((ExtendedButton)sender);

            if (Device.RuntimePlatform == Device.Windows)
            {
                // HANDLE TEXT
                if (w < App.GetRealSize(442))
                {
                    button.SetBinding(Button.TextProperty, "OfficialBookAbbreviation");
                }
                else if (w <= App.GetRealSize(720) && w > App.GetRealSize(442))
                {
                    button.SetBinding(Button.TextProperty, "StandardBookAbbreviation");
                }
                else if (w >= App.GetRealSize(720))
                {
                    button.SetBinding(Button.TextProperty, "StandardBookName");
                }

                // HANDLE WIDTH AND HEIGHT
                if (w < App.GetRealSize(442))
                {
                    button.WidthRequest = 60;
                    button.MinimumWidthRequest = 60;
                    button.HeightRequest = 60;
                    button.MinimumHeightRequest = 60;
                }
                else
                {
                    button.WidthRequest = 50;
                    button.MinimumWidthRequest = 50;
                    button.HeightRequest = 50;
                    button.MinimumHeightRequest = 50;
                }
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                // HANDLE TEXT
                if (Device.Idiom == TargetIdiom.Phone)
                {
                    button.SetBinding(Button.TextProperty, "OfficialBookAbbreviation");
                    //button.WidthRequest = h;
                    //button.MinimumWidthRequest = h;
                    //button.HeightRequest = h;
                    //button.MinimumHeightRequest = h;
                }
                else
                {
                    button.SetBinding(Button.TextProperty, "StandardBookName");
                    //button.WidthRequest = w;
                    //button.MinimumWidthRequest = w;
                    //button.HeightRequest = h;
                    //button.MinimumHeightRequest = h;
                }
            }
            else
            {
                //button.HeightRequest = 0;
                //button.WidthRequest = 0;

                //button.IsVisible = false;

                //// HANDLE TEXT
                //if (button.Width < 60)
                //{
                //    button.SetBinding(Button.TextProperty, "OfficialBookAbbreviation");
                //}
                //else if (button.Width < 100)
                //{
                //    button.SetBinding(Button.TextProperty, "StandardBookAbbreviation");
                //}
                //else if (button.Width >= 100)
                //{
                //    button.SetBinding(Button.TextProperty, "StandardBookName");
                //}
            }
        }

        public void FlowLayout_SizeChanged(object sender, EventArgs e)
        {
            var w = Objects.Orientation.Width;
            var h = Objects.Orientation.Height;

            //Debug.WriteLine("FlowLayout_SizeChanged");
            if (Device.RuntimePlatform == Device.Windows)
            {
                FlowLayout flow = ((FlowLayout)sender);

                // ADJUST COLUMNS
                if (w < App.GetRealSize(442))
                {
                    flow.Flow = true;
                    flow.Column = 6;
                    flow.ColumnSpacing = 2;
                    flow.RowSpacing = 2;
                }
                else if (w >= App.GetRealSize(442) && w < App.GetRealSize(866))
                {
                    flow.Column = 4;
                    flow.Flow = true;
                    flow.ColumnSpacing = 2;
                    flow.RowSpacing = 2;
                }
                else if (w >= App.GetRealSize(866) && w < App.GetRealSize(1012))
                {
                    flow.Column = 5;
                    flow.Flow = true;
                    flow.ColumnSpacing = 2;
                    flow.RowSpacing = 2;
                }
                else if (w >= App.GetRealSize(1012) && w < App.GetRealSize(1160))
                {
                    flow.Column = 6;
                    flow.Flow = true;
                    flow.ColumnSpacing = 2;
                    flow.RowSpacing = 2;
                }
                else
                {
                    flow.Flow = true;
                    HebrewFlowLayout.Column = 4;
                    GreekFlowLayout.Column = 3;

                    flow.ColumnSpacing = 2;
                    flow.RowSpacing = 2;
                }
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                FlowLayout flow = ((FlowLayout)sender);
                flow.Flow = true;

                if (Device.Idiom == TargetIdiom.Phone)
                {
                    if(Objects.Orientation.IsLandscape)
                    {
                        flow.Column = 8;
                    }
                    else
                    {
                        flow.Column = 6;
                    }
                }
                else
                {
                    if (Objects.Orientation.IsLandscape)
                    {
                        flow.Column = 7;
                    }
                    else
                    {
                        flow.Column = 5;
                    }
                }

                flow.ColumnSpacing = 2;
                flow.RowSpacing = 2;
            }
        }
    }

    static class XFVisualTreeHelper
    {
        public static T FindTemplateElementByName<T>(this Page page, string name) where T : Element
        {
            var pc = page as IPageController;
            if (pc == null)
            {
                return null;
            }

            foreach (var child in pc.InternalChildren)
            {
                var result = child.FindByName<T>(name);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        //public static T GetTemplateChild<T>(Element parent, string name) where T : Element
        //{
        //    if (parent == null) return null;

        //    var templateChild = parent.FindByName<T>(name);

        //    if (templateChild != null) return templateChild;
        //    foreach (var child in FindVisualChildren<Element>(parent, false))
        //    {
        //        templateChild = GetTemplateChild<T>(child, name);
        //        if (templateChild != null) return templateChild;
        //    }
        //    return null;
        //}

        //public static IEnumerable<T> FindVisualChildren<T>(Element element, bool recursive = true) where T : Element
        //{
        //    if (element != null && element is Layout)
        //    {
        //        var childrenProperty = element.GetType().GetProperty("InternalChildren", BindingFlags.Instance | BindingFlags.NonPublic);

        //        if (childrenProperty != null)
        //        {
        //            var children = (IEnumerable<Element>)childrenProperty.GetValue(element);

        //            foreach (var child in children)
        //            {
        //                if (child != null && child is T) { yield return (T)child;
        //            }
        //                if (recursive)
        //                {
        //                    foreach (T childOfChild in FindVisualChildren<T>(child))
        //                    {
        //                        yield return childOfChild;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
