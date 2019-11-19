using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cookbook2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewIngredientPage : ContentPage
    {
        public Ingredient ingredient { get; set; }

        public ViewIngredientPage(Ingredient item)
        {
            InitializeComponent();
            ingredient = item;
            DataGrid.BindingContext = ingredient;
            LoadIngredient();
       }

        private void LoadIngredient()
        {
            
            if (ingredient != null)
            {
                original.Text = ingredient.Unparsed;
                suggestedText.Text = ingredient.Unparsed;
            }

            //if (ingredient.Amount == 0)
            //{
            //    if (ingredient.TryToParseFromString())
            //    {
            //        AssighnFormFields();
            //    }
            //}

        }

        //private void amountManual_textChanged(object sender, EventArgs eventArgs)
        //{
        //    if (string.IsNullOrWhiteSpace(amountManual.Text))
        //    {
        //        return;
        //    }

        //    if (!double.TryParse(amountManual.Text, out double temp))
        //    {
        //        return;
        //    }

        //    ingredient.Amount = temp;  //TODO - make tryparse and warning
        //}


        //private void itemManual_textChanged(object sender, EventArgs eventArgs)
        //{
        //    ingredient.Item = itemManual.Text;
        //}

        //private void unitManual_Changed(object sender, TextChangedEventArgs e)
        //{
        //    //ingredient.Units = (string)unitsSpinner.SelectedItem;
        //}

        private void saveButton_click(object sender, EventArgs e)
        {
            //if (ingredient == null)
            //    ingredient = new Ingredient();

            //ingredient.Amount = Double.Parse(amountManual.Text); //TODO - make tryparse and worning
            //ingredient.Item = itemManual.Text;
            //ingredient.Units = (string)unitsSpinner.SelectedItem;
            //base.OnBackPressed();
            Navigation.PopAsync();
        }

        //private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        //{
        //    Spinner spinner = (Spinner)sender;
        //    ingredient.Units = spinner.GetItemAtPosition(e.Position).ToString();
        //}


        //private void AssighnFormFields()
        //{
        //    //if (original.Text == null)
        //    //{
        //    //    original.Text = ingredient.ToString();
        //    //}

        //    //suggestedText.Text = ingredient.ToString();

        //    //amountManual.Text = ingredient.Amount.ToString();

        //    //unitsSpinner.SetSelection(((ArrayAdapter<string>)(unitsSpinner.Adapter)).GetPosition(ingredient.Units));
        //    //itemManual.Text = ingredient.Item.ToString();
        //}
    }
}
