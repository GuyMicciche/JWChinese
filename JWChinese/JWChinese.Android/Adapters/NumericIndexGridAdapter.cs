using Android.App;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace JWChinese.Droid
{
    public class NumericIndexGridAdapter : BaseAdapter
    {
        private Activity activity;
        private int numElements;

        public NumericIndexGridAdapter(Activity activity, int numElements) : base()
        {
            this.activity = activity;
            this.numElements = numElements;
        }

        public override int Count
        {
            get { return numElements; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            TextView view;

            if (convertView == null)
            {
                int height = activity.Resources.GetDimensionPixelSize(Resource.Dimension.bible_nav_chapter_grid_height);
                int textSize = activity.Resources.GetDimensionPixelSize(Resource.Dimension.bible_nav_chapter_grid_text_size);

                view = new TextView(activity);
                view.SetHeight(height);
                view.SetTextSize(ComplexUnitType.Fraction, textSize);
                view.SetTextColor(activity.Resources.GetColorStateList(Resource.Color.btn_text_style));
                view.Gravity = GravityFlags.Center;
                view.SetBackgroundResource(Resource.Drawable.btn_num_selector);

                Typeface face = Typeface.CreateFromAsset(activity.Assets, "Fonts/Roboto-Regular.ttf");
                view.SetTypeface(face, TypefaceStyle.Normal);
            }
            else
            {
                view = (TextView)convertView;
            }

            view.SetText((position + 1).ToString(), TextView.BufferType.Normal);

            //Animation animation;
            //if (position % 4 == 0)
            //{
            //    animation = AnimationUtils.LoadAnimation(activity, Resource.Animation.grid_layout_animation_from_bottom);
            //    animation.Duration = 340;
            //}
            //else
            //{
            //    animation = AnimationUtils.LoadAnimation(activity, Resource.Animation.grid_layout_animation_from_bottom);
            //    animation.Duration = 280;
            //}

            //convertView.StartAnimation(animation);

            return view;
        }
    }
}