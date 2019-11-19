using System.Collections.Generic;
using Android.Content;
using Android.Widget;
using Android.Util;
using Android.Text;
using Java.Lang;

//using String = System.String;


namespace HtmlTextView
{

    public class FixingResult
    {
        public bool Fixed;
        public List<Java.Lang.Object> SpansWithSpacesBefore;
        public List<Java.Lang.Object> SpansWithSpacesAfter;

        public static FixingResult Ffixed (List<Java.Lang.Object> spansWithSpacesBefore,List<Java.Lang.Object> spansWithSpacesAfter)
        {
            return new FixingResult(true, spansWithSpacesBefore, spansWithSpacesAfter);
        }

        public static FixingResult NotFixed()
        {
            return new FixingResult(false, null, null);
        }

        private FixingResult(bool Fixed, List<Java.Lang.Object> spansWithSpacesBefore, List<Java.Lang.Object> spansWithSpacesAfter)
        {
            this.Fixed = Fixed;
            this.SpansWithSpacesBefore = spansWithSpacesBefore;
            this.SpansWithSpacesAfter = spansWithSpacesAfter;
        }
    }

    public class JellyBeanSpanFixTextView : EditText
    {

        public JellyBeanSpanFixTextView(Context context, IAttributeSet attrs, int defStyle) 
            : base(context, attrs, defStyle)
        {

        }

        public JellyBeanSpanFixTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public JellyBeanSpanFixTextView(Context context) : base(context)
        {
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            try
            {
                base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            }
            catch (IndexOutOfBoundsException)
            {
                fixOnMeasure(widthMeasureSpec, heightMeasureSpec);
            }
        }

        /**
         * If possible, fixes the Spanned text by adding spaces around spans when needed.
         */
        private void fixOnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            //CharSequence text = Text;
            if ( Text is ISpanned) //stragnge
            {
                SpannableStringBuilder builder = new SpannableStringBuilder(this.Text);
                FixSpannedWithSpaces(builder, widthMeasureSpec, heightMeasureSpec);
            }
            else
            {
                //if (HtmlTextView.DEBUG)
                //{
                //    Log.d(HtmlTextView.TAG, "The text isn't a Spanned");
                //}

                FallbackToString(widthMeasureSpec, heightMeasureSpec);
            }
        }

        /**
         * Add spaces around spans until the text is fixed, and then removes the unneeded spaces
         */
        private void FixSpannedWithSpaces(SpannableStringBuilder builder, int widthMeasureSpec, int heightMeasureSpec)
        {
            //long startFix = System.Time.Now();

            FixingResult result = AddSpacesAroundSpansUntilFixed(builder, widthMeasureSpec, heightMeasureSpec);

            if (result.Fixed)
            {
                RemoveUnneededSpaces(widthMeasureSpec, heightMeasureSpec, builder, result);
            }
            else
            {
                FallbackToString(widthMeasureSpec, heightMeasureSpec);
            }

            //if (HtmlTextView.DEBUG)
            //{
            //    long fixDuration = System.currentTimeMillis() - startFix;
            //    Log.d(HtmlTextView.TAG, "fixSpannedWithSpaces() duration in ms: " + fixDuration);
            //}
        }

        private FixingResult AddSpacesAroundSpansUntilFixed(SpannableStringBuilder builder, int widthMeasureSpec, int heightMeasureSpec)
        {

            Java.Lang.Object[] spans = builder.GetSpans(0, builder.Length(), Java.Lang.Class.FromType(typeof(Object)));
            List<Java.Lang.Object> spansWithSpacesBefore = new List<Java.Lang.Object>(spans.Length);
            List<Java.Lang.Object> spansWithSpacesAfter = new List<Java.Lang.Object>(spans.Length);

            foreach (Java.Lang.Object span in spans)
            {
                int spanStart = builder.GetSpanStart(span);
                if (IsNotSpace(builder, spanStart - 1))
                {
                    builder.Insert(spanStart, " ");
                    spansWithSpacesBefore.Add(span);
                }

                int spanEnd = builder.GetSpanEnd(span);
                if (IsNotSpace(builder, spanEnd))
                {
                    builder.Insert(spanEnd, " ");
                    spansWithSpacesAfter.Add(span);
                }

                try
                {
                    SetTextAndMeasure(builder, widthMeasureSpec, heightMeasureSpec);
                    return FixingResult.Ffixed(spansWithSpacesBefore, spansWithSpacesAfter);
                }
                catch (IndexOutOfBoundsException)
                {
                    ///TODO
                }
            }

            //if (HtmlTextView.DEBUG)
            //{
            //    Log.d(HtmlTextView.TAG, "Could not fix the Spanned by adding spaces around spans");
            //}

            return FixingResult.NotFixed();
        }

        private static bool IsNotSpace(SpannableStringBuilder text, int position)
        {
            return position < 0 || position >= text.Length() || text.CharAt(position) != ' ';
        }

        //@SuppressLint("WrongCall")

        private void SetTextAndMeasure(SpannableStringBuilder text, int widthMeasureSpec, int heightMeasureSpec)
        {
            SetText(text, TextView.BufferType.Spannable);
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
        }

        //@SuppressLint("WrongCall")

        private void RemoveUnneededSpaces(int widthMeasureSpec, int heightMeasureSpec, SpannableStringBuilder builder, FixingResult result)
        {

            foreach (Java.Lang.Object span in result.SpansWithSpacesAfter) {
                int spanEnd = builder.GetSpanEnd(span);
                builder.Delete(spanEnd, spanEnd + 1);
                try
                {
                    SetTextAndMeasure(builder, widthMeasureSpec, heightMeasureSpec);
                }
                catch (IndexOutOfBoundsException)
                {
                    builder.Insert(spanEnd, " ");
                }
            }

            bool needReset = true;
            foreach (Java.Lang.Object span in result.SpansWithSpacesBefore)
            {
                int spanStart = builder.GetSpanStart(span);
                builder.Delete(spanStart - 1, spanStart);
                try
                {
                    SetTextAndMeasure(builder, widthMeasureSpec, heightMeasureSpec);
                    needReset = false;
                }
                catch (IndexOutOfBoundsException)
                {
                    needReset = true;
                    int newSpanStart = spanStart - 1;
                    builder.Insert(newSpanStart, " ");
                }
            }

            if (needReset)
            {
                SetText(builder, TextView.BufferType.Spannable);
                base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            }
        }

        private void FallbackToString(int widthMeasureSpec, int heightMeasureSpec)
        {
            //if (HtmlTextView.DEBUG)
            //{
            //    Log.d(HtmlTextView.TAG, "Fallback to unspanned text");
            //}

            SpannableStringBuilder fallbackText = new SpannableStringBuilder(Text);
            SetTextAndMeasure(fallbackText, widthMeasureSpec, heightMeasureSpec);
        }

    }
}