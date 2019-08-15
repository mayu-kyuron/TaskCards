using Acr.UserDialogs;
using System;
using TaskCards.Consts;
using TaskCards.Dao;
using TaskCards.Divisions;
using TaskCards.Entities;
using TaskCards.MasterPages;
using TaskCards.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskCards.Pages {

	/// <summary>
	/// 入力ページ
	/// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InputPage : ContentPage {

		private InputViewModel viewModel;

		DateTime selectedDate = DateTime.Today; // 選択された日付
		TableDiv tableDiv; // データ登録先のテーブル区分
		PageDiv exPageDiv; // 元のページ区分
		ExecuteDiv executeDiv; // 実行区分
		long id; // ID

		public InputPage() {
			Initialize();
		}

		public InputPage(DateTime selectedDate, TableDiv tableDiv, PageDiv exPageDiv, ExecuteDiv executeDiv, long id = 0) {
			this.selectedDate = selectedDate;
			this.tableDiv = tableDiv;
			this.exPageDiv = exPageDiv;
			this.executeDiv = executeDiv;
			this.id = id;
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

			//var tgrProject = new TapGestureRecognizer();
			//tgrProject.Tapped += (sender, e) => OnClickProject(sender, e);
			//gdProject.GestureRecognizers.Add(tgrProject);

			//var tgrRepeatGrid = new TapGestureRecognizer();
			//tgrRepeatGrid.Tapped += (sender, e) => OnClickRepeatGrid(sender, e);
			//gdRepeat.GestureRecognizers.Add(tgrRepeatGrid);

			//var tgrRepeatCancel = new TapGestureRecognizer();
			//tgrRepeatCancel.Tapped += (sender, e) => OnClickRepeatCancel(sender, e);
			//imgRepeat.GestureRecognizers.Add(tgrRepeatCancel);

			// 各コントロールの表示切り替え
			switch (this.tableDiv) {

				case TableDiv.予定:
					gdTime.IsVisible = true;
					gdAllDay.IsVisible = true;
					etPlace.IsVisible = true;
					break;

				case TableDiv.タスク:
					gdDate.IsVisible = true;
					gdExpectedDailyWorkTime.IsVisible = true;
					break;
			}
		}

		/// <summary>
		/// 画面サイズ変更イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void OnSizeChanged(object sender, EventArgs args) {
			this.viewModel = new InputViewModel(this.selectedDate, this.tableDiv, this.executeDiv, this.id, 
				Height, cvDialogBack, swAllDay, Resources);
			BindingContext = this.viewModel;

			// 繰り返し選択ダイアログのリスト行の高さを設定
			lstRepeat.RowHeight = (int)Math.Round(Height * LayoutRateConst.ListItemHeight);
			double dialogHeight = lstRepeat.RowHeight * 4 + 2;
			gdDialogBack.RowDefinitions.Add(new RowDefinition { Height = (Height - dialogHeight) / 2 });
			gdDialogBack.RowDefinitions.Add(new RowDefinition { Height = dialogHeight });
			gdDialogBack.RowDefinitions.Add(new RowDefinition { Height = (Height - dialogHeight) / 2 });
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
			UserDialogs.Instance.ShowLoading("更新中...");

			switch (this.tableDiv) {
				case TableDiv.プロジェクト:
					break;
				case TableDiv.予定:
					InsertOrUpdateScheduleAndScheduleMembers();
					break;
				case TableDiv.タスク:
					InsertOrUpdateTaskAndTaskMembers();
					break;
			}

			UserDialogs.Instance.HideLoading();

			// 完了ダイアログ
			Device.BeginInvokeOnMainThread((async () => {
				await DisplayAlert(StringConst.DialogTitleSuccess, 
					String.Format(StringConst.MessageSucceeded, "更新"), StringConst.DialogAnswerPositive);
			}));

			// 遷移
			GoBackToExPage();
		}

		/// <summary>
		/// 終日トグルイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnToggleAllDay(object sender, ToggledEventArgs e) {

			if (swAllDay.IsToggled) {
				gdTime.IsVisible = false;
			}
			else {
				gdTime.IsVisible = true;
			}
		}

		/// <summary>
		/// プロジェクトクリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickProject(object sender, EventArgs e) {

		}

		/// <summary>
		/// 繰り返しグリッドクリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickRepeatGrid(object sender, EventArgs e) {
			cvDialogBack.IsVisible = true;
		}

		/// <summary>
		/// 繰り返しキャンセルクリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickRepeatCancel(object sender, EventArgs e) {
			this.viewModel.RepeatDiv = RepeatDiv.繰り返しなし;
			Resources["RepeatText"] = StringConst.RepeatNone;
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

			switch (this.exPageDiv) {
				case PageDiv.カレンダー:
					Application.Current.MainPage = new TaskCardsMasterDetailPage(new DetailPage());
					break;
				case PageDiv.スケジュール確認:
					Application.Current.MainPage = new ConfirmSchedulePage(this.id);
					break;
			}
		}

		/// <summary>
		/// 入力検証する。
		/// </summary>
		/// <returns>検証OKなら「true」、NGなら「false」</returns>
		private bool Validate() {

			// タイトル入力チェック
			if (String.IsNullOrEmpty(this.viewModel.TitleText)) {

				Device.BeginInvokeOnMainThread((async () => {
					await DisplayAlert(StringConst.DialogTitleError,
						String.Format(StringConst.MessageEntryNeeded, "タイトル"), StringConst.DialogAnswerPositive);
				}));

				return false;
			}

			// 時間または日付の整合性チェック
			if (this.tableDiv == TableDiv.予定) {
				if (TimeSpan.Compare(this.viewModel.StartTime, this.viewModel.EndTime) == 1) {

					Device.BeginInvokeOnMainThread((async () => {
						await DisplayAlert(StringConst.DialogTitleError,
							String.Format(StringConst.MessageWrongDateTime, "開始時間", "終了時間"),
							StringConst.DialogAnswerPositive);
					}));

					return false;
				}
			}
			else if (this.tableDiv == TableDiv.タスク) {
				if (this.viewModel.StartDate.CompareTo(this.viewModel.EndDate) == 1) {

					Device.BeginInvokeOnMainThread((async () => {
						await DisplayAlert(StringConst.DialogTitleError,
							String.Format(StringConst.MessageWrongDateTime, "開始日", "終了日"),
							StringConst.DialogAnswerPositive);
					}));

					return false;
				}
			}

			// 予定毎日作業時間の数値変換チェック
			if (this.tableDiv == TableDiv.タスク) {
				double expectedDailyWorkTimeNum;
				if (!double.TryParse(this.viewModel.ExpectedDailyWorkTimeText, out expectedDailyWorkTimeNum)) {

					Device.BeginInvokeOnMainThread((async () => {
						await DisplayAlert(StringConst.DialogTitleError,
							String.Format(StringConst.MessageWrongType, "作業時間", "数字"), 
							StringConst.DialogAnswerPositive);
					}));

					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// スケジュールと所属メンバーを登録または更新する。
		/// </summary>
		private void InsertOrUpdateScheduleAndScheduleMembers() {

			DateTime startDate = this.selectedDate.Date;
			DateTime endDate = this.selectedDate.Date;
			if (!swAllDay.IsToggled) {
				startDate += this.viewModel.StartTime;
				endDate += this.viewModel.EndTime;
			}

			var schedule = new Schedule {
				Id = this.id,
				Title = this.viewModel.TitleText,
				StartDate = startDate,
				EndDate = endDate,
				isAllDay = swAllDay.IsToggled,
				ProjectId = this.viewModel.ProjectId,
				RepeatDiv = this.viewModel.RepeatDiv,
				Place = this.viewModel.PlaceText,
				Notes = this.viewModel.NotesText
			};

			var scheduleDao = new ScheduleDao();
			var scheduleMemberDao = new ScheduleMemberDao();

			// スケジュールの登録または更新
			if (this.executeDiv == ExecuteDiv.追加) {
				this.id = scheduleDao.Insert(schedule);
			}
			else if (this.executeDiv == ExecuteDiv.更新) {
				this.id = scheduleDao.Update(schedule);

				// 前回のスケジュールメンバーの削除
				scheduleMemberDao.DeleteAllByScheduleId(this.id);
			}

			// スケジュールメンバーの登録
			foreach (long memberId in this.viewModel.MemberIdList) {

				var scheduleMember = new ScheduleMember {
					ScheduleId = this.id,
					MemberId = memberId,
					CanEdit = true // 仮に全員が編集可能に
				};

				scheduleMemberDao.Insert(scheduleMember);
			}
		}

		/// <summary>
		/// タスクと所属メンバーを登録または更新する。
		/// </summary>
		private void InsertOrUpdateTaskAndTaskMembers() {

			int expectedDailyWorkSeconds = (int)Math.Truncate(double.Parse(this.viewModel.ExpectedDailyWorkTimeText) * 3600);

			var task = new Task {
				Id = this.id,
				Title = this.viewModel.TitleText,
				StartDate = this.viewModel.StartDate,
				EndDate = this.viewModel.EndDate,
				ExpectedDailyWorkTime = new TimeSpan(0, 0, expectedDailyWorkSeconds),
				ProjectId = this.viewModel.ProjectId,
				Notes = this.viewModel.NotesText
			};

			var taskDao = new TaskDao();
			var taskMemberDao = new TaskMemberDao();

			// タスクの登録または更新
			if (this.executeDiv == ExecuteDiv.追加) {
				this.id = taskDao.Insert(task);
			}
			else if (this.executeDiv == ExecuteDiv.更新) {
				this.id = taskDao.Update(task);

				// 前回のタスクメンバーの削除
				taskMemberDao.DeleteAllByTaskId(this.id);
			}

			// タスクメンバーの登録
			foreach (long memberId in this.viewModel.MemberIdList) {

				var taskMember = new TaskMember {
					TaskId = this.id,
					MemberId = memberId,
					CanEdit = true // 仮に全員が編集可能に
				};

				taskMemberDao.Insert(taskMember);
			}
		}
	}
}