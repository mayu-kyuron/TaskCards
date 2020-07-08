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

		public ImageSource BackButtonSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "btn_back.png");
		public ImageSource TopRightButtonSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "btn_menu.png");
		public ImageSource InputConfirmDateTimeArrowSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "date_time_right_arrow.png");
		public ImageSource InputConfirmArrowIconSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "input_right_arrow.png");

		public string TopDateText { get; set; }
		public string TitleText { get; set; }
		public string ProjectText { get; set; }
		public string StartTimeText { get; set; }
		public string EndTimeText { get; set; }
		public string PlaceText { get; set; }
		public string NotesText { get; set; }

		public ConfirmScheduleViewModel(long scheduleId, double height, Grid gdTime, Label lblAllDay, Grid gdMember) {

			// レイアウト全体の高さの設定
			// 項目を増やすごとに、デバイスの高さに対して項目の高さ分、掛ける割合を増やしていく。
			// 比率の計算方法　変更前のレイアウト全体の高さ:変更前の比率＝変更後のレイアウト全体の高さ:今回の比率
			// レイアウト全体の高さは、<Grid.RowDefinitions> RowDefinitionのHeightの合計
			DialogBaseHeight = height * 1;

			ScheduleDao scheduleDao = new ScheduleDao();
			Schedule schedule = scheduleDao.GetScheduleById(scheduleId);

			ProjectDao projectDao = new ProjectDao();
			Project project = projectDao.GetProjectById(schedule.ProjectId);

			TopDateText = schedule.StartDate.ToString(StringConst.DateTappedDialogDateFormat);
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

			// メンバーのグリッドに要素を追加
			SetMember(scheduleId, gdMember);
		}

		/// <summary>
		/// メンバーを設定する。
		/// </summary>
		/// <param name="scheduleId">スケジュールID</param>
		/// <param name="gdMember">メンバーのグリッド</param>
		private void SetMember(long scheduleId, Grid gdMember) {

			var scheduleMemberDao = new ScheduleMemberDao();
			List<ScheduleMember> scheduleMemberList = scheduleMemberDao.GetScheduleMemberListByScheduleId(scheduleId);

			int scheduleMemberIndex = 0;
			foreach (ScheduleMember scheduleMember in scheduleMemberList) {

				// メンバーのグリッドに要素を追加
				gdMember.Children.Add(GetMemberGrid(scheduleMember.MemberId), 0, scheduleMemberIndex);

				scheduleMemberIndex++;
			}

			// メンバーのグリッドに不足分の空白を追加
			AddBlankTo3ItemsGrid(scheduleMemberList.Count, gdMember);
		}

		/// <summary>
		/// メンバーのグリッドを取得する。
		/// </summary>
		/// <param name="memberId">メンバーID</param>
		/// <returns>グリッド</returns>
		private Grid GetMemberGrid(long memberId) {

			// グリッドを生成
			var grid = new Grid {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				ColumnSpacing = 0,
				RowSpacing = 0,
			};

			// メンバーのラベルを生成
			var memberDao = new MemberDao();
			var memberLabel = new Label {
				Text = memberDao.GetMemberById(memberId).Name,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromHex("#2F4F4F"),
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
			};

			grid.Children.Add(memberLabel, 0, 1, 0, 1);

			return grid;
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