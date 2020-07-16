using System.Collections.Generic;
using System.Linq;
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
		/// メンバーIDよりプロジェクトリストを取得する。
		/// </summary>
		/// <param name="memberId">メンバーID</param>
		/// <returns>プロジェクトリスト</returns>
		public List<Project> GetProjectListByMemberId(long memberId) {
			var entityList = new List<Project>();

			var projectMemberDao = new ProjectMemberDao();
			List<ProjectMember> projectMemberList = projectMemberDao.GetProjectMemberListByMemberId(memberId);

			foreach(ProjectMember projectMember in projectMemberList) {

				Project entity = GetProjectById(projectMember.ProjectId);
				if (entity == null) continue;

				entityList.Add(entity);
			}

			// 終了していないプロジェクトを前にしたあと、予定開始日の降順に並び替える。
			entityList = entityList.OrderBy(v => v.EndDate).ThenByDescending(v => v.ExpectedStartDate).ToList();

			// マイプロジェクトを先頭に入れ替える。
			if (entityList.Exists(v => v.Id == 1)) {
				entityList.Insert(0, entityList.First(v => v.Id == 1));
				entityList.RemoveAt(entityList.FindLastIndex(v => v.Id == 1));
			}

			return entityList;
		}

		/// <summary>
		/// プロジェクトを登録する。
		/// </summary>
		/// <param name="entity">Entity</param>
		/// <returns>キー</returns>
		public long Insert(Project entity) {
			var preferences = new Preferences();

			using (SQLiteConnection con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<Project>();

					con.Insert(entity);
				});
			}

			return entity.Id;
		}

		/// <summary>
		/// プロジェクトを更新する。
		/// </summary>
		/// <param name="entity">Entity</param>
		/// <returns>キー</returns>
		public long Update(Project entity) {
			var preferences = new Preferences();

			using (SQLiteConnection con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<Project>();

					con.Update(entity);
				});
			}

			return entity.Id;
		}

		/// <summary>
		/// プロジェクトを削除する。
		/// </summary>
		/// <param name="id">ID</param>
		public void Delete(long id) {
			var preferences = new Preferences();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {

				// 自動マイグレーション
				con.CreateTable<Project>();

				Project project = (from p in con.Table<Project>() where p.Id == id select p).FirstOrDefault();

				if (project != null) con.Delete(project);
			}
		}
	}
}