using System;

using Android.App;
using Android.Widget;
using Android.OS;

namespace XamarinCryptoSpeed.II
{
	[Activity (Label = "XamarinCryptoSpeed.II", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			Button button = FindViewById<Button>(Resource.Id.AES128Button);
			button.Click += delegate {
				var AES128 = new AES(128);
				AES128.testAES("AES128", Application.Context);
			};

			button = FindViewById<Button>(Resource.Id.AES256Button);
			button.Click += delegate {
				var AES128 = new AES(256);
				AES128.testAES("AES256", Application.Context);
			};


			button = FindViewById<Button>(Resource.Id.TripleDESButton);
			button.Click += delegate {
				var AES128 = new AES(192);
				AES128.testAES("TripleDES", Application.Context);
			};

			button = FindViewById<Button>(Resource.Id.MD5Button);
			button.Click += delegate {
				Hash.TestHash("MD5");
			};

			button = FindViewById<Button>(Resource.Id.SHA1Button);
			button.Click += delegate {
				Hash.TestHash("SHA1");
			};
		}
	}
}


