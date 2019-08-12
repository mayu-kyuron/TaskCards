using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskCards.Data;
using TaskCards.Entities;

namespace TaskCards.Dao {

	/// <summary>
	/// ScheduleテーブルのDAO
	/// </summary>
	public class ScheduleDao {

		/// <summary>
		/// スケジュールIDよりスケジュールデータを取得する。
		/// </summary>
		/// <param name="id">スケジュールID</param>
		/// <returns>スケジュールデータ</returns>
		public Schedule GetScheduleById(long id) {
			var preferences = new Preferences();
			var entity = new Schedule();

			using (SQLiteConnection con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				TableQuery<Schedule> query = new TableQuery<Schedule>(con);

				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<Schedule>();

					query = con.Table<Schedule>()
					.Where(v => v.Id == id);
				});

				entity = query.FirstOrDefault();
			}

			return entity;
		}

		/// <summary>
		/// １日のスケジュールリストを取得する。
		/// </summary>
		/// <param name="date">日付</param>
		/// <returns>スケジュールリスト</returns>
		public List<Schedule> GetDayScheduleList(DateTime date) {
			var preferences = new Preferences();
			var entityList = new List<Schedule>();

			using (SQLiteConnection con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {

				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<Schedule>();

					// DateTime型のDateは、LinqのSQLでは使えないので、一度List化してからWhereで検索する。
					entityList = con.Table<Schedule>().ToList()
					.Where(v => v.StartDate.Date == date.Date)
					.OrderBy(v => v.StartDate)
					.ToList();
				});
			}

			return entityList;
		}

		/// <summary>
		/// 該当月の日付から１か月分のスケジュールマップを取得する。
		/// </summary>
		/// <param name="anyDateOfMonth">該当月の日付</param>
		/// <returns>スケジュールリストマップ（キー：開始日、値：スケジュールリスト）</returns>
		public Dictionary<int, List<Schedule>> GetMonthScheduleMap(DateTime anyDateOfMonth) {
			var preferences = new Preferences();
			var entityMap = new Dictionary<int, List<Schedule>>();

			DateTime startDateOfMonth = new DateTime(anyDateOfMonth.Year, anyDateOfMonth.Month, 1);
			DateTime endDateOfMonth = (new DateTime(anyDateOfMonth.Year, anyDateOfMonth.AddMonths(1).Month, 1)).AddDays(-1);

			using (SQLiteConnection con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				var query = new TableQuery<Schedule>(con);

				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<Schedule>();

					query = con.Table<Schedule>()
					.Where(v => v.StartDate >= startDateOfMonth && v.StartDate <= endDateOfMonth)
					.OrderBy(v => v.StartDate);
				});

				// 開始日をキーに、同日のスケジュールリストをマップに格納する。
				int? lastStartDay = null;
				List<Schedule> scheduleList = new List<Schedule>();
				foreach (Schedule entity in query) {

					if (lastStartDay != null && lastStartDay != entity.StartDate.Day) {
						entityMap.Add((int)lastStartDay, scheduleList);
						scheduleList = new List<Schedule>();
					}

					scheduleList.Add(entity);
					lastStartDay = entity.StartDate.Day;
				}

				if (lastStartDay != null) entityMap.Add((int)lastStartDay, scheduleList);
			}

			return entityMap;
		}

		/// <summary>
		/// 予定を登録する。
		/// </summary>
		/// <param name="entity">Entity</param>
		/// <returns>キー</returns>
		public long Insert(Schedule entity) {
			var preferences = new Preferences();

			using (SQLiteConnection con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<Schedule>();

					con.Insert(entity);
				});
			}

			return entity.Id;
		}

		/// <summary>
		/// 予定を更新する。
		/// </summary>
		/// <param name="entity">Entity</param>
		/// <returns>キー</returns>
		public long Update(Schedule entity) {
			var preferences = new Preferences();

			using (SQLiteConnection con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<Schedule>();

					con.Update(entity);
				});
			}

			return entity.Id;
		}

		/// <summary>
		/// 予定を削除する。
		/// </summary>
		/// <param name="id">ID</param>
		public void Delete(long id) {
			var preferences = new Preferences();

			using (SQLiteConnection con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {

				// 自動マイグレーション
				con.CreateTable<Schedule>();

				Schedule schedule = (from p in con.Table<Schedule>() where p.Id == id select p).FirstOrDefault();

				if (schedule != null) con.Delete(schedule);
			}
		}
	}
}