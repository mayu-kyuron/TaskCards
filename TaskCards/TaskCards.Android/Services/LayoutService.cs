using Android.App;
using TaskCards.Droid.Services;
using TaskCards.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(LayoutService))]
namespace TaskCards.Droid.Services {

	/// <summary>
	/// レイアウトサービス
	/// </summary>
	public class LayoutService : ILayoutService {

		public static Activity Activity { get; set; }

		/// <summary>
		/// ツールバーの高さを取得する。
		/// </summary>
		/// <returns>ツールバーの高さ（取得できなければ「-1」）</returns>
		public int GetToolBarHeight() {

			int statusBarHeight = -1;
			int resourceId = Activity.Resources.GetIdentifier("status_bar_height", "dimen", "android");

			if (resourceId > 0) {
				statusBarHeight = Activity.Resources.GetDimensionPixelSize(resourceId);
			}

			return statusBarHeight;
		}
	}
}