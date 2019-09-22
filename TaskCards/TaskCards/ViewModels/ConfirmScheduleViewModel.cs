using System.Collections.Generic;
using TaskCards.Consts;
using TaskCards.Dao;
using TaskCards.Entities;
using Xamarin.Forms;

namespace TaskCards.ViewModels {

	/// <summary>
	/// スケジュール確認ページのビューモデル
	/// </summary>
	public class ConfirmScheduleViewModel {

		public double DialogBaseHeight { get; set; }
		public double MemberHeight { get; set; }

		public ImageSource BackButtonSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "btn_back.png");
		public ImageSource TopRightButtonSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "btn_menu.png");
		public ImageSource InputConfirmDateTimeArrowSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "date_time_right_arrow.png");
		public ImageSource InputConfirmArrowIconSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "input_right_arrow.png");

		public string TopDateText { get; set; }
		public string TitleText { get; set; }
		public string ProjectText { get; set; }
		public string StartTimeText { get; set; }
		public string EndTimeText { get; set; }
		public string Member1Text { get; set; }
		public string PlaceText { get; set; }
		public string NotesText { get; set; }

		public long ProjectId { get; set; }
		public List<long> MemberIdList { get; set; } = new List<long>();

		public ConfirmScheduleViewModel(long scheduleId, double height, Grid gdTime, Label lblAllDay) {

			// レイアウト全体の高さの設定
			// 項目を増やすごとに、デバイスの高さに対して項目の高さ分、掛ける割合を増やしていく。
			// 比率の計算方法　変更前のレイアウト全体の高さ:変更前の比率＝変更後のレイアウト全体の高さ:今回の比率
			// レイアウト全体の高さは、<Grid.RowDefinitions> RowDefinitionのHeightの合計
			DialogBaseHeight = height * 1;

			// メンバー追加欄の高さ（(画面の高さ - 上下マージン) / 全行数 * メンバーの行数）
			MemberHeight = (height - height / 62 * 2) / 14 * 3;

			ScheduleDao scheduleDao = new ScheduleDao();
			Schedule schedule = scheduleDao.GetScheduleById(scheduleId);

			ProjectDao projectDao = new ProjectDao();
			Project project = projectDao.GetProjectById(schedule.ProjectId);

			TopDateText = schedule.StartDate.ToString(StringConst.DateTappedDialogDateFormat);
			ProjectId = project.Id;
			ProjectText = project.Title;
			TitleText = schedule.Title;
			if (schedule.isAllDay) {
				gdTime.IsVisible = false;
				lblAllDay.IsVisible = true;
			}
			else {
				StartTimeText = schedule.StartDate.ToString(StringConst.InputConfirmTimeFormat);
				EndTimeText = schedule.EndDate.ToString(StringConst.InputConfirmTimeFormat);
			}
			PlaceText = schedule.Place;
			NotesText = schedule.Notes;
		}
	}
}