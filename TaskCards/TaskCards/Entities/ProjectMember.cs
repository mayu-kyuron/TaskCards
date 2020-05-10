namespace TaskCards.Entities {

	/// <summary>
	/// プロジェクトメンバーテーブルEntity
	/// </summary>
	public class ProjectMember : BaseEventMember {

		/// <summary>
		/// プロジェクトID
		/// </summary>
		public long ProjectId { set; get; }
	}
}