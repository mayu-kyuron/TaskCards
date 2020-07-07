using System;
using System.Collections.Generic;
using TaskCards.Consts;
using TaskCards.Dao;
using TaskCards.Divisions;
using TaskCards.Entities;
using Xamarin.Forms;

namespace TaskCards.ViewModels {

	/// <summary>
	/// 確認ページのビューモデル
	/// </summary>
	public class ConfirmViewModel {

		public double DialogBaseHeight { get; set; }

		public ImageSource BackButtonSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "btn_back.png");
		public ImageSource TopRightButtonSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "btn_menu.png");
		public ImageSource InputConfirmDateTimeArrowSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "date_time_right_arrow.png");
		public ImageSource InputConfirmArrowIconSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "input_right_arrow.png");
		public ImageSource InputConfirmAddIconSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "btn_add.png");
		public ImageSource InputConfirmSalesArrowSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "input_right_arrow.png");

		public string TopDateText { get; set; }
		public string TitleText { get; set; }
		public string ProjectText { get; set; }
		public string StartTimeText { get; set; }
		public string EndTimeText { get; set; }
		public string AddText { get; set; }
		public string NotesText { get; set; }
		public string ExpectedSalesText { get; set; }
		public string SalesText { get; set; }

		public long ProjectId { get; set; }
		public List<long> MemberIdList { get; set; } = new List<long>();

		public ConfirmViewModel(TableDiv tableDiv, long id, double height, Grid gdWorkTime, Grid gdMember) {

			// レイアウト全体の高さの設定
			// 項目を増やすごとに、デバイスの高さに対して項目の高さ分、掛ける割合を増やしていく。
			// 比率の計算方法　変更前のレイアウト全体の高さ:変更前の比率＝変更後のレイアウト全体の高さ:今回の比率
			// レイアウト全体の高さは、<Grid.RowDefinitions> RowDefinitionのHeightの合計
			DialogBaseHeight = height * 1;

			// テーブル区分ごとに項目を設定
			switch (tableDiv) {
				case TableDiv.タスク:
					SetTask(id, gdWorkTime, gdMember);
					break;
				case TableDiv.プロジェクト:
					SetProject(id, gdWorkTime, gdMember);
					break;
			}
		}

		/// <summary>
		/// タスクを設定する。
		/// </summary>
		/// <param name="id">ID</param>
		/// <param name="gdWorkTime">作業時間のグリッド</param>
		/// <param name="gdMember">メンバーのグリッド</param>
		private void SetTask(long id, Grid gdWorkTime, Grid gdMember) {

			var taskDao = new TaskDao();
			Task task = taskDao.GetTaskById(id);

			var projectDao = new ProjectDao();
			Project project = projectDao.GetProjectById(task.ProjectId);

			var taskMemberDao = new TaskMemberDao();
			List<TaskMember> taskMemberList = taskMemberDao.GetTaskMemberListByTaskId(id);

			// 過去のタスク進捗をすべて取得
			var taskProgressList = new List<TaskProgress>();
			var taskProgressDao = new TaskProgressDao();
			foreach (TaskMember taskMember in taskMemberList) {
				taskProgressList.AddRange(taskProgressDao.GetTaskProgressListByTaskMemberId(taskMember.Id));
			}

			TopDateText = task.StartDate.ToString(StringConst.DateTappedDialogDateFormat);
			ProjectId = project.Id;
			ProjectText = project.Title;
			TitleText = task.Title;
			StartTimeText = task.StartDate.ToString(StringConst.InputConfirmDateFormat);
			EndTimeText = task.EndDate.ToString(StringConst.InputConfirmDateFormat);
			AddText = String.Format(StringConst.MessageAdd, "作業記録");
			NotesText = task.Notes;

			// 作業時間のグリッドに要素を追加
			gdWorkTime.Children.Add(GetWorkTimeStackLayout(task, taskProgressList));
			AddBlankTo3ItemsGrid(1, gdWorkTime);

			// メンバーのグリッドに要素を追加
			int taskMemberIndex = 0;
			foreach (TaskMember taskMember in taskMemberList) {

				gdMember.Children.Add(GetMemberStackLayout(task, taskMember, 
					taskProgressDao.GetTaskProgressListByTaskMemberId(taskMember.Id)), 0, taskMemberIndex);

				taskMemberIndex++;
			}
			AddBlankTo3ItemsGrid(taskMemberList.Count, gdMember);
		}

		/// <summary>
		/// プロジェクトを設定する。
		/// </summary>
		/// <param name="id">ID</param>
		/// <param name="gdWorkTime">作業時間のグリッド</param>
		/// <param name="gdMember">メンバーのグリッド</param>
		private void SetProject(long id, Grid gdWorkTime, Grid gdMember) {

			var projectDao = new ProjectDao();
			Project project = projectDao.GetProjectById(id);

			TopDateText = project.ExpectedStartDate.ToString(StringConst.DateTappedDialogDateFormat);
			TitleText = project.Title;
			StartTimeText = project.ExpectedStartDate.ToString(StringConst.InputConfirmDateFormat);
			EndTimeText = project.ExpectedEndDate.ToString(StringConst.InputConfirmDateFormat);
			AddText = string.Format(StringConst.MessageAdd, StringConst.WordTask);
			NotesText = project.Notes;
			ExpectedSalesText = project.ExpectedSales.ToString();
			SalesText = project.Sales.ToString();

			// 作業時間のグリッドに要素を追加
			Dictionary<long, List<TaskProgress>> taskProgressMap = SetProjectWorkTime(id, gdWorkTime);

			// メンバーのグリッドに要素を追加
			SetProjectMember(id, taskProgressMap, gdMember);
		}

		/// <summary>
		/// プロジェクトの作業時間を設定する。
		/// </summary>
		/// <param name="projectId">プロジェクトID</param>
		/// <param name="gdWorkTime">作業時間のグリッド</param>
		/// <returns>タスク進捗マップ（タスクID：登録順のタスク進捗リスト）</returns>
		private Dictionary<long, List<TaskProgress>> SetProjectWorkTime(long projectId, Grid gdWorkTime) {

			var taskProgressMap = new Dictionary<long, List<TaskProgress>>();

			var taskProgressDao = new TaskProgressDao();
			var taskDao = new TaskDao();
			List<Task> taskList = taskDao.GetTaskListByProjectId(projectId);

			int taskProgressIndex = 0;
			foreach (Task task in taskList) {

				// タスクに紐づく、全タスク進捗リストを登録順で取得
				List<TaskProgress> taskProgressList = taskProgressDao.GetOrderedTaskProgressListByTaskId(task.Id);
				taskProgressMap.Add(task.Id, taskProgressList);

				// 作業時間のグリッドに要素を追加
				gdWorkTime.Children.Add(GetWorkTimeStackLayout(task, taskProgressList, true), 0, taskProgressIndex);

				taskProgressIndex++;
			}

			// 作業時間のグリッドに不足分の空白を追加
			AddBlankTo3ItemsGrid(taskList.Count, gdWorkTime);

			return taskProgressMap;
		}

		/// <summary>
		/// 作業時間のスタックレイアウトを取得する。
		/// </summary>
		/// <param name="task">タスク</param>
		/// <param name="taskProgressList">タスク進捗リスト</param>
		/// <param name="isNeededTitle">（オプション）タイトル表示が必要か否か。デフォルトは false 。</param>
		/// <returns>スタックレイアウト</returns>
		private StackLayout GetWorkTimeStackLayout(Task task, List<TaskProgress> taskProgressList, bool isNeededTitle = false) {

			// スタックレイアウトを生成
			var stackLayout = new StackLayout {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
			};

			string workTimeLabelText = task.ExpectedDailyWorkTime.TotalHours.ToString() + StringConst.ItemTextDailyTime;
			if (isNeededTitle) workTimeLabelText = task.Title + "（" + workTimeLabelText + "）";

			// 作業時間のラベルを生成
			var workTimeLabel = new Label {
				Text = workTimeLabelText,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromHex("#2F4F4F"),
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			};

			// 両端寄せ用の間のラベルを生成
			var fullyJustifyingLabel = new Label {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};

			// 日数のラベルを生成
			var daysLabel = new Label {
				Text = "× " + taskProgressList.Count + "日",
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromHex("#2F4F4F"),
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			};

			stackLayout.Children.Add(workTimeLabel);
			stackLayout.Children.Add(fullyJustifyingLabel);
			stackLayout.Children.Add(daysLabel);

			return stackLayout;
		}

		/// <summary>
		/// プロジェクトのメンバーを設定する。
		/// </summary>
		/// <param name="projectId">プロジェクトID</param>
		/// <param name="taskProgressMap">タスク進捗マップ（タスクID：登録順のタスク進捗リスト）</param>
		/// <param name="gdMember">メンバーのグリッド</param>
		private void SetProjectMember(long projectId, Dictionary<long, List<TaskProgress>> taskProgressMap, Grid gdMember) {

			var taskMemberDao = new TaskMemberDao();
			var projectMemberDao = new ProjectMemberDao();
			List<ProjectMember> projectMemberList = projectMemberDao.GetProjectMemberListByProjectId(projectId);

			int projectMemberIndex = 0;
			int totalDateCount = 0;
			int totalProgressRate = 0;
			foreach (ProjectMember projectMember in projectMemberList) {

				// 全タスク中の該当メンバーによるタスク進捗を取得し、進捗率と日数をカウントする。
				int dateCount = 0;
				int progressRate = 0;
				foreach (List<TaskProgress> taskProgressList in taskProgressMap.Values) {
					for (int i = 0; i < taskProgressList.Count; i++) {

						TaskMember taskMember = taskMemberDao.GetTaskMemberById(taskProgressList[i].TaskMemberId);
						if (projectMember.MemberId != taskMember.MemberId) continue;

						int lastTaskProgressRate = (i == 0) ? 0 : (int)taskProgressList[i - 1].ProgressRate;

						dateCount++;
						progressRate += (int)(taskProgressList[i].ProgressRate - lastTaskProgressRate);
					}
				}

				if (taskProgressMap.Count != 0) progressRate /= taskProgressMap.Count;

				// メンバーのグリッドに要素を追加
				gdMember.Children.Add(GetMemberStackLayout(projectMember.MemberId, progressRate, dateCount), 0, projectMemberIndex);

				projectMemberIndex++;
				totalDateCount += dateCount;
				totalProgressRate += progressRate;
			}

			// メンバーのグリッドに合計値を追加
			gdMember.Children.Add(new Label {
				Text = "計 " + totalProgressRate + "％ / " + totalDateCount + "日",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = Color.FromHex("#2F4F4F"),
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			}, 0, projectMemberIndex);

			// メンバーのグリッドに不足分の空白を追加
			AddBlankTo3ItemsGrid(projectMemberList.Count + 1, gdMember);
		}

		/// <summary>
		/// メンバーのスタックレイアウトを取得する。
		/// </summary>
		/// <param name="memberId">メンバーID</param>
		/// <param name="rate">進捗率</param>
		/// <param name="days">日数</param>
		/// <returns>スタックレイアウト</returns>
		private StackLayout GetMemberStackLayout(long memberId, double rate, int days) {

			// スタックレイアウトを生成
			var stackLayout = new StackLayout {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
			};

			// メンバーのラベルを生成
			var memberDao = new MemberDao();
			var memberLabel = new Label {
				Text = memberDao.GetMemberById(memberId).Name,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromHex("#2F4F4F"),
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			};

			// 両端寄せ用の間のラベルを生成
			var fullyJustifyingLabel = new Label {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};

			// 進捗率と日数のラベルを生成
			var progressAndDaysLabel = new Label {
				Text = rate + "％ / " + days + "日",
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromHex("#2F4F4F"),
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			};

			stackLayout.Children.Add(memberLabel);
			stackLayout.Children.Add(fullyJustifyingLabel);
			stackLayout.Children.Add(progressAndDaysLabel);

			return stackLayout;
		}

		/// <summary>
		/// メンバーのスタックレイアウトを取得する。
		/// </summary>
		/// <param name="task">タスク</param>
		/// <param name="taskMember">タスクメンバー</param>
		/// <param name="taskProgressList">タスク進捗リスト</param>
		/// <returns>スタックレイアウト</returns>
		private StackLayout GetMemberStackLayout(Task task, TaskMember taskMember, List<TaskProgress> taskProgressList) {

			// TODO 仮にタスク全体の進捗率を設定
			return GetMemberStackLayout(taskMember.MemberId, task.ProgressRate, taskProgressList.Count);
		}

		/// <summary>
		/// ３項目用のグリッドに不足分の空白を追加する。
		/// </summary>
		/// <param name="childrenCount">子コントロールのカウント</param>
		/// <param name="grid">グリッド</param>
		private void AddBlankTo3ItemsGrid(int childrenCount, Grid grid) {

			if (childrenCount < 3) {
				grid.Children.Add(new StackLayout(), 0, 2);
				if (childrenCount < 2) grid.Children.Add(new StackLayout(), 0, 1);
			}
		}
	}
}