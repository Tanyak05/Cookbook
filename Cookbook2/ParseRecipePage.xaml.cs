using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cookbook2
{
    public interface IBaseUrl { string Get(); }

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ParseRecipePage : ContentPage
	{
        //public ToolbarItem Toolbar { get; set; }
        public ObservableCollection<Ingredient> ingredients { get; set; } 
        private HtmlWebViewSource htmlSource;


        public ParseRecipePage ()
        {
            InitializeComponent ();
            ingredients = new ObservableCollection<Ingredient>();
            ingredientsView.ItemsSource = ingredients;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            string recipeText = (string)BindingContext;

            if (recipeText == null)
            {
                if (Clipboard.HasText)
                {
                    recipeText = await Clipboard.GetTextAsync();
                    recipeText = recipeText.Replace("\n", "<br>");
                }
                else
                {
                    recipeText = "";
                }
            }

            //htmlSource = new HtmlWebViewSource { Html = $"<html><body><textarea class=\"content\" name=\"example\">{ recipeText }</textarea></body></html>" };
            //recepiePreview.Source = htmlSource;

            htmlSource = new HtmlWebViewSource();
            htmlSource.BaseUrl = DependencyService.Get<IBaseUrl>().Get();

            htmlSource.Html = @"<html>
                                <head>
                                    <meta charset=""utf - 8"">
<link href=""https://www.jqueryscript.net/css/jquerysctipttop.css"" rel=""stylesheet"" type=""text/css"">
                <link rel = ""stylesheet"" href = ""https://netdna.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css"">
                <link rel = ""stylesheet"" href = ""css/site.css"" >
                <link rel = ""stylesheet"" href = ""richtext.min.css"" >
                <script src = ""https://code.jquery.com/jquery-1.12.4.min.js"" ></script>
                <script src = ""jquery.richtext.js""></script>
                                           </head>
                                <body>
                                <textarea class=""content"" name=""example"">"+ recipeText + @"</textarea>
                                <script>
                                    $(document).ready(function() {
                                        $('.content').richText();
                                    });
                                </script>
                                </body>
                                </html>";

            recepiePreview.Source = htmlSource; //DependencyService.Get<IBaseUrl>().Get();
        }

        public void EditIngredientsButtonOnClick(object sender, ItemTappedEventArgs itemTappedEventArgs)
        {
            //Toast.MakeText(Application, ((TextView)args.View).Text, ToastLength.Short).Show();

            //var intent = new Intent(this, typeof(IngredientActivity));
            //intent.PutExtra(Constants.IngredientString, ((TextView)args.View).Text);
            //JSONObject json = new JSONObject();
            //ingredients[args.Position].ToJson(json);
            //intent.PutExtra(Constants.Ingredient, json.ToString());
            //StartActivity(intent);

            ViewIngredientPage viewIngredientPage = new ViewIngredientPage((Ingredient) itemTappedEventArgs.Item);

            Navigation.PushAsync(new NavigationPage(viewIngredientPage));
        }

        public async void SaveButton_Click(object sender, EventArgs e)
        {
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            documentsPath = Path.Combine(documentsPath, "recipes");
            if (!Directory.Exists(documentsPath))
            {
                Directory.CreateDirectory(documentsPath);
            }

            Recipe res = new Recipe
            {
                RecipeShort = new RecipeShort(Guid.NewGuid().ToString(), titleEdit.Text),
                Method = htmlSource.Html,
                Ingredients = ingredients.ToList()
            };

            string recipeSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(res);

            // Documents folder
            documentsPath = Path.Combine(documentsPath, res.RecipeShort.Id + ".json");
            using (StreamWriter file = File.CreateText(documentsPath))
            {
                file.Write(recipeSerialized);
            }

            await LocalDatabase.Database.SaveItemAsync<RecipeShort>(res.RecipeShort);

            await Navigation.PopAsync();

        }


        private async void ExtractTitleButton_Click(object sender, EventArgs e)
        {

            titleEdit.Text = await recepiePreview.EvaluateJavaScriptAsync("(function(){return window.getSelection().toString()})()");
            htmlSource.Html = htmlSource.Html.Replace(titleEdit.Text, "");

        }


        private async void IngredientsButton_Click(object sender, EventArgs e)
        {
            string substring = await recepiePreview.EvaluateJavaScriptAsync("(function(){return window.getSelection().toString()})()");

            if (substring == null)
            {
                return;
            }

            htmlSource.Html = htmlSource.Html.Replace(substring, "");

            string[] all = substring.Split(new string[]{"\n","\\n",@"\\n"}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in all)
            {
                Ingredient ingr = new Ingredient();
                if (!ingr.TryToParseFromString(s))
                {
                    ingr.Unparsed = s;
                }
                ingredients.Add(ingr);
            }

            ingredientsView.ItemsSource = ingredients;

        }


        private void AddImageButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

    }

    
}