using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cookbook2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewIngredientPage : ContentPage
    {
        public Ingredient Ingredient { get; set; }

        public ViewIngredientPage(Ingredient item)
        {
            InitializeComponent();
            Ingredient = item;
            DataGrid.BindingContext = Ingredient;
            UnitsPicker.ItemsSource = Ingredient.PossibleUnitsList.Keys.ToList();
            LoadIngredient();
       }

        private void LoadIngredient()
        {
            
            if (Ingredient != null)
            {
                Original.Text = Ingredient.Unparsed;
                suggestedText.Text = Ingredient.Unparsed;
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
            Navigation.PopAsync();
        }

        //private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        //{
        //    Spinner spinner = (Spinner)sender;
        //    ingredient.Units = spinner.GetItemAtPosition(e.Position).ToString();
        //}

    }
}
