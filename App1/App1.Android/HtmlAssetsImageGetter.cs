using System;
using System.IO;
using Android.Content;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.Text;
using IOException = System.IO.IOException;
using String = System.String;


namespace HtmlTextView
{
    public class HtmlAssetsImageGetter : Java.Lang.Object,  Html.IImageGetter
    {

        private readonly Context context;

        public HtmlAssetsImageGetter(Context context)
        {
            this.context = context;
        }

        public HtmlAssetsImageGetter(TextView textView)
        {
            this.context = textView.Context;
        }

        public Drawable GetDrawable(String source)
        {

            try
            {
                Stream inputStream = context.Assets.Open(source);
                Drawable d = Drawable.CreateFromStream(inputStream, null);
                d.SetBounds(0, 0, d.IntrinsicWidth, d.IntrinsicHeight);
                return d;
            }
            catch (IOException)
            {
                // prevent a crash if the resource still can't be found
                //Log.e(HtmlTextView.TAG, "source could not be found: " + source);
                return null;
            }

        }

    }
}
