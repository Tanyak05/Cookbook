using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using SQLite;
using Xamarin.Forms.Internals;

namespace Cookbook2
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Ingredient
    {
        public static Dictionary<string, string[]> PossibleUnitsList;
        public const string DefaultUnit = "units";

        private string units;

        [JsonProperty]
        public string Item { get; set; }

        [JsonProperty]
        public double Amount { get; set; }

        [JsonProperty]
        public string Unparsed { get; set; }

        [JsonProperty]
        public string Units
        {
            get => units;
            set => ConvertToPredefinedUnits(value);
        }

        public Ingredient()
        {
        }

        public Ingredient(string str)
        {
            FromString(str);
        }

        //public Ingredient(JSONObject item)
        //{
        //    FromJson(item);
        //}

        //public void FromJson(JSONObject item)
        //{
        //    Item = item.GetString("Item");
        //    Amount = item.GetDouble("Amount");
        //    Units = item.OptString("Units");
        //}

        //public void ToJson(JSONObject json)
        //{
        //    json.Put("Item", Item);
        //    json.Put("Amount", Amount);
        //    json.PutOpt("Units", Units);
        //}

        public override string ToString()
        {
            if (Unparsed != null)
                return Unparsed;

            string temp = Item + "\t" + Amount;
            if (!Units.Equals("units"))
                temp += "\t" + Units;
            return temp;
        }

        public void FromString(string ingr)
        {
            string[] temp = ingr.Split(new []{"\t"}, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length < 2)
            {
                return;
            }

            Item = temp[0];
            Amount = double.Parse(temp[1]);
            Units = temp.Length==3? temp[2] : "units";
        }

        public bool TryToParseFromString(string ingr)
        {
            string pattern = @"(?<Fraction>\d*\s*\d+/\d+)|(?<Number>\d+)";
            Regex rgx = new Regex(pattern);
            //string[] names = rgx.GetGroupNames();
            string[] splittedIngrLine = rgx.Split(ingr).Where(el=> !string.IsNullOrWhiteSpace(el)).ToArray();
            Match amountStr = Regex.Match(ingr, pattern);

            Console.WriteLine("Fraction: {0}, Number {1}, split: {2}", amountStr.Groups["Fraction"].Value, amountStr.Groups["Number"].Value, splittedIngrLine);

            if (!string.IsNullOrEmpty(amountStr.Groups["Fraction"].Value))
            {
                int amountInd = splittedIngrLine.IndexOf(w => w.Equals(amountStr.Groups["Fraction"].Value));
                if (amountInd == 0 && splittedIngrLine.Length > 1)
                {
                    Amount = ParseFraction(amountStr.Groups["Fraction"].Value);
                    int spaceInd = splittedIngrLine[1].Trim().IndexOf(' ');
                    Units = splittedIngrLine[1].Substring(0, spaceInd + 1);
                    Item = splittedIngrLine[1].Substring(spaceInd+1).Trim();
                }
                else
                {
                    if (amountInd == 1 && splittedIngrLine.Length > 1)
                    {
                        Amount = ParseFraction(amountStr.Groups["Fraction"].Value);
                        Item = splittedIngrLine[0].Trim();
                        Units = splittedIngrLine.Length == 3 ? splittedIngrLine[2] : "units";
                    }
                }

                Unparsed = ToString();
                return true;
            }

            if (!string.IsNullOrEmpty(amountStr.Groups["Number"].Value))
            {
                int amountInd = splittedIngrLine.IndexOf(w => w.Equals(amountStr.Groups["Number"].Value));

                if (amountInd == 0 && splittedIngrLine.Length > 1)
                {
                    Amount = double.Parse(amountStr.Groups["Number"].Value);
                    int spaceInd = splittedIngrLine[1].Trim().IndexOf(' ');
                    Units = splittedIngrLine[1].Substring(0, spaceInd+1);
                    Item = splittedIngrLine[1].Substring(spaceInd+1).Trim();
                }
                else
                {
                    if (amountInd == 1 && splittedIngrLine.Length > 1)
                    {
                        Amount = double.Parse(amountStr.Groups["Number"].Value);
                        Item = splittedIngrLine[0].Trim();
                        Units = splittedIngrLine.Length == 3 ? splittedIngrLine[2] : "units";
                    }
                }
                Unparsed = ToString();
                return true;
            }

            Unparsed = ingr;
            return false;
        }

        private double ParseFraction(string num)
        {
            //removes multiple spces between characters, cammas, and leading/trailing whitespace
            //num = Regex.Replace(num.Replace(",", ""), @"\s+", " ").Trim();
            double d = 0;
            int whole = 0;
            double numerator;
            double denominator;

            //is there a fraction?
            if (!num.Contains("/"))
            {
                //parse the whole thing
                return double.Parse(num);
            }

            //is there a space?
            if (num.Contains(" "))
            {
                //seperate the integer and fraction
                int firstspace = num.IndexOf(" ");
                string fraction = num.Substring(firstspace, num.Length - firstspace);
                //set the integer
                whole = int.Parse(num.Substring(0, firstspace));
                //set the numerator and denominator
                string[] temp = fraction.Split("/".ToCharArray());
                numerator = double.Parse(temp[0]);
                denominator = double.Parse(temp[1]);
            }
            else
            {
                string[] temp = num.Split("/".ToCharArray());
                //set the numerator and denominator
                numerator = double.Parse(temp[0]);
                denominator = double.Parse(temp[1]);
            }

            //is it a valid fraction?
            if (denominator != 0)
            {
                d = whole + (numerator / denominator);
            }

            return d;
        }

        private bool TryToParseFirstNumber(string temp, int charsToTry, out double tempAmount)
        {

            if (temp.Length >= charsToTry && double.TryParse(temp.Substring(0, charsToTry), out tempAmount))
            {
                Amount = tempAmount;
                Units = temp.Substring(0, temp.Length - charsToTry);
                return true;
            }

            tempAmount = 0;
            return false;
        }

        public void ConvertToPredefinedUnits(string unitToConvert)
        {

            if (string.IsNullOrWhiteSpace(unitToConvert))
            {
                units = "units";
                return;
            }

            string temp = unitToConvert.Trim();
            units = temp;

            var count = PossibleUnitsList.Keys.Count(el => el.Equals(temp));

            if (count == 0)
            {
                foreach (KeyValuePair<string, string[]> pair in PossibleUnitsList)
                {
                    if (pair.Value!= null && pair.Value.Count(el => el.Equals(temp)) > 0)
                    {
                        units = pair.Key;
                        break;
                    }
                }
            }
            else if (count == 1)
            {
                units = temp;
            }
        }

        public static void LoadPossibleUnits()
        {
            if (PossibleUnitsList != null)
            {
                return;
            }

            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(Ingredient)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("Cookbook2.possiblle_units_dictionary.json");

            PossibleUnitsList = new Dictionary<string, string[]>();

            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                PossibleUnitsList = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(json);
            }
        }

    }

    [JsonObject(MemberSerialization.OptIn)]
    public class RecipeShort : IDatabaseType
    {
        [JsonProperty, PrimaryKey]
        public string Id { get; set; }

        [JsonProperty]
        public string Title { get; set; }

        //[SQLite.Ignore]
        //public Bitmap Image { get; set; }

        //public RecipeShort(JSONObject item, Context context)
        //{
        //    this.FromJson(item, context);
        //}

        public RecipeShort(string id, string title)
        {
            Id=id;
            Title = title;
        }

        public RecipeShort()
        {
        }

        //public void FromJson(JSONObject item, Context context)
        //{
        //    Id = item.GetString(Constants.RecipeFieldId);
        //    Title = item.GetString(Constants.RecipeFieldTitle);

        //    if (item.Has(Constants.RecipeFieldImage))
        //    {
        //        string imageFile = item.GetString(Constants.RecipeFieldImage);
        //        Image = AssetUtils.LoadBitmapAsset(context, imageFile);
        //    }
        //}

        //public void ToJson(JSONObject json)
        //{
        //    json.Put(Constants.RecipeFieldId, Id);
        //    json.Put(Constants.RecipeFieldTitle, Title);
        //    json.PutOpt(Constants.RecipeFieldImage, Image);
        //}

    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Recipe 
    {
        [JsonProperty]
        public RecipeShort RecipeShort { get; set; }

        [JsonProperty]
        private List<Ingredient> ingredients;

        [JsonProperty]
        public string IngredientsText { get; private set; }

        [JsonProperty]
        public string Method { get; set; }

        public List<Ingredient> Ingredients
        {
            get => ingredients;
            set
            {
                ingredients = value;
                IngredientsText = string.Join("<br/>", ingredients.ToString()); ///TODO
            }
        }

        public Recipe() : base()
        {
            Ingredients = new List<Ingredient>();
        }

        //public Recipe(JSONObject json, Context context)
        //{
        //    RecipeShort = new RecipeShort(json, context);
        //    FromJson(json);
        //}

        //public void FromJson(JSONObject json)
        //{

        //    Method = json.OptString(Constants.RecipeFieldMethod);

        //    ingredients = new List<Ingredient>();
        //    JSONArray ingredientsJson = json.GetJSONArray(Constants.RecipeFieldIngredients);
        //    for (int i = 0; i < ingredientsJson.Length(); i++)
        //    {
        //        JSONObject ingredient = ingredientsJson.GetJSONObject(i);
        //        ingredients.Add(new Ingredient(ingredient));
        //        IngredientsText += " - " + ingredient + "\n";
        //    }

        //}

        //public void ToJson(JSONObject json)
        //{
        //    RecipeShort.ToJson(json);
        //    JSONArray ingredientsJason = new JSONArray(Ingredients);
        //    json.Put(Constants.RecipeFieldIngredients, ingredientsJason);
        //    json.Put(Constants.RecipeFieldMethod, Method);
        //}


        //public Bundle ToBundle()
        //{
        //    Bundle bundle = new Bundle();
        //    bundle.PutString(Constants.RecipeFieldId, Id);
        //    bundle.PutString(Constants.RecipeFieldTitle, Title);
        //    bundle.PutString(Constants.RecipeFieldSummary, Summary);
        //    bundle.PutString(Constants.RecipeFieldImage, RecipeImage);
        //    bundle.PutString(Constants.RecipeFieldIngredients, IngredientsText);
        //    bundle.PutString(Constants.RecipeFieldMethod, Method);
        //    return bundle;
        //}

        //public void FromBundle(Bundle bundle)
        //{
        //    Id = bundle.GetString(Constants.RecipeFieldId);
        //    Title = bundle.GetString(Constants.RecipeFieldTitle);
        //    Summary = bundle.GetString(Constants.RecipeFieldSummary);
        //    RecipeImage = bundle.GetString(Constants.RecipeFieldImage);//TODO - put default value
        //    IngredientsText = bundle.GetString(Constants.RecipeFieldIngredients);
        //    Ingredients = IngredientsText.Split(new char[] { '-', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        //    Method = bundle.GetString(Constants.RecipeFieldMethod,"");
        //}
    }
}

