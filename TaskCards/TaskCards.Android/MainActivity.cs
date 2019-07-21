using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using TaskCards.Consts;
using TaskCards.Droid.Services;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace TaskCards.Droid {

    [Activity(Label = StringConst.AppLabel, Icon = "@mipmap/icon", Theme = "@style/MainTheme",
		MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait,
		ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
	public class MainActivity : FormsAppCompatActivity {

        protected override void OnCreate(Bundle bundle) {

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

			UserDialogs.Init(this);
			Forms.Init(this, bundle);

			LayoutService.Activity = this;

			LoadApplication(new App());
        }

		public override void OnConfigurationChanged(Configuration newConfig) {
			base.OnConfigurationChanged(newConfig);
		}

		protected override void OnStart() {
			base.OnStart();
		}

		protected override void OnResume() {
			base.OnResume();
		}

		protected override void OnRestart() {
			base.OnRestart();
		}

		protected override void OnPause() {
			base.OnPause();
		}

		protected override void OnStop() {
			base.OnStop();
		}

		protected override void OnSaveInstanceState(Bundle outState) {
			base.OnSaveInstanceState(outState);
		}

		protected override void OnDestroy() {
			base.OnDestroy();
		}
	}
}