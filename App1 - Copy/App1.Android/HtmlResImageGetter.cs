using System;
using Android.Content;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.Text;
using String = System.String;

namespace HtmlTextView
{
    public class HtmlResImageGetter : Java.Lang.Object, Html.IImageGetter
    {
        private readonly TextView container;

        public HtmlResImageGetter(TextView textView)
        {
            this.container = textView;
        }

        public Drawable GetDrawable(String source)
        {
            Context context = container.Context;
            int id = context.Resources.GetIdentifier(source, "drawable", context.PackageName);

            if (id == 0)
            {
                // the drawable resource wasn't found in our package, maybe it is a stock android drawable?
                id = context.Resources.GetIdentifier(source, "drawable", "android");
            }

            if (id == 0)
            {
                // prevent a crash if the resource still can't be found
                //Log.e(HtmlTextView.TAG, "source could not be found: " + source);
                return null;
            }
            else
            {
                Drawable d = context.Resources.GetDrawable(id);
                d.SetBounds(0, 0, d.IntrinsicWidth, d.IntrinsicHeight);
                return d;
            }
        }

    }
}
