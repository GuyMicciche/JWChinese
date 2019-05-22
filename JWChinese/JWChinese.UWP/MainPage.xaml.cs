using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Xamarin.Forms.Platform.UWP;

namespace JWChinese.UWP
{
    public sealed partial class MainPage : WindowsPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadApplication(new JWChinese.App());

            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateRotation();
                
                //await Task.Delay(1500);

                Debug.WriteLine("Getting style..........");

                // Get listview controls inside the SplitView
                var spv = Helper.FindChild<SplitView>(((MainPage)sender), "SplitView");
                Debug.WriteLine(spv);

                foreach (ListView lv in Helper.FindVisualChildren<ListView>(spv))
                {
                    Debug.WriteLine("APPLYING STYLE!!!!!!!!!!!!!!!!!");
                    // Add custom style
                    lv.ItemContainerStyle = (Style)Application.Current.Resources["SplitViewItemContainerStyle"];

                    // Remove listview opening transition
                    TransitionCollection tc = lv.ItemContainerTransitions;
                    for (int i = tc.Count - 1; i >= 0; i--)
                    {
                        if (tc[i] is AddDeleteThemeTransition)
                        {
                            tc.RemoveAt(i);
                        }
                    }
                }

                spv.SizeChanged += Spv_SizeChanged;

                //var btn = this.FindChildControl<Button>("PaneTogglePane");
                //if (btn != null)
                //{
                //    btn.Click += Btn_Click;
                //}

                //var btn2 = this.FindChildControl<Button>("ContentTogglePane");
                //if (btn2 != null)
                //{
                //    btn2.Click += Btn_Click;
                //}
            }
            catch (Exception ex)
            {
                Debug.WriteLine("JWChinese.UWP MainPage_Loaded Exception ex => " + ex.Message);
            }
        }

        private void Spv_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var size = e.NewSize;

            UpdateRotation();
        }

        private void UpdateRotation()
        {
            var display = DisplayInformation.GetForCurrentView();
            var current = display.CurrentOrientation;

            int w = (int)Window.Current.Bounds.Width;
            int h = (int)Window.Current.Bounds.Height;

            if (current == DisplayOrientations.Portrait)
                JWChinese.Objects.Orientation.Rotation = JWChinese.Objects.Rotation.Rotation0;
            else if (current == DisplayOrientations.PortraitFlipped)
                JWChinese.Objects.Orientation.Rotation = JWChinese.Objects.Rotation.Rotation180;
            else if (current == DisplayOrientations.Landscape)
                JWChinese.Objects.Orientation.Rotation = JWChinese.Objects.Rotation.Rotation90;
            else
                JWChinese.Objects.Orientation.Rotation = JWChinese.Objects.Rotation.Rotation270;

            JWChinese.Objects.Orientation.Landscape = (w > h);
            JWChinese.Objects.Orientation.Width = w;
            JWChinese.Objects.Orientation.Height = h;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            var spv = this.FindChildControl<SplitView>("SplitView");
            if (spv != null)
            {
                spv.IsPaneOpen = !spv.IsPaneOpen;
            }
        }        
    }
}