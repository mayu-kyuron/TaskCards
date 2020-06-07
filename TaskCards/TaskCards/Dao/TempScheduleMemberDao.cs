using System.Collections.Generic;
using SQLite;
using TaskCards.Data;
using TaskCards.Entities;

namespace TaskCards.Dao {

	/// <summary>
	/// TempScheduleMemberテーブルのDAO
	/// </summary>
	public class TempScheduleMemberDao {

		/// <summary>
		/// 一時保存スケジュールIDから一時保存スケジュールメンバーリストを取得する。
		/// </summary>
		/// <param name="tempScheduleId">一時保存スケジュールID</param>
		/// <returns>一時保存スケジュールメンバーリスト</returns>
		public List<TempScheduleMember> GetTempScheduleMemberListByTempScheduleId(long tempScheduleId) {
			var preferences = new Preferences();
			var entityList = new List<TempScheduleMember>();

			using (SQLiteConnection con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				var query = new TableQuery<TempScheduleMember>(con);

				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<TempScheduleMember>();

					query = con.Table<TempScheduleMember>()
					.Where(v => v.ScheduleId == tempScheduleId);
				});

				foreach (TempScheduleMember entity in query) {
					entityList.Add(entity);
				}
			}

			return entityList;
		}

		/// <summary>
		/// 一時保存予定メンバーを登録する。
		/// </summary>
		/// <param name="entity">Entity</param>
		public void Insert(TempScheduleMember entity) {
			var preferences = new Preferences();

			using (SQLiteConnection con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<TempScheduleMember>();

					con.Insert(entity);
				});
			}
		}

		/// <summary>
		/// 一時保存予定メンバーを削除する。
		/// </summary>
		/// <param name="tempScheduleId">一時保存予定ID</param>
		/// <param name="tempMemberId">一時保存メンバーID</param>
		public void Delete(long tempScheduleId, long tempMemberId) {
			var preferences = new Preferences();

			using (SQLiteConnection con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {

				// 自動マイグレーション
				con.CreateTable<TempScheduleMember>();

				TempScheduleMember tempScheduleMember
					= (from p in con.Table<TempScheduleMember>()
					   where p.ScheduleId == tempScheduleId && p.MemberId == tempMemberId
					   select p).FirstOrDefault();

				if (tempScheduleMember != null) con.Delete(tempScheduleMember);
			}
		}

		/// <summary>
		/// 一時保存スケジュールIDから該当する一時保存予定メンバーデータをすべて削除する。
		/// </summary>
		/// <param name="tempScheduleId">一時保存スケジュールID</param>
		public void DeleteAllByTempScheduleId(long tempScheduleId) {
			var preferences = new Preferences();

			List<TempScheduleMember> entityList = GetTempScheduleMemberListByTempScheduleId(tempScheduleId);

			using (SQLiteConnection con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				foreach (TempScheduleMember entity in entityList) {
					con.Delete(entity);
				}
			}
		}

		/// <summary>
		/// 一時保存予定メンバーを全て削除する。
		/// </summary>
		public void DeleteAll() {
			var preferences = new Preferences();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {

				// 自動マイグレーション
				con.CreateTable<TempScheduleMember>();

				con.DeleteAll<TempScheduleMember>();
			}
		}
	}
}