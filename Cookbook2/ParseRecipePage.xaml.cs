using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cookbook2
{
    public interface IBaseUrl { string Get(); }

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ParseRecipePage : ContentPage
	{
        public ObservableCollection<Ingredient> Ingredients { get; set; } 
        private HtmlWebViewSource htmlSource;

        public ParseRecipePage ()
        {
            InitializeComponent ();
            Ingredients = new ObservableCollection<Ingredient>();
            IngredientsView.ItemsSource = Ingredients;
            LoadData();
        }

        private  void LoadData()
        {
            string recipeText = (string) BindingContext;

            if (recipeText == null)
            {
                if (Clipboard.HasText)
                {
                    recipeText = Clipboard.GetTextAsync().Result;
                    recipeText = recipeText.Replace("\n", "<br>");
                }
                else
                {
                    recipeText = "";
                }
            }
            else
            {
                recipeText = recipeText.Replace("\n", "<br>");
            }

            htmlSource = new HtmlWebViewSource
            {
                BaseUrl = DependencyService.Get<IBaseUrl>().Get(),
                Html = @"<html>
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
                                <textarea class=""content"" name=""example"">" + recipeText + @"</textarea>
                                <script>
                                    $(document).ready(function() {
                                        $('.content').richText();
                                    });
                                </script>
                                </body>
                                </html>"
            };

            RecipePreview.Source = htmlSource; 
        }

        public void EditIngredientsButtonOnClick(object sender, ItemTappedEventArgs itemTappedEventArgs)
        {
            ViewIngredientPage viewIngredientPage = new ViewIngredientPage((Ingredient) itemTappedEventArgs.Item);
            //viewIngredientPage.Disappearing += (o, args) => { 
            //    ingredients.Add(viewIngredientPage.ingredient);
            //};

            Navigation.PushAsync(viewIngredientPage);
        }

        public async void SaveButton_Click(object sender, EventArgs e)
        {
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            documentsPath = Path.Combine(documentsPath, "recipes");
            if (!Directory.Exists(documentsPath))
            {
                Directory.CreateDirectory(documentsPath);
            }

            string value = @"<textarea class=""content"" name=""example"">";
            int start = htmlSource.Html.IndexOf(value, StringComparison.Ordinal) + value.Length;
            int end = htmlSource.Html.IndexOf("</textarea>", StringComparison.Ordinal);

            Recipe res = new Recipe
            {
                RecipeShort = new RecipeShort(Guid.NewGuid().ToString(), TitleEdit.Text),
                Method = htmlSource.Html.Substring(start,end-start+1),
                Ingredients = Ingredients.ToList()
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
            TitleEdit.Text = await RecipePreview.EvaluateJavaScriptAsync("(function(){return window.getSelection().toString()})()");
            htmlSource.Html = htmlSource.Html.Replace(TitleEdit.Text, "");
        }

        private async void IngredientsButton_Click(object sender, EventArgs e)
        {
            string substring = await RecipePreview.EvaluateJavaScriptAsync("(function(){return window.getSelection().toString()})()");

            if (substring == null)
            {
                return;
            }

            htmlSource.Html = htmlSource.Html.Replace(substring, "");
            string[] all = substring.Split(new string[] { "\n", "\\n", @"\\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in all)
            {
                Ingredient ingr = new Ingredient();
                if (!ingr.TryToParseFromString(s))
                {
                    ingr.Unparsed = s;
                }
                Ingredients.Add(ingr);
            }
            IngredientsView.ItemsSource = Ingredients;
        }

        private void AddImageButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
 
}