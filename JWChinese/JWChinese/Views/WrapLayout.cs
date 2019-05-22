// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="WrapLayout.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace JWChinese
{
    /// <summary>
    /// New WrapLayout
    /// </summary>
    /// <author>Jason Smith</author>
    public class WrapLayout : Layout<View>
    {
        Dictionary<View, SizeRequest> layoutCache = new Dictionary<View, SizeRequest>();

        /// <summary>
        /// Backing Storage for the Spacing property
        /// </summary>
        public static readonly BindableProperty SpacingProperty =
            BindableProperty.Create<WrapLayout, double>(w => w.Spacing, 5,
                propertyChanged: (bindable, oldvalue, newvalue) => ((WrapLayout)bindable).layoutCache.Clear());

        /// <summary>
        /// Spacing added between elements (both directions)
        /// </summary>
        /// <value>The spacing.</value>
        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        public WrapLayout()
        {
            VerticalOptions = HorizontalOptions = LayoutOptions.FillAndExpand;
        }

        protected override void OnChildMeasureInvalidated()
        {
            base.OnChildMeasureInvalidated();
            layoutCache.Clear();
        }

        protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
        {

            double lastX;
            double lastY;
            var layout = NaiveLayout(widthConstraint, heightConstraint, out lastX, out lastY);

            return new SizeRequest(new Size(lastX, lastY));
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            double lastX, lastY;
            var layout = NaiveLayout(width, height, out lastX, out lastY);

            foreach (var t in layout)
            {
                var offset = (int)((width - t.Last().Item2.Right) / 2);
                foreach (var dingus in t)
                {
                    var location = new Rectangle(dingus.Item2.X + x + offset, dingus.Item2.Y + y, dingus.Item2.Width, dingus.Item2.Height);
                    LayoutChildIntoBoundingRegion(dingus.Item1, location);
                }
            }
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

                var paddedWidth = sizeRequest.Request.Width + Spacing;
                var paddedHeight = sizeRequest.Request.Height + Spacing;

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
    }

    /// <summary>
    /// Simple Layout panel which performs wrapping on the boundaries.
    /// Original Source:
    /// https://github.com/conceptdev/xamarin-forms-samples/blob/master/Evolve13/Evolve13/Controls/WrapLayout.cs
    /// </summary>
    public class WrapLayoutOld : Layout<View>
    {
        /// <summary>
        /// Backing Storage for the Orientation property
        /// </summary>
        public static readonly BindableProperty OrientationProperty =
            BindableProperty.Create<WrapLayoutOld, StackOrientation>(w => w.Orientation, StackOrientation.Horizontal,
                propertyChanged: (bindable, oldvalue, newvalue) => ((WrapLayoutOld)bindable).OnSizeChanged());

        /// <summary>
        /// Orientation (Horizontal or Vertical)
        /// </summary>
        public StackOrientation Orientation
        {
            get { return (StackOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Backing Storage for the Spacing property
        /// </summary>
        public static readonly BindableProperty SpacingProperty =
            BindableProperty.Create<WrapLayoutOld, double>(w => w.Spacing, 2,
                propertyChanged: (bindable, oldvalue, newvalue) => ((WrapLayoutOld)bindable).OnSizeChanged());

        /// <summary>
        /// Spacing added between elements (both directions)
        /// </summary>
        /// <value>The spacing.</value>
        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        /// <summary>
        /// This is called when the spacing or orientation properties are changed - it forces
        /// the control to go back through a layout pass.
        /// </summary>
        private void OnSizeChanged()
        {
            ForceLayout();
        }

        //http://forums.xamarin.com/discussion/17961/stacklayout-with-horizontal-orientation-how-to-wrap-vertically#latest
        //		protected override void OnPropertyChanged
        //		(string propertyName = null)
        //		{
        //			base.OnPropertyChanged(propertyName);
        //			if ((propertyName == WrapLayout.OrientationProperty.PropertyName) ||
        //				(propertyName == WrapLayout.SpacingProperty.PropertyName)) {
        //				this.OnSizeChanged();
        //			}
        //		}

        /// <summary>
        /// This method is called during the measure pass of a layout cycle to get the desired size of an element.
        /// </summary>
        /// <param name="widthConstraint">The available width for the element to use.</param>
        /// <param name="heightConstraint">The available height for the element to use.</param>
        protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
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

            return Orientation == StackOrientation.Vertical
                ? DoVerticalMeasure(internalWidth, internalHeight)
                    : DoHorizontalMeasure(internalWidth, internalHeight);
        }

        /// <summary>
        /// Does the vertical measure.
        /// </summary>
        /// <returns>The vertical measure.</returns>
        /// <param name="widthConstraint">Width constraint.</param>
        /// <param name="heightConstraint">Height constraint.</param>
        private SizeRequest DoVerticalMeasure(double widthConstraint, double heightConstraint)
        {
            var columnCount = 1;

            double width = 0;
            double height = 0;
            double minWidth = 0;
            double minHeight = 0;
            double heightUsed = 0;

            foreach (var size in Children.Where(c => c.IsVisible).Select(item => item.Measure(widthConstraint, heightConstraint)))
            {
                width = Math.Max(width, size.Request.Width);

                var newHeight = height + size.Request.Height + Spacing;

                if (newHeight > heightConstraint)
                {
                    columnCount++;
                    heightUsed = Math.Max(height, heightUsed);
                    height = size.Request.Height;
                }
                else
                {
                    height = newHeight;
                }

                minHeight = Math.Max(minHeight, size.Minimum.Height);
                minWidth = Math.Max(minWidth, size.Minimum.Width);
            }

            if (columnCount <= 1)
            {
                return new SizeRequest(new Size(width, height), new Size(minWidth, minHeight));
            }

            height = Math.Max(height, heightUsed);
            width *= columnCount;  // take max width

            return new SizeRequest(new Size(width, height), new Size(minWidth, minHeight));
        }

        /// <summary>
        /// Does the horizontal measure.
        /// </summary>
        /// <returns>The horizontal measure.</returns>
        /// <param name="widthConstraint">Width constraint.</param>
        /// <param name="heightConstraint">Height constraint.</param>
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

                var newWidth = width + size.Request.Width + Spacing;

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
            height = (height + Spacing) * rowCount;   // - Spacing;
            //height *= rowCount;  // take max height

            return new SizeRequest(new Size(width, height), new Size(minWidth, minHeight));
        }

        /// <summary>
        /// Positions and sizes the children of a Layout.
        /// </summary>
        /// <param name="x">A value representing the x coordinate of the child region bounding box.</param>
        /// <param name="y">A value representing the y coordinate of the child region bounding box.</param>
        /// <param name="width">A value representing the width of the child region bounding box.</param>
        /// <param name="height">A value representing the height of the child region bounding box.</param>
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            if (Orientation == StackOrientation.Vertical)
            {
                double colWidth = 0;
                var yPos = y;
                var xPos = x;

                foreach (var child in Children.Where(c => c.IsVisible))
                {
                    var request = child.Measure(width, height);

                    var childWidth = request.Request.Width;
                    var childHeight = request.Request.Height;

                    colWidth = Math.Max(colWidth, childWidth);

                    if (yPos + childHeight > height)
                    {
                        yPos = y;
                        xPos += colWidth + Spacing;
                        colWidth = 0;
                    }

                    var region = new Rectangle(xPos, yPos, childWidth, childHeight);

                    LayoutChildIntoBoundingRegion(child, region);

                    yPos += region.Height + Spacing;
                }
            }
            else
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
                    var request = child.Measure(width, height);

                    var childWidth = request.Request.Width;
                    var childHeight = request.Request.Height;

                    rowHeight = Math.Max(rowHeight, childHeight);

                    if (xPos + childWidth > width)
                    {
                        xPos = x;
                        yPos += rowHeight + Spacing;
                        rowHeight = 0;
                    }

                    var region = new Rectangle(xPos, yPos, childWidth, childHeight);

                    LayoutChildIntoBoundingRegion(child, region);

                    xPos += region.Width + Spacing;
                }
            }
        }
    }

    public class GridViewLayout : Layout<View>
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create<GridViewLayout, IEnumerable>(o => o.ItemsSource, default(IEnumerable), propertyChanged: OnItemsSourceChanged);
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void OnItemsSourceChanged(BindableObject bindable, IEnumerable oldvalue, IEnumerable newvalue)
        {
            var view = (GridViewLayout)bindable;
            view.ReCreateChildrens();
        }

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create<GridViewLayout, DataTemplate>(o => o.ItemTemplate, default(DataTemplate), propertyChanged: OnItemsTemplateChanged);

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        private static void OnItemsTemplateChanged(BindableObject bindable, DataTemplate oldvalue, DataTemplate newvalue)
        {
            var view = (GridViewLayout)bindable;
            view.ReCreateChildrens();   
        }

        public int MaxItemsPerRow { get; set; }
        public int ItemHeight { get; set; }
        public double RowSpacing { get; set; }
        public double ColumnSpacing { get; set; }


        public GridViewLayout()
        {
            //BackgroundColor = Color.Red;
        }

        private void ReCreateChildrens()
        {
            //if (ItemsSource == null || ItemTemplate == null)
            //    return;

            foreach (var item in ItemsSource)
            {
                //var view = ItemTemplate.CreateContent() as View;
                //view.BindingContext = item;
                //Children.Add(view);

                Children.Add(item as Frame);
            }
        }


        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            var colWidth = width / MaxItemsPerRow;
            for (int i = 0; i < Children.Count; i++)
            {
                var child = Children[i];
                if (!child.IsVisible)
                    continue;

                var virtualColumn = i % MaxItemsPerRow;
                var virtualRow = i / MaxItemsPerRow;

                var rowSpacing = (virtualRow != 0) ? RowSpacing : 0;
                var colSpacing = (virtualColumn != 0) ? ColumnSpacing : 0;

                var childX = x + (colWidth + colSpacing) * virtualColumn;
                var childY = y + (ItemHeight + rowSpacing) * virtualRow;
                LayoutChildIntoBoundingRegion(child, new Rectangle(childX, childY, colWidth, ItemHeight));
            }
        }

        protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
        {

            // Check our cache for existing results
            SizeRequest cachedResult;
            var constraintSize = new Size(widthConstraint, heightConstraint);
            if (_measureCache.TryGetValue(constraintSize, out cachedResult))
            {
                return cachedResult;
            }


            var height = 0.0;
            var minHeight = 0.0;
            var width = 0.0;
            var minWidth = 0.0;

            var visibleChildrensCount = (double)Children.Count(c => c.IsVisible);
            var rowsCount = Math.Ceiling(visibleChildrensCount / MaxItemsPerRow);
            height = minHeight = (ItemHeight + RowSpacing) * rowsCount - RowSpacing;
            width = minWidth = widthConstraint;

            // store our result in the cache for next time
            var result = new SizeRequest(new Size(width, height), new Size(minWidth, minHeight));
            _measureCache[constraintSize] = result;
            return result;
        }

        protected override void InvalidateMeasure()
        {
            _measureCache.Clear();
            base.InvalidateMeasure();
        }

        readonly Dictionary<Size, SizeRequest> _measureCache = new Dictionary<Size, SizeRequest>();

    }
}