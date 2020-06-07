using SQLite;
using TaskCards.Data;
using TaskCards.Entities;

namespace TaskCards.Dao {

	/// <summary>
	/// TempTaskテーブルのDAO
	/// </summary>
	public class TempTaskDao {

		/// <summary>
		/// 一時保存タスクIDより一時保存タスクデータを取得する。
		/// </summary>
		/// <param name="id">一時保存タスクID</param>
		/// <returns>一時保存タスクデータ</returns>
		public TempTask GetTempTaskById(long id) {
			var preferences = new Preferences();
			var entity = new TempTask();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				var query = new TableQuery<TempTask>(con);

				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<TempTask>();

					query = con.Table<TempTask>()
					.Where(v => v.Id == id);
				});

				entity = query.FirstOrDefault();
			}

			return entity;
		}

		/// <summary>
		/// 一時保存タスクを登録する。
		/// </summary>
		/// <param name="entity">Entity</param>
		/// <returns>キー</returns>
		public long Insert(TempTask entity) {
			var preferences = new Preferences();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<TempTask>();

					con.Insert(entity);
				});
			}

			return entity.Id;
		}

		/// <summary>
		/// 一時保存タスクを削除する。
		/// </summary>
		/// <param name="id">ID</param>
		public void Delete(long id) {
			var preferences = new Preferences();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {

				// 自動マイグレーション
				con.CreateTable<TempTask>();

				TempTask tempTask = (from p in con.Table<TempTask>() where p.Id == id select p).FirstOrDefault();

				if (tempTask != null) con.Delete(tempTask);
			}
		}

		/// <summary>
		/// 一時保存タスクを全て削除する。
		/// </summary>
		public void DeleteAll() {
			var preferences = new Preferences();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {

				// 自動マイグレーション
				con.CreateTable<TempTask>();

				con.DeleteAll<TempTask>();
			}
		}
	}
}