using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Widget;
using Json.Net;

namespace Cookbook
{

    [Activity(Label = "IngredientActivity")]
    public class IngredientActivity : Activity
    {
        private TextView original;
        private TextView suggestedText;
        private TextView amountManual;
        private Spinner unitsSpinner;
        private TextView itemManual;
        private TextView saveButton;
        private TextView cancelButton;

        private Ingredient ingredient;
        private string ingredientStr;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ingridients_edit);

            original = (TextView)FindViewById(Resource.Id.original);
            suggestedText = (TextView)FindViewById(Resource.Id.suggestedText);

            amountManual = (TextView) FindViewById(Resource.Id.amountManual);
            amountManual.TextChanged += amountManual_textChanged;

            unitsSpinner = (Spinner) FindViewById(Resource.Id.unitsSpinner);
            unitsSpinner.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem,
                Ingredient.PossibleUnitsList.Keys.ToArray());
            unitsSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);

            itemManual = (TextView) FindViewById(Resource.Id.itemManual);

            saveButton = (Button)FindViewById(Resource.Id.saveButton);
            saveButton.Click += saveButton_click;

            cancelButton = (Button)FindViewById(Resource.Id.cancelButton);
            cancelButton.Click += ((s, e) => LoadIngredient());

        }

        private void amountManual_textChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(amountManual.Text))
            {
                return;
            }

            if (!double.TryParse(amountManual.Text, out double temp))
            {
                return;
            }

            ingredient.Amount = temp;  //TODO - make tryparse and warning
        }

        private void itemManual_textChanged(object sender, TextChangedEventArgs e)
        {
            ingredient.Item = itemManual.Text;
        }

        private void unitManual_Changed(object sender, TextChangedEventArgs e)
        {
            ingredient.Units = (string)unitsSpinner.SelectedItem;
        }

        private void saveButton_click(object sender, EventArgs e)
        {
            if (ingredient==null)
                ingredient = new Ingredient();

            ingredient.Amount = Double.Parse(amountManual.Text); //TODO - make tryparse and worning
            ingredient.Item = itemManual.Text;
            ingredient.Units = (string) unitsSpinner.SelectedItem;
            base.OnBackPressed();
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            ingredient.Units = spinner.GetItemAtPosition(e.Position).ToString();
        }

        protected override void OnStart()
        {
            base.OnStart();

            LoadIngredient();
        }

        private void LoadIngredient()
        {
            ingredientStr = Intent.GetStringExtra(Constants.IngredientString);
            if (ingredientStr!=null)
            {
                original.Text = ingredientStr;
            }

            string stringExtra = Intent.GetStringExtra(Constants.Ingredient);
            if (stringExtra!=null)
            {
                ingredient = JsonNet.Deserialize<Ingredient>(stringExtra);
                AssighnFormFields();
            }
            else
            {
                ingredient = new Ingredient();
                if (ingredient.TryToParseFromString(ingredientStr))
                {
                    AssighnFormFields();
                }
            }
        }

        private void AssighnFormFields()
        {
            if (original.Text == null)
            {
                original.Text = ingredient.ToString();
            }

            suggestedText.Text = ingredient.ToString();

            amountManual.Text = ingredient.Amount.ToString();

            unitsSpinner.SetSelection(((ArrayAdapter<string>) (unitsSpinner.Adapter)).GetPosition(ingredient.Units));
            itemManual.Text = ingredient.Item.ToString();
        }
    }
}