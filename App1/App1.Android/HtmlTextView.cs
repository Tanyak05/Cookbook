using System.IO;
using Android.Content;
using Android.Util;
using Android.Text;
using Java.Util;
using String = System.String;


namespace HtmlTextView
{
    public class HtmlTextView : JellyBeanSpanFixTextView
    {

        //public static  String TAG = "HtmlTextView";
        //public static  bool DEBUG = false;

        //@Nullable
        private ClickableTableSpan clickableTableSpan;
        //@Nullable
        private DrawTableLinkSpan drawTableLinkSpan;
        private float indent = 24.0f; // Default to 24px.

        //private bool removeTrailingWhiteSpace = true;

        public HtmlTextView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {

        }


        public HtmlTextView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {

        }


        public HtmlTextView(Context context)
            : base(context)
        {
        }

        /**
         * @see org.sufficientlysecure.htmltextview.HtmlTextView#setHtml(int)
         */
        public void SetHtml(int resId)
        {
            SetHtml(resId, null);
        }

        /**
         * @see org.sufficientlysecure.htmltextview.HtmlTextView#setHtml(String)
         */
        public void SetHtml(String html)
        {
            SetHtml(html, null);
        }

        /**
         * Loads HTML from a raw resource, i.e., a HTML file in res/raw/.
         * This allows translatable resource (e.g., res/raw-de/ for german).
         * The containing HTML is parsed to Android's Spannable format and then displayed.
         *
         * @param resId       for example: R.raw.help
         * @param imageGetter for fetching images. Possible ImageGetter provided by this library:
         *                    HtmlLocalImageGetter and HtmlRemoteImageGetter
         */
        public void SetHtml(int resId, Html.IImageGetter imageGetter) {
            Stream inputStreamText = Context.Resources.OpenRawResource(resId);

            SetHtml(ConvertStreamToString(inputStreamText), imageGetter);
        }

        /**
         * Parses String containing HTML to Android's Spannable format and displays it in this TextView.
         * Using the implementation of Html.ImageGetter provided.
         *
         * @param html        String containing HTML, for example: "<b>Hello world!</b>"
         * @param imageGetter for fetching images. Possible ImageGetter provided by this library:
         *                    HtmlLocalImageGetter and HtmlRemoteImageGetter
         */
        public void SetHtml(String html, Html.IImageGetter imageGetter)
        {
            HtmlTagHandler htmlTagHandler = new HtmlTagHandler(Paint);
            htmlTagHandler.SetClickableTableSpan(clickableTableSpan);
            htmlTagHandler.SetDrawTableLinkSpan(drawTableLinkSpan);
            htmlTagHandler.SetListIndentPx(indent);

            html = htmlTagHandler.OverrideTags(html);

            ISpanned asa = Html.FromHtml(html, FromHtmlOptions.ModeCompact , imageGetter, htmlTagHandler);
            //if (removeTrailingWhiteSpace)
            //{
                //SetText(RemoveHtmlBottomPadding(asa));
            //}
            //else
            //{
                SetText(asa, BufferType.Spannable);
            //}

            // make links work
            this.MovementMethod = LocalLinkMovementMethod.GetInstance();
        }

        /**
         * The Html.fromHtml method has the behavior of adding extra whitespace at the bottom
         * of the parsed HTML displayed in for example a TextView. In order to remove this
         * whitespace call this method before setting the text with setHtml on this TextView.
         *
         * @param removeTrailingWhiteSpace true if the whitespace rendered at the bottom of a TextView
         *                                 after setting HTML should be removed.
         */
        //public void SetRemoveTrailingWhiteSpace(bool removeTrailingWhiteSpace)
        //{
        //    this.removeTrailingWhiteSpace = removeTrailingWhiteSpace;
        //}

        /**
         * The Html.fromHtml method has the behavior of adding extra whitespace at the bottom
         * of the parsed HTML displayed in for example a TextView. In order to remove this
         * whitespace call this method before setting the text with setHtml on this TextView.
         *
         * This method is deprecated, use setRemoveTrailingWhiteSpace instead.
         *
         * @param removeFromHtmlSpace true if the whitespace rendered at the bottom of a TextView
         *                            after setting HTML should be removed.
         */
        //@Deprecated()

        //public void setRemoveFromHtmlSpace(boolean removeFromHtmlSpace)
        //{
        //    this.removeTrailingWhiteSpace = removeFromHtmlSpace;
        //}

        public void SetClickableTableSpan( ClickableTableSpan clickableTableSpan)
        {
            this.clickableTableSpan = clickableTableSpan;
        }

        public void SetDrawTableLinkSpan( DrawTableLinkSpan drawTableLinkSpan)
        {
            this.drawTableLinkSpan = drawTableLinkSpan;
        }

        /**
         * Add ability to increase list item spacing. Useful for configuring spacing based on device
         * screen size. This applies to ordered and unordered lists.
         *
         * @param px    pixels to indent.
         */
        public void SetListIndentPx(float px)
        {
            this.indent = px;
        }

        /**
         * http://stackoverflow.com/questions/309424/read-convert-an-inputstream-to-a-string
         */
 
        private static String ConvertStreamToString( Stream isstream)
        {
            Scanner s = new Scanner(isstream).UseDelimiter("\\A");
            return s.HasNext ? s.Next() : "";

        }

        /**
         * Html.fromHtml sometimes adds extra space at the bottom.
         * This methods removes this space again.
         * See https://github.com/SufficientlySecure/html-textview/issues/19
         */


        //private ISpanned RemoveHtmlBottomPadding(ISpanned text)
        //{
        //    if (text == null)
        //    {
        //        return null;
        //    }

        //    while (text.Length() > 0 && text.CharAt(text.Length() - 1) == '\n')
        //    {
        //        text = text.subSequence(0, text.Length() - 1);
        //    }

        //    return text;
        //}
    }
}
