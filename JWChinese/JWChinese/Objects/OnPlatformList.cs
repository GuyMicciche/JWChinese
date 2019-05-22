using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.Forms;

namespace JWChinese
{
    public class OnPlatformList<T> : ObservableCollection<T>
    {
        public ObservableCollection<T> iOS { get; private set; }
        public ObservableCollection<T> Android { get; private set; }
        public ObservableCollection<T> UWP { get; private set; }

        public OnPlatformList()
        {
            iOS = new ObservableCollection<T>();
            Android = new ObservableCollection<T>();
            UWP = new ObservableCollection<T>();

            // Unfortunately, there's no way to see the complete initialization of the tag in XAML,
            // in this case, the ctor fires before we've seen the collection data
            // so hook the collections and wait for each one to change - 
            // when we find the right one, we'll pull it's data and ignore the others.
            iOS.CollectionChanged += OnInitialize;
            Android.CollectionChanged += OnInitialize;
            UWP.CollectionChanged += OnInitialize;
        }

        void OnInitialize(object sender, NotifyCollectionChangedEventArgs e)
        {
            bool foundCollection = false;
            if (Device.RuntimePlatform == Device.iOS && sender == iOS)
            {
                Setup(iOS);
                foundCollection = true;
            }
            else if (Device.RuntimePlatform == Device.Android && sender == Android)
            {
                Setup(Android);
                foundCollection = true;
            }
            else if (Device.RuntimePlatform == Device.WinPhone && sender == UWP)
            {
                Setup(UWP);
                foundCollection = true;
            }

            if (foundCollection)
            {
                iOS.CollectionChanged -= OnInitialize;
                Android.CollectionChanged -= OnInitialize;
                UWP.CollectionChanged -= OnInitialize;
            }
        }

        private ObservableCollection<T> realData;

        void Setup(ObservableCollection<T> data)
        {
            realData = data;
            foreach (var item in data)
                Add(item);

            data.CollectionChanged += (sender, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var item in e.NewItems.Cast<T>())
                            Add(item);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var item in e.OldItems.Cast<T>())
                            Remove(item);
                        break;
                    // TODO: add other operations.
                    default:
                        this.Clear();
                        foreach (var item in realData)
                            this.Add(item);
                        break;
                }

            };
        }
    }
}
