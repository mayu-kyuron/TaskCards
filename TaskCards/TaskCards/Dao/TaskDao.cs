using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskCards.Data;
using TaskCards.Entities;

namespace TaskCards.Dao {

	/// <summary>
	/// TaskテーブルのDAO
	/// </summary>
	public class TaskDao {

		/// <summary>
		/// タスクIDよりタスクデータを取得する。
		/// </summary>
		/// <param name="id">タスクID</param>
		/// <returns>タスクデータ</returns>
		public Task GetTaskById(long id) {
			var preferences = new Preferences();
			var entity = new Task();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				var query = new TableQuery<Task>(con);

				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<Task>();

					query = con.Table<Task>()
					.Where(v => v.Id == id);
				});

				entity = query.FirstOrDefault();
			}

			return entity;
		}

		/// <summary>
		/// １日のタスクリストを取得する。
		/// </summary>
		/// <param name="date">日付</param>
		/// <returns>タスクリスト</returns>
		public List<Task> GetDayTaskList(DateTime date) {
			var preferences = new Preferences();
			var entityList = new List<Task>();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {

				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<Task>();

					// DateTime型のDateは、LinqのSQLでは使えないので、一度List化してからWhereで検索する。
					entityList = con.Table<Task>().ToList()
					.Where(v => v.StartDate.Date <= date.Date && v.EndDate.Date >= date.Date)
					.OrderBy(v => v.StartDate)
					.ToList();
				});
			}

			return entityList;
		}

		/// <summary>
		/// 該当月の日付から１か月分のタスクマップを取得する。
		/// </summary>
		/// <param name="anyDateOfMonth">該当月の日付</param>
		/// <returns>タスクリストマップ（キー：開始日、値：タスクリスト）</returns>
		public Dictionary<int, List<Task>> GetMonthTaskMap(DateTime anyDateOfMonth) {
			var preferences = new Preferences();
			var entityMap = new Dictionary<int, List<Task>>();

			DateTime startDateOfMonth = new DateTime(anyDateOfMonth.Year, anyDateOfMonth.Month, 1);
			DateTime startDateOfNextMonth = startDateOfMonth.AddMonths(1);

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				var query = new TableQuery<Task>(con);

				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<Task>();

					query = con.Table<Task>()
					.Where(v => v.EndDate >= startDateOfMonth && v.StartDate < startDateOfNextMonth)
					.OrderBy(v => v.StartDate);
				});

				// 日付をキーに、同日のタスクリストをマップに格納する。
				for (int i = 1; i <= 31; i++) {

					if (i > startDateOfNextMonth.AddDays(-1).Day) break;

					DateTime dateTime = new DateTime(anyDateOfMonth.Year, anyDateOfMonth.Month, i);

					List<Task> taskList = query.ToList()
						.Where(v => v.StartDate <= dateTime && v.EndDate >= dateTime)
						.ToList();

					if (taskList.Count == 0) continue; 

					entityMap.Add(i, taskList);
				}
			}

			return entityMap;
		}

		/// <summary>
		/// プロジェクトIDからタスクリストを取得する。
		/// </summary>
		/// <param name="projectId">プロジェクトID</param>
		/// <returns>タスクリスト</returns>
		public List<Task> GetTaskListByProjectId(long projectId) {

			var preferences = new Preferences();
			var entityList = new List<Task>();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				var query = new TableQuery<Task>(con);

				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<Task>();

					query = con.Table<Task>()
					.Where(v => v.ProjectId == projectId)
					.OrderBy(v => v.StartDate);
				});

				foreach (Task entity in query) {
					entityList.Add(entity);
				}
			}

			return entityList;
		}

		/// <summary>
		/// タスクを登録する。
		/// </summary>
		/// <param name="entity">Entity</param>
		/// <returns>キー</returns>
		public long Insert(Task entity) {
			var preferences = new Preferences();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<Task>();

					con.Insert(entity);
				});
			}

			return entity.Id;
		}

		/// <summary>
		/// タスクを更新する。
		/// </summary>
		/// <param name="entity">Entity</param>
		/// <returns>キー</returns>
		public long Update(Task entity) {
			var preferences = new Preferences();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<Task>();

					con.Update(entity);
				});
			}

			return entity.Id;
		}

		/// <summary>
		/// タスクを削除する。
		/// </summary>
		/// <param name="id">ID</param>
		public void Delete(long id) {
			var preferences = new Preferences();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {

				// 自動マイグレーション
				con.CreateTable<Task>();

				Task task = (from p in con.Table<Task>() where p.Id == id select p).FirstOrDefault();

				if (task != null) con.Delete(task);
			}
		}
	}
}