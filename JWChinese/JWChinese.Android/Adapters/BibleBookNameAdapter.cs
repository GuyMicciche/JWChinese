using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using System.Linq;

using System;
using System.Collections.Generic;
using WolDownloader;

namespace JWChinese.Droid
{
    public class BibleBookNameAdapter : BaseAdapter
    {
        private Activity activity;
        private List<string> items;

        public BibleBookNameAdapter(Activity activity, List<BibleBook> items) : base()
        {
            this.activity = activity;

            int width = (int)(activity.Resources.GetDimension(Resource.Dimension.bible_nav_bible_book_grid_width) / activity.Resources.DisplayMetrics.Density);
            // Full title
            if (width > 100)
            {
                this.items = items.Select(a => a.StandardBookName).ToList();
            }
            // Short title
            else if (width > 60)
            {
                this.items = items.Select(a => a.OfficialBookAbbreviation).ToList();
            }
            // Abbreviated title
            else
            {
                this.items = items.Select(a => a.OfficialBookAbbreviation).ToList();
            }
        }

        public override int Count
        {
            get { return items.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null; // could wrap a BibleBook in a Java.Lang.Object to return it here if needed
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
                int height = activity.Resources.GetDimensionPixelSize(Resource.Dimension.bible_nav_bible_book_grid_height);
                int textSize = activity.Resources.GetDimensionPixelSize(Resource.Dimension.bible_nav_bible_book_text_size);
                int padding = activity.Resources.GetDimensionPixelSize(Resource.Dimension.bible_nav_bible_book_grid_cell_padding);

                view = new TextView(activity);
                view.SetHeight(height);
                view.SetTextSize(0, textSize);
                view.SetTextColor(activity.Resources.GetColorStateList(Resource.Color.btn_text_style));
                view.SetPadding(padding, 0, padding, 0);
                view.Gravity = GravityFlags.CenterVertical;

                Typeface face = Typeface.CreateFromAsset(activity.Assets, "Fonts/Roboto-Regular.ttf");
                view.SetTypeface(face, TypefaceStyle.Normal);
            }
            else
            {
                view = (TextView)convertView;
            }

            view.SetText(items[position], TextView.BufferType.Normal);
            view.SetBackgroundResource(Resource.Drawable.btn_bg_style);

            return view;
        }
    }
}