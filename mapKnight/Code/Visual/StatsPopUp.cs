
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

namespace mapKnight
{
    public class StatsPopUp : DialogFragment
    {
        private RecyclerView StatsView;
        private RecyclerView.LayoutManager StatsLayoutManager;
        private List<Statistic> StatsList;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

			base.OnCreateView (inflater, container, savedInstanceState);

			View View = inflater.Inflate (Resource.Layout.StatsPopUp, container, false);
			StatsView = View.FindViewById<RecyclerView> (Resource.Id.statsContainer);

			StatsLayoutManager = new LinearLayoutManager (Activity);
			StatsView.SetLayoutManager (StatsLayoutManager);
			StatsList = new List<Statistic> ();
			StatsList.Add (new Statistic () {
				Name = "Leben",
				Description = "Leben des Helden",
				Value = 2,
				Image = Resource.Drawable.heart
			});
			StatsList.Add (new Statistic () { Name = "Mana", Value = 20000000, Image = Resource.Drawable.heart });
			StatsList.Add (new Statistic () { Name = "Stärke", Value = 552, Image = Resource.Drawable.heart });
			StatsList.Add (new Statistic () { Name = "Schnelligkeit", Value = 23, Image = Resource.Drawable.heart });
			StatsList.Add (new Statistic () { Name = "Intelligenz", Value = 42, Image = Resource.Drawable.heart });
			StatsList.Add (new Statistic () { Name = "Ansehen", Value = 12, Image = Resource.Drawable.heart });
			StatsList.Add (new Statistic ());
			StatsView.SetAdapter (new StatisticRecyclerAdapter (StatsList));
			return View;
		}

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);//Kein Titel
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.InventoryAnimation;
            //definiert die Animationen
		}
    }
}
