using PropertyChanged;
using System;
using System.Collections;
using System.Windows.Input;
using Xamarin.Forms;

namespace JWChinese
{
    [AddINotifyPropertyChangedInterface]
    public class LibraryGridView : View
    {
        public LibraryGridView()
        {

        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(LibraryGridView), null);
        public static readonly BindableProperty RowSpacingProperty = BindableProperty.Create("RowSpacing", typeof(double), typeof(LibraryGridView), (double)0);
        public static readonly BindableProperty ColumnSpacingProperty = BindableProperty.Create("ColumnSpacing", typeof(double), typeof(LibraryGridView), (double)0);
        public static readonly BindableProperty ItemWidthProperty = BindableProperty.Create("ItemWidth", typeof(double), typeof(LibraryGridView), (double)100);
        public static readonly BindableProperty ItemHeightProperty = BindableProperty.Create("ItemHeight", typeof(double), typeof(LibraryGridView), (double)100);
        public static readonly BindableProperty IsNumberProperty = BindableProperty.Create("IsNumber", typeof(bool), typeof(LibraryGridView), false);
        public static readonly BindableProperty AnimateProperty = BindableProperty.Create("Animate", typeof(bool), typeof(LibraryGridView), false);
        public static readonly BindableProperty NumberOfElementsProperty = BindableProperty.Create("NumberOfElements", typeof(double), typeof(LibraryGridView), (double)0);
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(LibraryGridView), null);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(LibraryGridView), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value);}
        }

        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        public bool IsNumber
        {
            get { return (bool)GetValue(IsNumberProperty); }
            set { SetValue(IsNumberProperty, value); }
        }

        public bool Animate
        {
            get { return (bool)GetValue(AnimateProperty); }
            set { SetValue(AnimateProperty, value); }
        }

        public double NumberOfElements
        {
            get { return (double)GetValue(NumberOfElementsProperty); }
            set { SetValue(NumberOfElementsProperty, value); }
        }

        public event EventHandler<SelectedItemChangedEventArgs> ItemSelected;
        public void InvokeItemSelectedEvent(object sender, object item)
        {
            Command?.Execute(item);

            ItemSelected?.Invoke(sender, new SelectedItemChangedEventArgs(item));
        }
    }
}