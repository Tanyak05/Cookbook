using Android.OS;
using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Java.Lang;


namespace HtmlTextView
{

    /**
     * Class to use Numbered Lists in TextViews.
     * The span works the same as {@link android.text.style.BulletSpan} and all lines of the entry have
     * the same leading margin.
     */
    public class NumberSpan : Java.Lang.Object, ILeadingMarginSpan, IParcelableSpan
    {
        private readonly int mGapWidth;
        private readonly string mNumber;

        public static readonly int StandardGapWidth = 10;

        public NumberSpan(int gapWidth, int number)
        {
            mGapWidth = gapWidth;
            //mNumber = Integer.ToString(number).Concat(".");
            mNumber = number.ToString() + ".";
        }

        public NumberSpan(int number)
        {
            mGapWidth = StandardGapWidth;
            //number.ToString()
            //mNumber = Integer.ToString(number).Concat(".");
            mNumber = number.ToString() + ".";
        }

        public NumberSpan(Parcel src)
        {
            mGapWidth = src.ReadInt();
            mNumber = src.ReadString();
        }

        public int SpanTypeId => SpanTypeIdInternal;

        /** @hide */
        public int SpanTypeIdInternal => 8;
        //{
        //    return 8;
        //}

        public int DescribeContents()
        {
            return 0;
        }

        
        public void WriteToParcel(Parcel dest, ParcelableWriteFlags flags)
        {
            WriteToParcelInternal(dest, flags);
        }

        /** @hide */
        public void WriteToParcelInternal(Parcel dest, ParcelableWriteFlags flags)
        {
            dest.WriteInt(mGapWidth);
        }



        public int GetLeadingMargin(bool first)
        {
            return 2 * StandardGapWidth + mGapWidth;
        }

        //public void DrawLeadingMargin(Canvas c, Paint p, int x, int dir,
        //    int top, int baseline, int bottom,
        //    Android.Runtime.CharSequence text, int start, int end,
        //    bool first, Layout l)


        public void DrawLeadingMargin(Canvas c, Paint p, int x, int dir, int top, int baseline, int bottom, ICharSequence text,
            int start, int end, bool first, Layout layout)
        {
            if (((ISpanned)text).GetSpanStart(this) == start)
            {
                Paint.Style style = p.GetStyle();

                p.SetStyle(Paint.Style.Fill);

                if (c.IsHardwareAccelerated)
                {
                    c.Save();
                    c.DrawText(mNumber, x + dir, baseline, p);
                    c.Restore();
                }
                else
                {
                    c.DrawText(mNumber, x + dir, (top + bottom) / 2.0f, p);
                }

                p.SetStyle(style);
            }
        }
        //public void Dispose()
        //{
        //    throw new NotImplementedException();
        //}

        //public IntPtr Handle { get; }

    }
}
