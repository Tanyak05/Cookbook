using System;
using System.IO;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Text;
using Java.IO;
using Java.Net;
using Exception = System.Exception;
using String = System.String;


namespace HtmlTextView
{
    public class HtmlHttpImageGetter : Java.Lang.Object, Html.IImageGetter
    {
        public TextView Container;
        public readonly Java.Net.URI BaseUri;
        private readonly bool matchParentWidth;

        private bool compressImage = false;
        private int qualityImage = 50;

        public HtmlHttpImageGetter(TextView textView)
        {
            Container = textView;
            matchParentWidth = false;
        }

        public HtmlHttpImageGetter(TextView textView, String baseUrl)
        {
            this.Container = textView;
            if (baseUrl != null)
            {
                BaseUri = new Java.Net.URI(baseUrl);
            }
        }

        public HtmlHttpImageGetter(TextView textView, String baseUrl, bool matchParentWidth)
        {
            this.Container = textView;
            this.matchParentWidth = matchParentWidth;
            if (baseUrl != null)
            {
                this.BaseUri = new Java.Net.URI(baseUrl);
            }
        }

        public void EnableCompressImage(bool enable)
        {
            EnableCompressImage(enable, 50);
        }

        public void EnableCompressImage(bool enable, int quality)
        {
            compressImage = enable;
            qualityImage = quality;
        }

        public Drawable GetDrawable(String source)
        {
            UrlDrawable urlDrawable = new UrlDrawable();

            // get the actual source
            ImageGetterAsyncTask asyncTask = new ImageGetterAsyncTask(urlDrawable, this, Container,
                matchParentWidth, compressImage, qualityImage);

            asyncTask.Execute(source);

            // return reference to URLDrawable which will asynchronously load the image specified in the src tag
            return urlDrawable;
        }
    }

    /**
     * Static inner {@link AsyncTask} that keeps a {@link WeakReference} to the {@link UrlDrawable}
     * and {@link HtmlHttpImageGetter}.
     * <p/>
     * This way, if the AsyncTask has a longer life span than the UrlDrawable,
     * we won't leak the UrlDrawable or the HtmlRemoteImageGetter.
     */
    public class ImageGetterAsyncTask : AsyncTask<String, Java.Lang.Void, Drawable>
    {
        private readonly System.WeakReference<UrlDrawable> drawableReference;
        private readonly WeakReference<HtmlHttpImageGetter> imageGetterReference;
        private readonly WeakReference<View> containerReference;
        private readonly System.WeakReference<Resources> resources;
        private String source;
        private readonly bool matchParentWidth;
        private float scale;

        private readonly bool compressImage = false;
        private readonly int qualityImage = 50;

        public ImageGetterAsyncTask(UrlDrawable d, HtmlHttpImageGetter imageGetter, View container, bool matchParentWidth, bool compressImage, int qualityImage)
        {
            this.drawableReference = new WeakReference<UrlDrawable>(d);
            this.imageGetterReference = new WeakReference<HtmlHttpImageGetter>(imageGetter);
            this.containerReference = new WeakReference<View>(container);
            this.resources = new WeakReference<Resources>(container.Resources);
            this.matchParentWidth = matchParentWidth;
            this.compressImage = compressImage;
            this.qualityImage = qualityImage;
        }


        protected Drawable DoInBackground(params String[] valueStrings)
        {
            source = valueStrings[0];

            if (resources.TryGetTarget(out Resources ress))
            {
                return compressImage ? FetchCompressedDrawable(ress, source) : FetchDrawable(ress, source);
            }

            return null;
        }


        protected override void OnPostExecute(Drawable result)
        {
            if (result == null)
            {
                //Log.w(HtmlTextView.TAG, "Drawable result is null! (source: " + source + ")");
                return;
            }

            
            if (!drawableReference.TryGetTarget(out UrlDrawable urlDrawable))
            {
                return;
            }

            // set the correct bound according to the result from HTTP call
            urlDrawable.SetBounds(0, 0, (int)(result.IntrinsicWidth * scale), (int)(result.IntrinsicHeight * scale));

            // change the reference of the current drawable to the result from the HTTP call
            urlDrawable.Drawable = result;

            
            if (!imageGetterReference.TryGetTarget(out HtmlHttpImageGetter imageGetter))
            {
                return;
            }

            // redraw the image by invalidating the container
            imageGetter.Container.Invalidate();
            // re-set text to fix images overlapping text
            imageGetter.Container.Text = imageGetter.Container.Text; //TODO -strange
        }

        /**
         * Get the Drawable from URL
         */
        public Drawable FetchDrawable(Resources res, String urlString)
        {
            try
            {
                Stream stream = Fetch(urlString);
                Drawable drawable = new BitmapDrawable(res, stream);
                scale = GetScale(drawable);
                drawable.SetBounds(0, 0, (int)(drawable.IntrinsicWidth * scale), (int)(drawable.IntrinsicHeight * scale));
                return drawable;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /**
         * Get the compressed image with specific quality from URL
         */
        public Drawable FetchCompressedDrawable(Resources res, String urlString)
        {
            try
            {
                Stream stream = Fetch(urlString);
                Bitmap original = new BitmapDrawable(res, stream).Bitmap;

                MemoryStream outt = new MemoryStream();
                original.Compress(Bitmap.CompressFormat.Jpeg, qualityImage, outt);
                original.Recycle();
                stream.Close();

                Bitmap decoded = BitmapFactory.DecodeStream(outt);
                outt.Close();

                scale = GetScale(decoded);
                BitmapDrawable b = new BitmapDrawable(res, decoded);

                b.SetBounds(0, 0, (int)(b.IntrinsicWidth * scale), (int)(b.IntrinsicHeight * scale));
                return b;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private float GetScale(Bitmap bitmap)
        {
            if (!containerReference.TryGetTarget(out View container))
            {
                return 1f;
            }

            float maxWidth = container.Width;
            float originalDrawableWidth = bitmap.Width;

            return maxWidth / originalDrawableWidth;
        }

        private float GetScale(Drawable drawable)
        {
            if (!matchParentWidth || !containerReference.TryGetTarget(out View container))
            {
                return 1f;
            }

            float maxWidth = container.Width;
            float originalDrawableWidth = drawable.IntrinsicWidth;

            return maxWidth / originalDrawableWidth;
        }

        private Stream Fetch(String urlString)
        {
 
            if (!imageGetterReference.TryGetTarget(out HtmlHttpImageGetter imageGetter))
            {
                return null;
            }

            URL url;
            if (imageGetter.BaseUri != null)
            {

                url = imageGetter.BaseUri.Resolve(urlString).ToURL();
            }
            else
            {
                url = URI.Create(urlString).ToURL();
            }

            return url.OpenStream();
        }


        public Drawable GetDrawable(string source)
        {
            throw new NotImplementedException();
        }

        protected override Drawable RunInBackground(params string[] @params)
        {
            throw new NotImplementedException();
        }
    }


//[SuppressWarnings("deprecation")]
    public class UrlDrawable : BitmapDrawable
    {
        public Drawable Drawable;

        public void draw(Canvas canvas)
        {
            // override the draw to facilitate refresh function later
            if (Drawable != null)
            {
                Drawable.Draw(canvas);
            }
        }
    }
} 
