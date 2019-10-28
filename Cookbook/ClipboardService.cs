using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;


[assembly: Dependency(typeof(ClipboardDemo.ClipboardService))]
namespace ClipboardDemo
{
    public interface IClipboardService
    {
        void CopyToClipboard(string text);
    }

    public class ClipboardService : IClipboardService
    {
        //private Context context;

        //public ClipboardService(Context context) {
        //    this.context = context;
        //}

        public void CopyToClipboard(string text)
        {
            //Xamarin.Forms.serDependencyService.Get<ClipboardDemo.IClipboardService>();
            //Android.Content.ClipboardManager clipboard = (ClipboardManager) this.GetSystemService(Context.ClipboardService);

            //ClipboardManager clipboard = (ClipboardManager)getSystemService(Context.CLIPBOARD_SERVICE);
            ClipboardManager clipboardManager = (ClipboardManager)Android.App.Application.Context.GetSystemService(Context.ClipboardService);

            string aaa = clipboardManager.HasPrimaryClip ? "yes" : "no";

            ClipData clip = ClipData.NewHtmlText("Android Clipboard", "", aaa + text);
            clipboardManager.PrimaryClip = clip;
            ClipDescription decr = clipboardManager.PrimaryClip.Description;

            var htmlText = clipboardManager.PrimaryClip.GetItemAt(0).HtmlText;
            //var coercedHtmlText = clipboardManager.PrimaryClip.GetItemAt(0).CoerceToHtmlText;
            //var coercedFormattedStyledText = clipboardManager.PrimaryClip.GetItemAt(0).CoerceToStyledTextFormatted;
        }
    }
}