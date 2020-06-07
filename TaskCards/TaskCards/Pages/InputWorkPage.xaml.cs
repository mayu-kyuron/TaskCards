using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using TaskCards.Consts;
using TaskCards.Dao;
using TaskCards.Divisions;
using TaskCards.Entities;
using TaskCards.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskCards.Pages {

	/// <summary>
	/// 作業記録入力ページ
	/// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InputWorkPage : ContentPage {

		private InputWorkViewModel viewModel;

		long taskId; // タスクID

		public InputWorkPage() {
			Initialize();
		}

		public InputWorkPage(long taskId) {
			this.taskId = taskId;
			Initialize();
		}

		/// <summary>
		/// 初期化する。
		/// </summary>
		private void Initialize() {
			InitializeComponent();

			SizeChanged += OnSizeChanged;

			// タップイベントを付与
			var tgrDialogBack = new TapGestureRecognizer();
			tgrDialogBack.Tapped += (sender, e) => OnClickDialogBack(sender, e);
			cvDialogBack.GestureRecognizers.Add(tgrDialogBack);

			var tgrDialogFront = new TapGestureRecognizer();
			tgrDialogFront.Tapped += (sender, e) => OnClickDialogFront(sender, e);
			gdDialogFront.GestureRecognizers.Add(tgrDialogFront);

			var tgrBackButton = new TapGestureRecognizer();
			tgrBackButton.Tapped += (sender, e) => OnClickBackButton(sender, e);
			imgBackButton.GestureRecognizers.Add(tgrBackButton);

			var tgrTopRightButton = new TapGestureRecognizer();
			tgrTopRightButton.Tapped += (sender, e) => OnClickTopRightButton(sender, e);
			imgTopRightButton.GestureRecognizers.Add(tgrTopRightButton);
		}

		/// <summary>
		/// 画面サイズ変更イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void OnSizeChanged(object sender, EventArgs args) {
			this.viewModel = new InputWorkViewModel(this.taskId, Height);
			BindingContext = this.viewModel;
		}

		/// <summary>
		/// ダイアログ背面クリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickDialogBack(object sender, EventArgs e) {
			cvDialogBack.IsVisible = false;
		}

		/// <summary>
		/// ダイアログ前面クリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickDialogFront(object sender, EventArgs e) {
			// 何もしない
		}

		/// <summary>
		/// 戻るボタンクリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickBackButton(object sender, EventArgs e) => OnPageBack();
		protected override bool OnBackButtonPressed() {
			OnPageBack();
			return true;
		}

		/// <summary>
		/// 右上ボタン（確定ボタン）クリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickTopRightButton(object sender, EventArgs e) {

			// 入力検証
			if (!Validate()) return;

			// データ更新処理
			UserDialogs.Instance.ShowLoading("登録中...");

			InsertTaskProgressAndUpdateTask();

			UserDialogs.Instance.HideLoading();

			// 完了ダイアログ
			Device.BeginInvokeOnMainThread((async () => {
				await DisplayAlert(StringConst.DialogTitleSuccess,
					String.Format(StringConst.MessageSucceeded, "登録"), StringConst.DialogAnswerPositive);
			}));

			// 遷移
			GoBackToExPage();
		}

		/// <summary>
		/// 前ページに戻る。
		/// </summary>
		private void OnPageBack() {

			Device.BeginInvokeOnMainThread((async () => {
				var result = await DisplayAlert(StringConst.DialogTitleConfirm, StringConst.MessageInputCancelConfirm,
					StringConst.DialogAnswerPositive, StringConst.DialogAnswerNegative);

				if (result) GoBackToExPage();
			}));
		}

		/// <summary>
		/// 遷移元のページに戻る。
		/// </summary>
		private void GoBackToExPage() {

			TaskDao taskDao = new TaskDao();
			Task task = taskDao.GetTaskById(this.taskId);

			Application.Current.MainPage = new ConfirmPage(task.StartDate, TableDiv.タスク, PageDiv.カレンダー, this.taskId);
		}

		/// <summary>
		/// 入力検証する。
		/// </summary>
		/// <returns>検証OKなら「true」、NGなら「false」</returns>
		private bool Validate() {

			// 時間の整合性チェック
			if (TimeSpan.Compare(this.viewModel.StartTime, this.viewModel.EndTime) == 1) {

				Device.BeginInvokeOnMainThread((async () => {
					await DisplayAlert(StringConst.DialogTitleError,
						String.Format(StringConst.MessageWrongDateTime, "開始時間", "終了時間"),
						StringConst.DialogAnswerPositive);
				}));

				return false;
			}

			// 進捗率の数値変換チェック
			double progressRateNum;
			if (!double.TryParse(this.viewModel.ProgressRateText, out progressRateNum)) {

				Device.BeginInvokeOnMainThread((async () => {
					await DisplayAlert(StringConst.DialogTitleError,
						String.Format(StringConst.MessageWrongType, "進捗率", "数字"),
						StringConst.DialogAnswerPositive);
				}));

				return false;
			}

			// 進捗率の最大値チェック
			if (progressRateNum > 100) {

				Device.BeginInvokeOnMainThread((async () => {
					await DisplayAlert(StringConst.DialogTitleError,
						String.Format(StringConst.MessageWrongType, "進捗率", "100以下の数字"),
						StringConst.DialogAnswerPositive);
				}));

				return false;
			}

			// 進捗率の正の数チェック
			if (progressRateNum < 0) {

				Device.BeginInvokeOnMainThread((async () => {
					await DisplayAlert(StringConst.DialogTitleError,
						String.Format(StringConst.MessageWrongType, "進捗率", "0以上の数字"),
						StringConst.DialogAnswerPositive);
				}));

				return false;
			}

			// 進捗率の整数チェック
			if ((progressRateNum - Math.Floor(progressRateNum) != 0)) {

				Device.BeginInvokeOnMainThread((async () => {
					await DisplayAlert(StringConst.DialogTitleError,
						String.Format(StringConst.MessageWrongType, "進捗率", "小数を含まない数字"),
						StringConst.DialogAnswerPositive);
				}));

				return false;
			}

			return true;
		}

		/// <summary>
		/// 作業記録を登録し、タスクを更新する。
		/// </summary>
		private void InsertTaskProgressAndUpdateTask() {

			long taskMemberId = 0;
			int registerOrder = 0;

			var taskDao = new TaskDao();
			Task task = taskDao.GetTaskById(this.taskId);

			var taskMemberDao = new TaskMemberDao();
			List<TaskMember> taskMemberList = taskMemberDao.GetTaskMemberListByTaskId(this.taskId);

			// 過去のタスク進捗をすべて取得
			var taskProgressList = new List<TaskProgress>();
			var taskProgressDao = new TaskProgressDao();
			foreach (TaskMember taskMember in taskMemberList) {
				taskProgressList.AddRange(taskProgressDao.GetTaskProgressListByTaskMemberId(taskMember.Id));

				// TODO 仮のメンバーを初期選択に
				if (taskMember.MemberId == 1) taskMemberId = taskMember.Id;
			}

			// 過去の作業時間を加算し、現時点の総作業時間を取得
			TimeSpan totalWorkTime = new TimeSpan(0, 0, 0);
			foreach (TaskProgress pastTaskProgress in taskProgressList) {
				totalWorkTime += (pastTaskProgress.EndDate - pastTaskProgress.StartDate);
				if (pastTaskProgress.RegisterOrder > registerOrder) registerOrder = pastTaskProgress.RegisterOrder;
			}

			// タスクを更新
			task.TotalWorkTime = totalWorkTime + (this.viewModel.EndTime - this.viewModel.StartTime);
			task.ProgressRate = double.Parse(this.viewModel.ProgressRateText);
			task.Notes = this.viewModel.NotesText;

			taskDao.Update(task);

			// TODO taskProgressListの要素がひとつもない場合、プロジェクトの開始日を当日に設定する。

			// タスク進捗を追加
			var taskProgress = new TaskProgress {
				TaskMemberId = taskMemberId,
				RegisterOrder = registerOrder + 1,
				ProgressRate = double.Parse(this.viewModel.ProgressRateText),
				StartDate = DateTime.Now.Date + this.viewModel.StartTime,
				EndDate = DateTime.Now.Date + this.viewModel.EndTime,
			};

			taskProgressDao.Insert(taskProgress);
		}
	}
}