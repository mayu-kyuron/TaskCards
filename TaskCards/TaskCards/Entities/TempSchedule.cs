namespace TaskCards.Entities {

	/// <summary>
	/// 一時保存スケジュールテーブルEntity
	/// </summary>
	public class TempSchedule : Schedule {

		/// <summary>
		/// 本スケジュールID
		/// </summary>
		public long ScheduleId { set; get; }
	}
}