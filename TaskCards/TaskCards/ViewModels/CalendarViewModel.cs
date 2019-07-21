using System;
using System.Windows.Input;
using TaskCards.Consts;
using Xamarin.Forms;

namespace TaskCards.ViewModels {

	/// <summary>
	/// カレンダーページのビューモデル
	/// </summary>
    public class CalendarViewModel {

		public double DialogBaseHeight { get; set; }

		public ImageSource LeftArrowSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "left_arrow.png");
		public ImageSource RightArrowSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "right_arrow.png");
		public ImageSource AddSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "btn_add.png");
		public ImageSource DateTappedDialogTitleIconSource { get; set; } = ImageSource.FromResource(StringConst.ImageFolderPath + "repeat.png");

		public string DateText { get; set; }
		public string ScheduleTime1Text { get; set; }
		public string ScheduleTitle1Text { get; set; }
		public string ScheduleTime2Text { get; set; }
		public string ScheduleTitle2Text { get; set; }
		public string TaskTime1Text { get; set; }
		public string TaskTitle1Text { get; set; }
		public string TaskTime2Text { get; set; }
		public string TaskTitle2Text { get; set; }

		public Color ScheduleTitle1Color { get; set; }
		public Color ScheduleTitle2Color { get; set; }
		public Color TaskTitle1Color { get; set; }
		public Color TaskTitle2Color { get; set; }

		public ICommand TappedCellCommand { get; private set; }

		public DateTime selectedDate = DateTime.Today; // 選択された日付

		private ContentView cvDialogBack;
		private Label lblDate;

		public CalendarViewModel(DateTime selectedDate, ContentView cvDialogBack, Label lblDate) {

			this.cvDialogBack = cvDialogBack;
			this.lblDate = lblDate;

			DialogBaseHeight = 450;

			DateText = selectedDate.ToString(StringConst.DateTappedDialogDateFormat);
			ScheduleTime1Text = "終日";
			ScheduleTitle1Text = "スケジュール１";
			ScheduleTime2Text = "14:00〜15:00";
			ScheduleTitle2Text = "スケジュール２";
			TaskTime1Text = "〜5/8(水)";
			TaskTitle1Text = "タスク１";
			TaskTime2Text = "〜5/2(木)";
			TaskTitle2Text = "タスク２";

			ScheduleTitle1Color = Color.FromHex("#E40000");
			ScheduleTitle2Color = Color.FromHex("#E40000");
			TaskTitle1Color = Color.FromHex("#E40000");
			TaskTitle2Color = Color.FromHex("#E40000");

			TappedCellCommand = new Command<string>(OnTapCell);
		}

		private void OnTapCell(string selectedDateStr) {
			this.selectedDate = DateTime.Parse(selectedDateStr);
			this.lblDate.Text = this.selectedDate.ToString(StringConst.DateTappedDialogDateFormat);
			this.cvDialogBack.IsVisible = true;
		}
	}
}