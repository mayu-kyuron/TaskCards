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

		public string TopDateText { get; set; }
		public string TitleText { get; set; }
		public string ProjectText { get; set; }
		public string StartTimeText { get; set; }
		public string EndTimeText { get; set; }
		public string AddText { get; set; }
		public string NotesText { get; set; }
		public string ExpectedSalesText { get; set; }

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
		/// 作業時間のスタックレイアウトを取得する。
		/// </summary>
		/// <param name="task">タスク</param>
		/// <param name="taskProgressList">タスク進捗リスト</param>
		/// <returns>スタックレイアウト</returns>
		private StackLayout GetWorkTimeStackLayout(Task task, List<TaskProgress> taskProgressList) {

			// スタックレイアウトを生成
			var stackLayout = new StackLayout {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
			};

			// 作業時間のラベルを生成
			var workTimeLabel = new Label {
				Text = task.ExpectedDailyWorkTime.TotalHours.ToString() + StringConst.ItemTextDailyTime,
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
		/// メンバーのスタックレイアウトを取得する。
		/// </summary>
		/// <param name="task">タスク</param>
		/// <param name="taskMember">タスクメンバー</param>
		/// <param name="taskProgressList">タスク進捗リスト</param>
		/// <returns>スタックレイアウト</returns>
		private StackLayout GetMemberStackLayout(Task task, TaskMember taskMember, List<TaskProgress> taskProgressList) {

			// スタックレイアウトを生成
			var stackLayout = new StackLayout {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
			};

			// メンバーのラベルを生成
			var memberDao = new MemberDao();
			var memberLabel = new Label {
				Text = memberDao.GetMemberById(taskMember.MemberId).Name,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromHex("#2F4F4F"),
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			};

			// 両端寄せ用の間のラベルを生成
			var fullyJustifyingLabel = new Label {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};

			// TODO 仮にタスク全体の進捗率を設定
			// 進捗率と日数のラベルを生成
			var progressAndDaysLabel = new Label {
				Text = task.ProgressRate + "％ / " + taskProgressList.Count + "日",
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