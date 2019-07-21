namespace TaskCards.Entities {

	/// <summary>
	/// 予定メンバーテーブルEntity
	/// </summary>
	public class ScheduleMember {

		/// <summary>
		/// 予定ID
		/// </summary>
		public long ScheduleId { set; get; }

		/// <summary>
		/// メンバーID
		/// </summary>
		public long MemberId { set; get; }

		/// <summary>
		/// 編集可否
		/// </summary>
		public bool CanEdit { set; get; }
	}
}