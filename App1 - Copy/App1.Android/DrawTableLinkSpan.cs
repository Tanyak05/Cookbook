using Android.Graphics;
using Android.Text.Style;
using Java.Lang;
using String = System.String;


namespace HtmlTextView
{
    /**
     * This span defines how a table should be rendered in the HtmlTextView. The default implementation
     * is a cop-out which replaces the HTML table with some text ("[tap for table]" is the default).
     * <p/>
     * This is to be used in conjunction with the Clickable TableSpan which will redirect a click to the
     * text some application-defined action (i.e. render the raw HTML in a WebView).
     */
    public class DrawTableLinkSpan : ReplacementSpan
    {
        private const string DefaultTableLinkText = "";
        private const float DefaultTextSize = 80f;
        private static readonly int DefaultTextColor = Color.Blue;

        protected String MTableLinkText = DefaultTableLinkText;
        protected float MTextSize = DefaultTextSize;
        protected int MTextColor = DefaultTextColor;

        // This sucks, but we need this so that each table can get drawn.
        // Otherwise, we end up with the default table link text (nothing) for earlier tables.
        public DrawTableLinkSpan NewInstance()
        {
            DrawTableLinkSpan drawTableLinkSpan = new DrawTableLinkSpan();
            drawTableLinkSpan.SetTableLinkText(MTableLinkText);
            drawTableLinkSpan.SetTextSize(MTextSize);
            drawTableLinkSpan.SetTextColor(MTextColor);

            return drawTableLinkSpan;
        }

        public override int GetSize(Paint paint, ICharSequence text, int start, int end, Paint.FontMetricsInt fm)
        {
            // public int override getSize(Paint paint, CharSequence text, int start, int end, Paint.FontMetricsInt fm) {
            int width = (int) paint.MeasureText(MTableLinkText, 0, MTableLinkText.Length);
            MTextSize = paint.TextSize;
            return width;
        }

        public override void Draw(Canvas canvas, ICharSequence text, int start, int end, float x, int top, int y,
            int bottom, Paint paint)
        {
            //public void draw(Canvas canvas, CharSequence text, int start, int end, float x, int top, int y, int bottom, Paint paint) {
            Paint paint2 = new Paint();
            paint2.SetStyle(Paint.Style.Stroke);
            paint2.Color = new Color(MTextColor);
            paint2.AntiAlias = true;
            paint2.TextSize = MTextSize;

            canvas.DrawText(MTableLinkText, x, bottom, paint2);
        }

        public void SetTableLinkText(String tableLinkText)
        {
            this.MTableLinkText = tableLinkText;
        }

        public void SetTextSize(float textSize)
        {
            this.MTextSize = textSize;
        }

        public void SetTextColor(int textColor)
        {
            this.MTextColor = textColor;
        }

        public String GetTableLinkText()
        {
            return MTableLinkText;
        }

        public float GetTextSize()
        {
            return MTextSize;
        }

        public int GetTextColor()
        {
            return MTextColor;
        }

    }

}
