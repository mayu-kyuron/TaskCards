﻿using System;
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

		CalendarViewModel viewModel;

		bool isFirstShow = true; // 最初の表示かどうか
		DateTime dateOfLastShownMonth; // ひとつ前に表示された月の日付

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
			this.viewModel = new CalendarViewModel(cvDialogBack, lblDate, gdSchedule);
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
		/// カレンダーの各日付イベントとタップイベントを設定する。
		/// </summary>
		/// <param name="anyDateOfMonth">該当月の日付</param>
		private void SetCalendarDateEventsAndTapEvents(DateTime anyDateOfMonth) {

			DateTime startDateOfMonth = new DateTime(anyDateOfMonth.Year, anyDateOfMonth.Month, 1);
			DateTime endDateOfMonth = startDateOfMonth.AddMonths(1).AddDays(-1);
			int firstColumn = (int)startDateOfMonth.DayOfWeek;
			this.dateOfLastShownMonth = startDateOfMonth;

			var projectDao = new ProjectDao();

			var scheduleDao = new ScheduleDao();
			Dictionary<int, List<Schedule>> scheduleMap = scheduleDao.GetMonthScheduleMap(anyDateOfMonth);

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
					if (scheduleMap.ContainsKey(currentDate.Day)) {

						int insideRow = 0; 
						foreach (Schedule schedule in scheduleMap[currentDate.Day]) {

							if (insideRow > 2) break;

							Project project = projectDao.GetProjectById(schedule.ProjectId);

							var label = new Label {
								Text = schedule.Title,
								BackgroundColor = LayoutUtility.GetColorByColorDiv(project.ColorDiv)
							};

							grid.Children.Add(label, 0, insideRow);

							insideRow++;
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
	}
}