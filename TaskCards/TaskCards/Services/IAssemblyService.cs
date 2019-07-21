namespace TaskCards.Services {

	/// <summary>
	/// アプリ・デバイス情報取得インターフェース
	/// </summary>
    public interface IAssemblyService {

		/// <summary>
		/// アプリバージョン番号を取得する。
		/// </summary>
		/// <returns>アプリバージョン番号</returns>
		string GetAppVersionNo();

		/// <summary>
		/// アプリバージョンコードを取得する。
		/// </summary>
		/// <returns>アプリバージョンコード</returns>
		int GetAppVersionCode();

		/// <summary>
		/// OSバージョン番号を取得する。
		/// </summary>
		/// <returns>OSバージョン番号</returns>
		string GetOSVersionNo();

		/// <summary>
		/// OSバージョンが指定数値以上かどうかを判定する。
		/// </summary>
		/// <param name="major">指定数値</param>
		/// <param name="minor"></param>
		/// <returns>以上なら「true」、未満なら「false」</returns>
		bool IsUpperOSVersion(int major, int minor = 0);

		/// <summary>
		/// SDKバージョン番号を取得する。
		/// </summary>
		/// <returns>SDKバージョン番号</returns>
		string GetSDKVersionNo();

		/// <summary>
		/// 内部ストレージパスを取得する。
		/// </summary>
		/// <returns>内部ストレージパス</returns>
		string GetInternalStoragePath();

		/// <summary>
		/// 端末識別情報を取得する。
		/// </summary>
		/// <returns>端末識別情報</returns>
		string GetTerminalIdentifier();

		/// <summary>
		/// モデル番号を取得する。
		/// </summary>
		/// <returns>モデル番号</returns>
		string GetModelNumber();

		/// <summary>
		/// 通信キャリアを取得する。
		/// </summary>
		/// <returns>通信キャリア</returns>
		string GetTelecomCarrier();

		/// <summary>
		/// Android System Webview アプリバージョンが指定数値以上かどうかを判定する。
		/// </summary>
		/// <param name="versionNo">指定数値</param>
		/// <returns>以上なら「true」、未満またはアプリ自体なければ「false」</returns>
		bool IsUpperAndroidSystemWebviewVersion(int versionNo);

		//void MediaScan();
	}
}