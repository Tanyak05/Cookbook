using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Webkit;
using Android.Widget;
using System;

namespace Cookbook
{

    //[Activity(Label = "AddRecipe")]
    //public class AddRecipeActivity : Activity
    //{
    //    protected override void OnCreate(Bundle savedInstanceState)
    //    {
    //        base.OnCreate(savedInstanceState);
    //        // Set our view from the "main" layout resource
    //        SetContentView(Resource.Layout.add_resepie);
    //        Xamarin.Forms.Forms.Init(this, savedInstanceState);

    //        Button addWithUrlButton = FindViewById<Button>(Resource.Id.addWithUrlButton);
    //        addWithUrlButton.Click += AddWithUrlButton_Click;

    //        Button addFromClipboardButton = FindViewById<Button>(Resource.Id.addByTextCopyButtton);
    //        addFromClipboardButton.Click += AddFromClipboardButton_Click;

    //        //Button addByTextCopyButtton = FindViewById<Button>(Resource.Id.addByTextCopyButtton);
    //        //addWithUrlButton.Click += addByTextCopyButtton_Click;

    //    }

    //    private void AddByTextCopyButton_Click(object sender, EventArgs e)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    private void AddWithUrlButton_Click(object sender, EventArgs e)
    //    {
    //        var intent = new Intent(this, typeof(AddFromUrlRecipeActivity));
    //        StartActivity(intent);
    //    }

    //    private void AddFromClipboardButton_Click(object sender, EventArgs e)
    //    {
    //        var intent = new Intent(this, typeof(ParseRecipeActivity));
    //        StartActivity(intent);

    //    }
    //}

    [Activity(Label = "AddFromUrlRecipe")]
    internal class AddFromUrlRecipeActivity : Activity, IWebViewSelectionActivity
    {
        private TextView recipeUrl;
        private Xamarin.Forms.WebView recipePreview;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.add_url);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            recipeUrl = FindViewById<TextView>(Resource.Id.urlToLookText);
            recipeUrl.TextChanged += RecipeUrlTextChanged;

            recipePreview = FindViewById<Xamarin.Forms.WebView>(Resource.Id.recepiePreview);
            //recipePreview.Settings.JavaScriptEnabled = true;
            //web_view.SetWebViewClient(new HelloWebViewClient());
            recipePreview.Source ="https://chadeyka.livejournal.com/216950.html";

            Button paresSelectionButton = FindViewById<Button>(Resource.Id.startParseRecepieButton);
            paresSelectionButton.Click += ParesSelectionButton_Click;




            //Button addWithUrlButton = FindViewById<Button>(Resource.Id.addWithUrlButton);
            //addWithUrlButton.Click += addWithUrlButton_Click;


        }

        public static Intent intent;



       private async void ParesSelectionButton_Click(object sender, EventArgs e)
        {
            intent = new Intent(this, typeof(ParseRecipeActivity));

            var resultCallback = new ValueCallback(this);
            Selection = await recipePreview.EvaluateJavaScriptAsync("(function(){return window.getSelection().toString()})()");

            AfterSelection();
        }


         public void AfterSelection()
        {
            intent.PutExtra(Constants.RecipeSelection, Selection);
            StartActivity(intent);
        }

        public string Selection { get; set; }










        //private void AfterGetSelection_Click(object sender, EventArgs e)
        //{
        //    intent = new Intent(this, typeof(ParseRecipeActivity));

        //    ValueCallback resultCallback = new ValueCallback(this);
        //    recipePreview.EvaluateJavascript("(function(){return window.getSelection().toString()})()", resultCallback);

        //    //_pool.WaitOne();

        //    //intent.PutExtra(Constants.RecipeSelection, Selection);
        //    //StartActivity(intent);
        //}

        private void RecipeUrlTextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            recipePreview.Source = recipeUrl.Text;
        }
    }








    internal interface IWebViewSelectionActivity
    {
        void AfterSelection();
        string Selection { get; set; }
    }

    internal class ValueCallback : Java.Lang.Object, IValueCallback
    {
        private readonly IWebViewSelectionActivity addFromUrlRecipeActivity;

        public ValueCallback(IWebViewSelectionActivity addFromUrlRecipeActivity)
        {
            this.addFromUrlRecipeActivity = addFromUrlRecipeActivity;
        }

        public void OnReceiveValue(Java.Lang.Object value)
        {

            Log.Debug("AddRecipeActivity", "Webview selected text: " + value);
            addFromUrlRecipeActivity.Selection = value.ToString();
            addFromUrlRecipeActivity.AfterSelection();
            // _pool.Release();

        }
    }
}