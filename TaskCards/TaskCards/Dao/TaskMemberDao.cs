using SQLite;
using System.Collections.Generic;
using TaskCards.Data;
using TaskCards.Entities;

namespace TaskCards.Dao {

	/// <summary>
	/// TaskMemberテーブルのDAO
	/// </summary>
	public class TaskMemberDao {

		/// <summary>
		/// タスクメンバーIDよりタスクメンバーデータを取得する。
		/// </summary>
		/// <param name="id">タスクメンバーID</param>
		/// <returns>タスクメンバーデータ</returns>
		public TaskMember GetTaskMemberById(long id) {
			var preferences = new Preferences();
			var entity = new TaskMember();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				var query = new TableQuery<TaskMember>(con);

				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<TaskMember>();

					query = con.Table<TaskMember>()
					.Where(v => v.Id == id);
				});

				entity = query.FirstOrDefault();
			}

			return entity;
		}

		/// <summary>
		/// タスクIDからタスクメンバーリストを取得する。
		/// </summary>
		/// <param name="taskId">タスクID</param>
		/// <returns>タスクメンバーリスト</returns>
		public List<TaskMember> GetTaskMemberListByTaskId(long taskId) {
			var preferences = new Preferences();
			var entityList = new List<TaskMember>();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				var query = new TableQuery<TaskMember>(con);

				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<TaskMember>();

					query = con.Table<TaskMember>()
					.Where(v => v.TaskId == taskId);
				});

				foreach (TaskMember entity in query) {
					entityList.Add(entity);
				}
			}

			return entityList;
		}

		/// <summary>
		/// タスクメンバーを登録する。
		/// </summary>
		/// <param name="entity">Entity</param>
		public void Insert(TaskMember entity) {
			var preferences = new Preferences();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<TaskMember>();

					con.Insert(entity);
				});
			}
		}

		/// <summary>
		/// タスクメンバーを削除する。
		/// </summary>
		/// <param name="taskId">タスクID</param>
		/// <param name="memberId">メンバーID</param>
		public void Delete(long taskId, long memberId) {
			var preferences = new Preferences();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {

				// 自動マイグレーション
				con.CreateTable<TaskMember>();

				TaskMember taskMember
					= (from p in con.Table<TaskMember>()
					   where p.TaskId == taskId && p.MemberId == memberId
					   select p).FirstOrDefault();

				if (taskMember != null) con.Delete(taskMember);
			}
		}

		/// <summary>
		/// タスクIDから該当するタスクメンバーデータをすべて削除する。
		/// </summary>
		/// <param name="taskId">タスクID</param>
		public void DeleteAllByTaskId(long taskId) {
			var preferences = new Preferences();

			List<TaskMember> entityList = GetTaskMemberListByTaskId(taskId);

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				foreach (TaskMember entity in entityList) {
					con.Delete(entity);
				}
			}
		}
	}
}