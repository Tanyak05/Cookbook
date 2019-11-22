using System.Collections.Generic;

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
    }
}

