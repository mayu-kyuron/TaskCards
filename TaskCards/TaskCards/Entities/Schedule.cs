using TaskCards.Divisions;

namespace TaskCards.Entities {

	/// <summary>
	/// 予定テーブルEntity
	/// </summary>
	public class Schedule : BaseEvent {

		/// <summary>
		/// 終日か否か
		/// </summary>
		public bool isAllDay { set; get; }

		/// <summary>
		/// 繰り返し区分
		/// </summary>
		public RepeatDiv RepeatDiv { set; get; }

		/// <summary>
		/// 場所
		/// </summary>
		public string Place { set; get; }
	}
}