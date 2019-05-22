﻿
using UIKit;
using Xamarin.Forms;

namespace CarouselView.iOS
{
	public class ViewContainer : UIViewController
	{
		// To save current position
		public object Tag { get; set; }

		protected override void Dispose(bool disposing)
		{
			// because this runs in the finalizer thread and disposing is equal false
            InvokeOnMainThread( () => {
				// Significant Memory Leak for iOS when using custom layout for page content #125
				foreach (var view in View.Subviews)
				{
					view.RemoveFromSuperview();
					view.Dispose();
				}

				View.RemoveFromSuperview();
				View.Dispose();
				View = null;
			});

			base.Dispose(disposing);
		}
	}
}

