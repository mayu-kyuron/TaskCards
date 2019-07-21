using System;

namespace TaskCards.Services {

	/// <summary>
	/// ダイアログインタフェース
	/// </summary>
	public interface IDialogService {

		/// <summary>
		/// 日付タップ時のダイアログを表示する。
		/// </summary>
		/// <param name="selectedDate">選択日付</param>
		void ShowDateTappedDialog(DateTime selectedDate);
    }
}