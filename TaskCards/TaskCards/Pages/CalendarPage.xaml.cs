using System;
using System.Collections.Generic;
using System.Linq;
using TaskCards.Dao;
using TaskCards.Divisions;
using TaskCards.Entities;
using TaskCards.Utilities;
using TaskCards.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskCards.Pages {

	/// <summary>
	/// カレンダーページ（タブ1）
	/// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CalendarPage : ContentPage {

		const int CalendarDayEventMax = 3; // カレンダー日付上に表示するイベントの最大数

		CalendarViewModel viewModel;

		bool isFirstShow = true; // 最初の表示かどうか
		DateTime dateOfLastShownMonth; // ひとつ前に表示された月の日付
		long lastTopTaskId = 0; // 前日の最上段のタスクID
		Dictionary<int, int> lastEmptyTaskRowCountMap = new Dictionary<int, int>(); // 前日のタスク空行数のマップ

		public CalendarPage () {
			Initialize();
		}

		/// <summary>
		/// 初期化する。
		/// </summary>
		private void Initialize() {
			InitializeComponent();

			// 初期表示月、選択日を設定
			calenderBase.StartDate = CalendarViewModel.selectedDate;
			calenderBase.SelectedDate = CalendarViewModel.selectedDate;

			SizeChanged += OnSizeChanged;

			// タップイベントを付与
			var tgrDialogBack = new TapGestureRecognizer();
			tgrDialogBack.Tapped += (sender, e) => OnClickDialogBack(sender, e);
			cvDialogBack.GestureRecognizers.Add(tgrDialogBack);

			var tgrDialogFront = new TapGestureRecognizer();
			tgrDialogFront.Tapped += (sender, e) => OnClickDialogFront(sender, e);
			gdDialogFront.GestureRecognizers.Add(tgrDialogFront);

			var tgrAddSchedule = new TapGestureRecognizer();
			tgrAddSchedule.Tapped += (sender, e) => OnClickAddSchedule(sender, e);
			imgAddSchedule.GestureRecognizers.Add(tgrAddSchedule);

			var tgrAddTask = new TapGestureRecognizer();
			tgrAddTask.Tapped += (sender, e) => OnClickAddTask(sender, e);
			imgAddTask.GestureRecognizers.Add(tgrAddTask);

			// カレンダーの高さを設定
			calenderBase.OnEndRenderCalendar += (sender, e) => {
				(calenderBase.Content as StackLayout).Children.Last().HeightRequest = 450;
			};

			// TODO 仮のプロジェクトを追加
			long id = 1;
			ProjectDao projectDao = new ProjectDao();
			Project project = projectDao.GetProjectById(id);
			if (project == null) {
				Project myProject = new Project();
				myProject.Id = id;
				myProject.Title = "プロジェクトA";
				myProject.ColorDiv = ColorDiv.赤;
				projectDao.Insert(myProject);
			}

			// TODO メンバーに仮の自分を追加
			long id2 = 1;
			MemberDao memberDao = new MemberDao();
			Member member = memberDao.GetMemberById(id2);
			if (member == null) {
				Member myMember = new Member();
				myMember.Id = id2;
				myMember.Name = "メンバー1";
				memberDao.Insert(myMember);
			}
		}

		/// <summary>
		/// 画面サイズ変更イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void OnSizeChanged(object sender, EventArgs args) {
			this.viewModel = new CalendarViewModel(cvDialogBack, lblDate, gdSchedule, gdTask);
			BindingContext = this.viewModel;

			// カレンダーの各日付イベントとタップイベントを付与
			if(this.isFirstShow) SetCalendarDateEventsAndTapEvents(CalendarViewModel.selectedDate);

			this.isFirstShow = false;
		}

		/// <summary>
		/// 日付クリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickDate(object sender, EventArgs e) {

		//	// ひとつ前に選択したのと同じ日付を選んだ場合、受け渡すselectedDateがnullになるのを防ぐ。
		//	this.selectedDate = this.lastSelectedDate;
		//	if (calenderBase.SelectedDate != null) this.selectedDate = (DateTime)calenderBase.SelectedDate;
		//	this.lastSelectedDate = this.selectedDate;

		//	// その日の予定・タスクのダイアログを表示する。
		//	lblDate.Text = this.selectedDate.ToString(StringConst.DateTappedDialogDateFormat);
		//	cvDialogBack.IsVisible = true;
		}

		/// <summary>
		/// カレンダー左矢印クリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickLeftArrow(object sender, EventArgs e) {

			// カレンダーの各日付イベントとタップイベントを付与し直す
			gdSchedulesAndTasks.Children.Clear();
			SetCalendarDateEventsAndTapEvents(this.dateOfLastShownMonth.AddMonths(-1));
		}

		/// <summary>
		/// カレンダー右矢印クリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickRightArrow(object sender, EventArgs e) {

			// カレンダーの各日付イベントとタップイベントを付与し直す
			gdSchedulesAndTasks.Children.Clear();
			SetCalendarDateEventsAndTapEvents(this.dateOfLastShownMonth.AddMonths(1));
		}

		/// <summary>
		/// ダイアログ背面クリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickDialogBack(object sender, EventArgs e) {

			// 日付選択ダイアログを非表示に
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
		/// スケジュール追加ボタンクリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickAddSchedule(object sender, EventArgs e) {

			// 予定追加用の入力ページに遷移
			Application.Current.MainPage = new InputPage(CalendarViewModel.selectedDate, 
				TableDiv.予定, PageDiv.カレンダー, ExecuteDiv.追加);
		}

		/// <summary>
		/// タスク追加ボタンクリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickAddTask(object sender, EventArgs e) {

			// タスク追加用の入力ページに遷移
			Application.Current.MainPage = new InputPage(CalendarViewModel.selectedDate,
				TableDiv.タスク, PageDiv.カレンダー, ExecuteDiv.追加);
		}

		/// <summary>
		/// カレンダーの各日付イベントとタップイベントを設定する。
		/// </summary>
		/// <param name="anyDateOfMonth">該当月の日付</param>
		private void SetCalendarDateEventsAndTapEvents(DateTime anyDateOfMonth) {

			DateTime startDateOfMonth = new DateTime(anyDateOfMonth.Year, anyDateOfMonth.Month, 1);
			DateTime endDateOfMonth = startDateOfMonth.AddMonths(1).AddDays(-1);
			int firstColumn = (int)startDateOfMonth.DayOfWeek;
			this.dateOfLastShownMonth = startDateOfMonth;

			var scheduleDao = new ScheduleDao();
			Dictionary<int, List<Schedule>> scheduleMap = scheduleDao.GetMonthScheduleMap(anyDateOfMonth);

			var taskDao = new TaskDao();
			Dictionary<int, List<Task>> taskMap = taskDao.GetMonthTaskMap(anyDateOfMonth);

			bool isStartWeek = true;
			DateTime currentDate = startDateOfMonth;

			for (int row = 0; row < 6; row++) {
				for (int column = 0; column < 7; column++) {

					// 当月１日の列まではスキップ
					if (isStartWeek && firstColumn > column) continue;
					isStartWeek = false;

					// 日付セル用のグリッドを生成
					var grid = new Grid {
						RowDefinitions = {
							new RowDefinition { Height = new GridLength(.1, GridUnitType.Star) },
							new RowDefinition { Height = new GridLength(.1, GridUnitType.Star) },
							new RowDefinition { Height = new GridLength(.1, GridUnitType.Star) },
						}
					};

					// 日付にイベントがある場合、グリッドに３件まで表示を追加
					if (scheduleMap.ContainsKey(currentDate.Day) || taskMap.ContainsKey(currentDate.Day)) {

						List<Schedule> scheduleList = (scheduleMap.ContainsKey(currentDate.Day)) 
							? scheduleMap[currentDate.Day] : null;
						List<Task> taskList = (taskMap.ContainsKey(currentDate.Day))
							? taskMap[currentDate.Day] : null;

						List <Task> lastDayTaskList = null;
						if (currentDate.Day != startDateOfMonth.Day && taskMap.ContainsKey(currentDate.AddDays(-1).Day)) {
							lastDayTaskList = taskMap[currentDate.AddDays(-1).Day];
						}

						List<BaseEvent> eventList = GetBaseEventListForDateCellGrid(scheduleList, taskList, lastDayTaskList);

						var scheduleLabel = new Label();

						int insideRow = 0; 
						foreach (BaseEvent baseEvent in eventList) {

							Label label = GetEventLabelInDateCellGrid(baseEvent, currentDate);
							grid.Children.Add(label, 0, insideRow);

							if (baseEvent != null && baseEvent.GetType() == typeof(Schedule)) {
								scheduleLabel = label;
							}

							insideRow++;
						}

						// 前日続きのタスクによって当日のタスク表示がまったくない場合、その印を追加
						if(eventList.Count > 2 && eventList[1] == null && eventList[2] == null) {
							Grid.SetColumnSpan(scheduleLabel, 3);
							grid.Children.Add(GetOverEventsLabelInDateCellGrid(), 2, 2);
						}
					}

					// 日付を引数にしたタップイベントを付与
					grid.GestureRecognizers.Add(LayoutUtility.GetTapGestureRecognizerWithParameter(
						"TappedCellCommand", currentDate.ToString()));

					// カレンダーに日付セルグリッドを追加
					gdSchedulesAndTasks.Children.Add(grid, column, row);

					// 日付をインクリメントし、当月を過ぎたらループ終了
					currentDate = currentDate.AddDays(1);
					if (currentDate > endDateOfMonth) break;
				}

				// 当月を過ぎたらループ終了
				if (currentDate > endDateOfMonth) break;
			}
		}

		/// <summary>
		/// 日付セルグリッド表示用のイベントリストを取得する。
		/// </summary>
		/// <param name="scheduleList">スケジュールリスト</param>
		/// <param name="taskList">タスクリスト</param>
		/// <param name="lastDayTaskList">前日のタスクリスト</param>
		/// <returns>イベントリスト</returns>
		private List<BaseEvent> GetBaseEventListForDateCellGrid(
			List<Schedule> scheduleList, List<Task> taskList, List<Task> lastDayTaskList) {

			var baseEventList = new List<BaseEvent>();

			// TODO 予定が表示されるか要確認。
			// １行目に予定を追加（なければ空行を追加）
			if (scheduleList == null || scheduleList.Count == 0) {
				baseEventList.Add(null);
			}
			else {
				baseEventList.Add(scheduleList[0]);
			}

			// ２、３行目にタスクor空行を追加
			baseEventList = AddEmptiesAndTasksForDateCellGrid(baseEventList, taskList, lastDayTaskList);

			// 全部で３行に満たない場合、ある分の残りの当日タスクを追加
			baseEventList = AddLeftoverTasksForDateCellGrid(baseEventList, taskList);

			return baseEventList;
		}

		/// <summary>
		/// 日付セルグリッド表示用のイベントリストにタスクまたは空行を追加する。
		/// </summary>
		/// <param name="baseEventList">イベントリスト</param>
		/// <param name="taskList">タスクリスト</param>
		/// <param name="lastDayTaskList">前日のタスクリスト</param>
		/// <returns>イベントリスト</returns>
		private List<BaseEvent> AddEmptiesAndTasksForDateCellGrid(
			List<BaseEvent> baseEventList, List<Task> taskList, List<Task> lastDayTaskList) {

			if (taskList == null || taskList.Count == 0 || ClearLastEmptyTaskRowCount(taskList, lastDayTaskList)) {
				return baseEventList;
			}

			int lastSameEventIndex = 0;
			for (int i = 0; i < taskList.Count; i++) {

				if (lastDayTaskList.Where(v => v.Id == taskList[i].Id).ToList().Count == 0) continue;

				ResetLastEmptyMiddleTaskCountToTop(taskList, lastDayTaskList);

				// 前日のタスク空行と同じ分の空行を追加
				if (i == 0) {
					if (this.lastEmptyTaskRowCountMap.ContainsKey(0)) {
						for (int k = 0; k < this.lastEmptyTaskRowCountMap[0]; k++) {
							baseEventList.Add(null);
							if (baseEventList.Count >= CalendarDayEventMax) break;
						}
						if (baseEventList.Count >= CalendarDayEventMax) break;
					}
				}
				else if (this.lastEmptyTaskRowCountMap.ContainsKey(1)) {
					for (int k = 0; k < this.lastEmptyTaskRowCountMap[1]; k++) {
						baseEventList.Add(null);
						if (baseEventList.Count >= CalendarDayEventMax) break;
					}
					if (baseEventList.Count >= CalendarDayEventMax) break;
				}

				// 前日と同じタスクがあった場合、行を合わせるため、上に必要な分の空行を追加
				int sameEventIndex = lastDayTaskList.FindIndex(v => v.Id == taskList[i].Id);
				for (int j = lastSameEventIndex; j < sameEventIndex; j++) {
					if (lastSameEventIndex == 0) {
						AddLastEmptyTaskRowCount(0);
					}
					else {
						AddLastEmptyTaskRowCount(1);
					}
					baseEventList.Add(null);
					if (baseEventList.Count >= CalendarDayEventMax) break;
				}
				if (baseEventList.Count >= CalendarDayEventMax) break;

				// 前日と同じタスクを追加
				lastSameEventIndex = sameEventIndex + 1;
				baseEventList.Add(taskList[i]);
				if (baseEventList.Count >= CalendarDayEventMax) break;
			}

			return baseEventList;
		}

		/// <summary>
		/// 前日のタスク空行数をクリアする。
		/// </summary>
		/// <param name="taskList">タスクリスト</param>
		/// <param name="lastDayTaskList">前日のタスクリスト</param>
		/// <returns>処理を切り上げる場合は true、そのまま続行の場合は false</returns>
		private bool ClearLastEmptyTaskRowCount(List<Task> taskList, List<Task> lastDayTaskList) {

			if (lastDayTaskList == null) {
				this.lastEmptyTaskRowCountMap.Clear();
				return true;
			}

			bool hasSameEvent = false;
			foreach (Task task in taskList) {
				if (lastDayTaskList.Where(v => v.Id == task.Id).ToList().Count != 0) hasSameEvent = true;
			}
			if (!hasSameEvent) this.lastEmptyTaskRowCountMap.Clear();

			return false;
		}

		/// <summary>
		/// 前日のタスク中段空行数を上段用に設定し直す。
		/// </summary>
		/// <param name="taskList">タスクリスト</param>
		/// <param name="lastDayTaskList">前日のタスクリスト</param>
		private void ResetLastEmptyMiddleTaskCountToTop(List<Task> taskList, List<Task> lastDayTaskList) {

			// 複数タスクの間に空行がある状態から、最上段タスクがなくなった場合に、中段の空行数を上段の空行数として設定し直す。
			if (lastDayTaskList[0].Id == this.lastTopTaskId && taskList[0].Id != this.lastTopTaskId
								&& this.lastEmptyTaskRowCountMap.ContainsKey(1)) {

				if (!this.lastEmptyTaskRowCountMap.ContainsKey(0)) this.lastEmptyTaskRowCountMap.Add(0, 0);

				this.lastEmptyTaskRowCountMap[0] = this.lastEmptyTaskRowCountMap[1];
				this.lastEmptyTaskRowCountMap.Remove(1);
			}
		}

		/// <summary>
		/// 前日のタスク空行数を追加する。
		/// </summary>
		/// <param name="countMapKey">前日タスク空行数マップのキー</param>
		private void AddLastEmptyTaskRowCount(int countMapKey) {

			if (this.lastEmptyTaskRowCountMap.ContainsKey(countMapKey)) {
				this.lastEmptyTaskRowCountMap[countMapKey]++;
			}
			else {
				this.lastEmptyTaskRowCountMap.Add(countMapKey, 1);
			}
		}

		/// <summary>
		/// 日付セルグリッド表示用のイベントリストに残りのタスクを追加する。
		/// </summary>
		/// <param name="baseEventList">イベントリスト</param>
		/// <param name="taskList">タスクリスト</param>
		/// <returns>イベントリスト</returns>
		private List<BaseEvent> AddLeftoverTasksForDateCellGrid(List<BaseEvent> baseEventList, List<Task> taskList) {

			if (taskList == null || taskList.Count == 0) return baseEventList;

			if (baseEventList.Count < CalendarDayEventMax - 1) {
				if (taskList.Count == 0) return baseEventList;

				this.lastTopTaskId = taskList[0].Id;
				baseEventList.Add(taskList[0]);
				if (taskList.Count > 1) baseEventList.Add(taskList[1]);
			}
			else if (baseEventList.Count < CalendarDayEventMax) {
				if (taskList.Count > 1) baseEventList.Add(taskList[1]);
			}

			return baseEventList;
		}

		/// <summary>
		/// 日付セルグリッド内のイベントのラベルを取得する。
		/// </summary>
		/// <param name="baseEvent">イベントのインスタンス</param>
		/// <param name="currentDate">当日の日付</param>
		/// <returns>イベントのラベル</returns>
		private Label GetEventLabelInDateCellGrid(BaseEvent baseEvent, DateTime currentDate) {

			var label = new Label();

			if (baseEvent != null) {

				// TODO 月の初日はテキスト表示する。
				// タスクの２日目以降はテキスト表示なし
				if (baseEvent.StartDate == currentDate) label.Text = baseEvent.Title;

				var projectDao = new ProjectDao();
				Project project = projectDao.GetProjectById(baseEvent.ProjectId);

				// スケジュールの場合
				if (baseEvent.GetType() == typeof(Schedule)) {
					label.TextColor = LayoutUtility.GetColorByColorDiv(project.ColorDiv);
				}
				// タスクの場合
				else if (baseEvent.GetType() == typeof(Task)) {
					label.TextColor = Color.White;
					label.BackgroundColor = LayoutUtility.GetColorByColorDiv(project.ColorDiv);
					label.Opacity = 0.9;
				}
			}

			return label;
		}

		/// <summary>
		/// 日付セルグリッド内の表示イベント超過マークのラベルを取得する。
		/// </summary>
		/// <returns>表示イベント超過マークのラベル</returns>
		private Label GetOverEventsLabelInDateCellGrid() {

			return new Label {
				Text = "+",
				TextColor = Color.White,
				HorizontalTextAlignment = TextAlignment.Center,
				BackgroundColor = Color.FromHex("#6495ed")
			};
		}
	}
}