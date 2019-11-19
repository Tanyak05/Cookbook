using System;
using System.Collections.Generic;
using Android.Text;
using Android.Text.Style;
using Java.Lang;
using Org.Xml.Sax;
using String = System.String;


namespace HtmlTextView
{

    public class JavaObjectWrapper<T> : Java.Lang.Object { public T Obj { get; set; } }

    public class HtmlTagHandler : Java.Lang.Object, Html.ITagHandler
    {

        public static string UnorderedList = "HTML_TEXTVIEW_ESCAPED_UL_TAG";
        public static string OrderedList = "HTML_TEXTVIEW_ESCAPED_OL_TAG";
        public static string ListItem = "HTML_TEXTVIEW_ESCAPED_LI_TAG";
        private TextPaint mTextPaint;

        public HtmlTagHandler(TextPaint textPaint)
        {
            mTextPaint = textPaint;
        }

        /**
         * Newer versions of the Android SDK's {@link Html.TagHandler} handles &lt;ul&gt; and &lt;li&gt;
         * tags itself which means they never get delegated to this class. We want to handle the tags
         * ourselves so before passing the string html into Html.fromHtml(), we can use this method to
         * replace the &lt;ul&gt; and &lt;li&gt; tags with tags of our own.
         *
         * @see <a href="https://github.com/android/platform_frameworks_base/commit/8b36c0bbd1503c61c111feac939193c47f812190">Specific Android SDK Commit</a>
         *
         * @param html        String containing HTML, for example: "<b>Hello world!</b>"
         * @return html with replaced <ul> and <li> tags
         */
        public String OverrideTags(String html)
        {

            if (html == null)
                return null;

            html = html.Replace("<ul", "<" + UnorderedList);
            html = html.Replace("</ul>", "</" + UnorderedList + ">");
            html = html.Replace("<ol", "<" + OrderedList);
            html = html.Replace("</ol>", "</" + OrderedList + ">");
            html = html.Replace("<li", "<" + ListItem);
            html = html.Replace("</li>", "</" + ListItem + ">");

            return html;
        }

        /**
         * Keeps track of lists (ol, ul). On bottom of Stack is the outermost list
         * and on top of Stack is the most nested list
         */
        private readonly Stack<string> lists = new Stack<string>();

        /**
         * Tracks indexes of ordered lists so that after a nested list ends
         * we can continue with correct index of outer list
         */
        private readonly Stack<int> olNextIndex = new Stack<int>();

        /**
         * List indentation in pixels. Nested lists use multiple of this.
         */
        /**
         * Running HTML table string based off of the root table tag. Root table tag being the tag which
         * isn't embedded within any other table tag. Example:
         * <!-- This is the root level opening table tag. This is where we keep track of tables. -->
         * <table>
         * ...
         * <table> <!-- Non-root table tags -->
         * ...
         * </table>
         * ...
         * </table>
         * <!-- This is the root level closing table tag and the end of the string we track. -->
         */
        private System.Text.StringBuilder tableHtmlBuilder = new System.Text.StringBuilder();

        /**
         * Tells us which level of table tag we're on; ultimately used to find the root table tag.
         */
        private int tableTagLevel = 0;

        private static int userGivenIndent = -1;
        private static int defaultIndent = 10;
        private static readonly int DefaultListItemIndent = defaultIndent * 2;
        private static readonly BulletSpan DefaultBullet = new BulletSpan(defaultIndent);
        private ClickableTableSpan clickableTableSpan;
        private DrawTableLinkSpan drawTableLinkSpan;

        #region TagClases

        private class Ul : Java.Lang.Object
        {
        }

        private class Ol : Java.Lang.Object
        {
        }

        private class Code : Java.Lang.Object
        {
        }

        private class Center : Java.Lang.Object
        {
        }

        private class Strike : Java.Lang.Object
        {
        }

        private class Table : Java.Lang.Object
        {
        }

        private class Tr : Java.Lang.Object
        {
        }

        private class Th : Java.Lang.Object
        {
        }

        private class Td : Java.Lang.Object
        {
        }

        #endregion

        public void HandleTag(bool opening, string tag, IEditable output, IXMLReader xmlReader)
        {
            if (opening)
            {

                if (tag.Equals(UnorderedList, StringComparison.OrdinalIgnoreCase))
                {
                    lists.Push(tag);
                }
                else if (tag.Equals(OrderedList, StringComparison.OrdinalIgnoreCase))
                {
                    lists.Push(tag);
                    olNextIndex.Push(1);
                }
                else if (tag.Equals(ListItem, StringComparison.OrdinalIgnoreCase))
                {
                    if (output.Length() > 0 && output.CharAt(output.Length() - 1) != '\n')
                    {
                        output.Append("\n");
                    }

                    if (lists.Count != 0)
                    {
                        String parentList = lists.Peek();
                        if (parentList.Equals(OrderedList, StringComparison.OrdinalIgnoreCase))
                        {
                            Start(output, new Ol());
                            olNextIndex.Push(olNextIndex.Pop() + 1);
                        }
                        else if (parentList.Equals(UnorderedList, StringComparison.OrdinalIgnoreCase))
                        {
                            Start(output, new Ul());
                        }
                    }
                }
                else if (tag.Equals("code", StringComparison.OrdinalIgnoreCase))
                {
                    Start(output, new Code());
                }
                else if (tag.Equals("center", StringComparison.OrdinalIgnoreCase))
                {
                    Start(output, new Center());
                }
                else if (tag.Equals("s", StringComparison.OrdinalIgnoreCase) ||
                         tag.Equals("strike", StringComparison.OrdinalIgnoreCase))
                {
                    Start(output, new Strike());
                }
                else if (tag.Equals("table", StringComparison.OrdinalIgnoreCase))
                {
                    Start(output, new Table());
                    if (tableTagLevel == 0)
                    {
                        tableHtmlBuilder = new System.Text.StringBuilder();
                        // We need some text for the table to be replaced by the span because
                        // the other tags will remove their text when their text is extracted
                        output.Append("table placeholder");
                    }

                    tableTagLevel++;
                }
                else if (tag.Equals("tr", StringComparison.OrdinalIgnoreCase))
                {
                    Start(output, new Tr());
                }
                else if (tag.Equals("th", StringComparison.OrdinalIgnoreCase))
                {
                    Start(output, new Th());
                }
                else if (tag.Equals("td", StringComparison.OrdinalIgnoreCase))
                {
                    Start(output, new Td());
                }
            }
            else
            {

                if (tag.Equals(UnorderedList, StringComparison.OrdinalIgnoreCase))
                {
                    lists.Pop();
                }
                else if (tag.Equals(OrderedList, StringComparison.OrdinalIgnoreCase))
                {
                    lists.Pop();
                    olNextIndex.Pop();
                }
                else if (tag.Equals(ListItem, StringComparison.OrdinalIgnoreCase))
                {
                    if (lists.Count != 0)
                    {
                        int listItemIndent = (userGivenIndent > -1) ? (userGivenIndent * 2) : DefaultListItemIndent;
                        if (lists.Peek().Equals(UnorderedList, StringComparison.OrdinalIgnoreCase))
                        {
                            if (output.Length() > 0 && output.CharAt(output.Length() - 1) != '\n')
                            {
                                output.Append("\n");
                            }

                            // Nested BulletSpans increases distance between bullet and text, so we must prevent it.
                            int indent = (userGivenIndent > -1) ? userGivenIndent : defaultIndent;
                            BulletSpan bullet =
                                (userGivenIndent > -1) ? new BulletSpan(userGivenIndent) : DefaultBullet;
                            if (lists.Count > 1)
                            {
                                indent = indent - bullet.GetLeadingMargin(true);
                                if (lists.Count > 2)
                                {
                                    // This get's more complicated when we add a LeadingMarginSpan into the same line:
                                    // we have also counter it's effect to BulletSpan
                                    indent -= (lists.Count - 2) * listItemIndent;
                                }
                            }

                            BulletSpan newBullet = new BulletSpan(indent);
                            End(output, typeof(Ul), false, new LeadingMarginSpanStandard(listItemIndent * (lists.Count - 1)), newBullet);
                        }
                        else if (lists.Peek().Equals(OrderedList, StringComparison.OrdinalIgnoreCase))
                        {
                            if (output.Length() > 0 && output.CharAt(output.Length() - 1) != '\n')
                            {
                                output.Append("\n");
                            }

                            // Nested NumberSpans increases distance between number and text, so we must prevent it.
                            int indent = (userGivenIndent > -1) ? userGivenIndent : defaultIndent;
                            NumberSpan span = new NumberSpan(indent, olNextIndex.Count - 1);
                            if (lists.Count > 1)
                            {
                                indent = indent - span.GetLeadingMargin(true);
                                if (lists.Count > 2)
                                {
                                    // As with BulletSpan, we need to compensate for the spacing after the number.
                                    indent -= (lists.Count - 2) * listItemIndent;
                                }
                            }

                            NumberSpan numberSpan = new NumberSpan(indent, olNextIndex.Count - 1);
                            End(output, typeof(Ol), false, new LeadingMarginSpanStandard(listItemIndent * (lists.Count - 1)), numberSpan);
                        }
                    }
                }
                else if (tag.Equals("code", StringComparison.OrdinalIgnoreCase))
                {
                    End(output, typeof(Code), false, new TypefaceSpan("monospace"));
                }
                else if (tag.Equals("center", StringComparison.OrdinalIgnoreCase))
                {
                    End(output, typeof(Center) , true, new AlignmentSpanStandard(Layout.Alignment.AlignCenter));
                }
                else if (tag.Equals("s", StringComparison.OrdinalIgnoreCase) ||
                         tag.Equals("strike", StringComparison.OrdinalIgnoreCase))
                {
                    End(output, typeof(Strike) , false, new StrikethroughSpan());
                }
                else if (tag.Equals("table", StringComparison.OrdinalIgnoreCase))
                {
                    tableTagLevel--;

                    // When we're back at the root-level table
                    if (tableTagLevel == 0)
                    {
                        String tableHtml = tableHtmlBuilder.ToString();

                        ClickableTableSpan myClickableTableSpan = null;
                        if (clickableTableSpan != null)
                        {
                            myClickableTableSpan = clickableTableSpan.NewInstance();
                            myClickableTableSpan.SetTableHtml(tableHtml);
                        }

                        DrawTableLinkSpan myDrawTableLinkSpan = null;
                        if (drawTableLinkSpan != null)
                        {
                            myDrawTableLinkSpan = drawTableLinkSpan.NewInstance();
                        }

                        End(output, typeof(Table) , false, myDrawTableLinkSpan, myClickableTableSpan);
                    }
                    else
                    {
                        End(output, typeof(Table) , false);
                    }
                }
                else if (tag.Equals("tr", StringComparison.OrdinalIgnoreCase))
                {
                    End(output, typeof(Tr) , false);
                }
                else if (tag.Equals("th", StringComparison.OrdinalIgnoreCase))
                {
                    End(output, typeof(Th) , false);
                }
                else if (tag.Equals("td", StringComparison.OrdinalIgnoreCase))
                {
                    End(output, typeof(Td) , false);
                }
            }

            StoreTableTags(opening, tag);
        }

        /**
         * If we're arriving at a table tag or are already within a table tag, then we should store it
         * the raw HTML for our ClickableTableSpan
         */
        private void StoreTableTags(bool opening, String tag)
        {
            if (tableTagLevel > 0 || tag.Equals("table", StringComparison.OrdinalIgnoreCase))
            {
                tableHtmlBuilder.Append("<");
                if (!opening)
                {
                    tableHtmlBuilder.Append("/");
                }

                tableHtmlBuilder
                    .Append(tag.ToLower())
                    .Append(">");
            }
        }

        /**
         * Mark the opening tag by using private classes
         */
        private void Start(IEditable output, Java.Lang.Object mark)
        {
            int len = output.Length();
            output.SetSpan(mark, len, len, SpanTypes.MarkMark);

            //if (HtmlTextView.DEBUG)
            //{
            //    Log.d(HtmlTextView.TAG, "len: " + len);
            //}
        }

        /**
         * Modified from {@link android.text.Html}
         */
        private void End(IEditable output, Type kind, bool paragraphStyle, params Java.Lang.Object[] replaces)
        {
            Java.Lang.Object obj = GetLast(output, kind);
            // start of the tag
            int where = output.GetSpanStart(obj);
            // end of the tag
            int len = output.Length();

            // If we're in a table, then we need to store the raw HTML for later
            if (tableTagLevel > 0)
            {
                String extractedSpanText = ExtractSpanText(output, kind);
                tableHtmlBuilder.Append(extractedSpanText);
            }

            output.RemoveSpan(obj);

            if (where != len)
            {
                int thisLen = len;
                // paragraph styles like AlignmentSpan need to end with a new line!
                if (paragraphStyle)
                {
                    output.Append("\n");
                    thisLen++;
                }

                foreach (Java.Lang.Object replace in replaces) {
                    output.SetSpan(replace, where, thisLen, SpanTypes.ExclusiveExclusive);
                }

                //if (HtmlTextView.DEBUG)
                //{
                //    Log.d(HtmlTextView.TAG, "where: " + where);
                //    Log.d(HtmlTextView.TAG, "thisLen: " + thisLen);
                //}
            }
        }

        /**
         * Returns the text contained within a span and deletes it from the output string
         */
        private static String ExtractSpanText(IEditable output, Type kind)
        {
            Java.Lang.Object obj = GetLast(output, kind);
            // start of the tag
            int where = output.GetSpanStart(obj);
            // end of the tag
            int len = output.Length();

            String extractedSpanText = output.SubSequence(where, len);
            output.Delete(where, len);
            return extractedSpanText;
        }


        /**
         * Get last marked position of a specific tag kind (private class)
         */
        private static Java.Lang.Object GetLast(IEditable text, Type kind)
        {

            Java.Lang.Object[] objs = text.GetSpans(0, text.Length(), Java.Lang.Class.FromType(kind));
            if (objs.Length == 0)
            {
                return null;
            }
            else
            {
                for (int i = objs.Length; i > 0; i--)
                {
                    if (text.GetSpanFlags(objs[i - 1]) == SpanTypes.MarkMark)
                    {
                        return objs[i - 1];
                    }
                }

                return null;
            }
        }

        // Util method for setting pixels.
        public void SetListIndentPx(float px)
        {
            userGivenIndent = (int)System.Math.Round(px);
        }

        public void SetClickableTableSpan(ClickableTableSpan clickableTableSpan)
        {
            this.clickableTableSpan = clickableTableSpan;
        }

        public void SetDrawTableLinkSpan(DrawTableLinkSpan drawTableLinkSpan)
        {
            this.drawTableLinkSpan = drawTableLinkSpan;
        }

    }
}
