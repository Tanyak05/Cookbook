using Android.App;
using Android.OS;
using Cookbook2;
using Plugin.CurrentActivity;
using Xamarin.Forms;

namespace Cookbook
{

    [Activity(Label = "Cookbook", MainLauncher = true)]
    [assembly: Dependency(typeof(BaseUrl_Android))]
    public class MainActivity :  global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        //private RecipeListAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //CrossCurrentActivity.Current.Init(this, null);

            // Set our view from the "main" layout resource
            //SetContentView(Resource.Layout.activity_main);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App());

            // Get our UI controls from the loaded layout
            //ListView recipeListView = FindViewById<ListView>(Resource.Id.recepeListView);
            //adapter = new RecipeListAdapter(this);
            //recipeListView.Adapter = adapter;
            //recipeListView.ItemClick += OnListItemClick;

            //Button generalAddButton = FindViewById<Button>(Resource.Id.generalAddButton);
            //generalAddButton.Click += GeneralAddButton_Click;


        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

}