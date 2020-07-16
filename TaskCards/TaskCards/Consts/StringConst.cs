namespace TaskCards.Consts {

	/// <summary>
	/// 文字列定数
	/// </summary>
    public class StringConst {

		// アプリケーションラベル
		public const string AppLabel = "タスクカード";

		// 用語
		public const string WordSchedule = "スケジュール";
		public const string WordTask = "タスク";
		public const string WordProject = "プロジェクト";
		public const string WordMyProject = "マイプロジェクト";
		public const string WordMember = "メンバー";
		public const string WordProgressRate = "進捗率";

		// ダイアログタイトル
		public const string DialogTitleConfirm = "確認";
		public const string DialogTitleError = "エラー";
		public const string DialogTitleSuccess = "処理成功";
		public const string DialogTitleWarning = "警告";

		// ダイアログボタン
		public const string DialogAnswerPositive = "OK";
		public const string DialogAnswerNegative = "キャンセル";

		// 備考
		public const string NoteCharLimit = "{0:d}文字～{1:d}文字";
		public const string NoteHalfWidthChar = "半角英数字（{0:d}文字～{1:d}文字）";

		// 共通メッセージ
		public const string MessageEmpty = "入力必須です。";
		public const string MessageEntryNeeded = "{0:s}を入力してください。";
		public const string MessageWrongType = "{0:s}は{1:s}を入力してください。";
		public const string MessageChangeNeeded = "{0:s}を変更してください。";
		public const string MessageLength = "{0:d}文字以上{1:d}文字以内で入力してください。";
		public const string MessageMaxLength = "{0:s}は{1:d}文字以内で入力してください。";
		public const string MessageWordMismatch = "入力値が確認用と異なっています。";
		public const string MessageRiyouKiyakuNotChecked = "利用規約への同意が必要です。";
		public const string MessageMistake = "入力値が間違っています。";
		public const string MessageWrongFormat = "フォーマットが不正です。";
		public const string MessageFailed = "{0:s}に失敗しました。";
		public const string MessageSucceeded = "{0:s}が完了しました。";
		public const string MessageExecuted = "{0:s}が実行されました。";
		public const string MessageNetworkError = "インターネットに接続できません。\n接続環境を確認してください。";
		public const string MessageLoginAgain = "再度ログイン処理を行ってください。";
		public const string MessageTransition = "{0:s}画面に遷移します。";
		public const string MessageContentsNotSelected = "動画ファイルが選択されていません。";
		public const string MessagePleaseAskAdmin = "お手数ですが、管理者までお問い合わせください。";
		public const string MessageBackToTop = "TOP画面に戻ります。";
		public const string MessageAdd = "{0:s}を追加する";
		public const string MessageInputCancelConfirm = "入力した内容は失われます。\nキャンセルしてもよろしいですか？";
		public const string MessageWrongDateTime = "{0:s}は{1:s}より\n前でなければなりません。";
		public const string MessageDeleteConfirm = "削除したデータは元に戻せません。\nこの{0:s}を削除しますか？";
		public const string MessageDeleteTaskProgressConfirm = "{0:s}\n\nこの作業記録を削除します。\nよろしいですか？";
		public const string MessageFinishProjectConfirm = "このプロジェクトを終了します。\nよろしいですか？";
		public const string MessageMyProjectDescription = WordMyProject + "は自分専用の" + WordProject + "です。\n自分用の予定やタスク管理に利用することができます。";
		public const string MessageMyProjectError = WordMyProject + "は{0:s}できません。";
		public const string MessageRestartProjectSuccess = "プロジェクトを再開しました。";

		// 固有メッセージ
		public const string LogoutConfirmMessage = "ログアウトして\nログイン画面に戻ります。\nよろしいですか？";
		public const string ContentsUploadCategoryMessage = "第2カテゴリーまで選択してください。";
		public const string ContentsUploadWarningMessage = "ネットワーク上の問題により、\n動画投稿状態を確認できませんでした。\n投稿状態確認画面より動画をご確認ください。";
		public const string ContentsUploadPermissionMessage = "動画選択の際、外部ストレージへアクセスします。\n動画を投稿する場合は、アクセスを許可してください。";
		public const string ContentsUploadSuccessNotesMessage = "管理者の承認の後、コンテンツ公開されます。";
		public const string VersionUpdateConfirmMessage = "最新版のアプリが提供されています。\nダウンロードして最新版に更新しますか？";

		// 正規表現
		public const string RegexHalfWidthChar = @"^[0-9a-zA-Z@¥.¥_¥¥-]+$";
		public const string RegexFullWidthChar = @"^[^\x01-\x7E\xA1-\xDF]+$";
		public const string RegexEmail = @"[-\w]+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

		// パス
		public const string ImageFolderPath = "TaskCards.Images.";
		public const string DatabaseFileName = "TaskCards.db";

		// DateTimeフォーマット
		public const string DateTappedDialogDateFormat = "yyyy年MM月dd日(ddd)";
		public const string InputConfirmDateFormat = "MM月dd日(ddd)";
		public const string InputConfirmTimeFormat = "HH:mm";
		public const string TaskProgressDateTimeFormat = "yyyy/MM/dd(ddd) HH:mm";

		// 繰り返しタイプ
		public const string RepeatNone = "繰り返しなし";
		public const string RepeatEveryday = "毎日";
		public const string RepeatEveryWeek = "毎週";
		public const string RepeatEveryMonth = "毎月";
		public const string RepeatEveryYear = "毎年";

		// 各項目の固定テキスト
		public const string ItemTextDailyTime = "時間 / 日";
	}
}