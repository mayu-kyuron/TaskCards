using SQLite;
using System.Collections.Generic;
using System.Linq;
using TaskCards.Data;
using TaskCards.Entities;

namespace TaskCards.Dao {

	/// <summary>
	/// TaskProgressテーブルのDAO
	/// </summary>
	public class TaskProgressDao {

		/// <summary>
		/// タスクメンバーIDからタスク進捗リストを取得する。
		/// </summary>
		/// <param name="taskMemberId">タスクメンバーID</param>
		/// <returns>タスク進捗リスト</returns>
		public List<TaskProgress> GetTaskProgressListByTaskMemberId(long taskMemberId) {
			var preferences = new Preferences();
			var entityList = new List<TaskProgress>();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				var query = new TableQuery<TaskProgress>(con);

				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<TaskProgress>();

					query = con.Table<TaskProgress>()
					.Where(v => v.TaskMemberId == taskMemberId);
				});

				foreach (TaskProgress entity in query) {
					entityList.Add(entity);
				}
			}

			return entityList;
		}

		/// <summary>
		/// タスクIDから登録順のタスク進捗リストを取得する。
		/// </summary>
		/// <param name="taskId">タスクID</param>
		/// <returns>登録順のタスク進捗リスト</returns>
		public List<TaskProgress> GetOrderedTaskProgressListByTaskId(long taskId) {

			var taskMemberDao = new TaskMemberDao();
			List<TaskMember> taskMemberList = taskMemberDao.GetTaskMemberListByTaskId(taskId);

			// 過去のタスク進捗をすべて取得
			var taskProgressList = new List<TaskProgress>();
			foreach (TaskMember taskMember in taskMemberList) {
				taskProgressList.AddRange(GetTaskProgressListByTaskMemberId(taskMember.Id));
			}

			return taskProgressList.OrderBy(e => e.RegisterOrder).ToList();
		}

		/// <summary>
		/// タスク進捗を登録する。
		/// </summary>
		/// <param name="entity">Entity</param>
		public void Insert(TaskProgress entity) {
			var preferences = new Preferences();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<TaskProgress>();

					con.Insert(entity);
				});
			}
		}

		/// <summary>
		/// タスク進捗を更新する。
		/// </summary>
		/// <param name="entity">Entity</param>
		/// <returns>キー</returns>
		public long Update(TaskProgress entity) {
			var preferences = new Preferences();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<TaskProgress>();

					con.Update(entity);
				});
			}

			return entity.Id;
		}

		/// <summary>
		/// タスク進捗を削除する。
		/// </summary>
		/// <param name="id">ID</param>
		public void Delete(long id) {
			var preferences = new Preferences();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {

				// 自動マイグレーション
				con.CreateTable<TaskProgress>();

				TaskProgress taskProgress = (from p in con.Table<TaskProgress>() where p.Id == id select p).FirstOrDefault();

				if (taskProgress != null) con.Delete(taskProgress);
			}
		}

		/// <summary>
		/// タスクメンバーIDから該当するタスク進捗データをすべて削除する。
		/// </summary>
		/// <param name="taskMemberId">タスクメンバーID</param>
		public void DeleteAllByTaskMemberId(long taskMemberId) {
			var preferences = new Preferences();

			List<TaskProgress> entityList = GetTaskProgressListByTaskMemberId(taskMemberId);

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				foreach (TaskProgress entity in entityList) {
					con.Delete(entity);
				}
			}
		}
	}
}