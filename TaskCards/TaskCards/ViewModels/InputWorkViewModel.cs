using System;
using TaskCards.Consts;
using TaskCards.Dao;
using TaskCards.Entities;
using Xamarin.Forms;

namespace TaskCards.ViewModels {

	/// <summary>
	/// 作業記録入力ページのビューモデル
	/// </summary>
	public class InputWorkViewModel {

		public double DialogBaseHeight { get; set; }

		public ImageSource BackButtonSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "btn_back.png");
		public ImageSource TopRightButtonSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "btn_check.png");
		public ImageSource InputConfirmDateTimeArrowSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "date_time_right_arrow.png");

		public string TopDateText { get; set; }
		public string ProjectText { get; set; }
		public string TitleText { get; set; }
		public string ProgressRateText { get; set; }
		public string NotesText { get; set; }

		public TimeSpan StartTime { get; set; }
		public TimeSpan EndTime { get; set; }
		public long ProjectId { get; set; }

		public double exProgressRate = 0;

		public InputWorkViewModel(long taskId, double height) {

			// レイアウト全体の高さの設定
			// 項目を増やすごとに、デバイスの高さに対して項目の高さ分、掛ける割合を増やしていく。
			// 比率の計算方法　変更前のレイアウト全体の高さ:変更前の比率＝変更後のレイアウト全体の高さ:今回の比率
			// レイアウト全体の高さは、<Grid.RowDefinitions> RowDefinitionのHeightの合計
			DialogBaseHeight = height * 1;

			TaskDao taskDao = new TaskDao();
			Task task = taskDao.GetTaskById(taskId);

			ProjectDao projectDao = new ProjectDao();
			Project project = projectDao.GetProjectById(task.ProjectId);

			TopDateText = task.StartDate.ToString(StringConst.DateTappedDialogDateFormat);
			ProjectId = project.Id;
			ProjectText = project.Title;
			TitleText = task.Title;
			ProgressRateText = task.ProgressRate.ToString("0");
			NotesText = task.Notes;

			// 開始時間：現在時間－現在の分数（例：12:34→12:00）
			// 終了時間：現在時間＋1時間－現在の分数（例：12:34→13:00）
			StartTime = new TimeSpan(DateTime.Now.Hour, 0, 0);
			EndTime = new TimeSpan(DateTime.Now.AddHours(1).Hour, 0, 0);

			this.exProgressRate = task.ProgressRate;
		}
	}
}