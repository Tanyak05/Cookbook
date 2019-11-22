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
    public partial class MainPage : ContentPage
    {
        

        public MainPage()
        {
            LoadTestData();
            Ingredient.LoadPossibleUnits();
            
            InitializeComponent();
            rl = new RecipeList();

            ShortRecipeList.ItemsSource = rl.items;

        }

        public RecipeList rl { get; set; }


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

        async void Handle_ItemSelected(object sender, ItemTappedEventArgs itemTappedEventArgs)
        {
            if (((ListView)sender).SelectedItem == null)
                return;

            //await DisplayAlert("Selected", e.SelectedItem.ToString(), "OK");

            await Navigation.PushAsync( new ViewRecepiePage((RecipeShort) itemTappedEventArgs.Item));

            ////Deselect Item
            //((ListView)sender).SelectedItem = null;
        }

        //protected void OnListItemClick(object sender, AdapterView.ItemClickEventArgs itemClickEventArgs)
        //{
        //    string itemName = adapter[itemClickEventArgs.Position].Id;
        //    Intent intent = new Intent(ApplicationContext, typeof(RecipeActivity));
        //    intent.PutExtra(Constants.RecipeNameToLoad, itemName);
        //    StartActivity(intent);
        //}

        private async void GeneralAddButton_Click(object sender, EventArgs e)
        {
            //var intent = new Intent(this, typeof(AddRecipeActivity));
            //StartActivity(intent);
            //await Navigation.PushAsync(new AddRecipePage());
            
        }

        private async void AddWithUrlButton_Click(object sender, EventArgs e)
        {
            //var intent = new Intent(this, typeof(AddFromUrlRecipeActivity));
            //StartActivity(intent);
            await Navigation.PushAsync(new AddWithUrlPage());
        }

        private async void AddFromClipboardButton_Click(object sender, EventArgs e)
        {
            //var intent = new Intent(this, typeof(ParseRecipeActivity));
            //StartActivity(intent);
            
            await Navigation.PushAsync(new ParseRecipePage());

        }

        private void AddFromPictureButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AddManuallyButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}