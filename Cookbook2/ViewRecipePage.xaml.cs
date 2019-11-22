using System;
using System.IO;
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

        private void EditRecipePage(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}