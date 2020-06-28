using System;
using System.Collections.Generic;
using System.Windows.Input;
using TaskCards.Consts;
using TaskCards.Dao;
using TaskCards.Divisions;
using TaskCards.Dtos;
using TaskCards.Entities;
using TaskCards.Utilities;
using Xamarin.Forms;

namespace TaskCards.ViewModels {

	/// <summary>
	/// 入力ページのビューモデル
	/// </summary>
	public class InputViewModel {

		public const long NotSelectedProjectId = -1;
		public const string RepeatTextKey = "RepeatText";
		public const string ProjectTextKey = "ProjectText";
		public const string ColorTextKey = "ColorText";

		public List<SelectedItemDto> RepeatItems { protected set; get; } = new List<SelectedItemDto>();
		public List<SelectedItemDto> ColorItems { protected set; get; } = new List<SelectedItemDto>();

		public ICommand SelectedRepeatCommand { get; set; }
		public ICommand SelectedProjectCommand { get; private set; }
		public ICommand SelectedColorCommand { get; private set; }

		public double DialogBaseHeight { get; set; }
		public double MemberHeight { get; set; }

		public ImageSource BackButtonSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "btn_back.png");
		public ImageSource TopRightButtonSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "btn_check.png");
		public ImageSource InputConfirmDateTimeArrowSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "date_time_right_arrow.png");
		public ImageSource InputConfirmArrowIconSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "input_right_arrow.png");
		public ImageSource InputConfirmSalesArrowSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "input_right_arrow.png");
		public ImageSource InputConfirmCancelIconSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "btn_cancel.png");
		public ImageSource InputConfirmAddIconSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "btn_add.png");

		public string TopDateText { get; set; }
		public string TitleText { get; set; }
		public string ExpectedDailyWorkTimeText { get; set; }
		public string ExpectedSalesText { get; set; }
		public string SalesText { get; set; }
		public string Member1Text { get; set; }
		public string AddText { get; set; }
		public string PlaceText { get; set; }
		public string NotesText { get; set; }
		public string AddProjectText { get; set; }

		public TimeSpan StartTime { get; set; }
		public TimeSpan EndTime { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public long ProjectId { get; set; }
		public List<long> MemberIdList { get; set; } = new List<long>();
		public RepeatDiv RepeatDiv { get; set; }
		public ColorDiv ColorDiv { get; set; }

		private ContentView cvDialogBack;
		private Grid gdDialogRepeat;
		private Grid gdDialogProject;
		private Grid gdDialogColor;
		private ResourceDictionary resources;

		public InputViewModel(DateTime selectedDate, TableDiv tableDiv, ExecuteDiv executeDiv, long id, BaseEvent tempEvent,
			double height, ContentView cvDialogBack, Grid gdDialogRepeat, Grid gdDialogProject, Grid gdProjects,
			Grid gdDialogColor, Switch swAllDay, ResourceDictionary resources) {

			this.cvDialogBack = cvDialogBack;
			this.gdDialogRepeat = gdDialogRepeat;
			this.gdDialogProject = gdDialogProject;
			this.gdDialogColor = gdDialogColor;
			this.resources = resources;

			// レイアウト全体の高さの設定
			// 項目を増やすごとに、デバイスの高さに対して項目の高さ分、掛ける割合を増やしていく。
			// 比率の計算方法　変更前のレイアウト全体の高さ:変更前の比率＝変更後のレイアウト全体の高さ:今回の比率
			// レイアウト全体の高さは、<Grid.RowDefinitions> RowDefinitionのHeightの合計
			DialogBaseHeight = height * 1;

			// メンバー追加欄の高さ（(画面の高さ - 上下マージン) / 全行数 * メンバーの行数）
			MemberHeight = (height - height / 62 * 2) / 14 * 3;

			TopDateText = selectedDate.ToString(StringConst.DateTappedDialogDateFormat);
			AddText = string.Format(StringConst.MessageAdd, StringConst.WordMember);
			AddProjectText = string.Format(StringConst.MessageAdd, StringConst.WordProject);

			// 各ダイアログ項目を設定
			List<Project> projectList = SetProjectsInDialog(gdProjects);
			SetColorItemsInDialog();
			SetRepeatItemsInDialog();

			// 各コマンドを設定
			SelectedProjectCommand = new Command<string>(OnSelectProject);
			SelectedColorCommand = new Command<SelectedItemDto>(OnSelectColor);
			SelectedRepeatCommand = new Command<SelectedItemDto>(OnSelectRepeat);

			// テーブル区分・実行区分ごとに項目を設定
			switch (tableDiv) {

				case TableDiv.予定:
					switch (executeDiv) {
						case ExecuteDiv.追加:
							SetAddScheduleTaskProject(tableDiv, selectedDate, projectList);
							break;
						case ExecuteDiv.更新:
							SetEditSchedule(id, swAllDay, tempEvent);
							break;
					}
					break;

				case TableDiv.タスク:
					switch (executeDiv) {
						case ExecuteDiv.追加:
							SetAddScheduleTaskProject(tableDiv, selectedDate, projectList);
							break;
						case ExecuteDiv.更新:
							SetEditTask(id, tempEvent);
							break;
					}
					break;

				case TableDiv.プロジェクト:
					switch (executeDiv) {
						case ExecuteDiv.追加:
							SetAddScheduleTaskProject(tableDiv, selectedDate, projectList);
							break;
						case ExecuteDiv.更新:
							SetEditProject(id);
							break;
					}
					break;
			}
		}

		/// <summary>
		/// プロジェクト選択イベント
		/// </summary>
		/// <param name="idStr">ID（文字列）</param>
		private void OnSelectProject(string idStr) {

			var dao = new ProjectDao();
			Project project = dao.GetProjectById(long.Parse(idStr));

			ProjectId = project.Id;
			this.resources[ProjectTextKey] = project.Title;

			this.cvDialogBack.IsVisible = false;
			this.gdDialogProject.IsVisible = false;
		}

		/// <summary>
		/// テーマカラー選択イベント
		/// </summary>
		/// <param name="selectedItemDto">選択項目DTO</param>
		private void OnSelectColor(SelectedItemDto selectedItemDto) {

			ColorDiv = (ColorDiv)selectedItemDto.Id;
			this.resources[ColorTextKey] = selectedItemDto.Name;

			this.cvDialogBack.IsVisible = false;
			this.gdDialogColor.IsVisible = false;
		}

		/// <summary>
		/// 繰り返し選択イベント
		/// </summary>
		/// <param name="selectedItemDto">選択項目DTO</param>
		private void OnSelectRepeat(SelectedItemDto selectedItemDto) {

			RepeatDiv = (RepeatDiv)selectedItemDto.Id;
			this.resources[RepeatTextKey] = selectedItemDto.Name;

			this.cvDialogBack.IsVisible = false;
			this.gdDialogRepeat.IsVisible = false;
		}

		/// <summary>
		/// スケジュール・タスク・プロジェクト追加時の項目を設定する。
		/// </summary>
		private void SetAddScheduleTaskProject(TableDiv tableDiv, DateTime selectedDate, List<Project> projectList) {

			if (tableDiv == TableDiv.予定) {

				// 開始時間：現在時間＋1時間－現在の分数（例：12:34→13:00）
				// 終了時間：現在時間＋2時間－現在の分数（例：12:34→14:00）
				StartTime = new TimeSpan(DateTime.Now.AddHours(1).Hour, 0, 0);
				EndTime = new TimeSpan(DateTime.Now.AddHours(2).Hour, 0, 0);
			}
			else if (tableDiv == TableDiv.タスク || tableDiv == TableDiv.プロジェクト) {

				// 開始日：選択日
				// 終了日：選択日＋7日
				StartDate = selectedDate;
				EndDate = selectedDate.AddDays(7);
			}

			// プロジェクトの初期値を設定
			if (tableDiv == TableDiv.予定 || tableDiv == TableDiv.タスク) {

				if (projectList.Count == 0) {
					ProjectId = NotSelectedProjectId;
					this.resources[ProjectTextKey] = string.Format(StringConst.MessageEntryNeeded, StringConst.WordProject);
				}
				else {
					ProjectId = projectList[0].Id;
					this.resources[ProjectTextKey] = projectList[0].Title;
				}
			}

			// テーマカラーの初期値を設定
			if (tableDiv == TableDiv.プロジェクト) {
				ColorDiv = ColorDiv.スカイブルー;
				this.resources[ColorTextKey] = GetColorText(ColorDiv);
			}

			// 繰り返しの初期値を設定
			if (tableDiv == TableDiv.予定) {
				RepeatDiv = RepeatDiv.繰り返しなし;
				this.resources[RepeatTextKey] = StringConst.RepeatNone;
			}

			// TODO 仮のメンバーを初期選択に
			long memberId = 1;
			MemberDao memberDao = new MemberDao();
			Member member = memberDao.GetMemberById(memberId);
			MemberIdList.Add(member.Id);
			Member1Text = member.Name;
		}

		/// <summary>
		/// スケジュール編集時の項目を設定する。
		/// </summary>
		/// <param name="id">ID</param>
		/// <param name="swAllDay">終日Switch</param>
		/// <param name="tempEvent">一時保存データ</param>
		private void SetEditSchedule(long id, Switch swAllDay, BaseEvent tempEvent) {

			Schedule schedule = null;

			if (tempEvent == null) {
				var scheduleDao = new ScheduleDao();
				schedule = scheduleDao.GetScheduleById(id);
			}
			else {
				schedule = tempEvent as Schedule;
				schedule.Id = (tempEvent as TempSchedule).ScheduleId;
			}

			ProjectDao projectDao = new ProjectDao();
			Project project = projectDao.GetProjectById(schedule.ProjectId);

			ScheduleMemberDao scheduleMemberDao = new ScheduleMemberDao();
			List<ScheduleMember> scheduleMemberList = scheduleMemberDao.GetScheduleMemberListByScheduleId(schedule.Id);

			TitleText = schedule.Title;
			StartTime = new TimeSpan(schedule.StartDate.Hour, schedule.StartDate.Minute, schedule.StartDate.Second);
			EndTime = new TimeSpan(schedule.EndDate.Hour, schedule.EndDate.Minute, schedule.EndDate.Second);
			if(schedule.isAllDay) swAllDay.IsToggled = true;
			ProjectId = project.Id;
			this.resources[ProjectTextKey] = project.Title;
			RepeatDiv = schedule.RepeatDiv;
			this.resources[RepeatTextKey] = GetRepeatText(schedule.RepeatDiv);
			PlaceText = schedule.Place;
			NotesText = schedule.Notes;

			// TODO 全メンバーIDを設定。全メンバー名も画面表示用に要設定
			foreach (ScheduleMember scheduleMember in scheduleMemberList) {
				MemberIdList.Add(scheduleMember.MemberId);
			}

			var tempScheduleDao = new TempScheduleDao();
			tempScheduleDao.DeleteAll();

			var tempScheduleMemberDao = new TempScheduleMemberDao();
			tempScheduleMemberDao.DeleteAll();
		}

		/// <summary>
		/// タスク編集時の項目を設定する。
		/// </summary>
		/// <param name="id">ID</param>
		/// <param name="tempEvent">一時保存データ</param>
		private void SetEditTask(long id, BaseEvent tempEvent) {

			Task task = null;

			if (tempEvent == null) {
				TaskDao taskDao = new TaskDao();
				task = taskDao.GetTaskById(id);
			}
			else {
				task = tempEvent as Task;
				task.Id = (tempEvent as TempTask).TaskId;
			}

			ProjectDao projectDao = new ProjectDao();
			Project project = projectDao.GetProjectById(task.ProjectId);

			TaskMemberDao taskMemberDao = new TaskMemberDao();
			List<TaskMember> taskMemberList = taskMemberDao.GetTaskMemberListByTaskId(task.Id);

			TitleText = task.Title;
			StartDate = task.StartDate;
			EndDate = task.EndDate;
			ProjectId = project.Id;
			this.resources[ProjectTextKey] = project.Title;
			ExpectedDailyWorkTimeText = task.ExpectedDailyWorkTime.TotalHours.ToString();
			NotesText = task.Notes;

			// TODO 全メンバーIDを設定。全メンバー名も画面表示用に要設定
			foreach (TaskMember taskMember in taskMemberList) {
				MemberIdList.Add(taskMember.MemberId);
			}

			var tempTaskDao = new TempTaskDao();
			tempTaskDao.DeleteAll();

			var tempTaskMemberDao = new TempTaskMemberDao();
			tempTaskMemberDao.DeleteAll();
		}

		/// <summary>
		/// プロジェクト編集時の項目を設定する。
		/// </summary>
		/// <param name="id">ID</param>
		private void SetEditProject(long id) {

			ProjectDao projectDao = new ProjectDao();
			Project project = projectDao.GetProjectById(id);

			ProjectMemberDao projectMemberDao = new ProjectMemberDao();
			List<ProjectMember> projectMemberList = projectMemberDao.GetProjectMemberListByProjectId(project.Id);

			TitleText = project.Title;
			StartDate = project.StartDate;
			EndDate = project.EndDate;
			ColorDiv = project.ColorDiv;
			this.resources[ColorTextKey] = GetColorText(project.ColorDiv);
			ExpectedSalesText = project.ExpectedSales.ToString();
			SalesText = project.Sales.ToString();
			NotesText = project.Notes;

			// TODO 全メンバーIDを設定。全メンバー名も画面表示用に要設定
			foreach (ProjectMember projectMember in projectMemberList) {
				MemberIdList.Add(projectMember.MemberId);
			}
		}

		/// <summary>
		/// ダイアログ内のプロジェクトを設定する。
		/// </summary>
		private List<Project> SetProjectsInDialog(Grid gdProjects) {

			// TODO 仮のメンバーを初期選択に
			long memberId = 1;

			var dao = new ProjectDao();
			List<Project> entityList = dao.GetProjectListByMemberId(memberId);

			int index = 0;
			foreach (Project entity in entityList) {
				gdProjects.Children.Add(GetProjectGrid(entity), 0, index);
				index++;
			}

			int maxItemCount = 4;
			if (entityList.Count < maxItemCount) {
				for (int i = 0; i < maxItemCount - entityList.Count; i++) {
					gdProjects.Children.Add(new Grid(), 0, index);
					index++;
				}
			}

			return entityList;
		}

		/// <summary>
		/// プロジェクトのグリッドを取得する。
		/// </summary>
		/// <param name="project">プロジェクトデータ</param>
		/// <returns>グリッド</returns>
		private Grid GetProjectGrid(Project project) {

			// グリッドを生成
			var grid = new Grid {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				ColumnSpacing = 0,
				RowSpacing = 0,
			};

			// プロジェクトカラーのボックスビューを生成
			var colorBoxView = new BoxView {
				BackgroundColor = LayoutUtility.GetColorByColorDiv(project.ColorDiv),
			};

			string text = project.Title;
			if (project.Title.Length > 25) text = text.Substring(0, 24) + "…";

			// タイトルのラベルを生成
			var titleLabel = new Label {
				Text = text,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromHex("#2F4F4F"),
			};

			// IDを引数にしたタップイベントを付与
			grid.GestureRecognizers.Add(LayoutUtility.GetTapGestureRecognizerWithParameter(
				"SelectedProjectCommand", project.Id.ToString()));

			// グリッドに項目を追加
			grid.Children.Add(colorBoxView, 1, 2, 0, 1);
			grid.Children.Add(new BoxView(), 2, 3, 0, 1);
			grid.Children.Add(titleLabel, 3, 26, 0, 1);
			grid.Children.Add(new BoxView(), 26, 27, 0, 1);

			return grid;
		}

		/// <summary>
		/// ダイアログ内の色項目を設定する。
		/// </summary>
		private void SetColorItemsInDialog() {

			for (int i = 1; i <= 4; i++) {

				var selectedItemDto = new SelectedItemDto();
				selectedItemDto.Id = i;
				selectedItemDto.Name = GetColorText((ColorDiv)selectedItemDto.Id);
				selectedItemDto.Color = LayoutUtility.GetColorByColorDiv((ColorDiv)selectedItemDto.Id);

				this.ColorItems.Add(selectedItemDto);
			}
		}

		/// <summary>
		/// テーマカラーテキストを取得する。
		/// </summary>
		/// <param name="colorDiv">色区分</param>
		/// <returns>テーマカラーテキスト</returns>
		private string GetColorText(ColorDiv colorDiv) {

			switch (colorDiv) {
				case ColorDiv.スカイブルー:
					return "スカイブルー";
				case ColorDiv.シーグリーン:
					return "シーグリーン";
				case ColorDiv.コーラルレッド:
					return "コーラルレッド";
				case ColorDiv.プラムバイオレット:
					return "プラムバイオレット";
				default:
					return "";
			}
		}

		/// <summary>
		/// ダイアログ内の繰り返し項目を設定する。
		/// </summary>
		private void SetRepeatItemsInDialog() {

			for (int i = 1; i <= 4; i++) {

				var selectedItemDto = new SelectedItemDto();
				selectedItemDto.Id = i;
				selectedItemDto.Name = GetRepeatText((RepeatDiv)selectedItemDto.Id);

				this.RepeatItems.Add(selectedItemDto);
			}
		}

		/// <summary>
		/// 繰り返しテキストを取得する。
		/// </summary>
		/// <param name="repeatDiv">繰り返し区分</param>
		/// <returns>繰り返しテキスト</returns>
		private string GetRepeatText(RepeatDiv repeatDiv) {

			switch (repeatDiv) {
				case RepeatDiv.毎日:
					return StringConst.RepeatEveryday;
				case RepeatDiv.毎週:
					return StringConst.RepeatEveryWeek;
				case RepeatDiv.毎月:
					return StringConst.RepeatEveryMonth;
				case RepeatDiv.毎年:
					return StringConst.RepeatEveryYear;
				default:
					return "";
			}
		}
	}
}