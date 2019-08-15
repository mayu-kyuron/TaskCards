namespace TaskCards.Entities {

	/// <summary>
	/// タスクメンバーテーブルEntity
	/// </summary>
	public class TaskMember : BaseEventMember {

		/// <summary>
		/// タスクID
		/// </summary>
		public long TaskId { set; get; }
	}
}