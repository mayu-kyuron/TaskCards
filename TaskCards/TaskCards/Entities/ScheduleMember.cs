namespace TaskCards.Entities {

	/// <summary>
	/// 予定メンバーテーブルEntity
	/// </summary>
	public class ScheduleMember : BaseEventMember {

		/// <summary>
		/// 予定ID
		/// </summary>
		public long ScheduleId { set; get; }
	}
}