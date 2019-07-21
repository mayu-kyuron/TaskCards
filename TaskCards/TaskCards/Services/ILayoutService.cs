namespace TaskCards.Services {

	/// <summary>
	/// レイアウトインタフェース
	/// </summary>
	public interface ILayoutService {

		/// <summary>
		/// ツールバーの高さを取得する。
		/// </summary>
		/// <returns>ツールバーの高さ（取得できなければ「-1」）</returns>
		int GetToolBarHeight();
    }
}