using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using System.Collections.Specialized;
using Android.App;
using Android.Content.Res;
using Android.Views;
using WolDownloader;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using JWChinese.Droid;
using Android.Content;
using Android.Views.Animations;

[assembly: ExportRenderer(typeof(JWChinese.LibraryGridView), typeof(LibraryGridViewRenderer))]
namespace JWChinese.Droid
{
    public class LibraryGridViewRenderer : ViewRenderer<JWChinese.LibraryGridView, global::Android.Views.View>
    {
        private readonly global::Android.Content.Res.Orientation _orientation = global::Android.Content.Res.Orientation.Undefined;

        global::Android.Views.View containerView;

        LibraryGridView libraryView;
        BibleBookNameAdapter bibleBookNameAdapter;

        CenteredGridView numericIndexView;
        NumericIndexGridAdapter numericIndexAdapter;

        //protected override void OnAttachedToWindow()
        //{
        //    if (Control == null)
        //    {
        //        Element_SizeChanged(Element, null);
        //    }

        //    base.OnAttachedToWindow();
        //}

        protected override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            if (newConfig.Orientation != _orientation)
            {
                OnElementChanged(new ElementChangedEventArgs<JWChinese.LibraryGridView>(Element, Element));
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<JWChinese.LibraryGridView> e)
        {
            base.OnElementChanged(e);

            //if (Control == null)
            //{

            //}

            //if (e.OldElement != null)
            //{
            //    if (Element != null)
            //    {
            //        Element.SizeChanged -= Element_SizeChanged;

            //        if (Element.ItemsSource is INotifyCollectionChanged)
            //        {
            //            (Element.ItemsSource as INotifyCollectionChanged).CollectionChanged -= ItemsSource_CollectionChanged;
            //        }
            //    }
            //}

            //if (e.NewElement != null)
            //{
            //    if (Control == null)
            //    {
            //        SetNativeView();
            //    }

            //    Element.SizeChanged += Element_SizeChanged;

            //    if (Element.ItemsSource is INotifyCollectionChanged)
            //    {
            //        (Element.ItemsSource as INotifyCollectionChanged).CollectionChanged += ItemsSource_CollectionChanged;
            //    }
            //}

            Unbind(e.OldElement);
            Bind(e.NewElement);

            //var inflater = LayoutInflater.From(Forms.Context);
            //containerView = inflater.Inflate(Resource.Layout.LibraryFragment, null);

            //libraryView = containerView.FindViewById<LibraryGridView>(Resource.Id.primaryLibraryGridView);
            //adapter = new LibraryGridViewAdapter(Context as Activity, (Element.ItemsSource as ObservableCollection<BibleBook>).ToList());
            //libraryView.Adapter = adapter;
            //libraryView.ItemClick += libraryView_ItemClick;

            //SetNativeControl(containerView);
        }

        private void Unbind(JWChinese.LibraryGridView oldElement)
        {
            if (oldElement != null)
            {
                oldElement.PropertyChanging -= ElementPropertyChanging;
                oldElement.PropertyChanged -= ElementPropertyChanged;
                if (oldElement.ItemsSource is INotifyCollectionChanged)
                {
                    (oldElement.ItemsSource as INotifyCollectionChanged).CollectionChanged -= ItemsSource_CollectionChanged;
                }
            }
        }

        private void Bind(JWChinese.LibraryGridView newElement)
        {
            if (newElement != null)
            {
                if (Control == null)
                {
                    SetNativeView();
                }

                newElement.PropertyChanging += ElementPropertyChanging;
                newElement.PropertyChanged += ElementPropertyChanged;
                if (newElement.ItemsSource is INotifyCollectionChanged)
                {
                    (newElement.ItemsSource as INotifyCollectionChanged).CollectionChanged += ItemsSource_CollectionChanged;
                }
            }
        }

        private void ReloadData()
        {
            if (bibleBookNameAdapter != null)
            {
                bibleBookNameAdapter.NotifyDataSetChanged();
            }

            if(numericIndexAdapter != null)
            {
                numericIndexAdapter.NotifyDataSetChanged();
            }
        }

        private void SetNativeView()
        {
            var inflater = LayoutInflater.From(Forms.Context);

            if (!Element.IsNumber)
            {
                CleanUpLibraryGridView();

                containerView = inflater.Inflate(Resource.Layout.LibraryFragment, null);
                libraryView = containerView.FindViewById<LibraryGridView>(Resource.Id.primaryLibraryGridView);
                bibleBookNameAdapter = new BibleBookNameAdapter(Context as Activity, (Element.ItemsSource as ObservableCollection<BibleBook>).ToList());

                if (Element.Animate)
                {
                    GridLayoutAnimationController controller = (GridLayoutAnimationController)AnimationUtils.LoadLayoutAnimation(Context, Resource.Animation.grid_layout_animation_from_bottom);
                    libraryView.LayoutAnimation = controller;
                }

                libraryView.Adapter = bibleBookNameAdapter;
                libraryView.ItemClick += libraryView_ItemClick;
            }
            else
            {
                CleanUpCenteredGridView();

                containerView = inflater.Inflate(Resource.Layout.NumbersFragment, null);
                numericIndexView = containerView.FindViewById<CenteredGridView>(Resource.Id.primaryCenteredGridView);
                numericIndexView.Visibility = ViewStates.Invisible;
                numericIndexView.VerticalScrollBarEnabled = false;

                if (Element.NumberOfElements > 0)
                {
                    numericIndexAdapter = new NumericIndexGridAdapter(Context as Activity, (int)Element.NumberOfElements);
                }
                else
                {
                    numericIndexAdapter = new NumericIndexGridAdapter(Context as Activity, Element.ItemsSource.Cast<object>().Count());
                }

                //AnimationSet set = new AnimationSet(true);

                //Android.Views.Animations.Animation animation = new AlphaAnimation(0.0f, 1.0f)
                //{
                //    Duration = 500
                //};
                //set.AddAnimation(animation);

                //animation = new TranslateAnimation(Dimension.RelativeToSelf, 0.0f, Dimension.RelativeToSelf, 0.0f, Dimension.RelativeToSelf, -1.0f, Dimension.RelativeToSelf, 0.0f)
                //{
                //    Duration = 500
                //};
                //set.AddAnimation(animation);

                //GridLayoutAnimationController controller = new GridLayoutAnimationController(set, 0.5f, 0.5f);

                //numericIndexView.LayoutAnimation = controller;

                if (Element.Animate)
                {
                    GridLayoutAnimationController controller = (GridLayoutAnimationController)AnimationUtils.LoadLayoutAnimation(Context, Resource.Animation.grid_layout_animation_from_bottom);
                    numericIndexView.LayoutAnimation = controller;
                }
                
                numericIndexView.Adapter = numericIndexAdapter;
                numericIndexView.ItemClick += libraryView_ItemClick;
            }

            SetNativeControl(containerView);
        }

        private void ResizeElement()
        {
            if (libraryView.Height > 100)
            {
                int height = libraryView.Height;
                int proportion = (int)App.GetRealSize(height) / height;
                Element.HeightRequest += 20;
            }
        }

        private void CleanUpLibraryGridView()
        {
            if (libraryView != null)
            {
                libraryView.ItemClick -= libraryView_ItemClick;
                if (libraryView.Adapter != null)
                {
                    libraryView.Adapter.Dispose();
                }
                libraryView.Dispose();
                libraryView = null;
            }
        }

        private void CleanUpCenteredGridView()
        {
            if (numericIndexView != null)
            {
                numericIndexView.ItemClick -= libraryView_ItemClick;
                if (numericIndexView.Adapter != null)
                {
                    numericIndexView.Adapter.Dispose();
                }
                numericIndexView.Dispose();
                numericIndexView = null;
            }
        }

        private void libraryView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var item = Element.ItemsSource.Cast<object>().ElementAt(e.Position);
            Element.InvokeItemSelectedEvent(this, item);
        }

        private void Element_SizeChanged(object sender, EventArgs e)
        {
            if (Element != null)
            {
                //ResizeElement();

                SetNativeView();
            }
        }

        private void ElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == GridView.ItemsSourceProperty.PropertyName)
            {
                if (Element != null && Element.ItemsSource is INotifyCollectionChanged)
                {
                    (Element.ItemsSource as INotifyCollectionChanged).CollectionChanged += ItemsSource_CollectionChanged;
                    ReloadData();
                }
            }
        }
         
        private void ElementPropertyChanging(object sender, Xamarin.Forms.PropertyChangingEventArgs e)
        {
            if (e.PropertyName == GridView.ItemsSourceProperty.PropertyName)
            {
                if (Element != null && Element.ItemsSource is INotifyCollectionChanged)
                {
                    (Element.ItemsSource as INotifyCollectionChanged).CollectionChanged -= ItemsSource_CollectionChanged;
                }
            }
        }

        private void ItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ReloadData();
        }

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                //CleanUpLibraryGridView();

                if (Element != null)
                {
                    Element.SizeChanged -= Element_SizeChanged;
                    if (Element.ItemsSource != null && Element.ItemsSource is INotifyCollectionChanged)
                        ((INotifyCollectionChanged)Element.ItemsSource).CollectionChanged -= ItemsSource_CollectionChanged;
                }

                _disposed = true;
            }

            try
            {
                base.Dispose(disposing);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Dispose => " + ex.Message);
                return;
            }
        }
    }
}



//using Android.App;
//using Android.Views;
//using Android.Widget;
//using JWChinese.Droid;
//using System.Collections.ObjectModel;
//using System.Collections.Specialized;
//using System.ComponentModel;
//using System.Linq;
//using WolDownloader;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.Android;

//[assembly: ExportRenderer(typeof(JWChinese.LibraryGridView), typeof(LibraryGridViewRenderer))]
//namespace JWChinese.Droid
//{
//    public class LibraryGridViewRenderer : ViewRenderer<JWChinese.LibraryGridView, global::Android.Views.View>
//    {
//        private readonly global::Android.Content.Res.Orientation _orientation = global::Android.Content.Res.Orientation.Undefined;

//        global::Android.Views.View containerView;
//        LibraryGridView libraryView;

//        protected override void OnElementChanged(ElementChangedEventArgs<JWChinese.LibraryGridView> e)
//        {
//            base.OnElementChanged(e);

//            //var inflatorservice = (LayoutInflater)Context.GetSystemService("layout_inflater");
//            var inflatorservice = LayoutInflater.From(Forms.Context);
//            containerView = inflatorservice.Inflate(Resource.Layout.LibraryFragment, null, false);

//            libraryView = containerView.FindViewById<LibraryGridView>(Resource.Id.primaryLibraryGridView);

//            Unbind(e.OldElement);
//            Bind(e.NewElement);

//            libraryView.Adapter = new LibraryGridViewAdapter(Context as Activity, (e.NewElement.ItemsSource as ObservableCollection<BibleBook>).ToList());
//            libraryView.ItemClick += CollectionViewItemClick;

//            if (e.OldElement == null)
//            {
//                // perform initial setup
//                SetNativeControl(libraryView);
//            }
//        }

//        void CollectionViewItemClick(object sender, AdapterView.ItemClickEventArgs e)
//        {
//            var item = Element.ItemsSource.Cast<BibleBook>().ElementAt(e.Position);
//            Element.InvokeItemSelectedEvent(this, item);
//        }

//        private void Unbind(JWChinese.LibraryGridView oldElement)
//        {
//            if (oldElement != null)
//            {
//                oldElement.PropertyChanging -= ElementPropertyChanging;
//                oldElement.PropertyChanged -= ElementPropertyChanged;
//                if (oldElement.ItemsSource is INotifyCollectionChanged)
//                {
//                    (oldElement.ItemsSource as INotifyCollectionChanged).CollectionChanged -= DataCollectionChanged;
//                }
//            }
//        }

//        private void Bind(JWChinese.LibraryGridView newElement)
//        {
//            if (newElement != null)
//            {
//                newElement.PropertyChanging += ElementPropertyChanging;
//                newElement.PropertyChanged += ElementPropertyChanged;
//                if (newElement.ItemsSource is INotifyCollectionChanged)
//                {
//                    (newElement.ItemsSource as INotifyCollectionChanged).CollectionChanged += DataCollectionChanged;
//                }
//            }
//        }

//        private void ElementPropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            if (e.PropertyName == "ItemsSource")
//            {
//                if (Element.ItemsSource is INotifyCollectionChanged)
//                {
//                    (Element.ItemsSource as INotifyCollectionChanged).CollectionChanged -= DataCollectionChanged;
//                }
//            }
//        }

//        private void ElementPropertyChanging(object sender, Xamarin.Forms.PropertyChangingEventArgs e)
//        {
//            if (e.PropertyName == "ItemsSource")
//            {
//                if (Element.ItemsSource is INotifyCollectionChanged)
//                {
//                    (Element.ItemsSource as INotifyCollectionChanged).CollectionChanged += DataCollectionChanged;
//                }
//            }
//        }

//        private void DataCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
//        {
//            libraryView.Adapter = new LibraryGridViewAdapter(Context as Activity, (Element.ItemsSource as ObservableCollection<BibleBook>).ToList());
//        }
//    }
//}