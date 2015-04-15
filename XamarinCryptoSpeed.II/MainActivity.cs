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
				var AES128 = new SymmetricCipher("AES", 128, 16, Application.Context);
				AES128.TestSymmetricCipher();
			};

			button = FindViewById<Button>(Resource.Id.AES256Button);
			button.Click += delegate {
				var AES256 = new SymmetricCipher("AES", 256, 16, Application.Context);
				AES256.TestSymmetricCipher();
			};
				
			button = FindViewById<Button>(Resource.Id.TripleDESButton);
			button.Click += delegate {
				var tripleDES = new SymmetricCipher("TripleDES", 196, 8, Application.Context);
				tripleDES.TestSymmetricCipher();
			};

			button = FindViewById<Button>(Resource.Id.MD5Button);
			button.Click += delegate {
				Hash.TestHash("MD5");
			};

			button = FindViewById<Button>(Resource.Id.SHA1Button);
			button.Click += delegate {
				Hash.TestHash("SHA1");
			};

			button = FindViewById<Button>(Resource.Id.ARC4Button);
			button.Click += delegate {
				var tripleDES = new SymmetricCipher("ARC4", 128, 8, Application.Context);
				tripleDES.TestSymmetricCipher();
			};
		}
	}
}


