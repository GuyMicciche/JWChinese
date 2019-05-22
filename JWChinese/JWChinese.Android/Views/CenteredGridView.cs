using Android.Content;
using Android.Content.Res;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using ViewTreeObserver = Android.Views.ViewTreeObserver;
using IOnGlobalLayoutListener = Android.Views.ViewTreeObserver.IOnGlobalLayoutListener;

using System;
using Android.Graphics;
using Android.OS;
using Android.Views.Animations;

namespace JWChinese.Droid
{
    public class CenteredGridView : Android.Widget.GridView, IOnGlobalLayoutListener
    {
        public CenteredGridView(Context context) : base(context)
        {
            Visibility = ViewStates.Invisible;
        }

        public CenteredGridView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Visibility = ViewStates.Invisible;
        }

        public CenteredGridView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Visibility = ViewStates.Invisible;
        }

        public CenteredGridView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }

        public override IListAdapter Adapter
        {
            set
            {
                base.Adapter = value;
                ViewTreeObserver.AddOnGlobalLayoutListener(this);
            }
        }

        private bool PaddingCorrection(ViewGroup paramViewGroup, int paramInt1, int paramInt2, int paramInt3, int paramInt4, int paramInt5)
        {
            bool correct;
            int i;

            if (paramInt4 >= ConvertDpToPx(Context, 16))
            {
                correct = false;
            }
            else
            {
                i = (paramInt2 + (paramInt5 + (paramViewGroup.MeasuredWidth - paramInt1))) / 2;
                SetPadding(i, paramInt3, i, paramInt3);
                correct = true;
            }
            return correct;
        }

        public override void OnGlobalLayout()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean)
            {
                ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
            }
            else
            {
                //noinspection deprecation
                ViewTreeObserver.RemoveGlobalOnLayoutListener(this);
            }

            int spacing = Resources.GetDimensionPixelSize(Resource.Dimension.bible_nav_bible_book_grid_spacing);
            int width = Resources.GetDimensionPixelSize(Resource.Dimension.bible_nav_chapter_grid_width);
            int columns = NumColumns;
            int i;

            if (columns >= 10)
            {
                i = 10 * (width + spacing);
            }
            else
            {
                i = columns * (width + spacing);
            }

            ViewGroup localViewGroup = (ViewGroup)Parent;
            if (localViewGroup != null)
            {
                int m = Resources.GetDimensionPixelSize(Resource.Dimension.bible_nav_chapter_horizontal_padding);

                if (columns >= 10)
                {
                    columns = localViewGroup.MeasuredWidth - i - m;
                    if (!PaddingCorrection(localViewGroup, i, spacing, m, columns, width))
                    {
                        SetPadding(m, m, columns, m);
                    }
                }
                else
                {
                    columns = (localViewGroup.MeasuredWidth - i) / 2;
                    if (!PaddingCorrection(localViewGroup, i, spacing, m, columns, width))
                    {
                        SetPadding(columns, m, columns, m);
                    }
                }
            }

            ViewTreeObserver.AddOnGlobalLayoutListener(new CenteredGridViewListener(this));
        }

        private Int32 ConvertDpToPx(Context context, Single dp)
        {
            return (Int32)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, context.Resources.DisplayMetrics);
        }
    }

    public class CenteredGridViewListener : Java.Lang.Object, IOnGlobalLayoutListener
    {
        CenteredGridView paramCenteredGridView;

        public CenteredGridViewListener(CenteredGridView paramCenteredGridView)
        {
            this.paramCenteredGridView = paramCenteredGridView;
        }

        public void OnGlobalLayout()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean)
            {
                paramCenteredGridView.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
            }
            else
            {
                //noinspection deprecation
                paramCenteredGridView.ViewTreeObserver.RemoveGlobalOnLayoutListener(this);
            }
            paramCenteredGridView.Visibility = ViewStates.Visible;
        }
    }
}