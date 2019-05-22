using Android.Content;
using Android.Content.Res;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using System;
using Android.Graphics;

namespace JWChinese.Droid
{
    public class LibraryGridView : AdapterView<IListAdapter>
    {
        private IListAdapter adapter = null;
        private int horizontalSpacing = 4;
        private int verticalSpacing = 4;
        private int numColumns = 10;
        private int columnWidth = 48;

        public LibraryGridView(Context context) : base(context)
        {

        }

        public LibraryGridView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            TypedArray localTypedArray = context.ObtainStyledAttributes(attrs, Resource.Styleable.LibraryGridView);
            GridViewParams(localTypedArray);
            localTypedArray.Recycle();
        }

        public LibraryGridView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            TypedArray localTypedArray = context.ObtainStyledAttributes(attrs, Resource.Styleable.LibraryGridView, defStyle, 0);
            GridViewParams(localTypedArray);
            localTypedArray.Recycle();
        }

        public LibraryGridView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }

        public void GridViewParams(TypedArray paramTypedArray)
        {
            HorizontalSpacing = paramTypedArray.GetDimensionPixelSize(0, horizontalSpacing);
            VerticalSpacing = paramTypedArray.GetDimensionPixelSize(1, verticalSpacing);
            NumColumns = paramTypedArray.GetInt(3, numColumns);
            ColumnWidth = paramTypedArray.GetDimensionPixelSize(2, columnWidth);
        }

        public override IListAdapter Adapter
        {
            get
            {
                return adapter;
            }
            set
            {
                adapter = value;
                RemoveAllViewsInLayout();

                for (int i = 0; i < adapter.Count; i++)
                {
                    View localView = adapter.GetView(i, null, this);
                    LayoutParams localLayoutParams = localView.LayoutParameters;
                    if (localLayoutParams == null)
                    {
                        localLayoutParams = new LayoutParams(-2, -2);
                    }

                    localView.Clickable = true;
                    localView.SetOnClickListener(new LibraryGridViewListener(this, i));
                    AddViewInLayout(localView, i, localLayoutParams);
                    localView.Measure(GetChildMeasureSpec(MeasureSpec.MakeMeasureSpec(columnWidth, MeasureSpecMode.Exactly), 0, localLayoutParams.Width), GetChildMeasureSpec(MeasureSpec.MakeMeasureSpec(0, 0), 0, localLayoutParams.Height));
                }
                RequestLayout();
            }
        }

        public override Java.Lang.Object GetItemAtPosition(int position)
        {
            return adapter.GetItem(position);
        }

        public int NumColumns
        {
            get
            {
                return numColumns;
            }
            set
            {
                numColumns = value;
                RequestLayout();
            }
        }

        public int ColumnWidth
        {
            get
            {
                return columnWidth;
            }
            set
            {
                columnWidth = value;
            }
        }

        public int HorizontalSpacing
        {
            get
            {
                return horizontalSpacing;
            }
            set
            {
                horizontalSpacing = value;
                RequestLayout();
            }
        }

        public int VerticalSpacing
        {
            get
            {
                return verticalSpacing;
            }
            set
            {
                verticalSpacing = value;
                RequestLayout();
            }
        }

        public override View SelectedView
        {
            get
            {
                return null;
            }
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            base.OnLayout(changed, left, top, right, bottom);

            if (adapter != null)
            {
                int padLeft = PaddingLeft;
                int padRight = PaddingRight;
                int xPos = padLeft;
                int yPos = PaddingTop;
                int columnNumber = 0;
                int totalWidth = ColumnWidth + HorizontalSpacing;

                for (int i = 0; i < ChildCount; i++)
                {
                    View localView = GetChildAt(i);
                    int height = localView.MeasuredHeight;
                    if (NumColumns != 0)
                    {
                        if (columnNumber >= NumColumns)
                        {
                            xPos = padLeft;
                            yPos += height + VerticalSpacing;
                            columnNumber = 0;
                        }
                    }
                    else if (padRight + (xPos + totalWidth) > right)
                    {
                        xPos = padLeft;
                        yPos += height + VerticalSpacing;
                    }
                    localView.Layout(xPos, yPos, xPos + ColumnWidth, yPos + height);
                    xPos += ColumnWidth + HorizontalSpacing;
                    columnNumber++;
                }

                Invalidate();
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            int k = MeasureSpec.GetSize(widthMeasureSpec) - HorizontalSpacing - PaddingLeft - PaddingRight;
            int i = ColumnWidth + HorizontalSpacing;
            View localView = GetChildAt(0);
            int j;
            if (localView != null)
            {
                j = localView.MeasuredHeight;
            }
            else
            {
                j = 16;
            }
            int m = ChildCount;
            if (NumColumns > 0)
            {
                k = Math.Max(NumColumns, 1);
            }
            else
            {
                k = Math.Max(k / i, 1);
            }
            m = 1 + m / k;

            int width = (k * i + HorizontalSpacing);
            int height = (m * (j + VerticalSpacing) + VerticalSpacing);

            SetMeasuredDimension(width, height);
        }

        public override void SetSelection(int position)
        {
            throw new NotImplementedException();
        }
    }

    public class LibraryGridViewListener : Java.Lang.Object, View.IOnClickListener
    {
        LibraryGridView paramLibraryGridView;
        int paramInt;

        public LibraryGridViewListener(LibraryGridView paramLibraryGridView, int paramInt)
        {
            this.paramLibraryGridView = paramLibraryGridView;
            this.paramInt = paramInt;
        }

        public void OnClick(View paramView)
        {
            paramLibraryGridView.PerformItemClick(paramView, paramInt, paramLibraryGridView.GetItemIdAtPosition(paramInt));
        }
    }
}