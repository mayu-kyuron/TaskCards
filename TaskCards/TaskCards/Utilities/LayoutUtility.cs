using TaskCards.Divisions;
using Xamarin.Forms;

namespace TaskCards.Utilities {

	/// <summary>
	/// レイアウトユーティリティ
	/// </summary>
    public static class LayoutUtility {

		/// <summary>
		/// ヘッダとタブを除いたビュー全体の高さ（double型）を取得する。
		/// </summary>
		/// <param name="screenHeight">画面の高さ</param>
		/// <returns>ビュー全体の高さ</returns>
		public static double GetHeightDoubleWithoutHeaderTab(double screenHeight) {
			double height;

			switch (Device.RuntimePlatform) {
				case Device.iOS:
					height = screenHeight * 0.75;
					break;
				default:
					height = screenHeight * 0.8;
					break;
			}

			return height;
		}

		/// <summary>
		/// 色区分から表示するカラーを取得する。
		/// </summary>
		/// <param name="colorDiv">色区分</param>
		/// <returns>カラー</returns>
		public static Color GetColorByColorDiv(ColorDiv colorDiv) {

			// TODO 色区分が増えるごとに分岐を追加する。
			switch (colorDiv) {
				case ColorDiv.赤:
					return Color.Red;
				default:
					return Color.White;
			}
		}
	}
}