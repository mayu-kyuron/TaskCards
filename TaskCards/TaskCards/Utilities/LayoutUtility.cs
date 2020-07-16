using System;
using TaskCards.Divisions;
using TaskCards.Entities;
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

			switch (colorDiv) {
				case ColorDiv.スカイブルー:
					return Color.SkyBlue;
				case ColorDiv.シーグリーン:
					return Color.MediumSeaGreen;
				case ColorDiv.コーラルレッド:
					return Color.Coral;
				case ColorDiv.プラムバイオレット:
					return Color.Plum;
				default:
					return Color.SkyBlue;
			}
		}

		/// <summary>
		/// 色区分から表示するカラーを薄めた色を取得する。
		/// </summary>
		/// <param name="colorDiv">色区分</param>
		/// <returns>カラーを薄めた色</returns>
		public static Color GetLightColorByColorDiv(ColorDiv colorDiv) {

			switch (colorDiv) {
				case ColorDiv.スカイブルー:
					return Color.FromHex("#B0DFF1");
				case ColorDiv.シーグリーン:
					return Color.FromHex("#57C587");
				case ColorDiv.コーラルレッド:
					return Color.FromHex("#FFA17F");
				case ColorDiv.プラムバイオレット:
					return Color.FromHex("#E9C3E9");
				default:
					return Color.FromHex("#B0DFF1");
			}
		}

		/// <summary>
		/// プロジェクトの終了日によって表示するカラーを取得する。
		/// </summary>
		/// <param name="project">プロジェクトデータ</param>
		/// <returns>カラー</returns>
		public static Color GetColorByEndDate(Project project) {

			if (project.EndDate.Equals(DateTime.MinValue)) {
				return GetColorByColorDiv(project.ColorDiv);
			}
			else {
				return Color.Silver;
			}
		}

		/// <summary>
		/// プロジェクトの終了日によって表示するカラーを薄めた色を取得する。
		/// </summary>
		/// <param name="project">プロジェクトデータ</param>
		/// <returns>カラーを薄めた色</returns>
		public static Color GetLightColorByEndDate(Project project) {

			if (project.EndDate.Equals(DateTime.MinValue)) {
				return GetLightColorByColorDiv(project.ColorDiv);
			}
			else {
				return Color.LightGray;
			}
		}

		/// <summary>
		/// 引数ありのTapGestureRecognizerを取得する。
		/// </summary>
		/// <param name="commandName">コマンド名</param>
		/// <param name="parameter">引数</param>
		/// <returns>TapGestureRecognizer</returns>
		public static TapGestureRecognizer GetTapGestureRecognizerWithParameter(string commandName, string parameter) {

			var tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, commandName);

			var binding = new Binding();
			binding.Source = parameter;
			tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandParameterProperty, binding);

			return tapGestureRecognizer;
		}
	}
}