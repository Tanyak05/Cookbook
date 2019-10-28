using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Org.Json;

namespace Cookbook
{
    [Activity(Label = "@string/app_name")]
    public class ParseRecipeActivity : Activity //, IWebViewSelectionActivity
    {

        Recipe recipe;

        //TODO: support images
        ImageView imageView;
        private EditText titleTextView;
        ListView ingredientsListView;
        private HtmlTextView.HtmlTextView methodTextWebView;

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

            methodTextWebView = (HtmlTextView.HtmlTextView)FindViewById(Resource.Id.recepiePreview);

            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            documentsPath = Path.Combine(documentsPath, "recipes");
            if (!Directory.Exists(documentsPath))
            {
                Directory.CreateDirectory(documentsPath);
            }
        }

        private void EditIngredientsButtonOnClick(object sender, AdapterView.ItemClickEventArgs args)
        {
            Toast.MakeText(Application, ((TextView)args.View).Text, ToastLength.Short).Show();

            var intent = new Intent(this, typeof(IngredientActivity));
            intent.PutExtra(Constants.IngredientString, ((TextView)args.View).Text);
            JSONObject json = new JSONObject();
            ingredients[args.Position].ToJson(json);
            intent.PutExtra(Constants.Ingredient, json.ToString());
            StartActivity(intent);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {

            Recipe res = new Recipe
            {
                RecipeShort = new RecipeShort(Guid.NewGuid().ToString(), titleTextView.Text),
                Method = methodTextWebView.Text,
                Ingredients = ingredients
            };

            string recipeSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(res);

            // Documents folder
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string documentsPath = Path.Combine(folderPath, "recipes", res.RecipeShort.Id+".json");
            using (StreamWriter file = File.CreateText(documentsPath))
            {
                file.Write(recipeSerialized);
            }

            LocalDatabase.Database.SaveItemAsync<RecipeShort>(res.RecipeShort);

            //var x = await Xamarin.Forms.NavigationPage.PopAsync();

        }


        protected override void OnStart()
        {
            base.OnStart();
            string recipeText = Intent.GetStringExtra(Constants.RecipeSelection);

            if (recipeText != null)
            {
                //methodTextWebView.LoadData(recipeText, "text/html", "utf-8");
                methodTextWebView.SetHtml(recipeText);
            }
            else 
            {
                ClipboardManager clipboardManager = (ClipboardManager)Android.App.Application.Context.GetSystemService(Context.ClipboardService);

                if (clipboardManager.HasPrimaryClip)
                {
                    if (clipboardManager.PrimaryClipDescription.HasMimeType(ClipDescription.MimetypeTextHtml))
                    {
                        for (int i = 0; i < clipboardManager.PrimaryClip.ItemCount; i++)
                        {
                            ClipData.Item itemAt = clipboardManager.PrimaryClip.GetItemAt(i);
                            if (itemAt.HtmlText != null)
                            {
                                String sss = itemAt.HtmlText.Replace("\n", "<br>");
                                methodTextWebView.SetHtml(sss);
                                break;
                            }
                        }
                    }
                    else if (clipboardManager.PrimaryClipDescription.HasMimeType(ClipDescription.MimetypeTextPlain))
                    {
                        for (int i = 0; i < clipboardManager.PrimaryClip.ItemCount; i++)
                        {
                            ClipData.Item itemAt = clipboardManager.PrimaryClip.GetItemAt(i);
                            if (itemAt.Text != null)
                            {
                                string sss = itemAt.Text.Replace("\n", "<br>");
                                methodTextWebView.SetHtml(sss);
                                break;
                            }
                        }
                    }
                }

            }

        }


        private void ExtractTitleButton_Click(object sender, EventArgs e)
        {

            titleTextView.Text = methodTextWebView.Text.Substring(methodTextWebView.SelectionStart,
                methodTextWebView.SelectionEnd - methodTextWebView.SelectionStart + 1);
            methodTextWebView.Text = methodTextWebView.Text.Remove(methodTextWebView.SelectionStart,
                methodTextWebView.SelectionEnd - methodTextWebView.SelectionStart + 1);
        }


        private void IngredientsButton_Click(object sender, EventArgs e)
        {
            if (!methodTextWebView.HasSelection)
            {
                return;
            }

            string substring = methodTextWebView.Text.Substring(methodTextWebView.SelectionStart,methodTextWebView.SelectionEnd - methodTextWebView.SelectionStart);

            string[] all = substring.Split("\n",StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in all)
            {
                Ingredient ingr = new Ingredient();
                if (!ingr.TryToParseFromString(s))
                {
                    ingr.Unparsed = s;
                }
                ingredients.Add(ingr);
                arrayAdapter.Add(ingr);                
            }


            methodTextWebView.Text = methodTextWebView.Text.Remove(methodTextWebView.SelectionStart,
                methodTextWebView.SelectionEnd - methodTextWebView.SelectionStart);

        }


        private void AddImageButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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