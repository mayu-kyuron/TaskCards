using Acr.UserDialogs;
using System;
using System.Collections.Generic;
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

			var tgrProject = new TapGestureRecognizer();
			tgrProject.Tapped += (sender, e) => OnClickProject(sender, e);
			gdProject.GestureRecognizers.Add(tgrProject);

			var tgrAddProject = new TapGestureRecognizer();
			tgrAddProject.Tapped += (sender, e) => OnClickAddProject(sender, e);
			gdAddProject.GestureRecognizers.Add(tgrAddProject);

			var tgrColor = new TapGestureRecognizer();
			tgrColor.Tapped += (sender, e) => OnClickColor(sender, e);
			gdColor.GestureRecognizers.Add(tgrColor);

			//var tgrRepeatGrid = new TapGestureRecognizer();
			//tgrRepeatGrid.Tapped += (sender, e) => OnClickRepeatGrid(sender, e);
			//gdRepeat.GestureRecognizers.Add(tgrRepeatGrid);

			//var tgrRepeatCancel = new TapGestureRecognizer();
			//tgrRepeatCancel.Tapped += (sender, e) => OnClickRepeatCancel(sender, e);
			//imgRepeat.GestureRecognizers.Add(tgrRepeatCancel);
		}

		/// <summary>
		/// 画面サイズ変更イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void OnSizeChanged(object sender, EventArgs args) {

			BaseEvent tempEvent = ResetValuesAfterTempSave();

			SetControlsVisibility();

			this.viewModel = new InputViewModel(this.selectedDate, this.tableDiv, this.executeDiv, this.id, tempEvent,
				Height, cvDialogBack, gdDialogRepeat, gdDialogProject, gdProjects, gdDialogColor, swAllDay, Resources);
			BindingContext = this.viewModel;

			if (this.id == 0 && this.executeDiv == ExecuteDiv.更新) this.executeDiv = ExecuteDiv.追加;

			// 項目選択ダイアログのリスト行の高さを設定
			int rowHeight = (int)Math.Round(Height * LayoutRateConst.ListItemHeight);
			lstRepeat.RowHeight = rowHeight;
			lstColor.RowHeight = rowHeight;
			double dialogHeight = rowHeight * 4 + 2;
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
			gdDialogRepeat.IsVisible = false;
			gdDialogProject.IsVisible = false;
			gdDialogColor.IsVisible = false;
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
					InsertOrUpdateProjectAndProjectMembers();
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
			cvDialogBack.IsVisible = true;
			gdDialogProject.IsVisible = true;
		}

		/// <summary>
		/// プロジェクト追加クリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickAddProject(object sender, EventArgs e) {

			long id = 0;
			switch (this.tableDiv) {
				case TableDiv.予定:
					id = InsertTempScheduleAndTempScheduleMembers();
					break;
				case TableDiv.タスク:
					id = InsertTempTaskAndTempTaskMembers();
					break;
			}

			// プロジェクト追加用の入力ページに遷移
			Application.Current.MainPage = new InputPage(this.selectedDate,
				TableDiv.プロジェクト, PageDiv.入力, ExecuteDiv.追加, id);
		}

		/// <summary>
		/// テーマカラークリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickColor(object sender, EventArgs e) {
			cvDialogBack.IsVisible = true;
			gdDialogColor.IsVisible = true;
		}

		/// <summary>
		/// 繰り返しグリッドクリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickRepeatGrid(object sender, EventArgs e) {
			cvDialogBack.IsVisible = true;
			gdDialogRepeat.IsVisible = true;
		}

		/// <summary>
		/// 繰り返しキャンセルクリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickRepeatCancel(object sender, EventArgs e) {
			this.viewModel.RepeatDiv = RepeatDiv.繰り返しなし;
			Resources[InputViewModel.RepeatTextKey] = StringConst.RepeatNone;
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

				case PageDiv.確認:
					switch (this.tableDiv) {

						case TableDiv.タスク:
							Application.Current.MainPage = new ConfirmPage(this.selectedDate, 
								TableDiv.タスク, PageDiv.カレンダー, this.id);
							break;

						case TableDiv.プロジェクト:
							break;
					}
					break;

				case PageDiv.入力:
					Application.Current.MainPage = new InputPage(this.selectedDate,
						TableDiv.不明, PageDiv.カレンダー, ExecuteDiv.不明, this.id);
					break;
			}
		}

		/// <summary>
		/// データを一時保存して戻ってきた場合に、各値を再設定する。
		/// </summary>
		/// <returns>一時保存データ（なければ null）</returns>
		private BaseEvent ResetValuesAfterTempSave() {

			if (this.executeDiv != ExecuteDiv.不明) return null;

			this.tableDiv = TableDiv.予定;
			this.executeDiv = ExecuteDiv.更新;

			var tempScheduleDao = new TempScheduleDao();
			TempSchedule tempSchedule = tempScheduleDao.GetTempScheduleById(id);

			if (tempSchedule != null) {
				this.id = tempSchedule.ScheduleId;
				return tempSchedule;
			}

			var tempTaskDao = new TempTaskDao();
			TempTask tempTask = tempTaskDao.GetTempTaskById(id);

			if (tempTask != null) {
				this.tableDiv = TableDiv.タスク;
				this.id = tempTask.TaskId;
				return tempTask;
			}

			return null;
		}

		/// <summary>
		/// 各コントロールの表示非表示を切り替える。
		/// </summary>
		private void SetControlsVisibility() {

			switch (this.tableDiv) {

				case TableDiv.予定:
					gdTime.IsVisible = true;
					gdAllDay.IsVisible = true;
					gdProject.IsVisible = true;
					etPlace.IsVisible = true;
					break;

				case TableDiv.タスク:
					gdDate.IsVisible = true;
					gdExpectedDailyWorkTime.IsVisible = true;
					gdProject.IsVisible = true;
					break;

				case TableDiv.プロジェクト:
					gdDate.IsVisible = true;
					gdSales.IsVisible = true;
					gdColor.IsVisible = true;
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
			else if (this.tableDiv == TableDiv.タスク || this.tableDiv == TableDiv.プロジェクト) {
				if (this.viewModel.StartDate.CompareTo(this.viewModel.EndDate) == 1) {

					Device.BeginInvokeOnMainThread((async () => {
						await DisplayAlert(StringConst.DialogTitleError,
							String.Format(StringConst.MessageWrongDateTime, "開始日", "終了日"),
							StringConst.DialogAnswerPositive);
					}));

					return false;
				}
			}

			if (this.tableDiv == TableDiv.タスク) {
				double expectedDailyWorkTimeNum;

				// 予定毎日作業時間の数値変換チェック
				if (!double.TryParse(this.viewModel.ExpectedDailyWorkTimeText, out expectedDailyWorkTimeNum)) {

					Device.BeginInvokeOnMainThread((async () => {
						await DisplayAlert(StringConst.DialogTitleError,
							String.Format(StringConst.MessageWrongType, "作業時間", "数字"), 
							StringConst.DialogAnswerPositive);
					}));

					return false;
				}

				// 予定毎日作業時間の整数の最大値チェック
				if (expectedDailyWorkTimeNum > 24) {

					Device.BeginInvokeOnMainThread((async () => {
						await DisplayAlert(StringConst.DialogTitleError,
							String.Format(StringConst.MessageWrongType, "作業時間", "24以下の数字"),
							StringConst.DialogAnswerPositive);
					}));

					return false;
				}

				// 予定毎日作業時間の正の数チェック
				if (expectedDailyWorkTimeNum < 0) {

					Device.BeginInvokeOnMainThread((async () => {
						await DisplayAlert(StringConst.DialogTitleError,
							String.Format(StringConst.MessageWrongType, "作業時間", "0以上の数字"),
							StringConst.DialogAnswerPositive);
					}));

					return false;
				}

				// 予定毎日作業時間の小数点以下桁数チェック
				string expectedDailyWorkTimeStr = expectedDailyWorkTimeNum.ToString().TrimEnd('0');
				int index = expectedDailyWorkTimeStr.IndexOf('.');
				if (index != -1 && expectedDailyWorkTimeStr.Substring(index + 1).Length > 2) {

					Device.BeginInvokeOnMainThread((async () => {
						await DisplayAlert(StringConst.DialogTitleError,
							String.Format(StringConst.MessageWrongType, "作業時間", "小数点以下第二位までの数字"),
							StringConst.DialogAnswerPositive);
					}));

					return false;
				}
			}
			else if (this.tableDiv == TableDiv.プロジェクト) {

				// 予定売上の整数チェック
				if (!ValidateSales(1)) return false;

				// 実売上の整数チェック
				if (!ValidateSales(2)) return false;
			}

			if (this.tableDiv == TableDiv.予定 || this.tableDiv == TableDiv.タスク) {

				// プロジェクト入力チェック
				if (this.viewModel.ProjectId == InputViewModel.NotSelectedProjectId) {

					Device.BeginInvokeOnMainThread((async () => {
						await DisplayAlert(StringConst.DialogTitleError,
							String.Format(StringConst.MessageEntryNeeded, StringConst.WordProject), StringConst.DialogAnswerPositive);
					}));

					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// 売上を入力検証する。
		/// </summary>
		/// <param name="div">売上区分</param>
		/// <returns>検証OKなら「true」、NGなら「false」</returns>
		private bool ValidateSales(int div) {

			string sales = "";
			string salesName = "";

			switch (div) {
				case 1:
					sales = this.viewModel.ExpectedSalesText;
					salesName = "予定売上";
					if (string.IsNullOrEmpty(sales)) {
						this.viewModel.ExpectedSalesText = "0";
						return true;
					}
					break;
				case 2:
					sales = this.viewModel.SalesText;
					salesName = "実売上";
					if (string.IsNullOrEmpty(sales)) {
						this.viewModel.SalesText = "0";
						return true;
					}
					break;
			}

			long salesNum;

			// 売上の整数変換チェック
			if (!long.TryParse(sales, out salesNum)) {

				Device.BeginInvokeOnMainThread(async () => {
					await DisplayAlert(StringConst.DialogTitleError,
						string.Format(StringConst.MessageWrongType, salesName, "整数"),
						StringConst.DialogAnswerPositive);
				});

				return false;
			}

			// 売上の整数の最大値チェック
			if (salesNum > 999999999999) {

				Device.BeginInvokeOnMainThread(async () => {
					await DisplayAlert(StringConst.DialogTitleError,
						string.Format(StringConst.MessageWrongType, salesName, "999,999,999,999以下の整数"),
						StringConst.DialogAnswerPositive);
				});

				return false;
			}

			// 売上の正の数チェック
			if (salesNum < 0) {

				Device.BeginInvokeOnMainThread(async () => {
					await DisplayAlert(StringConst.DialogTitleError,
						string.Format(StringConst.MessageWrongType, salesName, "0以上の整数"),
						StringConst.DialogAnswerPositive);
				});

				return false;
			}

			return true;
		}

		/// <summary>
		/// プロジェクトと所属メンバーを登録または更新する。
		/// </summary>
		private void InsertOrUpdateProjectAndProjectMembers() {

			long id = (this.executeDiv == ExecuteDiv.追加) ? 0 : this.id;

			var project = new Project {
				Id = id,
				Title = this.viewModel.TitleText,
				ExpectedStartDate = this.viewModel.StartDate,
				ExpectedEndDate = this.viewModel.EndDate,
				ColorDiv = this.viewModel.ColorDiv,
				Notes = this.viewModel.NotesText,
				ExpectedSales = long.Parse(this.viewModel.ExpectedSalesText),
				Sales = long.Parse(this.viewModel.SalesText),
			};

			var projectDao = new ProjectDao();
			var projectMemberDao = new ProjectMemberDao();

			// プロジェクトの登録または更新
			if (this.executeDiv == ExecuteDiv.追加) {
				id = projectDao.Insert(project);
			}
			else if (this.executeDiv == ExecuteDiv.更新) {
				id = projectDao.Update(project);

				// 前回のプロジェクトメンバーの削除
				projectMemberDao.DeleteAllByProjectId(id);
			}

			// プロジェクトメンバーの登録
			foreach (long memberId in this.viewModel.MemberIdList) {

				var projectMember = new ProjectMember {
					ProjectId = id,
					MemberId = memberId,
					CanEdit = true // 仮に全員が編集可能に
				};

				projectMemberDao.Insert(projectMember);
			}
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
			var taskProgressDao = new TaskProgressDao();

			var lastTaskMemberList = new List<TaskMember>();
			var currentTaskMemberMap = new Dictionary<long, TaskMember>();

			// タスクの登録または更新
			if (this.executeDiv == ExecuteDiv.追加) {
				this.id = taskDao.Insert(task);
			}
			else if (this.executeDiv == ExecuteDiv.更新) {
				Task lastTask = taskDao.GetTaskById(this.id);

				task.TotalWorkTime = lastTask.TotalWorkTime;
				task.ProgressRate = lastTask.ProgressRate;

				this.id = taskDao.Update(task);

				lastTaskMemberList = taskMemberDao.GetTaskMemberListByTaskId(this.id);

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

			List<TaskMember> currentTaskMemberList = taskMemberDao.GetTaskMemberListByTaskId(this.id);
			foreach (TaskMember currentTaskMember in currentTaskMemberList) {
				currentTaskMemberMap.Add(currentTaskMember.MemberId, currentTaskMember);
			}

			// タスク進捗の更新または削除
			foreach (TaskMember lastTaskMember in lastTaskMemberList) {
				List<TaskProgress> taskProgressList = taskProgressDao.GetTaskProgressListByTaskMemberId(lastTaskMember.Id);

				if (currentTaskMemberMap.ContainsKey(lastTaskMember.MemberId)) {
					TaskMember currentTaskMember = currentTaskMemberMap[lastTaskMember.MemberId];

					foreach (TaskProgress taskProgress in taskProgressList) {
						taskProgress.TaskMemberId = currentTaskMember.Id;
						taskProgressDao.Update(taskProgress);
					}
				}
				else {
					foreach (TaskProgress taskProgress in taskProgressList) {
						taskProgressDao.Delete(taskProgress.TaskMemberId);
					}
				}
			}

			// TODO プロジェクトの予定時間を再計算・更新
		}

		/// <summary>
		/// 一時保存スケジュールと所属メンバーを登録する。
		/// </summary>
		/// <returns>一時保存スケジュールID</returns>
		private long InsertTempScheduleAndTempScheduleMembers() {

			DateTime startDate = this.selectedDate.Date;
			DateTime endDate = this.selectedDate.Date;
			if (!swAllDay.IsToggled) {
				startDate += this.viewModel.StartTime;
				endDate += this.viewModel.EndTime;
			}

			var tempSchedule = new TempSchedule {
				ScheduleId = this.id,
				Title = this.viewModel.TitleText,
				StartDate = startDate,
				EndDate = endDate,
				isAllDay = swAllDay.IsToggled,
				ProjectId = this.viewModel.ProjectId,
				RepeatDiv = this.viewModel.RepeatDiv,
				Place = this.viewModel.PlaceText,
				Notes = this.viewModel.NotesText
			};

			var tempScheduleDao = new TempScheduleDao();
			var tempScheduleMemberDao = new TempScheduleMemberDao();

			// 一時保存スケジュールの登録
			long id = tempScheduleDao.Insert(tempSchedule);

			// 一時保存スケジュールメンバーの登録
			foreach (long memberId in this.viewModel.MemberIdList) {

				var tempScheduleMember = new TempScheduleMember {
					ScheduleId = id,
					MemberId = memberId,
					CanEdit = true // 仮に全員が編集可能に
				};

				tempScheduleMemberDao.Insert(tempScheduleMember);
			}

			return id;
		}

		/// <summary>
		/// 一時保存タスクと所属メンバーを登録する。
		/// </summary>
		/// <returns>一時保存タスクID</returns>
		private long InsertTempTaskAndTempTaskMembers() {

			int expectedDailyWorkSeconds = 0;
			if (!string.IsNullOrEmpty(this.viewModel.ExpectedDailyWorkTimeText)) {
				expectedDailyWorkSeconds = (int)Math.Truncate(double.Parse(this.viewModel.ExpectedDailyWorkTimeText) * 3600);
			}

			var tempTask = new TempTask {
				TaskId = this.id,
				Title = this.viewModel.TitleText,
				StartDate = this.viewModel.StartDate,
				EndDate = this.viewModel.EndDate,
				ExpectedDailyWorkTime = new TimeSpan(0, 0, expectedDailyWorkSeconds),
				ProjectId = this.viewModel.ProjectId,
				Notes = this.viewModel.NotesText
			};

			var tempTaskDao = new TempTaskDao();
			var tempTaskMemberDao = new TempTaskMemberDao();

			// 一時保存タスクの登録
			long id = tempTaskDao.Insert(tempTask);

			// 一時保存タスクメンバーの登録
			foreach (long memberId in this.viewModel.MemberIdList) {

				var tempTaskMember = new TempTaskMember {
					TaskId = id,
					MemberId = memberId,
					CanEdit = true // 仮に全員が編集可能に
				};

				tempTaskMemberDao.Insert(tempTaskMember);
			}

			return id;
		}
	}
}