using System;
using TaskCards.Divisions;

namespace TaskCards.Entities {

	/// <summary>
	/// 予定テーブルEntity
	/// </summary>
	public class Schedule {

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
		/// 終日か否か
		/// </summary>
		public bool isAllDay { set; get; }

		/// <summary>
		/// プロジェクトID
		/// </summary>
		public long ProjectId { set; get; }

		/// <summary>
		/// 繰り返し区分
		/// </summary>
		public RepeatDiv RepeatDiv { set; get; }

		/// <summary>
		/// 場所
		/// </summary>
		public string Place { set; get; }

		/// <summary>
		/// メモ
		/// </summary>
		public string Notes { set; get; }
	}
}