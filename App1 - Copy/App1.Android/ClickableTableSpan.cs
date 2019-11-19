using System;
using Android.Text.Style;


namespace HtmlTextView
{

/**
 * This span defines what should happen if a table is clicked. This abstract class is defined so
 * that applications can access the raw table HTML and do whatever they'd like to render it (e.g.
 * show it in a WebView).
 */
    public abstract class ClickableTableSpan : ClickableSpan
    {
        protected String TableHtml;

        // This sucks, but we need this so that each table can get its own ClickableTableSpan.
        // Otherwise, we end up removing the clicking from earlier tables.
        public abstract ClickableTableSpan NewInstance();

        public void SetTableHtml(String tableHtml)
        {
            this.TableHtml = tableHtml;
        }

        public String GetTableHtml()
        {
            return TableHtml;
        }
    }
}