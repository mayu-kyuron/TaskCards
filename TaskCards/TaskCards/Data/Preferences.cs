using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using PCLAppConfig;
using TaskCards.Consts;

namespace TaskCards.Data {

	/// <summary>
	/// アプリケーションの設定を一元管理するクラス
	/// </summary>
	public class Preferences {

		[JsonIgnore]
		public const string CONTACT_US_EMAIL = "contact_us_email";

		/// <summary>
		/// アプリケーションルート URL と、それ以下の相対パスを組み合わせて完全な URL を作成する。
		/// </summary>
		/// <param name="appRootUrl">アプリケーションルートURL</param>
		/// <param name="relativeUrl">アプリケーションルート以下の URL</param>
		/// <returns>完全 URL</returns>
		protected string BuildUrl(string appRootUrl, string relativeUrl) {

			StringBuilder builder = new StringBuilder();
			builder.Append(appRootUrl);

			if (!appRootUrl.EndsWith("/")) {
				builder.Append("/");
			}

			if (relativeUrl.StartsWith("/")) {
				relativeUrl = relativeUrl.Substring(1);
			}

			return builder.Append(relativeUrl).ToString();
		}

		/// <summary>
		/// データベースファイルパス を取得する。
		/// </summary>
		/// <returns>ファイルパス</returns>
		public string GetDatabaseFilePath() {

			return Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), StringConst.DatabaseFileName);
		}

		/// <summary>
		/// お問い合わせメールアドレスを取得する。
		/// </summary>
		/// <returns>メールアドレス</returns>
		public string GetContactUsEmail() {
			return ConfigurationManager.AppSettings[CONTACT_US_EMAIL];
		}

		/// <summary>
		/// 個人識別文字列
		/// </summary>
		public string IdentificationText { get; set; }

		/// <summary>
		/// メンバー名
		/// </summary>
		public string MemberName { get; set; }

		/// <summary>
		/// メンバーID
		/// </summary>
		public long MemberId { get; set; }
	}
}