using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace XamarianTest
{
    public enum DishUnits { cup, spoon, tea_spoon };
    public enum MetricUnits { gramms, milligrams, kilograms, units };
    public enum OzUnits {oz, lb, l_oz, units };

    

    public class CategoryList
    {
        private String StoredFileName { get;}
        Dictionary<int, string> Categories { get; }
        public static CategoryList MainCategoryList = new CategoryList("main.xml");
        public static CategoryList SubCategoryList = new CategoryList("sub.xml");
        public static CategoryList TubsList = new CategoryList("tags.xml");

        //todo later
        //int Rating;
        //int Complicated;
        //int TimesDone;

        public CategoryList(string fileName) {
            StoredFileName = fileName;
            Categories = LoadCategories();
        }

        private Dictionary<int, string> LoadCategories()
        {
            return null;
        }

        public void AddCategory(string category) { }

        public void RemoveCategory(string category = null, int id = -1) { }

    }


    public class Recepie
    {
        public int Id { get; internal set; }
        String Title { get; set; }
        String PreferedUnits { get; set; }

        //int - order id, amount of ingridient, units type - should be converted to enum or maybe string list
        Dictionary<int, (int Amount, int UnitsType)> IngridientsList { get; set; }

        //the body of the recepy - should be hml
        String Method { get; set; }

        String StoredFileName { get; set; }
        String ImageName { get; set; }
        int Category { get; set; }
        int SubCategory { get; set; }
        List<int> tagsList { get; set; }

        void Export() { }
        void Import() { }

    }
}