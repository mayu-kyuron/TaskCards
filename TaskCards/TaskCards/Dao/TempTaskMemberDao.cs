using System.Collections.Generic;
using SQLite;
using TaskCards.Data;
using TaskCards.Entities;

namespace TaskCards.Dao {

	/// <summary>
	/// TempTaskMemberテーブルのDAO
	/// </summary>
	public class TempTaskMemberDao {

		/// <summary>
		/// 一時保存タスクIDから一時保存タスクメンバーリストを取得する。
		/// </summary>
		/// <param name="tempTaskId">一時保存タスクID</param>
		/// <returns>一時保存タスクメンバーリスト</returns>
		public List<TempTaskMember> GetTempTaskMemberListByTempTaskId(long tempTaskId) {
			var preferences = new Preferences();
			var entityList = new List<TempTaskMember>();

			using (SQLiteConnection con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				var query = new TableQuery<TempTaskMember>(con);

				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<TempTaskMember>();

					query = con.Table<TempTaskMember>()
					.Where(v => v.TaskId == tempTaskId);
				});

				foreach (TempTaskMember entity in query) {
					entityList.Add(entity);
				}
			}

			return entityList;
		}

		/// <summary>
		/// 一時保存タスクメンバーを登録する。
		/// </summary>
		/// <param name="entity">Entity</param>
		public void Insert(TempTaskMember entity) {
			var preferences = new Preferences();

			using (SQLiteConnection con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<TempTaskMember>();

					con.Insert(entity);
				});
			}
		}

		/// <summary>
		/// 一時保存タスクメンバーを削除する。
		/// </summary>
		/// <param name="tempTaskId">一時保存タスクID</param>
		/// <param name="tempMemberId">一時保存メンバーID</param>
		public void Delete(long tempTaskId, long tempMemberId) {
			var preferences = new Preferences();

			using (SQLiteConnection con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {

				// 自動マイグレーション
				con.CreateTable<TempTaskMember>();

				TempTaskMember tempTaskMember
					= (from p in con.Table<TempTaskMember>()
					   where p.TaskId == tempTaskId && p.MemberId == tempMemberId
					   select p).FirstOrDefault();

				if (tempTaskMember != null) con.Delete(tempTaskMember);
			}
		}

		/// <summary>
		/// 一時保存タスクIDから該当する一時保存タスクメンバーデータをすべて削除する。
		/// </summary>
		/// <param name="tempTaskId">一時保存タスクID</param>
		public void DeleteAllByTempTaskId(long tempTaskId) {
			var preferences = new Preferences();

			List<TempTaskMember> entityList = GetTempTaskMemberListByTempTaskId(tempTaskId);

			using (SQLiteConnection con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				foreach (TempTaskMember entity in entityList) {
					con.Delete(entity);
				}
			}
		}

		/// <summary>
		/// 一時保存タスクメンバーを全て削除する。
		/// </summary>
		public void DeleteAll() {
			var preferences = new Preferences();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {

				// 自動マイグレーション
				con.CreateTable<TempTaskMember>();

				con.DeleteAll<TempTaskMember>();
			}
		}
	}
}