using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cookbook2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddWithUrlPage : ContentPage
    {

        public AddWithUrlPage()
        {
            InitializeComponent();
            
        }

        private void UrlTextChanged(object sender, EventArgs eventArgs)
        {
            RecipePreview.Source = UrlToLookText.Text;
            
        }

        void WebViewNavigating(object sender, WebNavigatingEventArgs e)
        {
            LabelLoading.IsVisible = true;
        }

        void WebViewNavigated(object sender, WebNavigatedEventArgs e)
        {
            LabelLoading.IsVisible = false;
        }

        private async void ParesSelectionButton_Click(object sender, EventArgs e)
        {
            string selection = await RecipePreview.EvaluateJavaScriptAsync("(function(){return window.getSelection().toString()})()");

            ParseRecipePage parseRecipePage = new ParseRecipePage {BindingContext = selection};
            await Navigation.PushAsync(parseRecipePage);
        }

    }

}