using SQLite;
using TaskCards.Data;
using TaskCards.Entities;

namespace TaskCards.Dao {

	/// <summary>
	/// MemberテーブルのDAO
	/// </summary>
	public class MemberDao {

		/// <summary>
		/// メンバーIDよりメンバーデータを取得する。
		/// </summary>
		/// <param name="id">メンバーID</param>
		/// <returns>メンバーデータ</returns>
		public Member GetMemberById(long id) {
			var preferences = new Preferences();
			var entity = new Member();

			using (SQLiteConnection con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				TableQuery<Member> query = new TableQuery<Member>(con);

				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<Member>();

					query = con.Table<Member>()
					.Where(v => v.Id == id);
				});

				entity = query.FirstOrDefault();
			}

			return entity;
		}

		/// <summary>
		/// メンバーを登録する。
		/// </summary>
		/// <param name="entity">Entity</param>
		public void Insert(Member entity) {
			var preferences = new Preferences();

			using (SQLiteConnection con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {
				con.RunInTransaction(() => {

					// 自動マイグレーション
					con.CreateTable<Member>();

					con.Insert(entity);
				});
			}
		}

		/// <summary>
		/// メンバーを削除する。
		/// </summary>
		/// <param name="id">ID</param>
		public void Delete(long id) {
			var preferences = new Preferences();

			using (SQLiteConnection con = new SQLiteConnection(preferences.GetDatabaseFilePath())) {

				// 自動マイグレーション
				con.CreateTable<Member>();

				Member member = (from p in con.Table<Member>() where p.Id == id select p).FirstOrDefault();

				if (member != null) con.Delete(member);
			}
		}
	}
}