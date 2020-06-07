namespace TaskCards.Entities {

	/// <summary>
	/// 一時保存タスクテーブルEntity
	/// </summary>
	public class TempTask : Task {

		/// <summary>
		/// 本タスクID
		/// </summary>
		public long TaskId { set; get; }
	}
}