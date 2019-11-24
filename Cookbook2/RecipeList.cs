using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.Internals;

namespace Cookbook2
{

    public class RecipeList  
    {

        public List<RecipeShort> items { get; set; }

        //private readonly Activity context;

        public RecipeList ()
		{
            items = new List<RecipeShort>(LocalDatabase.Database.GetItemsAsync<RecipeShort>().Result);
		}

        public void Delete(RecipeShort recipe)
        {
            int ind = items.IndexOf(el => el.Id == recipe.Id);
            items.RemoveAt(ind);
        }
    }
}

