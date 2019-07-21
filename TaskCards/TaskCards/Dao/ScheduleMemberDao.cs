using SQLite;
using TaskCards.Data;
using TaskCards.Entities;

namespace TaskCards.Dao {

	/// <summary>
	/// ScheduleMemberテーブルのDAO
	/// </summary>
	public class ScheduleMemberDao {

		/// <summary>
		/// 予定メンバーを登録する。
		/// </summary>
		/// <param name="entity">Entity</param>
		public void Insert(ScheduleMember entity) {
			var preferences = new Preferences();

			using (SQLiteConnection con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<ScheduleMember>();

					con.Insert(entity);
				});
			}
		}

		/// <summary>
		/// 予定メンバーを削除する。
		/// </summary>
		/// <param name="id">ID</param>
		public void Delete(int scheduleId, int memberId) {
			var preferences = new Preferences();

			using (SQLiteConnection con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {

				// 自動マイグレーション
				con.CreateTable<ScheduleMember>();

				ScheduleMember scheduleMember 
					= (from p in con.Table<ScheduleMember>()
					   where p.ScheduleId == scheduleId && p.MemberId == memberId
					   select p).FirstOrDefault();

				if (scheduleMember != null) con.Delete(scheduleMember);
			}
		}
	}
}