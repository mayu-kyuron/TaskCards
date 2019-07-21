using CoreTelephony;
using Foundation;
using UIKit;
using Xamarin.Forms;
using TaskCards.iOS.Services;
using TaskCards.Services;

[assembly: Dependency(typeof(AssemblyService))]
namespace TaskCards.iOS.Services {

	/// <summary>
	/// アプリ・デバイス情報取得サービスクラス
	/// </summary>
	public class AssemblyService : IAssemblyService {

		/// <summary>
		/// アプリバージョン番号を取得する。
		/// </summary>
		/// <returns>アプリバージョン番号</returns>
		public string GetAppVersionNo() {
			string versionNo = NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();
			return versionNo.ToString();
		}

		/// <summary>
		/// アプリバージョンコードを取得する。
		/// </summary>
		/// <returns>アプリバージョンコード</returns>
		public int GetAppVersionCode() {
			var versionCode = NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString();
			return int.Parse(versionCode);
		}

		/// <summary>
		/// OSバージョン番号を取得する。
		/// </summary>
		/// <returns>OSバージョン番号</returns>
		public string GetOSVersionNo() {
			return UIDevice.CurrentDevice.SystemVersion;
		}

		/// <summary>
		/// OSバージョンが指定した数値より大きいかどうかを判定する。
		/// </summary>
		/// <param name="major">指定数値</param>
		/// <param name="minor"></param>
		/// <returns>大きければ「true」、小さければ「false」</returns>
		public bool IsUpperOSVersion(int major, int minor = 0) {
			return UIDevice.CurrentDevice.CheckSystemVersion(major, minor);
		}

		/// <summary>
		/// SDKバージョン番号を取得する。
		/// </summary>
		/// <returns>SDKバージョン番号</returns>
		public string GetSDKVersionNo() {
			return UIDevice.CurrentDevice.SystemVersion;
		}

		/// <summary>
		/// 内部ストレージパスを取得する。
		/// </summary>
		/// <returns>内部ストレージパス</returns>
		public string GetInternalStoragePath() {
			// TODO ストレージパスを返す。
			return null;
		}

		/// <summary>
		/// 端末識別情報を取得する。
		/// </summary>
		/// <returns>端末識別情報</returns>
		public string GetTerminalIdentifier() {
			return UIDevice.CurrentDevice.IdentifierForVendor.AsString();
		}

		/// <summary>
		/// モデル番号を取得する。
		/// </summary>
		/// <returns>モデル番号</returns>
		public string GetModelNumber() {
			return UIDevice.CurrentDevice.LocalizedModel;
		}

		/// <summary>
		/// 通信キャリアを取得する。
		/// </summary>
		/// <returns>通信キャリア</returns>
		public string GetTelecomCarrier() {
			using (var info = new CTTelephonyNetworkInfo()) {
				return info.SubscriberCellularProvider.CarrierName;
			}
		}

		/// <summary>
		/// Android System Webview アプリバージョンが指定数値以上かどうかを判定する。
		/// </summary>
		/// <param name="versionNo">指定数値</param>
		/// <returns>以上なら「true」、未満またはアプリ自体なければ「false」</returns>
		public bool IsUpperAndroidSystemWebviewVersion(int versionNo) {
			return false;
		}
	}
}