using System;
using Android.Widget;
using System.Collections.Generic;
using System.IO;
using Android.Database;
using Org.Json;
using Android.Util;
using Android.Views;
using Android.App;
using Cookbook2;


namespace Cookbook
{

    public class RecipeListAdapter : BaseAdapter<RecipeShort>
    {

        private  List<RecipeShort> items = new List<RecipeShort>();
        private readonly Activity context;

        public RecipeListAdapter (Activity context)
		{
			this.context = context;
			LoadRecipeList ();
		}

        private void LoadRecipeList()
        {
            items = LocalDatabase.Database.GetItemsAsync<RecipeShort>().Result;
        }

        private void ParseJson(JSONObject json)
		{
			try 
			{
                List<RecipeShort> result = new List<RecipeShort>();

                JSONArray temp = json.GetJSONArray(Constants.RecipeFieldList);

                if (temp == null)
                    return;

				for (int i = 0; i < temp.Length(); i ++)
                {
                    JSONObject item = temp.GetJSONObject(i);
                    RecipeShort parsed = new RecipeShort(item, context);
                    items.Add(parsed);
                }

                return;
            }
            catch (Exception)
            {
				//Log.Error (Tag, "Failed to parse recipe list: " + ex);
                return;
			}
		}

        private void AppendItemsToList(List<RecipeShort> items)
		{
			this.items.AddRange (items);
            NotifyDataSetChanged();
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.list_item, null);

            RecipeShort item = items[position];
			TextView titleView = (TextView)view.FindViewById (Resource.Id.textTitle);
			titleView.Text = item.Title;

			//var iv = (ImageView)view.FindViewById (Resource.Id.imageView);
   //         if (item.Image != null)
   //         {
   //             iv.SetImageBitmap(item.Image);
   //         }
   //         else
   //         {
   //             iv.SetImageResource(Resource.Drawable.ic_noimage);
   //         }
            return view;
		}

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count => items.Count;

        public override RecipeShort this[int position] => items[position];
    }
}

