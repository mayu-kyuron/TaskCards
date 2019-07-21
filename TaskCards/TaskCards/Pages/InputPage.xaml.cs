using Acr.UserDialogs;
using System;
using TaskCards.Consts;
using TaskCards.Dao;
using TaskCards.Divisions;
using TaskCards.Entities;
using TaskCards.MasterPages;
using TaskCards.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskCards.Pages {

	/// <summary>
	/// 入力ページ
	/// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InputPage : ContentPage {

		private InputViewModel viewModel;

		DateTime selectedDate = DateTime.Today; // 選択された日付
		TableDiv tableDiv; // データ登録先のテーブル区分
		PageDiv exPageDiv; // 元のページ区分
		ExecuteDiv executeDiv; // 実行区分

		public InputPage() {
			Initialize();
		}

		public InputPage(DateTime selectedDate, TableDiv tableDiv, PageDiv exPageDiv, ExecuteDiv executeDiv) {
			this.selectedDate = selectedDate;
			this.tableDiv = tableDiv;
			this.exPageDiv = exPageDiv;
			this.executeDiv = executeDiv;
			Initialize();
		}

		/// <summary>
		/// 初期化する。
		/// </summary>
		private void Initialize() {
			InitializeComponent();

			SizeChanged += OnSizeChanged;

			// タップイベントを付与
			var tgrDialogBack = new TapGestureRecognizer();
			tgrDialogBack.Tapped += (sender, e) => OnClickDialogBack(sender, e);
			cvDialogBack.GestureRecognizers.Add(tgrDialogBack);

			var tgrDialogFront = new TapGestureRecognizer();
			tgrDialogFront.Tapped += (sender, e) => OnClickDialogFront(sender, e);
			gdDialogFront.GestureRecognizers.Add(tgrDialogFront);

			var tgrBackButton = new TapGestureRecognizer();
			tgrBackButton.Tapped += (sender, e) => OnClickBackButton(sender, e);
			imgBackButton.GestureRecognizers.Add(tgrBackButton);

			var tgrTopRightButton = new TapGestureRecognizer();
			tgrTopRightButton.Tapped += (sender, e) => OnClickTopRightButton(sender, e);
			imgTopRightButton.GestureRecognizers.Add(tgrTopRightButton);

			//var tgrProject = new TapGestureRecognizer();
			//tgrProject.Tapped += (sender, e) => OnClickProject(sender, e);
			//gdProject.GestureRecognizers.Add(tgrProject);

			var tgrRepeatGrid = new TapGestureRecognizer();
			tgrRepeatGrid.Tapped += (sender, e) => OnClickRepeatGrid(sender, e);
			gdRepeat.GestureRecognizers.Add(tgrRepeatGrid);

			var tgrRepeatCancel = new TapGestureRecognizer();
			tgrRepeatCancel.Tapped += (sender, e) => OnClickRepeatCancel(sender, e);
			imgRepeat.GestureRecognizers.Add(tgrRepeatCancel);
		}

		/// <summary>
		/// 画面サイズ変更イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void OnSizeChanged(object sender, EventArgs args) {
			this.viewModel = new InputViewModel(this.selectedDate, Height, cvDialogBack, Resources);
			BindingContext = this.viewModel;

			lstRepeat.RowHeight = (int)Math.Round(Height * LayoutRateConst.ListItemHeight);

			double dialogHeight = lstRepeat.RowHeight * 4 + 2;
			gdDialogBack.RowDefinitions.Add(new RowDefinition { Height = (Height - dialogHeight) / 2 });
			gdDialogBack.RowDefinitions.Add(new RowDefinition { Height = dialogHeight });
			gdDialogBack.RowDefinitions.Add(new RowDefinition { Height = (Height - dialogHeight) / 2 });
		}

		/// <summary>
		/// ダイアログ背面クリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickDialogBack(object sender, EventArgs e) {
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
		/// 戻るボタンクリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickBackButton(object sender, EventArgs e) => OnPageBack();
		protected override bool OnBackButtonPressed() {
			OnPageBack();
			return true;
		}

		/// <summary>
		/// 右上ボタン（確定ボタン）クリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickTopRightButton(object sender, EventArgs e) {

			// 入力検証
			if (!Validate()) return;

			// データ更新処理
			UserDialogs.Instance.ShowLoading("更新中...");

			switch (this.tableDiv) {
				case TableDiv.プロジェクト:
					break;
				case TableDiv.予定:
					InsertScheduleAndScheduleMembers();
					break;
				case TableDiv.タスク:
					break;
			}

			UserDialogs.Instance.HideLoading();

			// 完了ダイアログ
			Device.BeginInvokeOnMainThread((async () => {
				await DisplayAlert(StringConst.DialogTitleSuccess, 
					String.Format(StringConst.MessageSucceeded, "更新"), StringConst.DialogAnswerPositive);
			}));

			// 遷移
			switch (this.exPageDiv) {
				case PageDiv.カレンダー:
					Application.Current.MainPage = new TaskCardsMasterDetailPage(new DetailPage());
					break;
			}
		}

		/// <summary>
		/// 終日トグルイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnToggleAllDay(object sender, ToggledEventArgs e) {

			if (swAllDay.IsToggled) {
				gdTime.IsVisible = false;
			}
			else {
				gdTime.IsVisible = true;
			}
		}

		/// <summary>
		/// プロジェクトクリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickProject(object sender, EventArgs e) {

		}

		/// <summary>
		/// 繰り返しグリッドクリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickRepeatGrid(object sender, EventArgs e) {
			cvDialogBack.IsVisible = true;
		}

		/// <summary>
		/// 繰り返しキャンセルクリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickRepeatCancel(object sender, EventArgs e) {
			this.viewModel.RepeatDiv = RepeatDiv.繰り返しなし;
			Resources["RepeatText"] = StringConst.RepeatNone;
		}

		/// <summary>
		/// 前ページに戻る。
		/// </summary>
		private void OnPageBack() {

			Device.BeginInvokeOnMainThread((async () => {
				var result = await DisplayAlert(StringConst.DialogTitleConfirm, StringConst.MessageInputCancelConfirm, 
					StringConst.DialogAnswerPositive, StringConst.DialogAnswerNegative);

				if (result) Application.Current.MainPage = new TaskCardsMasterDetailPage(new DetailPage());
			}));
		}

		/// <summary>
		/// 入力検証する。
		/// </summary>
		/// <returns>検証OKなら「true」、NGなら「false」</returns>
		private bool Validate() {

			// タイトル入力チェック
			if (String.IsNullOrEmpty(this.viewModel.TitleText)) {

				Device.BeginInvokeOnMainThread((async () => {
					await DisplayAlert(StringConst.DialogTitleError,
						String.Format(StringConst.MessageEntryNeeded, "タイトル"), StringConst.DialogAnswerPositive);
				}));

				return false;
			}

			// 時間または日付の整合性チェック
			if (TimeSpan.Compare(this.viewModel.StartTime, this.viewModel.EndTime) == 1) {

				string dateOrTime = "日";
				if (this.tableDiv == TableDiv.予定) dateOrTime = "時間";

				Device.BeginInvokeOnMainThread((async () => {
					await DisplayAlert(StringConst.DialogTitleError,
						String.Format(StringConst.MessageWrongTimeSpan, "開始" + dateOrTime, "終了" + dateOrTime),
						StringConst.DialogAnswerPositive);
				}));

				return false;
			}

			return true;
		}

		/// <summary>
		/// スケジュールと所属メンバーを登録する。
		/// </summary>
		private void InsertScheduleAndScheduleMembers() {

			DateTime startDate = this.selectedDate.Date;
			DateTime endDate = this.selectedDate.Date;
			if (!swAllDay.IsToggled) {
				startDate += this.viewModel.StartTime;
				endDate += this.viewModel.EndTime;
			}

			// スケジュールの登録
			var schedule = new Schedule {
				Title = this.viewModel.TitleText,
				StartDate = startDate,
				EndDate = endDate,
				isAllDay = swAllDay.IsToggled,
				ProjectId = this.viewModel.ProjectId,
				RepeatDiv = this.viewModel.RepeatDiv,
				Place = this.viewModel.PlaceText,
				Notes = this.viewModel.NotesText
			};

			var scheduleDao = new ScheduleDao();
			long scheduleId = scheduleDao.Insert(schedule);

			// スケジュールメンバーの登録
			var scheduleMemberDao = new ScheduleMemberDao();
			foreach (long memberId in this.viewModel.MemberIdList) {

				var scheduleMember = new ScheduleMember {
					ScheduleId = scheduleId,
					MemberId = memberId,
					CanEdit = true // 仮に全員が編集可能に
				};

				scheduleMemberDao.Insert(scheduleMember);
			}
		}
	}
}