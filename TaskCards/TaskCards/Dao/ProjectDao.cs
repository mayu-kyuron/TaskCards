using SQLite;
using TaskCards.Data;
using TaskCards.Entities;

namespace TaskCards.Dao {

	/// <summary>
	/// ProjectテーブルのDAO
	/// </summary>
	public class ProjectDao {

		/// <summary>
		/// プロジェクトIDよりプロジェクトデータを取得する。
		/// </summary>
		/// <param name="id">プロジェクトID</param>
		/// <returns>プロジェクトデータ</returns>
		public Project GetProjectById(long id) {
			var preferences = new Preferences();
			var entity = new Project();

			using (SQLiteConnection con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				TableQuery<Project> query = new TableQuery<Project>(con);

				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<Project>();

					query = con.Table<Project>()
					.Where(v => v.Id == id);
				});

				entity = query.FirstOrDefault();
			}

			return entity;
		}

		/// <summary>
		/// プロジェクトを登録する。
		/// </summary>
		/// <param name="entity">Entity</param>
		public void Insert(Project entity) {
			var preferences = new Preferences();

			using (SQLiteConnection con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<Project>();

					con.Insert(entity);
				});
			}
		}
	}
}