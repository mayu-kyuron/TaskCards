using System;

namespace TaskCards.Entities {

	/// <summary>
	/// タスク進捗テーブルEntity
	/// </summary>
	public class TaskProgress {

		/// <summary>
		/// ID
		/// </summary>
		[SQLite.PrimaryKey, SQLite.AutoIncrement]
		public long Id { set; get; }

		/// <summary>
		/// タスクメンバーID
		/// </summary>
		public long TaskMemberId { set; get; }

		/// <summary>
		/// 登録順
		/// </summary>
		public int RegisterOrder { set; get; }

		/// <summary>
		/// 進捗率
		/// </summary>
		public double ProgressRate { set; get; }

		/// <summary>
		/// 開始日
		/// </summary>
		public DateTime StartDate { set; get; }

		/// <summary>
		/// 終了日
		/// </summary>
		public DateTime EndDate { set; get; }
	}
}