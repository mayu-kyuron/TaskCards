using System;
using System.Collections.Generic;
using System.Windows.Input;
using TaskCards.Consts;
using TaskCards.Dao;
using TaskCards.Divisions;
using TaskCards.Dtos;
using TaskCards.Entities;
using Xamarin.Forms;

namespace TaskCards.ViewModels {

	/// <summary>
	/// 入力ページのビューモデル
	/// </summary>
	public class InputViewModel {

		public List<SelectedItemDto> RepeatItems { protected set; get; } = new List<SelectedItemDto>();

		public ICommand SelectedRepeatCommand { get; set; }

		public double DialogBaseHeight { get; set; }
		public double MemberHeight { get; set; }

		public ImageSource BackButtonSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "btn_back.png");
		public ImageSource TopRightButtonSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "btn_check.png");
		public ImageSource InputConfirmDateTimeArrowSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "date_time_right_arrow.png");
		public ImageSource InputConfirmArrowIconSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "input_right_arrow.png");
		public ImageSource InputConfirmCancelIconSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "btn_cancel.png");
		public ImageSource InputConfirmAddIconSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "btn_add.png");

		public string TopDateText { get; set; }
		public string TitleText { get; set; }
		public string ExpectedDailyWorkTimeText { get; set; }
		public string ProjectText { get; set; }
		public string Member1Text { get; set; }
		public string AddText { get; set; }
		public string RepeatText { get; set; }
		public string PlaceText { get; set; }
		public string NotesText { get; set; }

		public TimeSpan StartTime { get; set; }
		public TimeSpan EndTime { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public long ProjectId { get; set; }
		public List<long> MemberIdList { get; set; } = new List<long>();
		public RepeatDiv RepeatDiv { get; set; }

		public InputViewModel(DateTime selectedDate, TableDiv tableDiv, ExecuteDiv executeDiv, long id,
			double height, ContentView cvDialogBack, Switch swAllDay, ResourceDictionary resources) {

			// レイアウト全体の高さの設定
			// 項目を増やすごとに、デバイスの高さに対して項目の高さ分、掛ける割合を増やしていく。
			// 比率の計算方法　変更前のレイアウト全体の高さ:変更前の比率＝変更後のレイアウト全体の高さ:今回の比率
			// レイアウト全体の高さは、<Grid.RowDefinitions> RowDefinitionのHeightの合計
			DialogBaseHeight = height * 1;

			// メンバー追加欄の高さ（(画面の高さ - 上下マージン) / 全行数 * メンバーの行数）
			MemberHeight = (height - height / 62 * 2) / 14 * 3;

			TopDateText = selectedDate.ToString(StringConst.DateTappedDialogDateFormat);
			AddText = String.Format(StringConst.MessageAdd, "メンバー");
			RepeatDiv = RepeatDiv.繰り返しなし;
			resources["RepeatText"] = StringConst.RepeatNone;

			// テーブル区分・実行区分ごとに項目を設定
			switch (tableDiv) {

				case TableDiv.予定:
					switch (executeDiv) {
						case ExecuteDiv.追加:
							SetAddScheduleOrTask(tableDiv, selectedDate);
							break;
						case ExecuteDiv.更新:
							SetEditSchedule(id, swAllDay, resources);
							break;
					}
					break;

				case TableDiv.タスク:
					switch (executeDiv) {
						case ExecuteDiv.追加:
							SetAddScheduleOrTask(tableDiv, selectedDate);
							break;
						case ExecuteDiv.更新:
							SetEditTask(id);
							break;
					}
					break;
			}

			// 繰り返し項目を設定
			for (int i = 1; i <= 4; i++) {

				var selectedItemDto = new SelectedItemDto();
				selectedItemDto.Id = i;
				selectedItemDto.Name = GetRepeatText((RepeatDiv)selectedItemDto.Id);

				// データを画面表示リストに設定
				this.RepeatItems.Add(selectedItemDto);
			}

			// 繰り返しコマンドを設定
			this.SelectedRepeatCommand = new Command<SelectedItemDto>((selectedItemDto) => {
				RepeatDiv = (RepeatDiv)selectedItemDto.Id;
				resources["RepeatText"] = selectedItemDto.Name;
				cvDialogBack.IsVisible = false;
			});
		}

		/// <summary>
		/// スケジュールまたはタスク追加時の項目を設定する。
		/// </summary>
		private void SetAddScheduleOrTask(TableDiv tableDiv, DateTime selectedDate) {

			if (tableDiv == TableDiv.予定) {

				// 開始時間：現在時間＋1時間－現在の分数（例：12:34→13:00）
				// 終了時間：現在時間＋2時間－現在の分数（例：12:34→14:00）
				StartTime = new TimeSpan(DateTime.Now.AddHours(1).Hour, 0, 0);
				EndTime = new TimeSpan(DateTime.Now.AddHours(2).Hour, 0, 0);
			}
			else if (tableDiv == TableDiv.タスク) {

				StartDate = selectedDate;
				EndDate = selectedDate.AddDays(7);
			}

			// TODO 仮のプロジェクトを初期選択に
			long id1 = 1;
			ProjectDao projectDao = new ProjectDao();
			Project project = projectDao.GetProjectById(id1);
			ProjectId = project.Id;
			ProjectText = project.Title;

			// TODO 仮のメンバーを初期選択に
			long id2 = 1;
			MemberDao memberDao = new MemberDao();
			Member member = memberDao.GetMemberById(id2);
			MemberIdList.Add(member.Id);
			Member1Text = member.Name;
		}

		/// <summary>
		/// スケジュール編集時の項目を設定する。
		/// </summary>
		/// <param name="id">ID</param>
		/// <param name="swAllDay">終日Switch</param>
		/// <param name="resources">リソース</param>
		private void SetEditSchedule(long id, Switch swAllDay, ResourceDictionary resources) {

			ScheduleDao scheduleDao = new ScheduleDao();
			Schedule schedule = scheduleDao.GetScheduleById(id);

			ProjectDao projectDao = new ProjectDao();
			Project project = projectDao.GetProjectById(schedule.ProjectId);

			ScheduleMemberDao scheduleMemberDao = new ScheduleMemberDao();
			List<ScheduleMember> scheduleMemberList = scheduleMemberDao.GetScheduleMemberListByScheduleId(schedule.Id);

			TitleText = schedule.Title;
			StartTime = new TimeSpan(schedule.StartDate.Hour, schedule.StartDate.Minute, schedule.StartDate.Second);
			EndTime = new TimeSpan(schedule.EndDate.Hour, schedule.EndDate.Minute, schedule.EndDate.Second);
			if(schedule.isAllDay) swAllDay.IsToggled = true;
			ProjectId = project.Id;
			ProjectText = project.Title;
			RepeatDiv = schedule.RepeatDiv;
			resources["RepeatText"] = GetRepeatText(schedule.RepeatDiv);
			PlaceText = schedule.Place;
			NotesText = schedule.Notes;

			// TODO 全メンバーIDを設定。全メンバー名も画面表示用に要設定
			foreach (ScheduleMember scheduleMember in scheduleMemberList) {
				MemberIdList.Add(scheduleMember.MemberId);
			}
		}

		/// <summary>
		/// タスク編集時の項目を設定する。
		/// </summary>
		/// <param name="id">ID</param>
		private void SetEditTask(long id) {

			TaskDao taskDao = new TaskDao();
			Task task = taskDao.GetTaskById(id);

			ProjectDao projectDao = new ProjectDao();
			Project project = projectDao.GetProjectById(task.ProjectId);

			TaskMemberDao taskMemberDao = new TaskMemberDao();
			List<TaskMember> taskMemberList = taskMemberDao.GetTaskMemberListByTaskId(task.Id);

			TitleText = task.Title;
			StartDate = task.StartDate;
			EndDate = task.EndDate;
			ProjectId = project.Id;
			ProjectText = project.Title;
			ExpectedDailyWorkTimeText = task.ExpectedDailyWorkTime.TotalHours.ToString();
			NotesText = task.Notes;

			// TODO 全メンバーIDを設定。全メンバー名も画面表示用に要設定
			foreach (TaskMember taskMember in taskMemberList) {
				MemberIdList.Add(taskMember.MemberId);
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
					return null;
			}
		}
	}
}