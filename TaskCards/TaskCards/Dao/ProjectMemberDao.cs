using System.Collections.Generic;
using SQLite;
using TaskCards.Data;
using TaskCards.Entities;

namespace TaskCards.Dao {

	/// <summary>
	/// ProjectMemberテーブルのDAO
	/// </summary>
	public class ProjectMemberDao {

		/// <summary>
		/// プロジェクトIDからプロジェクトメンバーリストを取得する。
		/// </summary>
		/// <param name="projectId">プロジェクトID</param>
		/// <returns>プロジェクトメンバーリスト</returns>
		public List<ProjectMember> GetProjectMemberListByProjectId(long projectId) {
			var preferences = new Preferences();
			var entityList = new List<ProjectMember>();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				var query = new TableQuery<ProjectMember>(con);

				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<ProjectMember>();

					query = con.Table<ProjectMember>()
					.Where(v => v.ProjectId == projectId);
				});

				foreach (ProjectMember entity in query) {
					entityList.Add(entity);
				}
			}

			return entityList;
		}

		/// <summary>
		/// メンバーIDからプロジェクトメンバーリストを取得する。
		/// </summary>
		/// <param name="memberId">メンバーID</param>
		/// <returns>プロジェクトメンバーリスト</returns>
		public List<ProjectMember> GetProjectMemberListByMemberId(long memberId) {
			var preferences = new Preferences();
			var entityList = new List<ProjectMember>();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				var query = new TableQuery<ProjectMember>(con);

				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<ProjectMember>();

					query = con.Table<ProjectMember>()
					.Where(v => v.MemberId == memberId);
				});

				foreach (ProjectMember entity in query) {
					entityList.Add(entity);
				}
			}

			return entityList;
		}

		/// <summary>
		/// プロジェクトメンバーを登録する。
		/// </summary>
		/// <param name="entity">Entity</param>
		public void Insert(ProjectMember entity) {
			var preferences = new Preferences();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<ProjectMember>();

					con.Insert(entity);
				});
			}
		}

		/// <summary>
		/// プロジェクトメンバーを削除する。
		/// </summary>
		/// <param name="projectId">プロジェクトID</param>
		/// <param name="memberId">メンバーID</param>
		public void Delete(long projectId, long memberId) {
			var preferences = new Preferences();

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {

				// 自動マイグレーション
				con.CreateTable<ProjectMember>();

				ProjectMember projectMember
					= (from p in con.Table<ProjectMember>()
					   where p.ProjectId == projectId && p.MemberId == memberId
					   select p).FirstOrDefault();

				if (projectMember != null) con.Delete(projectMember);
			}
		}

		/// <summary>
		/// プロジェクトIDから該当するプロジェクトメンバーデータをすべて削除する。
		/// </summary>
		/// <param name="projectId">プロジェクトID</param>
		public void DeleteAllByProjectId(long projectId) {
			var preferences = new Preferences();

			List<ProjectMember> entityList = GetProjectMemberListByProjectId(projectId);

			using (var con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				foreach (ProjectMember entity in entityList) {
					con.Delete(entity);
				}
			}
		}
	}
}