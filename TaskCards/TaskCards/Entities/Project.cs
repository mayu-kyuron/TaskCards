using System;
using TaskCards.Divisions;

namespace TaskCards.Entities {

	/// <summary>
	/// ProjectテーブルEntity
	/// </summary>
	public class Project {

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
		/// 予定開始日
		/// </summary>
		public DateTime ExpectedStartDate { set; get; }

		/// <summary>
		/// 予定終了日
		/// </summary>
		public DateTime ExpectedEndDate { set; get; }

		/// <summary>
		/// 開始日
		/// </summary>
		public DateTime StartDate { set; get; }

		/// <summary>
		/// 終了日
		/// </summary>
		public DateTime EndDate { set; get; }

		/// <summary>
		/// 予定時間
		/// </summary>
		public TimeSpan ExpectedTime { set; get; }

		/// <summary>
		/// 色区分
		/// </summary>
		public ColorDiv ColorDiv { set; get; }

		/// <summary>
		/// メモ
		/// </summary>
		public string Notes { set; get; }

		/// <summary>
		/// 予定売上
		/// </summary>
		public int ExpectedSales { set; get; }

		/// <summary>
		/// 実売上
		/// </summary>
		public int Sales { set; get; }
	}
}