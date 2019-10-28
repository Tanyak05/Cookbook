using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;
using System;
using System.IO;
using Plugin.CurrentActivity;


namespace Cookbook
{


    [Activity(Label = "Cookbook", MainLauncher = true)]
    public class MainActivity : Activity
    {



        private RecipeListAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            CrossCurrentActivity.Current.Init(this, null);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadTestData();
            Ingredient.LoadPossibleUnits(this);

            // Get our UI controls from the loaded layout
            ListView recipeListView = FindViewById<ListView>(Resource.Id.recepeListView);
            adapter = new RecipeListAdapter(this);
            recipeListView.Adapter = adapter;
            recipeListView.ItemClick += OnListItemClick;

            Button generalAddButton = FindViewById<Button>(Resource.Id.generalAddButton);
            generalAddButton.Click += GeneralAddButton_Click;


        }

        private static void LoadTestData()
        {
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            documentsPath = Path.Combine(documentsPath, "recipes");

            if (!Directory.Exists(documentsPath))
            {
                Directory.CreateDirectory(documentsPath);
            }

            LocalDatabase.Database.CreateTable<RecipeShort>();
        }


        private void GeneralAddButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(AddRecipeActivity));
            StartActivity(intent);
        }


        protected void OnListItemClick(object sender, AdapterView.ItemClickEventArgs itemClickEventArgs)
        {
            string itemName = adapter[itemClickEventArgs.Position].Id;
            Intent intent = new Intent(ApplicationContext, typeof(RecipeActivity));
            intent.PutExtra(Constants.RecipeNameToLoad, itemName);
            StartActivity(intent);
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }




}