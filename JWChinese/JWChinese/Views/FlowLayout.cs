using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace JWChinese
{
    //public class FlowLayout : Layout<View>
    //{
    //    public static BindableProperty SpacingProperty = BindableProperty.Create("Spacing", typeof(Thickness), typeof(FlowLayout), new Thickness(6));

    //    public Thickness Spacing
    //    {
    //        get
    //        {
    //            return (Thickness)GetValue(SpacingProperty);
    //        }

    //        set
    //        {
    //            SetValue(SpacingProperty, value);
    //            InvalidateLayout();
    //        }
    //    }

    //    public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create("ItemTemplate", typeof(DataTemplate), typeof(FlowLayout), null);
    //    public DataTemplate ItemTemplate
    //    {
    //        get { return (DataTemplate)this.GetValue(ItemTemplateProperty); }
    //        set { SetValue(ItemTemplateProperty, value); }
    //    }
    //    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(IList), typeof(FlowLayout), null, propertyChanged: ItemsSource_PropertyChanged);
    //    public IList ItemsSource
    //    {
    //        get { return (IList)this.GetValue(ItemsSourceProperty); }
    //        set { SetValue(ItemsSourceProperty, value); }
    //    }

    //    private static void ItemsSource_PropertyChanged(BindableObject bindable, object oldValue, object newValue)
    //    {
    //        var flowLayout = (FlowLayout)bindable;
    //        var newItems = newValue as IList;
    //        var oldItems = oldValue as IList;
    //        var oldCollection = oldValue as INotifyCollectionChanged;
    //        if (oldCollection != null)
    //        {
    //            oldCollection.CollectionChanged -= flowLayout.OnCollectionChanged;
    //        }

    //        if (newValue == null)
    //        {
    //            return;
    //        }

    //        if (newItems == null)
    //            return;
    //        if (oldItems == null || newItems.Count != oldItems.Count)
    //        {
    //            flowLayout.Children.Clear();
    //            for (int i = 0; i < newItems.Count; i++)
    //            {
    //                var child = flowLayout.ItemTemplate.CreateContent();
    //                ((BindableObject)child).BindingContext = newItems[i];
    //                flowLayout.Children.Add((View)child);
    //            }

    //        }

    //        var newCollection = newValue as INotifyCollectionChanged;
    //        if (newCollection != null)
    //        {
    //            newCollection.CollectionChanged += flowLayout.OnCollectionChanged;
    //        }

    //        flowLayout.UpdateChildrenLayout();
    //        flowLayout.InvalidateLayout();
    //    }


    //    protected override void OnBindingContextChanged()
    //    {
    //        base.OnBindingContextChanged();
    //    }

    //    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    //    {
    //        if (e.OldItems != null)
    //        {
    //            this.Children.RemoveAt(e.OldStartingIndex);
    //            this.UpdateChildrenLayout();
    //            this.InvalidateLayout();
    //        }

    //        if (e.NewItems == null)
    //        {
    //            return;
    //        }
    //        for (int i = 0; i < e.NewItems.Count; i++)
    //        {
    //            var child = this.ItemTemplate.CreateContent();
    //            ((BindableObject)child).BindingContext = e.NewItems[i];
    //            this.Children.Add((View)child);
    //        }

    //        this.UpdateChildrenLayout();
    //        this.InvalidateLayout();
    //    }

    //    protected override void LayoutChildren(double x, double y, double width, double height)
    //    {
    //        var layoutInfo = new LayoutInfo(Spacing);
    //        layoutInfo.ProcessLayout(Children, width);

    //        for (int i = 0; i < layoutInfo.Bounds.Count; i++)
    //        {
    //            if (!Children[i].IsVisible)
    //            {
    //                continue;
    //            }

    //            var bounds = layoutInfo.Bounds[i];
    //            bounds.Left += x;
    //            bounds.Top += y;

    //            LayoutChildIntoBoundingRegion(Children[i], bounds);
    //        }
    //    }

    //    protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
    //    {
    //        var layoutInfo = new LayoutInfo(Spacing);
    //        layoutInfo.ProcessLayout(Children, widthConstraint);
    //        return new SizeRequest(new Size(widthConstraint, layoutInfo.HeightRequest));
    //    }

    //    public class LayoutInfo
    //    {
    //        double _x = 0;
    //        double _y = 0;
    //        double _rowHeight = 0;
    //        Thickness _spacing;

    //        public LayoutInfo(Thickness spacing)
    //        {
    //            _spacing = spacing;
    //        }

    //        public List<Rectangle> Bounds { get; private set; }

    //        public double HeightRequest { get; private set; }

    //        public void ProcessLayout(IList<View> views, double widthConstraint)
    //        {
    //            Bounds = new List<Rectangle>();
    //            var sizes = SizeViews(views, widthConstraint);
    //            LayoutViews(views, sizes, widthConstraint);
    //        }

    //        private List<Rectangle> SizeViews(IList<View> views, double widthConstraint)
    //        {
    //            var sizes = new List<Rectangle>();

    //            foreach (var view in views)
    //            {
    //                var sizeRequest = view.Measure(widthConstraint, double.PositiveInfinity).Request;
    //                var viewWidth = sizeRequest.Width;
    //                var viewHeight = sizeRequest.Height;

    //                if (viewWidth > widthConstraint)
    //                {
    //                    viewWidth = widthConstraint;
    //                }

    //                sizes.Add(new Rectangle(0, 0, viewWidth, viewHeight));
    //            }

    //            return sizes;
    //        }

    //        private void LayoutViews(IList<View> views, List<Rectangle> sizes, double widthConstraint)
    //        {
    //            Bounds = new List<Rectangle>();
    //            _x = 0d;
    //            _y = 0d;
    //            HeightRequest = 0;

    //            for (int i = 0; i < views.Count(); i++)
    //            {
    //                if (!views[i].IsVisible)
    //                {
    //                    Bounds.Add(new Rectangle(0, 0, 0, 0));
    //                    continue;
    //                }

    //                var sizeRect = sizes[i];

    //                CheckNewLine(sizeRect.Width, widthConstraint);
    //                UpdateRowHeight(sizeRect.Height);

    //                var bound = new Rectangle(_x, _y, sizeRect.Width, sizeRect.Height);
    //                Bounds.Add(bound);

    //                _x += bound.Width;
    //                _x += _spacing.HorizontalThickness;
    //            }

    //            HeightRequest += _rowHeight;
    //        }

    //        private void CheckNewLine(double viewWidth, double widthConstraint)
    //        {
    //            if (_x + viewWidth > widthConstraint)
    //            {
    //                _y += _rowHeight + _spacing.VerticalThickness;
    //                HeightRequest = _y;
    //                _x = 0;
    //                _rowHeight = 0;
    //            }
    //        }

    //        private void UpdateRowHeight(double viewHeight)
    //        {
    //            if (viewHeight > _rowHeight)
    //            {
    //                _rowHeight = viewHeight;
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// cjw1115's FlowLayout
    /// </summary>
    public class FlowLayout : Layout<View>
    {
        Dictionary<View, SizeRequest> layoutCache = new Dictionary<View, SizeRequest>();

        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(FlowLayout), null, BindingMode.OneWay);
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(FlowLayout), null);
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly BindableProperty ColumnProperty = BindableProperty.Create("Column", typeof(int), typeof(FlowLayout), 1);
        public int Column
        {
            get { return (int)this.GetValue(ColumnProperty); }
            set { SetValue(ColumnProperty, value); }
        }
        private double columnWidth = 0;

        public static readonly BindableProperty RowSpacingProerpty = BindableProperty.Create("RowSpacing", typeof(double), typeof(FlowLayout), 0.0);
        public double RowSpacing
        {
            get { return (double)this.GetValue(RowSpacingProerpty); }
            set { SetValue(RowSpacingProerpty, value); }
        }
        public static readonly BindableProperty ColumnSpacingProerpty = BindableProperty.Create("ColumnSpacing", typeof(double), typeof(FlowLayout), 0.0);
        public double ColumnSpacing
        {
            get { return (double)this.GetValue(ColumnSpacingProerpty); }
            set { SetValue(ColumnSpacingProerpty, value); }
        }
        public static readonly BindableProperty FlowProperty = BindableProperty.Create("Flow", typeof(bool), typeof(FlowLayout), true, propertyChanged: OnFlowChanged);
        public bool Flow
        {
            get { return (bool)this.GetValue(FlowProperty); }
            set { SetValue(FlowProperty, value); }
        }

        private static void OnFlowChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as FlowLayout;
            //control.ForceLayout();

            //control.UpdateChildrenLayout();
            //control.InvalidateLayout();
        }

        public FlowLayout()
        {
            VerticalOptions = HorizontalOptions = LayoutOptions.FillAndExpand;
        }

        protected override void OnChildMeasureInvalidated()
        {
            base.OnChildMeasureInvalidated();

            if (Device.RuntimePlatform == Device.Android)
            {
                return;
            }

            layoutCache.Clear();
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            if(Device.RuntimePlatform == Device.Android)
            {
                return;
            }

            // Don't do flow
            if (!Flow)
            {
                double rowHeight = 0;
                var yPos = y;
                var xPos = x;

                double max = 0;

                foreach (var child in Children.Where(c => c.IsVisible))
                {
                    var request = child.Measure(width, height);
                    max = Math.Max(max, request.Request.Width);
                }

                foreach (var child in Children.Where(c => c.IsVisible))
                {
                    if(yPos == y)
                    {
                        Column = Column+1;
                    }

                    var request = child.Measure(width, height);

                    var childWidth = request.Request.Width;
                    var childHeight = request.Request.Height;

                    rowHeight = Math.Max(rowHeight, childHeight);

                    if (xPos + childWidth > width)
                    {
                        xPos = x;
                        yPos += rowHeight + RowSpacing;
                        rowHeight = 0;
                    }

                    var region = new Rectangle(xPos, yPos, childWidth, childHeight);

                    LayoutChildIntoBoundingRegion(child, region);

                    xPos += region.Width + ColumnSpacing;
                }
            }
            // INITIALIZE flow
            else
            {
                double[] colHeights = new double[Column];
                double allColumnSpacing = ColumnSpacing * (Column - 1);
                columnWidth = (width - allColumnSpacing) / Column;
                foreach (var item in this.Children)
                {
                    var measuredSize = item.Measure(columnWidth, height, MeasureFlags.IncludeMargins);
                    int col = 0;
                    for (int i = 1; i < Column; i++)
                    {
                        try
                        {
                            if (colHeights[i] < colHeights[col])
                            {
                                col = i;
                            }
                        }
                        catch (Exception e) { Debug.WriteLine(e.Message); }
                    }
                    item.Layout(new Rectangle(col * (columnWidth + ColumnSpacing), colHeights[col], columnWidth, measuredSize.Request.Height));

                    colHeights[col] += measuredSize.Request.Height + RowSpacing;
                }
            }
        }
        private double _maxHeight;
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                base.OnMeasure(widthConstraint, heightConstraint);
            }

            // Don't do flow
            if (!Flow)
            {
                if (WidthRequest > 0)
                {
                    widthConstraint = Math.Min(widthConstraint, WidthRequest);
                }

                if (HeightRequest > 0)
                {
                    heightConstraint = Math.Min(heightConstraint, HeightRequest);
                }

                var internalWidth = double.IsPositiveInfinity(widthConstraint) ? double.PositiveInfinity : Math.Max(0, widthConstraint);
                var internalHeight = double.IsPositiveInfinity(heightConstraint) ? double.PositiveInfinity : Math.Max(0, heightConstraint);

                return DoHorizontalMeasure(internalWidth, internalHeight);

            }
            else
            {
                // INITIALIZE flow
                double[] colHeights = new double[Column];
                double allColumnSpacing = ColumnSpacing * (Column - 1);
                columnWidth = (widthConstraint - allColumnSpacing) / Column;
                foreach (var item in this.Children)
                {
                    var measuredSize = item.Measure(columnWidth, heightConstraint, MeasureFlags.IncludeMargins);
                    int col = 0;
                    for (int i = 1; i < Column; i++)
                    {
                        if (colHeights[i] < colHeights[col])
                        {
                            col = i;
                        }
                    }
                    colHeights[col] += measuredSize.Request.Height + RowSpacing;
                }
                _maxHeight = colHeights.OrderByDescending(m => m).First();
                
                if(Device.RuntimePlatform == Device.Android)
                {
                    return new SizeRequest(new Size(widthConstraint, _maxHeight));
                }
                else
                {
                    return new SizeRequest(new Size(widthConstraint, _maxHeight));
                }

            }
        }

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create("ItemTemplate", typeof(DataTemplate), typeof(FlowLayout), null);
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)this.GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(IList), typeof(FlowLayout), null, propertyChanged: ItemsSource_PropertyChanged);
        public IList ItemsSource
        {
            get { return (IList)this.GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void ItemsSource_PropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var flowLayout = (FlowLayout)bindable;
            var newItems = newValue as IList;
            var oldItems = oldValue as IList;
            var oldCollection = oldValue as INotifyCollectionChanged;
            if (oldCollection != null)
            {
                oldCollection.CollectionChanged -= flowLayout.OnCollectionChanged;
            }

            if (newValue == null)
            {
                return;
            }

            if (newItems == null)
                return;
            if (oldItems == null || newItems.Count != oldItems.Count)
            {
                flowLayout.Children.Clear();
                for (int i = 0; i < newItems.Count; i++)
                {
                    try
                    {
                        var child = flowLayout.ItemTemplate.CreateContent();
                        ((BindableObject)child).BindingContext = newItems[i];
                        flowLayout.Children.Add((View)child);
                    }
                    catch (Exception ex) { Debug.WriteLine(ex.Message); }
                }

            }

            var newCollection = newValue as INotifyCollectionChanged;
            if (newCollection != null)
            {
                newCollection.CollectionChanged += flowLayout.OnCollectionChanged;
            }

            flowLayout.UpdateChildrenLayout();
            flowLayout.InvalidateLayout();
        }


        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                return;
            }

            if (e.OldItems != null)
            {
                this.Children.RemoveAt(e.OldStartingIndex);
                this.UpdateChildrenLayout();
                this.InvalidateLayout();
            }

            if (e.NewItems == null)
            {
                return;
            }
            for (int i = 0; i < e.NewItems.Count; i++)
            {
                var child = this.ItemTemplate.CreateContent();
                ((BindableObject)child).BindingContext = e.NewItems[i];
                this.Children.Add((View)child);
            }

            this.UpdateChildrenLayout();
            this.InvalidateLayout();
        }

        private SizeRequest DoHorizontalMeasure(double widthConstraint, double heightConstraint)
        {
            var rowCount = 1;

            double width = 0;
            double height = 0;
            double minWidth = 0;
            double minHeight = 0;
            double widthUsed = 0;

            foreach (var item in Children.Where(c => c.IsVisible))
            {
                var size = item.Measure(widthConstraint, heightConstraint);

                height = Math.Max(height, size.Request.Height);

                var newWidth = width + size.Request.Width + RowSpacing;

                if (newWidth > widthConstraint)
                {
                    rowCount++;
                    widthUsed = Math.Max(width, widthUsed);
                    width = size.Request.Width;
                }
                else
                {
                    width = newWidth;
                }

                minHeight = Math.Max(minHeight, size.Minimum.Height);
                minWidth = Math.Max(minWidth, size.Minimum.Width);
            }

            if (rowCount <= 1)
            {
                return new SizeRequest(new Size(width, height), new Size(minWidth, minHeight));
            }

            width = Math.Max(width, widthUsed);
            height = (height + RowSpacing) * rowCount;   // - Spacing;
            //height *= rowCount;  // take max height

            return new SizeRequest(new Size(width, height), new Size(minWidth, minHeight));
        }

        private List<List<Tuple<View, Rectangle>>> NaiveLayout(double width, double height, out double lastX, out double lastY)
        {
            double startX = 0;
            double startY = 0;
            double right = width;
            double nextY = 0;

            lastX = 0;
            lastY = 0;

            var result = new List<List<Tuple<View, Rectangle>>>();
            var currentList = new List<Tuple<View, Rectangle>>();

            foreach (var child in Children)
            {
                SizeRequest sizeRequest;
                if (!layoutCache.TryGetValue(child, out sizeRequest))
                {
                    layoutCache[child] = sizeRequest = child.Measure(double.PositiveInfinity, double.PositiveInfinity);
                }

                var paddedWidth = sizeRequest.Request.Width + ColumnSpacing;
                var paddedHeight = sizeRequest.Request.Height + RowSpacing;

                if (startX + paddedWidth > right)
                {
                    startX = 0;
                    startY += nextY;

                    if (currentList.Count > 0)
                    {
                        result.Add(currentList);
                        currentList = new List<Tuple<View, Rectangle>>();
                    }
                }

                currentList.Add(new Tuple<View, Rectangle>(child, new Rectangle(startX, startY, sizeRequest.Request.Width, sizeRequest.Request.Height)));

                lastX = Math.Max(lastX, startX + paddedWidth);
                lastY = Math.Max(lastY, startY + paddedHeight);

                nextY = Math.Max(nextY, paddedHeight);
                startX += paddedWidth;
            }
            result.Add(currentList);
            return result;
        }

        public event EventHandler<SelectedItemChangedEventArgs> ItemSelected;
        public void InvokeItemSelectedEvent(object sender, object item)
        {
            Command?.Execute(item);

            ItemSelected?.Invoke(sender, new SelectedItemChangedEventArgs(item));
        }
    }

    public class LayoutInfo
    {
        double _x = 0;
        double _y = 0;
        double _rowHeight = 0;
        Thickness _spacing;

        public LayoutInfo(Thickness spacing)
        {
            _spacing = spacing;
        }

        public List<Rectangle> Bounds { get; private set; }

        public double HeightRequest { get; private set; }

        public void ProcessLayout(IList<View> views, double widthConstraint)
        {
            Bounds = new List<Rectangle>();
            var sizes = SizeViews(views, widthConstraint);
            LayoutViews(views, sizes, widthConstraint);
        }

        private List<Rectangle> SizeViews(IList<View> views, double widthConstraint)
        {
            var sizes = new List<Rectangle>();

            foreach (var view in views)
            {
                var sizeRequest = view.Measure(widthConstraint, double.PositiveInfinity).Request;
                var viewWidth = sizeRequest.Width;
                var viewHeight = sizeRequest.Height;

                if (viewWidth > widthConstraint)
                {
                    viewWidth = widthConstraint;
                }

                sizes.Add(new Rectangle(0, 0, viewWidth, viewHeight));
            }

            return sizes;
        }

        private void LayoutViews(IList<View> views, List<Rectangle> sizes, double widthConstraint)
        {
            Bounds = new List<Rectangle>();
            _x = 0d;
            _y = 0d;
            HeightRequest = 0;

            for (int i = 0; i < views.Count(); i++)
            {
                if (!views[i].IsVisible)
                {
                    Bounds.Add(new Rectangle(0, 0, 0, 0));
                    continue;
                }

                var sizeRect = sizes[i];

                CheckNewLine(sizeRect.Width, widthConstraint);
                UpdateRowHeight(sizeRect.Height);

                var bound = new Rectangle(_x, _y, sizeRect.Width, sizeRect.Height);
                Bounds.Add(bound);

                _x += bound.Width;
                _x += _spacing.HorizontalThickness;
            }

            HeightRequest += _rowHeight;
        }

        private void CheckNewLine(double viewWidth, double widthConstraint)
        {
            if (_x + viewWidth > widthConstraint)
            {
                _y += _rowHeight + _spacing.VerticalThickness;
                HeightRequest = _y;
                _x = 0;
                _rowHeight = 0;
            }
        }

        private void UpdateRowHeight(double viewHeight)
        {
            if (viewHeight > _rowHeight)
            {
                _rowHeight = viewHeight;
            }
        }

        
    }

    /// <summary>
    /// 
    /// </summary>
    //public class FlowLayout : Layout<View>
    //{
    //    public static readonly BindableProperty ColumnSpacingProperty =
    //            BindableProperty.Create(
    //                "ColumnSpacing",
    //                typeof(double),
    //                typeof(FlowLayout),
    //                6.0,
    //                propertyChanged: (bindable, oldvalue, newvalue) =>
    //                {
    //                    ((FlowLayout)bindable).InvalidateLayout();
    //                });

    //    public static readonly BindableProperty RowSpacingProperty =
    //            BindableProperty.Create(
    //                "RowSpacing",
    //                typeof(double),
    //                typeof(FlowLayout),
    //                6.0,
    //                propertyChanged: (bindable, oldvalue, newvalue) =>
    //                {
    //                    ((FlowLayout)bindable).InvalidateLayout();
    //                });


    //    public double ColumnSpacing
    //    {
    //        set { SetValue(ColumnSpacingProperty, value); }
    //        get { return (double)GetValue(ColumnSpacingProperty); }
    //    }

    //    public double RowSpacing
    //    {
    //        set { SetValue(RowSpacingProperty, value); }
    //        get { return (double)GetValue(RowSpacingProperty); }
    //    }


    //    protected override void LayoutChildren(double x, double y, double width, double height)
    //    {
    //        double xChild = 0, yChild = 0;
    //        //循环子视图
    //        foreach (View child in Children)
    //        {
    //            if (!child.IsVisible)
    //            {
    //                continue;
    //            }

    //            SizeRequest childSizeRequest = child.Measure(width, height);

    //            // Initialize child position and size.
    //            var childWidth = childSizeRequest.Request.Width;
    //            var childHeight = childSizeRequest.Request.Height;

    //            if (xChild + childWidth > width)
    //            {
    //                xChild = 0;
    //                yChild += childHeight + RowSpacing;
    //            }

    //            //判断HorizontalOptions和VerticalOptions的值计算子视图 x,y,width,height的值
    //            //switch (child.HorizontalOptions.Alignment)
    //            //{
    //            //    case LayoutAlignment.Start:
    //            //        break;
    //            //    case LayoutAlignment.Center:
    //            //        //xChild += (width - childWidth) / 2;
    //            //        break;
    //            //    case LayoutAlignment.End:
    //            //        //xChild += (width - childWidth);
    //            //        break;
    //            //    case LayoutAlignment.Fill:
    //            //        //childWidth = width;
    //            //        break;
    //            //}

    //            // Layout the child.
    //            LayoutChildIntoBoundingRegion(child, new Rectangle(xChild, yChild, width, childHeight));

    //            xChild = xChild + childWidth + ColumnSpacing;
    //        }
    //    }

    //    /// <summary>
    //    /// 计算出显示子元素所需大小 
    //    /// </summary>
    //    /// <returns>The measure.</returns>
    //    /// <param name="widthConstraint">Width constraint.</param>
    //    /// <param name="heightConstraint">Height constraint.</param>
    //    protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
    //    {
    //        var size = new Size();
    //        var width = 0.0;

    //        //循环子视图
    //        foreach (View child in Children)
    //        {
    //            if (!child.IsVisible)
    //            {
    //                continue;
    //            }

    //            // Get the child's requested size.
    //            SizeRequest childSizeRequest = child.Measure(widthConstraint, Double.PositiveInfinity);
    //            // Initialize child position and size.
    //            var childWidth = childSizeRequest.Request.Width;
    //            var childHeight = childSizeRequest.Request.Height;

    //            if (width + childWidth >= widthConstraint)
    //            {
    //                size.Height = size.Height + childHeight + RowSpacing;
    //            }
    //            else
    //            {
    //                width = width + childWidth + ColumnSpacing;
    //                size.Width = Math.Max(size.Width, width);
    //            }

    //        }
    //        //widthConstraint或heightConstraint的值可能为Double.PositiveInfinity，
    //        //但是OnMeasure不能返回Double.PositiveInfinity所以不能有如下代码
    //        //return new SizeRequest(new Size(widthConstraint, heightConstraint));

    //        return new SizeRequest(size);
    //    }

    //    #region 测试时编写的无关代码

    //    protected override bool ShouldInvalidateOnChildAdded(View child)
    //    {
    //        return false;
    //    }

    //    protected override bool ShouldInvalidateOnChildRemoved(View child)
    //    {
    //        return base.ShouldInvalidateOnChildRemoved(child);
    //    }

    //    protected override void OnAdded(View view)
    //    {
    //        base.OnAdded(view);
    //        //根据自己需要编码
    //    }

    //    protected override void OnRemoved(View view)
    //    {
    //        base.OnRemoved(view);
    //        //根据自己需要编码
    //    }


    //    protected override void OnChildMeasureInvalidated()
    //    {
    //        base.OnChildMeasureInvalidated();
    //    }

    //    #endregion

    //}
}
