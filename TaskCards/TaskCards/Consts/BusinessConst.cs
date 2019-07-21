namespace TaskCards.Consts {

	/// <summary>
	/// 業務定数
	/// </summary>
    public class BusinessConst {

		// サーバーからの応答ステータス: 成功
		public const int STATUS_SUCCESS = 1;

		// サーバーからの応答ステータス: 失敗
		public const int STATUS_FAILURE = 0;

		// サーバーとの通信失敗
		public const int STATUS_NETWORK_ERROR = -1;

		// サーバーとの通信の必要なし
		public const int STATUS_NO_NEED = 2;

	}
}