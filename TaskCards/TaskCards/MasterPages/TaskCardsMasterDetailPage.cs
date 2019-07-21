using System;
using System.Collections.Generic;
using Xamarin.Forms;
//using TaskCards.Consts;
using TaskCards.Pages;
using TaskCards.Services;
//using TaskCards.Utilities;

namespace TaskCards.MasterPages {

	public class TaskCardsMasterDetailPage : MasterDetailPage {

		public static Page currentDetailPage;

		SlideMenuPage menuPage;

		public TaskCardsMasterDetailPage() { }

		public TaskCardsMasterDetailPage(Page displayPage) {

			this.menuPage = new SlideMenuPage();

			this.menuPage.Menu.ItemSelected += (sender, e) => NavigateTo(e.SelectedItem as SlideMenuItem);
			this.IsPresentedChanged += (sender, e) => PresentedChanged(sender, e);

			Master = menuPage;

			currentDetailPage = displayPage;

			Detail = displayPage;

			NavigationPage.SetHasBackButton(this, false);
		}

		/// <summary>
		/// ページ切換えイベント
		/// </summary>
		/// <param name="menu">選択メニュー項目</param>
		private async void NavigateTo(SlideMenuItem menu) {

			if (menu == null) return;

			Page displayPage = null;
			if (menu.TargetType != null) displayPage = (Page)Activator.CreateInstance(menu.TargetType);

			currentDetailPage = displayPage;

			// お問い合わせ（ページ遷移なし）
			//if (displayPage == null) {
			//	string errMsg = String.Empty;
			//	string email = new Preferences().GetContactUsEmail();

			//	bool isStartedMailer = DependencyService.Get<IMailService>().StartMailer("お問い合わせ", null, email, ref errMsg);
			//	if (!isStartedMailer) await this.DisplayAlert(StringConst.DialogTitleError, String.Format(StringConst.MessageFailed, "メーラーの起動"), "OK");
			//}
			//// ページ遷移
			//else {
				// ログアウト処理
				//if (displayPage.GetType().Equals(typeof(LoginPage))) {

				//	Device.BeginInvokeOnMainThread((Action)(async () => {

				//		// 確認ダイアログの表示
				//		var result = await DisplayAlert(StringConst.DialogTitleConfirm,
				//			StringConst.LogoutConfirmMessage, StringConst.DialogAnswerPositive, StringConst.DialogAnswerNegative);

				//		if (result) {
				//			PreferenceUtility.ClearPreferences();
				//			Application.Current.MainPage = new LoginPage();
				//		}
				//		else {
				//			this.menuPage.Menu.SelectedItem = null;
				//			IsPresented = false;
				//		}
				//	}));

				//	return;
				//}

				//if(displayPage.GetType().Equals(typeof(ContentsUploadPage))) {
				//	Analytics.TrackEvent(LogUtility.EventClickedContentsUp,
				//		new Dictionary<string, string> { { LogUtility.ElementPageName, LogUtility.SourceMaster } });
				//}

				Detail = displayPage;
			//}

			this.menuPage.Menu.SelectedItem = null;
			IsPresented = false;
		}

		/// <summary>
		/// マスターページ表示切替えイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PresentedChanged(object sender, EventArgs e) {

			// iPadの場合のみ、メニューボタンを再描画する。
			//if (Device.RuntimePlatform == Device.iOS && Device.Idiom == TargetIdiom.Tablet) {
			//	DependencyService.Get<INavigationBarService>().ResetMenuButton();
			//}
		}
	}
}