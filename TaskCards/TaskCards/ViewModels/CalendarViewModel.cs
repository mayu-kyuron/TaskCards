using System;
using System.Collections.Generic;
using System.Windows.Input;
using TaskCards.Consts;
using TaskCards.Dao;
using TaskCards.Divisions;
using TaskCards.Entities;
using TaskCards.Pages;
using TaskCards.Utilities;
using Xamarin.Forms;

namespace TaskCards.ViewModels {

	/// <summary>
	/// カレンダーページのビューモデル
	/// </summary>
    public class CalendarViewModel {

		public static DateTime selectedDate = DateTime.Today; // 選択された日付

		public ImageSource LeftArrowSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "left_arrow.png");
		public ImageSource RightArrowSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "right_arrow.png");
		public ImageSource AddSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "btn_add.png");
		public ImageSource DateTappedDialogTitleIconSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "repeat.png");

		public ICommand TappedCellCommand { get; private set; }
		public ICommand TappedScheduleCommand { get; private set; }
		public ICommand TappedTaskCommand { get; private set; }

		private ContentView cvDialogBack;
		private Label lblDate;
		private Grid gdSchedule;
		private Grid gdTask;

		public CalendarViewModel(ContentView cvDialogBack, Label lblDate, Grid gdSchedule, Grid gdTask) {

			this.cvDialogBack = cvDialogBack;
			this.lblDate = lblDate;
			this.gdSchedule = gdSchedule;
			this.gdTask = gdTask;

			TappedCellCommand = new Command<string>(OnTapCell);
			TappedScheduleCommand = new Command<string>(OnTapSchedule);
			TappedTaskCommand = new Command<string>(OnTapTask);
		}

		/// <summary>
		/// カレンダーセルのタップイベント
		/// </summary>
		/// <param name="selectedDateStr">選択日付（文字列）</param>
		private void OnTapCell(string selectedDateStr) {

			selectedDate = DateTime.Parse(selectedDateStr);
			this.lblDate.Text = selectedDate.ToString(StringConst.DateTappedDialogDateFormat);

			// １日のスケジュールとタスクを設定する。
			SetSchedulesAndTasksInDateTappedDialog();

			this.cvDialogBack.IsVisible = true;
		}

		/// <summary>
		/// スケジュールのタップイベント
		/// </summary>
		/// <param name="idStr">ID（文字列）</param>
		private void OnTapSchedule(string idStr) {

			// 予定の確認ページに遷移
			Application.Current.MainPage = new ConfirmSchedulePage(long.Parse(idStr));
		}

		/// <summary>
		/// タスクのタップイベント
		/// </summary>
		/// <param name="idStr">ID（文字列）</param>
		private void OnTapTask(string idStr) {

			// タスクの確認ページに遷移
			Application.Current.MainPage = new ConfirmPage(selectedDate, 
				TableDiv.タスク, PageDiv.カレンダー, long.Parse(idStr));
		}

		/// <summary>
		/// 日付タップダイアログ内のスケジュールとタスクを設定する。
		/// </summary>
		private void SetSchedulesAndTasksInDateTappedDialog() {

			// スケジュールリストを取得
			var scheduleDao = new ScheduleDao();
			List<Schedule> scheduleList = scheduleDao.GetDayScheduleList(selectedDate);

			// １日のスケジュールを設定
			this.gdSchedule.Children.Clear();
			int scheduleIndex = 0;
			foreach (Schedule schedule in scheduleList) {
				this.gdSchedule.Children.Add(GetScheduleOrTaskGrid(schedule), 0, scheduleIndex);
				scheduleIndex++;
			}
			if (scheduleList.Count == 1) this.gdSchedule.Children.Add(new Grid(), 0, scheduleIndex);

			// タスクリストを取得
			var taskDao = new TaskDao();
			List<Task> taskList = taskDao.GetDayTaskList(selectedDate);

			// １日のタスクを設定
			this.gdTask.Children.Clear();
			int taskIndex = 0;
			foreach (Task task in taskList) {
				this.gdTask.Children.Add(GetScheduleOrTaskGrid(task), 0, taskIndex);
				taskIndex++;
			}
			if (taskList.Count == 1) this.gdTask.Children.Add(new Grid(), 0, taskIndex);
		}

		/// <summary>
		/// スケジュールまたはタスクのグリッドを取得する。
		/// </summary>
		/// <param name="baseEvent">基底イベントの子インスタンス</param>
		/// <returns>グリッド</returns>
		private Grid GetScheduleOrTaskGrid(BaseEvent baseEvent) {

			// グリッドを生成
			var grid = new Grid {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				ColumnSpacing = 0,
				RowSpacing = 0,
				RowDefinitions = {
					new RowDefinition { Height = new GridLength(.1, GridUnitType.Star) },
					new RowDefinition { Height = new GridLength(.2, GridUnitType.Star) },
				}
			};

			// 時刻（日付）のラベルを生成
			var timeLabel = new Label {
				Text = GetScheduleOrTaskTimeText(baseEvent),
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromHex("#2F4F4F")
			};

			// タイトルのグリッドを生成
			RepeatDiv repeatDiv = RepeatDiv.繰り返しなし;
			if (baseEvent.GetType() == typeof(Schedule)) repeatDiv = (baseEvent as Schedule).RepeatDiv;
			Grid titleGrid = GetScheduleOrTaskTitleGrid(baseEvent, repeatDiv);

			// IDを引数にしたタップイベントを付与
			grid.GestureRecognizers.Add(LayoutUtility.GetTapGestureRecognizerWithParameter(
				GetTappedScheduleOrTaskCommandName(baseEvent), baseEvent.Id.ToString()));

			// グリッドに項目を追加
			grid.Children.Add(timeLabel, 0, 0);
			grid.Children.Add(titleGrid, 0, 1);

			return grid;
		}

		/// <summary>
		/// スケジュールまたはタスクの時刻（日付）テキストを取得する。
		/// </summary>
		/// <param name="baseEvent">基底イベントの子インスタンス</param>
		/// <returns>時刻（日付）テキスト</returns>
		private string GetScheduleOrTaskTimeText(BaseEvent baseEvent) {

			string timeText = "終日";
			if (baseEvent.GetType() == typeof(Schedule)) {

				var schedule = baseEvent as Schedule;
				if (!schedule.isAllDay) {
					timeText = baseEvent.StartDate.ToString(StringConst.InputConfirmTimeFormat)
						 + "〜" + baseEvent.EndDate.ToString(StringConst.InputConfirmTimeFormat);
				}
			}
			else {
				timeText = "〜" + baseEvent.EndDate.ToString(StringConst.DateTappedDialogDateFormat);
			}

			return timeText;
		}

		/// <summary>
		/// スケジュールまたはタスクのタイトル表示用グリッドを取得する。
		/// </summary>
		/// <param name="baseEvent">基底イベントの子インスタンス</param>
		/// <param name="repeatDiv">（オプション）繰り返し区分。既定値は「繰り返しなし」。</param>
		/// <returns>タイトル表示用グリッド</returns>
		private Grid GetScheduleOrTaskTitleGrid(BaseEvent baseEvent, RepeatDiv repeatDiv = RepeatDiv.繰り返しなし) {

			// タイトル用のグリッドを生成
			var titleGrid = new Grid {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				ColumnSpacing = 0,
				RowSpacing = 0,
			};

			var projectDao = new ProjectDao();
			Project project = projectDao.GetProjectById(baseEvent.ProjectId);

			// タイトルのカラーボックスビューを生成
			var titleBoxView = new BoxView {
				BackgroundColor = LayoutUtility.GetColorByColorDiv(project.ColorDiv),
			};

			string text = baseEvent.Title;
			if (baseEvent.Title.Length > 13) text = text.Substring(0, 12) + "…";

			// タイトルのラベルを生成
			var titleLabel = new Label {
				Text = text,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromHex("#2F4F4F"),
			};

			// タイトルの繰り返しアイコン画像を生成
			var titleImage = new Image {
				IsVisible = (repeatDiv == RepeatDiv.繰り返しなし) ? false : true,
				Source = ImageSource.FromResource(StringConst.ImageFolderPath + "repeat.png"),
				Aspect = Aspect.Fill,
			};

			// グリッドに項目を追加
			titleGrid.Children.Add(titleBoxView, 1, 2, 0, 1);
			titleGrid.Children.Add(new BoxView(), 2, 3, 0, 1);
			titleGrid.Children.Add(titleLabel, 3, 26, 0, 1);
			titleGrid.Children.Add(titleImage, 26, 31, 0, 1);
			titleGrid.Children.Add(new BoxView(), 31, 32, 0, 1);

			return titleGrid;
		}

		/// <summary>
		/// スケジュールまたはタスクのタップコマンド名を取得する。
		/// </summary>
		/// <param name="baseEvent">基底イベントの子インスタンス</param>
		/// <returns>タップコマンド名</returns>
		private string GetTappedScheduleOrTaskCommandName(BaseEvent baseEvent) {

			string commandName = "";
			if (baseEvent.GetType() == typeof(Schedule)) {
				commandName = "TappedScheduleCommand";
			}
			else {
				commandName = "TappedTaskCommand";
			}

			return commandName;
		}
	}
}