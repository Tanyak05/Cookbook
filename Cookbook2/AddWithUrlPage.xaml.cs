using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NavigationPage = Xamarin.Forms.PlatformConfiguration.TizenSpecific.NavigationPage;

namespace Cookbook2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddWithUrlPage : ContentPage
    {

        public AddWithUrlPage()
        {
            InitializeComponent();
            
        }

        private void RecipeUrlTextChanged(object sender, EventArgs eventArgs)
        {
            recepiePreview.Source = urlToLookText.Text;
            
        }

        void webviewNavigating(object sender, WebNavigatingEventArgs e)
        {
            labelLoading.IsVisible = true;
        }

        void webviewNavigated(object sender, WebNavigatedEventArgs e)
        {
            labelLoading.IsVisible = false;
        }

        private async void ParesSelectionButton_Click(object sender, EventArgs e)
        {
            string selection = await recepiePreview.EvaluateJavaScriptAsync("(function(){return window.getSelection().toString()})()");

            ParseRecipePage parseRecipePage = new ParseRecipePage {BindingContext = selection};
            await Navigation.PushAsync(parseRecipePage);
        }

        
        //private void RecipeUrlTextChanged(object sender, Android.Text.TextChangedEventArgs e)
        //{
        //    recipePreview.Source = recipeUrl.Text;
        //}
    }

}