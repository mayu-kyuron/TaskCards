using PCLAppConfig;
using System;
using System.Reflection;
using TaskCards.MasterPages;
using TaskCards.Pages;
using Xamarin.Forms;

namespace TaskCards {

	public partial class App : Application {

		public App() {
			InitializeComponent();

			try {
				Assembly assembly = typeof(App).GetTypeInfo().Assembly;
				ConfigurationManager.Initialise(assembly.GetManifestResourceStream("TaskCards.Config.xml"));
			}
			catch (Exception ex) {
				// 何もしない
			}

			MainPage = new TaskCardsMasterDetailPage(new DetailPage());
		}

		protected override void OnStart() {
			// Handle when your app starts
		}

		protected override void OnSleep() {
			// Handle when your app sleeps
		}

		protected override void OnResume() {
			// Handle when your app resumes
		}
	}
}
