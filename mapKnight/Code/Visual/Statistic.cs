using System;
using System.Collections.Generic;
using System.IO;

//using Android.Graphics;
//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Util;
//using Android.Views;
//using Android.Widget;
//using Android.Support.V7.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;



namespace mapKnight
{
    public class Statistic
    {

        public string Name {get;set;}
        public string Description { get; set;}
		public int Image { get; set;} //byte array des bilds
        public int Value { get ; set;}

        public Statistic()
        {
			Name = "No Name";
			Description = "No Description";
			Image = Resource.Drawable.error;
			Value = -1;
        }
    }

    public class StatisticRecyclerAdapter : RecyclerView.Adapter
    {
        private List<Statistic> StatisticsList;

        public StatisticRecyclerAdapter(List<Statistic> statisticsToShow)
        {
            StatisticsList = statisticsToShow;
        }

        public class MyView :RecyclerView.ViewHolder
        {
            public View MainView { get; set;}
            public TextView StatsName { get; set;}
            public TextView StatsDescription { get; set;}
            public TextView StatsValue { get; set;}
            public ImageView StatsImage { get; set;}

            public MyView(View View) : base(View)
            {
                MainView = View;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View Entry = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.StatsListElement, parent, false);

            TextView Name = Entry.FindViewById<TextView>(Resource.Id.stats_name);
            TextView Description = Entry.FindViewById<TextView>(Resource.Id.stats_description);
            TextView Value = Entry.FindViewById<TextView>(Resource.Id.stats_value);
			ImageView Image = Entry.FindViewById<ImageView>(Resource.Id.stats_image);

            MyView View = new MyView(Entry) { StatsName = Name, StatsDescription = Description, StatsValue = Value, StatsImage = Image };
            return View;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView Holder = holder as MyView;
            Holder.StatsName.Text = StatisticsList[position].Name;
            Holder.StatsDescription.Text = StatisticsList[position].Description;
            Holder.StatsValue.Text = StatisticsList[position].Value.ToString();

			Holder.StatsImage.SetImageResource (StatisticsList [position].Image);
		}

        public override int ItemCount
        {
            get
            {
                return StatisticsList.Count;
            }
        }
    }

}

