using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using Android.Telephony;
using Xamarin.Forms;
using TaskCards.Droid.Services;
using TaskCards.Services;

[assembly: Dependency(typeof(AssemblyService))]
namespace TaskCards.Droid.Services {

	/// <summary>
	/// アプリ・デバイス情報取得サービスクラス
	/// </summary>
	public class AssemblyService : IAssemblyService {

		/// <summary>
		/// バージョン番号を取得する。
		/// </summary>
		/// <returns>バージョン番号</returns>
		public string GetAppVersionNo() {
			Context context = Forms.Context;
			var versionNo = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
			return versionNo;
		}

		/// <summary>
		/// バージョンコードを取得する。
		/// </summary>
		/// <returns>バージョン番号</returns>
		public int GetAppVersionCode() {
			Context context = Forms.Context;
			var versionCode = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionCode;
			return versionCode;
		}

		/// <summary>
		/// OSバージョン番号を取得する。
		/// </summary>
		/// <returns>OSバージョン番号</returns>
		public string GetOSVersionNo() {
			return Build.VERSION.Sdk + ' ' + Build.VERSION.SdkInt.ToString();
		}

		/// <summary>
		/// OSバージョンが指定した数値より大きいかどうかを判定する。
		/// </summary>
		/// <param name="major">指定数値</param>
		/// <param name="minor"></param>
		/// <returns>大きければ「true」、小さければ「false」</returns>
		public bool IsUpperOSVersion(int major, int minor = 0) {
			if ((int)(Build.VERSION.SdkInt) >= major) return true;
			return false;
		}

		/// <summary>
		/// SDKバージョン番号を取得する。
		/// </summary>
		/// <returns>SDKバージョン番号</returns>
		public string GetSDKVersionNo() {
			return Build.VERSION.Sdk;
		}

		/// <summary>
		/// 内部ストレージパスを取得する。
		/// </summary>
		/// <returns>内部ストレージパス</returns>
		public string GetInternalStoragePath() {
			return Environment.ExternalStorageDirectory.Path;
		}

		/// <summary>
		/// 端末識別情報を取得する。
		/// </summary>
		/// <returns>端末識別情報</returns>
		public string GetTerminalIdentifier() {
			return Settings.Secure.GetString(Forms.Context.ContentResolver, Settings.Secure.AndroidId);
		}

		/// <summary>
		/// モデル番号を取得する。
		/// </summary>
		/// <returns>モデル番号</returns>
		public string GetModelNumber() {
			return Build.Model;
		}

		/// <summary>
		/// 通信キャリアを取得する。
		/// </summary>
		/// <returns>通信キャリア</returns>
		public string GetTelecomCarrier() {
			TelephonyManager manager = (TelephonyManager)Forms.Context.GetSystemService(Context.TelephonyService);
			return manager.NetworkOperatorName;
		}

		/// <summary>
		/// Android System Webview アプリバージョンが指定数値以上かどうかを判定する。
		/// </summary>
		/// <param name="versionNo">指定数値</param>
		/// <returns>以上なら「true」、未満またはアプリ自体なければ「false」</returns>
		public bool IsUpperAndroidSystemWebviewVersion(int versionNo) {
			PackageInfo webviewPackageInfo = null;
			var installedPackages = Forms.Context.PackageManager.GetInstalledPackages(0);

			foreach (PackageInfo packageInfo in installedPackages) {

				if(packageInfo.PackageName == "com.google.android.webview") {
					webviewPackageInfo = Forms.Context.PackageManager.GetPackageInfo("com.google.android.webview", 0);
				}
				else if (packageInfo.PackageName == "com.android.webview") {
					webviewPackageInfo = Forms.Context.PackageManager.GetPackageInfo("com.android.webview", 0);
				}
			}

			if (webviewPackageInfo == null) return false;

			int thisVersionNo = int.Parse(webviewPackageInfo.VersionName.Substring(0, webviewPackageInfo.VersionName.IndexOf(".")));

			if (thisVersionNo < versionNo) return false;

			return true;
		}

		//public void MediaScan() {
		//	Intent intent = new Intent();
		//	intent.SetAction(Intent.ActionMediaScannerScanFile);
		//	Forms.Forms.Context.SendBroadcast(intent);
		//}
	}
}