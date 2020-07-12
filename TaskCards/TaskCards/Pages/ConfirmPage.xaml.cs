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
	/// 確認ページ
	/// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConfirmPage : ContentPage {

		private ConfirmViewModel viewModel;

		DateTime selectedDate = DateTime.Today; // 選択された日付
		TableDiv tableDiv; // データ登録先のテーブル区分
		PageDiv exPageDiv; // 元のページ区分
		long id; // ID

		public ConfirmPage() {
			Initialize();
		}

		public ConfirmPage(DateTime selectedDate, TableDiv tableDiv, PageDiv exPageDiv, long id) {
			this.selectedDate = selectedDate;
			this.tableDiv = tableDiv;
			this.exPageDiv = exPageDiv;
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

			var tgrEdit = new TapGestureRecognizer();
			tgrEdit.Tapped += (sender, e) => OnClickEdit(sender, e);
			gdEditNormal.GestureRecognizers.Add(tgrEdit);
			gdEditProject.GestureRecognizers.Add(tgrEdit);

			var tgrDelete = new TapGestureRecognizer();
			tgrDelete.Tapped += (sender, e) => OnClickDelete(sender, e);
			gdDeleteNormal.GestureRecognizers.Add(tgrDelete);
			gdDeleteProject.GestureRecognizers.Add(tgrDelete);

			var tgrAdd = new TapGestureRecognizer();
			tgrAdd.Tapped += (sender, e) => OnClickAdd(sender, e);
			gdAdd.GestureRecognizers.Add(tgrAdd);

			// 各コントロールの表示切り替え
			switch (this.tableDiv) {
				case TableDiv.タスク:
					break;
				case TableDiv.プロジェクト:
					gdSales.IsVisible = true;
					break;
			}
		}

		/// <summary>
		/// 画面サイズ変更イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void OnSizeChanged(object sender, EventArgs args) {
			this.viewModel = new ConfirmViewModel(this.tableDiv, this.id, Height, gdWorkTime, gdMember);
			BindingContext = this.viewModel;

			int rowHeight = (int)Math.Round(Height * LayoutRateConst.ListItemHeight);
			int rowCount = (this.tableDiv == TableDiv.プロジェクト) ? 3 : 2;

			double dialogHeight = rowHeight * rowCount + (rowCount - 1);
			double dialogMarginTop = gdHeader.Height + Height / 62;

			// ダイアログの高さを設定
			gdDialogBack.RowDefinitions.Add(new RowDefinition { Height = dialogMarginTop });
			gdDialogBack.RowDefinitions.Add(new RowDefinition { Height = dialogHeight });
			gdDialogBack.RowDefinitions.Add(new RowDefinition { Height = Height - (dialogMarginTop + dialogHeight) });
		}

		/// <summary>
		/// ダイアログ背面クリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickDialogBack(object sender, EventArgs e) {
			cvDialogBack.IsVisible = false;
			gdDialogOptionProject.IsVisible = false;
			gdDialogOptionNormal.IsVisible = false;
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
		/// 右上ボタン（メニューボタン）クリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickTopRightButton(object sender, EventArgs e) {

			cvDialogBack.IsVisible = true;

			if (this.tableDiv == TableDiv.プロジェクト) {
				gdDialogOptionProject.IsVisible = true;
			}
			else {
				gdDialogOptionNormal.IsVisible = true;
			}
		}

		/// <summary>
		/// 編集クリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickEdit(object sender, EventArgs e) {

			switch (this.tableDiv) {

				case TableDiv.タスク: {
						var taskDao = new TaskDao();
						Task task = taskDao.GetTaskById(this.id);

						// タスク編集用の入力ページに遷移
						Application.Current.MainPage = new InputPage(task.StartDate,
							TableDiv.タスク, PageDiv.確認, ExecuteDiv.更新, this.id);
					}
					break;

				case TableDiv.プロジェクト: {
						var projectDao = new ProjectDao();
						Project project = projectDao.GetProjectById(this.id);

						// プロジェクト編集用の入力ページに遷移
						Application.Current.MainPage = new InputPage(project.ExpectedStartDate,
							TableDiv.プロジェクト, PageDiv.確認, ExecuteDiv.更新, this.id);
					}
					break;
			}
		}

		/// <summary>
		/// 削除クリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickDelete(object sender, EventArgs e) {

			// マイプロジェクトは削除させない。
			if (this.tableDiv == TableDiv.プロジェクト && this.id == 1) {
				Device.BeginInvokeOnMainThread(async () => {
					await DisplayAlert(StringConst.DialogTitleError,
						StringConst.MessageDeleteMyProjectError, StringConst.DialogAnswerPositive);
				});
				return;
			}

			string messageArg0 = null;
			switch (this.tableDiv) {
				case TableDiv.タスク:
					messageArg0 = StringConst.WordTask;
					break;
				case TableDiv.プロジェクト:
					messageArg0 = StringConst.WordProject;
					break;
			}

			Device.BeginInvokeOnMainThread(async () => {
				var result = await DisplayAlert(StringConst.DialogTitleConfirm,
					string.Format(StringConst.MessageDeleteConfirm, messageArg0),
					StringConst.DialogAnswerPositive, StringConst.DialogAnswerNegative);

				if (result) {
					switch (this.tableDiv) {

						case TableDiv.タスク:
							DeleteTaskAndRelatedData(this.id);
							OnPageBack();
							break;

						case TableDiv.プロジェクト:
							DeleteProjectAndRelatedData(this.id);
							OnPageBack();
							break;
					}
				}
			});
		}

		/// <summary>
		/// 〜を追加するクリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickAdd(object sender, EventArgs e) {

			switch (this.tableDiv) {
				case TableDiv.タスク:
					// 作業記録入力ページに遷移
					Application.Current.MainPage = new InputWorkPage(this.id);
					break;
				case TableDiv.プロジェクト:
					// タスク追加用の入力ページに遷移
					Application.Current.MainPage = new InputPage(CalendarViewModel.selectedDate,
						TableDiv.タスク, PageDiv.確認, ExecuteDiv.追加, projectId: this.id);
					break;
			}
		}

		/// <summary>
		/// 前ページに戻る。
		/// </summary>
		private void OnPageBack() {

			switch (this.exPageDiv) {
				case PageDiv.カレンダー:
				case PageDiv.タスク:
					Application.Current.MainPage = new TaskCardsMasterDetailPage(new DetailPage());
					break;
			}
		}

		/// <summary>
		/// スケジュールとその関連データを削除する。
		/// </summary>
		/// <param name="scheduleId">スケジュールID</param>
		private void DeleteScheduleAndRelatedData(long scheduleId) {

			// スケジュールの削除
			var scheduleDao = new ScheduleDao();
			scheduleDao.Delete(scheduleId);

			// 全スケジュールメンバーの削除
			var scheduleMemberDao = new ScheduleMemberDao();
			scheduleMemberDao.DeleteAllByScheduleId(scheduleId);
		}

		/// <summary>
		/// タスクとその関連データを削除する。
		/// </summary>
		/// <param name="taskId">タスクID</param>
		private void DeleteTaskAndRelatedData(long taskId) {

			// タスクの削除
			var taskDao = new TaskDao();
			taskDao.Delete(taskId);

			// 全タスクメンバーの削除
			var taskMemberDao = new TaskMemberDao();
			List<TaskMember> taskMemberList = taskMemberDao.GetTaskMemberListByTaskId(taskId);
			taskMemberDao.DeleteAllByTaskId(taskId);

			// 全タスク進捗の削除
			var taskProgressDao = new TaskProgressDao();
			foreach (TaskMember taskMember in taskMemberList) {
				taskProgressDao.DeleteAllByTaskMemberId(taskMember.Id);
			}
		}

		/// <summary>
		/// プロジェクトとその関連データを削除する。
		/// </summary>
		/// <param name="projectId">プロジェクトID</param>
		private void DeleteProjectAndRelatedData(long projectId) {

			// 全スケジュールと関連データの削除
			var scheduleDao = new ScheduleDao();
			List<Schedule> scheduleList = scheduleDao.GetScheduleListByProjectId(projectId);
			foreach (Schedule schedule in scheduleList) {
				DeleteScheduleAndRelatedData(schedule.Id);
			}

			// 全タスクと関連データの削除
			var taskDao = new TaskDao();
			List<Task> taskList = taskDao.GetTaskListByProjectId(projectId);
			foreach (Task task in taskList) {
				DeleteTaskAndRelatedData(task.Id);
			}

			// プロジェクトの削除
			var projectDao = new ProjectDao();
			projectDao.Delete(projectId);

			// 全プロジェクトメンバーの削除
			var projectMemberDao = new ProjectMemberDao();
			projectMemberDao.DeleteAllByProjectId(projectId);
		}
	}
}