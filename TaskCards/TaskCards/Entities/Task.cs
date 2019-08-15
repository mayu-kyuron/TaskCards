using System;

namespace TaskCards.Entities {

	/// <summary>
	/// タスクテーブルEntity
	/// </summary>
	public class Task : BaseEvent {

		/// <summary>
		/// 予定毎日作業時間
		/// </summary>
		public TimeSpan ExpectedDailyWorkTime { set; get; }

		/// <summary>
		/// 総作業時間
		/// </summary>
		public TimeSpan TotalWorkTime { set; get; }

		/// <summary>
		/// 進捗率
		/// </summary>
		public double ProgressRate { set; get; }
	}
}