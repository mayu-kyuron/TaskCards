﻿using System;
using System.Collections.Generic;
using System.Linq;
using TaskCards.Consts;
using TaskCards.Dao;
using TaskCards.Divisions;
using TaskCards.Entities;
using TaskCards.MasterPages;
using TaskCards.Utilities;
using TaskCards.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskCards.Pages {

	/// <summary>
	/// タスクページ（タブ2）
	/// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TaskPage : ContentPage {

		TaskViewModel viewModel;

		bool isFirstShow = true; // 最初の表示かどうか

		public TaskPage () {
			Initialize();
		}

		/// <summary>
		/// 初期化する。
		/// </summary>
		private void Initialize() {
			InitializeComponent();

			SizeChanged += OnSizeChanged;
		}

		/// <summary>
		/// 画面サイズ変更イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void OnSizeChanged(object sender, EventArgs args) {

			if (!this.isFirstShow) {

				this.viewModel = new TaskViewModel(Height);
				BindingContext = this.viewModel;

				// プロジェクト一覧を生成
				SetProjects();
			}

			this.isFirstShow = false;
		}

		/// <summary>
		/// プロジェクトタイトルクリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickProjectTitle(object sender, EventArgs e) {

			long projectId = long.Parse(((Label)sender).ClassId);

			// プロジェクトの確認ページに遷移
			Application.Current.MainPage = new ConfirmPage(CalendarViewModel.selectedDate,
				TableDiv.プロジェクト, PageDiv.タスク, projectId);
		}

		/// <summary>
		/// タスク追加ボタンクリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickAddTask(object sender, EventArgs e) {

			long projectId = long.Parse(((Image)sender).ClassId);

			// タスク追加用の入力ページに遷移
			Application.Current.MainPage = new InputPage(CalendarViewModel.selectedDate,
				TableDiv.タスク, PageDiv.タスク, ExecuteDiv.追加, projectId: projectId);
		}

		/// <summary>
		/// タスクタイトルクリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickTaskTitle(object sender, EventArgs e) {

			long taskId = long.Parse(((Grid)sender).ClassId);

			// タスクの確認ページに遷移
			Application.Current.MainPage = new ConfirmPage(CalendarViewModel.selectedDate,
				TableDiv.タスク, PageDiv.タスク, taskId);
		}

		/// <summary>
		/// タスク進捗バークリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickTaskProgressBar(object sender, EventArgs e) {

			long taskId = long.Parse(((Grid)sender).ClassId);

			var taskProgressDao = new TaskProgressDao();
			TaskProgress lastTaskProgress = taskProgressDao.GetOrderedTaskProgressListByTaskId(taskId).LastOrDefault();
			if (lastTaskProgress == null) return;

			// 作業記録削除の確認ダイアログを表示
			ShowDeleteTaskProgressConfirmDialog(taskId, lastTaskProgress);
		}

		/// <summary>
		/// 作業記録削除の確認ダイアログを表示する。
		/// </summary>
		/// <param name="taskId">タスクID</param>
		/// <param name="taskProgress">タスク進捗データ</param>
		private void ShowDeleteTaskProgressConfirmDialog(long taskId, TaskProgress taskProgress) {

			var taskDao = new TaskDao();
			Task task = taskDao.GetTaskById(taskId);

			var projectDao = new ProjectDao();
			Project project = projectDao.GetProjectById(task.ProjectId);

			string startDateStr = taskProgress.StartDate.ToString(StringConst.TaskProgressDateTimeFormat);
			string endDateStr = taskProgress.EndDate.ToString(StringConst.InputConfirmTimeFormat);

			// 表示する作業記録の詳細を設定
			string taskProgressDescription = string.Format("{0:s}（{1:s}）\n{2:s}〜{3:s}\n{4:s} {5:s}％",
				task.Title, project.Title,
				startDateStr, endDateStr,
				StringConst.WordProgressRate, taskProgress.ProgressRate.ToString("0")
			);

			// ダイアログ表示
			Device.BeginInvokeOnMainThread(async () => {
				var result = await DisplayAlert(StringConst.DialogTitleConfirm,
					string.Format(StringConst.MessageDeleteTaskProgressConfirm, taskProgressDescription),
					StringConst.DialogAnswerPositive, StringConst.DialogAnswerNegative);

				// 作業記録を削除後、ページを再表示する。
				if (result) {
					DeleteLastTaskProgressAndUpdateParents(taskProgress, task, project);
					Application.Current.MainPage = new TaskCardsMasterDetailPage(new DetailPage());
				}
			});
		}

		/// <summary>
		/// プロジェクト群を設定する。
		/// </summary>
		private void SetProjects() {

			// TODO 仮のメンバーを初期選択に
			long memberId = 1;

			var projectDao = new ProjectDao();
			List<Project> projectList = projectDao.GetProjectListByMemberId(memberId);

			var taskDao = new TaskDao();

			int index = 0;
			foreach (Project entity in projectList) {

				List<Task> taskList = taskDao.GetTaskListByProjectId(entity.Id);

				int projectRowNum = GetRowNum(this.viewModel.GetProjectGridHeight(Height, taskList.Count));
				int marginRowNum = GetRowNum(Height * TaskViewModel.MarginVerticalRate);

				// プロジェクト項目を追加
				gdProjects.Children.Add(GetProjectGrid(entity, taskList), 0, 1, index, index + projectRowNum);
				gdProjects.Children.Add(new BoxView(), 0, 1, index + projectRowNum, index + projectRowNum + marginRowNum);

				index += projectRowNum + marginRowNum;
			}

			// 画面の高さに満たない場合、最下部に空白を追加
			if (this.viewModel.marginBottom > 0) {
				gdProjects.Children.Add(new BoxView(), 0, 1, index, index + GetRowNum(this.viewModel.marginBottom));
			}
		}

		/// <summary>
		/// プロジェクトグリッドを取得する。
		/// </summary>
		/// <param name="project">プロジェクトデータ</param>
		/// <param name="taskList">タスクリスト</param>
		/// <returns>グリッド</returns>
		private Grid GetProjectGrid(Project project, List<Task> taskList) {

			// ベースのグリッドを生成
			var baseGrid = new Grid {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				ColumnSpacing = 0,
				RowSpacing = 0,
				HeightRequest = this.viewModel.GetProjectGridHeight(Height, taskList.Count),
				BackgroundColor = LayoutUtility.GetColorByEndDate(project),
			};

			// タイトル用グリッドを生成
			var titleGrid = GetProjectTitleGrid(project);

			// タスク一覧用スタックレイアウトを生成
			var tasksGrid = GetTasksGrid(project, taskList);

			int titleRowNum = GetRowNum(Height * TaskViewModel.ProjectTitleHeightRate);
			int tasksRowNum = GetRowNum(this.viewModel.GetTasksGridHeight(Height, taskList.Count));

			// ベースのグリッドに項目を追加
			baseGrid.Children.Add(titleGrid, 0, 1, 0, titleRowNum);
			baseGrid.Children.Add(tasksGrid, 0, 1, titleRowNum, titleRowNum + tasksRowNum);

			return baseGrid;
		}

		/// <summary>
		/// プロジェクトタイトルグリッドを取得する。
		/// </summary>
		/// <param name="project">プロジェクトデータ</param>
		/// <returns>グリッド</returns>
		private Grid GetProjectTitleGrid(Project project) {

			// グリッドを生成
			var grid = new Grid {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				ColumnSpacing = 0,
				RowSpacing = 0,
			};

			// ラベルを生成
			var label = new Label {
				Text = project.Title,
				TextColor = Color.White,
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
				VerticalTextAlignment = TextAlignment.Center,
				LineBreakMode = LineBreakMode.TailTruncation,
				ClassId = project.Id.ToString(),
			};

			var tgrLabel = new TapGestureRecognizer();
			tgrLabel.Tapped += (sender, e) => OnClickProjectTitle(sender, e);
			label.GestureRecognizers.Add(tgrLabel);

			// 追加ボタンを生成
			var addImage = new Image {
				Source = ImageSource.FromResource(StringConst.ImageFolderPath + "btn_add.png"),
				Aspect = Aspect.AspectFit,
				ClassId = project.Id.ToString(),
			};

			var tgrAddImage = new TapGestureRecognizer();
			tgrAddImage.Tapped += (sender, e) => OnClickAddTask(sender, e);
			addImage.GestureRecognizers.Add(tgrAddImage);

			// グリッドに項目を追加
			grid.Children.Add(new BoxView(), 0, 1, 0, 1);
			grid.Children.Add(label, 1, 57, 0, 1);
			grid.Children.Add(addImage, 57, 65, 0, 1);

			return grid;
		}

		/// <summary>
		/// タスク群グリッドを取得する。
		/// </summary>
		/// <param name="project">プロジェクトデータ</param>
		/// <param name="taskList">タスクリスト</param>
		/// <returns>グリッド</returns>
		private Grid GetTasksGrid(Project project, List<Task> taskList) {

			double borderWidth = 2;

			// グリッドを生成
			var grid = new Grid {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				ColumnSpacing = 0,
				RowSpacing = 0,
				ColumnDefinitions = {
					new ColumnDefinition { Width = new GridLength(borderWidth) },
					new ColumnDefinition { Width = new GridLength(Width / 32 * 30 - borderWidth * 2) }, // TaskPage.xamlのColumnDefinitionsによる。
					new ColumnDefinition { Width = new GridLength(borderWidth) },
				},
			};

			// ボーダーを描写するため、背景白色のグリッドを生成
			var whiteGrid = new Grid {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				ColumnSpacing = 0,
				RowSpacing = 0,
				BackgroundColor = Color.White,
			};

			// 背景白色のグリッドに項目を追加
			int index = 0;
			foreach (Task entity in taskList) {
				whiteGrid.Children.Add(GetTaskGrid(project, entity), 0, index);
				index++;
			}
			if (taskList.Count == 0) whiteGrid.Children.Add(GetNoTaskGrid());

			// 下部ボーダー用ビューを生成
			var bottomBorderView = new BoxView { BackgroundColor = LayoutUtility.GetColorByEndDate(project) };

			int whiteGridRowNum = (int)Math.Round(
				this.viewModel.GetTasksGridHeight(Height, taskList.Count) / borderWidth, 0, MidpointRounding.AwayFromZero);

			// グリッドに項目を追加
			grid.Children.Add(whiteGrid, 1, 2, 0, whiteGridRowNum);
			grid.Children.Add(bottomBorderView, 1, 2, whiteGridRowNum, whiteGridRowNum + 1);

			return grid;
		}

		/// <summary>
		/// タスクなしグリッドを取得する。
		/// </summary>
		/// <returns>グリッド</returns>
		private Grid GetNoTaskGrid() {

			// グリッドを生成
			var grid = new Grid {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				ColumnSpacing = 0,
				RowSpacing = 0,
			};

			// ラベルを生成
			var label = new Label {
				Text = "該当データなし",
				TextColor = Color.Gray,
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
				VerticalTextAlignment = TextAlignment.Center,
			};

			// グリッドに項目を追加
			grid.Children.Add(new BoxView(), 0, 1, 0, 1);
			grid.Children.Add(label, 1, 64, 0, 1);
			grid.Children.Add(new BoxView(), 64, 65, 0, 1);

			return grid;
		}

		/// <summary>
		/// タスクグリッドを取得する。
		/// </summary>
		/// <param name="project">プロジェクトデータ</param>
		/// <param name="task">タスクデータ</param>
		/// <returns>グリッド</returns>
		private Grid GetTaskGrid(Project project, Task task) {

			// グリッドを生成
			var grid = new Grid {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				ColumnSpacing = 0,
				RowSpacing = 0,
			};

			// タイトル用グリッドを生成
			var titleGrid = GetTaskTitleGrid(task);

			var tgrTitleGrid = new TapGestureRecognizer();
			tgrTitleGrid.Tapped += (sender, e) => OnClickTaskTitle(sender, e);
			titleGrid.GestureRecognizers.Add(tgrTitleGrid);

			// タスク進捗用グリッドを生成
			var progressGrid = GetTaskProgressGrid(project, task);

			int titleRowNum = GetRowNum(Height * TaskViewModel.TaskTitleHeightRate);
			int progressRowNum = GetRowNum(Height * TaskViewModel.TaskProgressHeightRate);

			// グリッドに項目を追加
			grid.Children.Add(titleGrid, 0, 1, 0, titleRowNum);
			grid.Children.Add(progressGrid, 0, 1, titleRowNum, titleRowNum + progressRowNum);

			return grid;
		}

		/// <summary>
		/// タスクタイトルグリッドを取得する。
		/// </summary>
		/// <param name="task">タスクデータ</param>
		/// <returns>グリッド</returns>
		private Grid GetTaskTitleGrid(Task task) {

			// グリッドを生成
			var grid = new Grid {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				ColumnSpacing = 0,
				RowSpacing = 0,
				ClassId = task.Id.ToString(),
			};

			// タイトルラベルを生成
			var titleLabel = new Label {
				Text = task.Title,
				TextColor = Color.Black,
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
				VerticalTextAlignment = TextAlignment.Center,
				LineBreakMode = LineBreakMode.TailTruncation,
			};

			string dateFormat = "MM/dd";

			// 日付ラベルを生成
			var dateLabel = new Label {
				Text = task.StartDate.ToString(dateFormat) + "〜" + task.EndDate.ToString(dateFormat),
				TextColor = Color.Gray,
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
			};

			// グリッドに項目を追加
			grid.Children.Add(new BoxView(), 0, 1, 0, 1);
			grid.Children.Add(titleLabel, 1, 38, 0, 1);
			grid.Children.Add(new BoxView(), 38, 39, 0, 1);
			grid.Children.Add(dateLabel, 39, 64, 0, 1);
			grid.Children.Add(new BoxView(), 64, 65, 0, 1);

			return grid;
		}

		/// <summary>
		/// タスク進捗グリッドを取得する。
		/// </summary>
		/// <param name="project">プロジェクトデータ</param>
		/// <param name="task">タスクデータ</param>
		/// <returns>グリッド</returns>
		private Grid GetTaskProgressGrid(Project project, Task task) {

			// グリッドを生成
			var grid = new Grid {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				ColumnSpacing = 0,
				RowSpacing = 0,
			};

			// 進捗バー用グリッドを生成
			var progressBarGrid = GetTaskProgressBarGrid(project, task);

			var tgrProgressBarGrid = new TapGestureRecognizer();
			tgrProgressBarGrid.Tapped += (sender, e) => OnClickTaskProgressBar(sender, e);
			progressBarGrid.GestureRecognizers.Add(tgrProgressBarGrid);

			// グリッドに項目を追加
			grid.Children.Add(progressBarGrid, 0, 1, 0, 2);
			grid.Children.Add(new BoxView(), 0, 1, 2, 3);

			return grid;
		}

		/// <summary>
		/// タスク進捗バーグリッドを取得する。
		/// </summary>
		/// <param name="project">プロジェクトデータ</param>
		/// <param name="task">タスクデータ</param>
		/// <returns>グリッド</returns>
		private Grid GetTaskProgressBarGrid(Project project, Task task) {

			// グリッドを生成
			var grid = new Grid {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				ColumnSpacing = 0,
				RowSpacing = 0,
				BackgroundColor = Color.Beige,
				ClassId = task.Id.ToString(),
			};

			// タスクに紐づく、全タスク進捗リストを登録順で取得
			var taskProgressDao = new TaskProgressDao();
			List<TaskProgress> taskProgressList = taskProgressDao.GetOrderedTaskProgressListByTaskId(task.Id);

			int index = 0;
			TaskProgress lastTaskProgress = null;
			bool isOdd = true;
			foreach (TaskProgress taskProgress in taskProgressList) {

				// 進捗率が前回より下回っている場合は、何もしない。
				if (lastTaskProgress != null) {
					if (taskProgress.ProgressRate <= lastTaskProgress.ProgressRate) continue;
				}

				int columnNum = (lastTaskProgress == null) ?
					(int)taskProgress.ProgressRate : (int)(taskProgress.ProgressRate - lastTaskProgress.ProgressRate);

				// グリッドに項目を追加
				grid.Children.Add(GetTaskProgressLabel(project, taskProgress, isOdd), index, index + columnNum, 0, 1);

				index += columnNum;
				lastTaskProgress = taskProgress;
				isOdd = isOdd ? false : true;
			}

			// グリッド末尾に空白を追加
			if (index < 100) grid.Children.Add(new BoxView(), index, index + (100 - index), 0, 1);

			return grid;
		}

		/// <summary>
		/// タスク進捗ラベルを取得する。
		/// </summary>
		/// <param name="project">プロジェクトデータ</param>
		/// <param name="taskProgress">タスク進捗データ</param>
		/// <param name="isOdd">奇数番目か否か</param>
		/// <returns>ラベル</returns>
		private Label GetTaskProgressLabel(Project project, TaskProgress taskProgress, bool isOdd) {

			var memberDao = new MemberDao();
			var taskMemberDao = new TaskMemberDao();

			TaskMember taskMember = taskMemberDao.GetTaskMemberById(taskProgress.TaskMemberId);

			// 並べ順に交互に背景色を変える。
			Color backgroundColor = isOdd ? LayoutUtility.GetColorByEndDate(project) : LayoutUtility.GetLightColorByEndDate(project);

			return new Label {
				Text = " " + memberDao.GetMemberById(taskMember.MemberId).Name,
				TextColor = Color.White,
				BackgroundColor = backgroundColor,
				FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
				VerticalTextAlignment = TextAlignment.Center,
				LineBreakMode = LineBreakMode.TailTruncation,
			};
		}

		/// <summary>
		/// 高さをグリッド行の幅用に変換して取得する。
		/// </summary>
		/// <param name="height">高さ</param>
		/// <returns>変換後の整数</returns>
		private int GetRowNum(double height) {
			return (int)Math.Round(height / 15, 0, MidpointRounding.AwayFromZero);
		}

		/// <summary>
		/// 前回のタスク進捗を削除し、その親データ群を更新する。
		/// </summary>
		/// <param name="taskProgress">タスク進捗データ</param>
		/// <param name="task">タスクデータ</param>
		/// <param name="project">プロジェクトデータ</param>
		private void DeleteLastTaskProgressAndUpdateParents(TaskProgress taskProgress, Task task, Project project) {

			// 前回のタスク進捗を削除
			var taskProgressDao = new TaskProgressDao();
			taskProgressDao.Delete(taskProgress.Id);

			// タスクに紐づく、全タスク進捗リストを登録順で取得
			List<TaskProgress> taskProgressList = taskProgressDao.GetOrderedTaskProgressListByTaskId(task.Id);

			// 過去の作業時間を加算し、現時点の総作業時間を取得
			var totalWorkTime = new TimeSpan(0, 0, 0);
			foreach (TaskProgress pastTaskProgress in taskProgressList) {
				totalWorkTime += pastTaskProgress.EndDate - pastTaskProgress.StartDate;
			}

			// 前前回のタスク進捗を取得
			TaskProgress lastTaskProgress = taskProgressList.LastOrDefault();

			// タスクを更新
			var taskDao = new TaskDao();
			task.TotalWorkTime = totalWorkTime;
			task.ProgressRate = (lastTaskProgress == null) ? 0 : lastTaskProgress.ProgressRate;
			task.Notes = null;
			taskDao.Update(task);

			// プロジェクトを更新
			if (taskProgressList.Count == 0) {
				var projectDao = new ProjectDao();
				project.StartDate = new DateTime();
				projectDao.Update(project);
			}
		}
	}
}