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
        public ViewRecepiePage()
        {
            //InitializeComponent();
            LoadRecipe();
        }

        private void LoadRecipe()
        {
            //string id = Intent.GetStringExtra(Constants.RecipeNameToLoad);
            //string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            //documentsPath = Path.Combine(documentsPath, "recipes", id + ".json");

            ////JSONObject jsonObject;
            //using (StreamReader r = new StreamReader(documentsPath))
            //{
            //    string json = r.ReadToEnd();
            //    recipe = Newtonsoft.Json.JsonConvert.DeserializeObject<Recipe>(json);
            //    //jsonObject = new JSONObject(json);
            //}

            ////recipe = new Recipe(jsonObject, this);
            //DisplayRecipe();
        }

        private void DisplayRecipe()
        {
            //titleTextView.Text = recipe.RecipeShort.Title;

            //if (recipe.RecipeShort.Image != null)
            //{
            //    //Bitmap recipeImage = AssetUtils.LoadBitmapAsset(this, recipe.Image);
            //    imageView.SetImageBitmap(recipe.RecipeShort.Image);
            //}

            //arrayAdapter.AddAll(recipe.Ingredients);
            //methodTextView.Text = recipe.Method;
        }

    }
}