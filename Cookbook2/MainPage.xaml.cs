using System;
using System.IO;
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
            Recipes = new RecipeList();

            ShortRecipeList.ItemsSource = Recipes.items;
        }

        public RecipeList Recipes { get; set; }

        private static void LoadTestData()
        {
            string documentsPath = Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
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

            await Navigation.PushAsync( new ViewRecipePage((RecipeShort) itemTappedEventArgs.Item));
        }

        public void DeleteFromList(RecipeShort recipe)
        {
            Recipes.Delete(recipe);
            ShortRecipeList.ItemsSource = Recipes.items;
        }

        private async void GeneralAddButton_Click(object sender, EventArgs e)
        {
            //var intent = new Intent(this, typeof(AddRecipeActivity));
            //StartActivity(intent);
            //await Navigation.PushAsync(new AddRecipePage());
            
        }

        private async void AddWithUrlButton_Click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddWithUrlPage());
        }

        private async void AddFromClipboardButton_Click(object sender, EventArgs e)
        {
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