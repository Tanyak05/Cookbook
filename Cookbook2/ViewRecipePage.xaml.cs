using System;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cookbook2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewRecipePage : ContentPage
    {
        public Recipe Recipe { get; set; }

        public ViewRecipePage(RecipeShort shortRecipe)
        {
            InitializeComponent();
            LoadRecipe(shortRecipe);
            DisplayRecipe();
        }

        private void LoadRecipe(RecipeShort shortRecipe)
        {
            string documentsPath = Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            documentsPath = Path.Combine(documentsPath, "recipes", shortRecipe.Id + ".json");

            using (StreamReader r = new StreamReader(documentsPath))
            {
                string json = r.ReadToEnd();
                Recipe = Newtonsoft.Json.JsonConvert.DeserializeObject<Recipe>(json);
            }
        }

        private void DisplayRecipe()
        {
            RecipeTextTitle.Text = Recipe.RecipeShort.Title;

            //if (recipe.RecipeShort.Image != null)
            //{
            //    //Bitmap recipeImage = AssetUtils.LoadBitmapAsset(this, recipe.Image);
            //    imageView.SetImageBitmap(recipe.RecipeShort.Image);
            //}
            TextIngredients.BindingContext = Recipe.Ingredients;
            TextIngredients.ItemsSource = Recipe.Ingredients;
            TextMethod.Source = new HtmlWebViewSource() { Html = Recipe.Method};
        }

        private async void EditRecipePage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ParseRecipePage(Recipe));
        }

        private async void DeleteRecipePage(object sender, EventArgs e)
        {
            //todo: add warning
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            documentsPath = Path.Combine(documentsPath, "recipes");
            documentsPath = Path.Combine(documentsPath, Recipe.RecipeShort.Id + ".json");
            File.Delete(documentsPath);

            await LocalDatabase.Database.DeleteItemAsync(Recipe.RecipeShort);
            ((MainPage)Navigation.NavigationStack.ElementAt(Navigation.NavigationStack.Count-2)).DeleteFromList(Recipe.RecipeShort);
            await Navigation.PopAsync();
        }
    }
}