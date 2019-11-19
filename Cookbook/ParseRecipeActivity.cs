using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Cookbook2;
using Org.Json;
using Xamarin.Forms;
using ListView = Android.Widget.ListView;

namespace Cookbook
{
    [Activity(Label = "@string/app_name")]
    public class ParseRecipeActivity : Activity //, IWebViewSelectionActivity
    {
        private Recipe recipe;

        //TODO: support images
        private ImageView imageView;
        private EditText titleTextView;
        private ListView ingredientsListView;
        private WebView methodTextWebView;
        private HtmlWebViewSource htmlSource;


        private List<Ingredient> ingredients;
        private ArrayAdapter<Ingredient> arrayAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.parsing_recepie);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar2);
            SetActionBar(toolbar);

            titleTextView = (EditText) FindViewById(Resource.Id.titleEdit);
            //imageView = (ImageView)FindViewById(Resource.Id.recipeImageView);

            ingredientsListView = (ListView) FindViewById(Resource.Id.ingredientsView);
            ingredients = new List<Ingredient>();
            arrayAdapter = new ArrayAdapter<Ingredient>(this, Android.Resource.Layout.SimpleListItem1, ingredients);
            ingredientsListView.Adapter = arrayAdapter;
            ingredientsListView.ChoiceMode = ChoiceMode.Single;
            ingredientsListView.ItemClick += EditIngredientsButtonOnClick;

            //editIngredientsButton = (Button)FindViewById(Resource.Id.editIngerientsButton);
            //editIngredientsButton.Click += EditIngredientsButtonOnClick;

            methodTextWebView = (Xamarin.Forms.WebView)FindViewById(Resource.Id.recepiePreview);

            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            documentsPath = Path.Combine(documentsPath, "recipes");
            if (!Directory.Exists(documentsPath))
            {
                Directory.CreateDirectory(documentsPath);
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
        }



        /// <Docs>The options menu in which you place your items.</Docs>
        /// <returns>To be added.</returns>
        /// <summary>
        /// This is the menu for the Toolbar/Action Bar to use
        /// </summary>
        /// <param name="menu">Menu.</param>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_parse, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_add_image:
                    AddImageButton_Click(null, null);
                    return true;
                case Resource.Id.menu_to_title:
                    ExtractTitleButton_Click(null, null);
                    return true;
                case Resource.Id.menu_to_ingr:
                    IngredientsButton_Click(null, null);
                    return true;
                case Resource.Id.menu_save:
                    SaveButton_Click(null,null);
                    return true;

                default: return base.OnOptionsItemSelected(item);
            }
        }

    }
}