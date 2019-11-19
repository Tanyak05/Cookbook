/*
 * Copyright (C) 2015 Heliangwei
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Android.Views;
using Android.Widget;
using Android.Text;
using Android.Text.Method;
using Android.Text.Style;


namespace HtmlTextView
{

    /**
     * Copied from http://stackoverflow.com/questions/8558732
     */
    public class LocalLinkMovementMethod : LinkMovementMethod
    {
        static LocalLinkMovementMethod sInstance;

        public static LocalLinkMovementMethod GetInstance()
        {
            return sInstance ?? (sInstance = new LocalLinkMovementMethod());
        }

        /*
        public override bool OnTouchEvent(TextView widget, ISpannable buffer, MotionEvent event1) 
        {
            MotionEventActions action = event1.Action;

            if (action == MotionEventActions.Up || action == MotionEventActions.Down)
            {
                int x = (int) event1.GetX();
                int y = (int) event1.GetY();

                x -= widget.TotalPaddingLeft;
                y -= widget.TotalPaddingTop;

                x += widget.ScrollX;
                y += widget.ScrollY;

                Layout layout = widget.Layout;
                int line = layout.GetLineForVertical(y);
                int off = layout.GetOffsetForHorizontal(line, x);

                ClickableSpan[] link = (ClickableSpan[]) buffer.GetSpans(off, off, Java.Lang.Class.FromType(typeof(ClickableSpan)));

                if (link.Length != 0)
                {
                    if (action == MotionEventActions.Up)
                    {
                        link[0].OnClick(widget);
                    }
                    else if (action == MotionEventActions.Down)
                    {
                        Selection.SetSelection(buffer, buffer.GetSpanStart(link[0]), buffer.GetSpanEnd(link[0]));
                    }

                    return true;
                }
                else
                {
                    Selection.RemoveSelection(buffer);
                    Touch.OnTouchEvent(widget, buffer,  event1);
                    return false;
                }
            }

            return Touch.OnTouchEvent(widget, buffer,  event1);
        }*/
    }
}
