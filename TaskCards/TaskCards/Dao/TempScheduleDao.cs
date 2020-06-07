using SQLite;
using TaskCards.Data;
using TaskCards.Entities;

namespace TaskCards.Dao {

	/// <summary>
	/// TempScheduleテーブルのDAO
	/// </summary>
	public class TempScheduleDao {

		/// <summary>
		/// 一時保存スケジュールIDより一時保存スケジュールデータを取得する。
		/// </summary>
		/// <param name="id">一時保存スケジュールID</param>
		/// <returns>一時保存スケジュールデータ</returns>
		public TempSchedule GetTempScheduleById(long id) {
			var preferences = new Preferences();
			var entity = new TempSchedule();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				var query = new TableQuery<TempSchedule>(con);

				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<TempSchedule>();

					query = con.Table<TempSchedule>()
					.Where(v => v.Id == id);
				});

				entity = query.FirstOrDefault();
			}

			return entity;
		}

		/// <summary>
		/// 一時保存スケジュールを登録する。
		/// </summary>
		/// <param name="entity">Entity</param>
		/// <returns>キー</returns>
		public long Insert(TempSchedule entity) {
			var preferences = new Preferences();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<TempSchedule>();

					con.Insert(entity);
				});
			}

			return entity.Id;
		}

		/// <summary>
		/// 一時保存スケジュールを削除する。
		/// </summary>
		/// <param name="id">ID</param>
		public void Delete(long id) {
			var preferences = new Preferences();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {

				// 自動マイグレーション
				con.CreateTable<TempSchedule>();

				TempSchedule tempSchedule = (from p in con.Table<TempSchedule>() where p.Id == id select p).FirstOrDefault();

				if (tempSchedule != null) con.Delete(tempSchedule);
			}
		}

		/// <summary>
		/// 一時保存スケジュールを全て削除する。
		/// </summary>
		public void DeleteAll() {
			var preferences = new Preferences();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {

				// 自動マイグレーション
				con.CreateTable<TempSchedule>();

				con.DeleteAll<TempSchedule>();
			}
		}
	}
}