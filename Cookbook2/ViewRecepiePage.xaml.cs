using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cookbook2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewRecepiePage : ContentPage
    {
        public Recipe Recipe { get; set; }

        public ViewRecepiePage(RecipeShort shortRecipe)
        {
            InitializeComponent();
            LoadRecipe(shortRecipe);
            DisplayRecipe();
        }

        private void LoadRecipe(RecipeShort shortRecipe)
        {
            //string id = Intent.GetStringExtra(Constants.RecipeNameToLoad);
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            documentsPath = Path.Combine(documentsPath, "recipes", shortRecipe.Id + ".json");

            using (StreamReader r = new StreamReader(documentsPath))
            {
                string json = r.ReadToEnd();
                Recipe = Newtonsoft.Json.JsonConvert.DeserializeObject<Recipe>(json);
            }
        }

        private void DisplayRecipe()
        {
            recipeTextTitle.Text = Recipe.RecipeShort.Title;

            //if (recipe.RecipeShort.Image != null)
            //{
            //    //Bitmap recipeImage = AssetUtils.LoadBitmapAsset(this, recipe.Image);
            //    imageView.SetImageBitmap(recipe.RecipeShort.Image);
            //}
            textIngredients.BindingContext = Recipe.Ingredients;
            textIngredients.ItemsSource = Recipe.Ingredients;
            textMethod.Source = new HtmlWebViewSource() { Html = Recipe.Method};

        }

        private void EditRecipePage(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}