using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Org.Json;
using Android.Views.Animations;
using Path = System.IO.Path;


namespace Cookbook
{
    [Activity(Label = "@string/app_name")]
    public class ShortRecipeActivity : Activity
    {
        private RecipeShort recipe;
        private ImageView imageView;
        private TextView titleTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.list_item);

            titleTextView = (TextView)FindViewById(Resource.Id.textTitle);
            imageView = (ImageView)FindViewById(Resource.Id.recipeImageView);
        }

        protected override void OnStart()
        {
            base.OnStart();
            LoadRecipe();
        }

        private void LoadRecipe()
        {
            string id = Intent.GetStringExtra(Constants.RecipeNameToLoad);
            recipe = LocalDatabase.Database.GetItemAsync<RecipeShort>(id).Result;
            DisplayRecipe();
        }

        private void DisplayRecipe()
        {
            titleTextView.Text = recipe.Title;

            if (recipe.Image != null)
            {
                imageView.SetImageBitmap(recipe.Image);
            }
        }
    }

    [Activity(Label = "@string/app_name")]
    public class RecipeActivity : Activity
    {
        private Recipe recipe;
        private ImageView imageView;
        private TextView titleTextView;
        private ListView ingredientsListView;
        private TextView methodTextView;
        private ArrayAdapter<Ingredient> arrayAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.recipe);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar1);
            SetActionBar(toolbar);

            titleTextView = (TextView)FindViewById(Resource.Id.recipeTextTitle);
            imageView = (ImageView)FindViewById(Resource.Id.recipeImageView);

            ingredientsListView = (ListView)FindViewById(Resource.Id.textIngredients);
            arrayAdapter = new ArrayAdapter<Ingredient>(this, Android.Resource.Layout.SimpleListItemChecked, new List<Ingredient>());
            ingredientsListView.Adapter = arrayAdapter;

            methodTextView = (TextView)FindViewById(Resource.Id.textMethod);
        }

        protected override void OnStart()
        {
            base.OnStart();
            LoadRecipe();
        }

        private void LoadRecipe()
        {
            string id = Intent.GetStringExtra(Constants.RecipeNameToLoad);
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            documentsPath = Path.Combine(documentsPath, "recipes", id + ".json");

            //JSONObject jsonObject;
            using (StreamReader r = new StreamReader(documentsPath))
            {
                string json = r.ReadToEnd();
                recipe = Newtonsoft.Json.JsonConvert.DeserializeObject<Recipe>(json);
               //jsonObject = new JSONObject(json);
            }

            //recipe = new Recipe(jsonObject, this);
            DisplayRecipe();
        }

        private void DisplayRecipe()
        {
            titleTextView.Text = recipe.RecipeShort.Title;

            if (recipe.RecipeShort.Image != null)
            {
                //Bitmap recipeImage = AssetUtils.LoadBitmapAsset(this, recipe.Image);
                imageView.SetImageBitmap(recipe.RecipeShort.Image);
            }

            arrayAdapter.AddAll(recipe.Ingredients);
            methodTextView.Text = recipe.Method;
        }


        /// <Docs>The options menu in which you place your items.</Docs>
        /// <returns>To be added.</returns>
        /// <summary>
        /// This is the menu for the Toolbar/Action Bar to use
        /// </summary>
        /// <param name="menu">Menu.</param>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_settings:
                    //EditRecepieButton_Click(null, null);
                    return true;
                case Resource.Id.menu_share:
                    //IngredientsButton_Click(null, null);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}

