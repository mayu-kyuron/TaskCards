using System;
using TaskCards.Services;
using Xamarin.Forms;
//using TaskCards.Divisions;
//using TaskCards.Utilities;

namespace TaskCards.MasterPages {

	/// <summary>
	/// スライド（ハンバーガー）メニューページ
	/// </summary>
	public class SlideMenuPage : ContentPage {

		public ListView Menu { get; set; }

		public SlideMenuPage () {

			string versionNo = DependencyService.Get<IAssemblyService>().GetAppVersionNo();

			this.Icon = "btn_option";
			this.Title = "メニュー";

			// iPhoneにおいて、ステータスバーとの重なりを防ぐためパディングを調整する.
			//Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);

			this.Menu = new SlideMenuListView();

			var layout = new StackLayout {
				Spacing = 0,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			layout.Children.Add(this.Menu);
			layout.Children.Add(new Label { Text = String.Format("ver: {0}", versionNo) });

			Content = layout;

			SizeChanged += OnSizeChanged;
		}

		/// <summary>
		/// 画面サイズ変更イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void OnSizeChanged(object sender, EventArgs args) {

			// プラットフォームごとにPaddingの設定
			Padding = GetLayoutPadding();
		}

		/// <summary>
		/// ヘッダの高さ分のPaddingを取得する。
		/// </summary>
		/// <returns>プラットフォームごとのPadding</returns>
		private Thickness GetLayoutPadding() {
			double paddingTop;

			switch (Device.RuntimePlatform) {
				case Device.iOS:
					paddingTop = Height * 0.12;
					break;
				default:
					paddingTop = Height * 0.08;
					break;
			}

			return new Thickness(0, paddingTop, 0, 0);
		}
	}
}