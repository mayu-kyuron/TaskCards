using SQLite;
using System.Collections.Generic;
using TaskCards.Data;
using TaskCards.Entities;

namespace TaskCards.Dao {

	/// <summary>
	/// ScheduleMemberテーブルのDAO
	/// </summary>
	public class ScheduleMemberDao {

		/// <summary>
		/// スケジュールIDからスケジュールメンバーリストを取得する。
		/// </summary>
		/// <param name="scheduleId">スケジュールID</param>
		/// <returns>スケジュールメンバーリスト</returns>
		public List<ScheduleMember> GetScheduleMemberListByScheduleId(long scheduleId) {
			var preferences = new Preferences();
			var entityList = new List<ScheduleMember>();

			using (SQLiteConnection con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				var query = new TableQuery<ScheduleMember>(con);

				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<ScheduleMember>();

					query = con.Table<ScheduleMember>()
					.Where(v => v.ScheduleId == scheduleId);
				});

				foreach (ScheduleMember entity in query) {
					entityList.Add(entity);
				}
			}

			return entityList;
		}

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
		/// <param name="scheduleId">予定ID</param>
		/// <param name="memberId">メンバーID</param>
		public void Delete(long scheduleId, long memberId) {
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

		/// <summary>
		/// スケジュールIDから該当する予定メンバーデータをすべて削除する。
		/// </summary>
		/// <param name="scheduleId">スケジュールID</param>
		public void DeleteAllByScheduleId(long scheduleId) {
			var preferences = new Preferences();

			List<ScheduleMember> entityList = GetScheduleMemberListByScheduleId(scheduleId);

			using (SQLiteConnection con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				foreach (ScheduleMember entity in entityList) {
					con.Delete(entity);
				}
			}
		}
	}
}