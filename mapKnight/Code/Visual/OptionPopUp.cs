using mapKnightLibrary;

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

namespace mapKnight
{
	public class OptionPopUp : DialogFragment
	{
		public event EventHandler<ControlType> ControlTypeToggled;

		ControlType InitialisedControlType;

		public OptionPopUp(ControlType CurrentControlType){
			InitialisedControlType = CurrentControlType;

		
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View View = inflater.Inflate (Resource.Layout.OptionWindow, container, false);

			RadioButton SlideOption = View.FindViewById<RadioButton> (Resource.Id.useSlide);
			RadioButton ButtonOption = View.FindViewById<RadioButton> (Resource.Id.useButton);

			switch (InitialisedControlType) {
			case ControlType.Button:
				ButtonOption.Checked = true;
				break;
			case ControlType.Slide:
				SlideOption.Checked = true;
				break;
			}

			SlideOption.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) => {
				if (e.IsChecked) {
					ButtonOption.Checked = false;
					if (ControlTypeToggled != null)
						ControlTypeToggled (this, ControlType.Slide);
				}
			};
			ButtonOption.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) => {
				if (e.IsChecked) {
					SlideOption.Checked = false;
					if (ControlTypeToggled != null)
						ControlTypeToggled (this, ControlType.Button);
				}
			};

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

