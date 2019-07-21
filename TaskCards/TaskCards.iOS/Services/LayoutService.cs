using TaskCards.iOS.Services;
using TaskCards.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(LayoutService))]
namespace TaskCards.iOS.Services {

	/// <summary>
	/// レイアウトサービス
	/// </summary>
	public class LayoutService : ILayoutService {

		/// <summary>
		/// ツールバーの高さを取得する。
		/// </summary>
		/// <returns>ツールバーの高さ（取得できなければ「-1」）</returns>
		public int GetToolBarHeight() {
			return (int)UIApplication.SharedApplication.StatusBarFrame.Height;
		}
	}
}