namespace TaskCards.Entities {

	/// <summary>
	///	基底のイベントテーブルEntity
	/// </summary>
	public abstract class BaseEventMember {

		/// <summary>
		/// ID
		/// </summary>
		[SQLite.PrimaryKey, SQLite.AutoIncrement]
		public long Id { set; get; }

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