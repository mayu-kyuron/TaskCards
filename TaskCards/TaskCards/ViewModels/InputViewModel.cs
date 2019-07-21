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
		public string ProjectText { get; set; }
		public string Member1Text { get; set; }
		public string AddText { get; set; }
		public string RepeatText { get; set; }
		public string PlaceText { get; set; }
		public string NotesText { get; set; }

		public TimeSpan StartTime { get; set; }
		public TimeSpan EndTime { get; set; }
		public long ProjectId { get; set; }
		public List<long> MemberIdList { get; set; } = new List<long>();
		public RepeatDiv RepeatDiv { get; set; }

		public InputViewModel(DateTime selectedDate, double height, ContentView cvDialogBack, ResourceDictionary resources) {

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

			// 開始時間：現在時間＋1時間－現在の分数（例：12:34→13:00）
			// 終了時間：現在時間＋2時間－現在の分数（例：12:34→14:00）
			StartTime = new TimeSpan(DateTime.Now.AddHours(1).Hour, 0, 0);
			EndTime = new TimeSpan(DateTime.Now.AddHours(2).Hour, 0, 0);

			// TODO 仮のプロジェクトを初期選択に
			long id = 1;
			ProjectDao projectDao = new ProjectDao();
			Project project = projectDao.GetProjectById(id);
			ProjectId = project.Id;
			ProjectText = project.Title;

			// TODO 仮のメンバーを初期選択に
			long id2 = 1;
			MemberDao memberDao = new MemberDao();
			Member member = memberDao.GetMemberById(id2);
			MemberIdList.Add(member.Id);
			Member1Text = member.Name;

			// 繰り返し項目を設定
			for (int i = 1; i <= 4; i++) {

				var selectedItemDto = new SelectedItemDto();
				selectedItemDto.Id = i;

				switch (selectedItemDto.Id) {
					case (long)RepeatDiv.毎日:
						selectedItemDto.Name = StringConst.RepeatEveryday;
						break;
					case (long)RepeatDiv.毎週:
						selectedItemDto.Name = StringConst.RepeatEveryWeek;
						break;
					case (long)RepeatDiv.毎月:
						selectedItemDto.Name = StringConst.RepeatEveryMonth;
						break;
					case (long)RepeatDiv.毎年:
						selectedItemDto.Name = StringConst.RepeatEveryYear;
						break;
				}

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
	}
}