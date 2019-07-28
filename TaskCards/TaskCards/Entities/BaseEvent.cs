using System;

namespace TaskCards.Entities {

	/// <summary>
	///	基底のイベントテーブルEntity
	/// </summary>
	public abstract class BaseEvent {

		/// <summary>
		/// ID
		/// </summary>
		[SQLite.PrimaryKey, SQLite.AutoIncrement]
		public long Id { set; get; }

		/// <summary>
		/// タイトル
		/// </summary>
		public string Title { set; get; }

		/// <summary>
		/// 開始日
		/// </summary>
		public DateTime StartDate { set; get; }

		/// <summary>
		/// 終了日
		/// </summary>
		public DateTime EndDate { set; get; }

		/// <summary>
		/// プロジェクトID
		/// </summary>
		public long ProjectId { set; get; }

		/// <summary>
		/// メモ
		/// </summary>
		public string Notes { set; get; }
	}
}