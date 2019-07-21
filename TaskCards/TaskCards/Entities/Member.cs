namespace TaskCards.Entities {

	/// <summary>
	/// MemberテーブルEntity
	/// </summary>
	public class Member {

		/// <summary>
		/// ID
		/// </summary>
		[SQLite.PrimaryKey, SQLite.AutoIncrement]
		public long Id { set; get; }

		/// <summary>
		/// パスワード
		/// </summary>
		public string Pw { set; get; }

		/// <summary>
		/// 名前
		/// </summary>
		public string Name { set; get; }

		/// <summary>
		/// 単価
		/// </summary>
		public int UnitCost { set; get; }

		/// <summary>
		/// プッシュ通知が必要か否か
		/// </summary>
		public bool IsNeededPush { set; get; }

		/// <summary>
		/// 予定通知が必要か否か
		/// </summary>
		public bool IsNeededSchedulePush { set; get; }

		/// <summary>
		/// 広告の有無
		/// </summary>
		public bool HasAdvertisement { set; get; }

		/// <summary>
		/// 充実機能の有無
		/// </summary>
		public bool HasMoreFunctions { set; get; }
	}
}