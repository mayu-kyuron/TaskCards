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
	/// 予定確認ページ
	/// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConfirmSchedulePage : ContentPage {

		private ConfirmScheduleViewModel viewModel;

		long scheduleId; // スケジュールID

		public ConfirmSchedulePage() {
			Initialize();
		}

		public ConfirmSchedulePage(long scheduleId) {
			this.scheduleId = scheduleId;
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

			var tgrEdit = new TapGestureRecognizer();
			tgrEdit.Tapped += (sender, e) => OnClickEdit(sender, e);
			gdEdit.GestureRecognizers.Add(tgrEdit);

			var tgrDelete = new TapGestureRecognizer();
			tgrDelete.Tapped += (sender, e) => OnClickDelete(sender, e);
			gdDelete.GestureRecognizers.Add(tgrDelete);
		}

		/// <summary>
		/// 画面サイズ変更イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void OnSizeChanged(object sender, EventArgs args) {
			this.viewModel = new ConfirmScheduleViewModel(this.scheduleId, Height, gdTime, lblAllDay, gdMember);
			BindingContext = this.viewModel;

			double dialogHeight = (int)Math.Round(Height * LayoutRateConst.ListItemHeight) * 2 + 1;
			double dialogMarginTop = gdHeader.Height + Height / 62;

			// ダイアログの高さを設定
			gdDialogBack.RowDefinitions.Add(new RowDefinition { Height = dialogMarginTop });
			gdDialogBack.RowDefinitions.Add(new RowDefinition { Height = dialogHeight });
			gdDialogBack.RowDefinitions.Add(new RowDefinition { Height = Height - (dialogMarginTop + dialogHeight) });
		}

		/// <summary>
		/// ダイアログ背面クリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickDialogBack(object sender, EventArgs e) {
			cvDialogBack.IsVisible = false;
			gdDialogOptionNormal.IsVisible = false;
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
		/// 右上ボタン（メニューボタン）クリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickTopRightButton(object sender, EventArgs e) {
			cvDialogBack.IsVisible = true;
			gdDialogOptionNormal.IsVisible = true;
		}

		/// <summary>
		/// 編集クリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickEdit(object sender, EventArgs e) {

			ScheduleDao scheduleDao = new ScheduleDao();
			Schedule schedule = scheduleDao.GetScheduleById(this.scheduleId);

			// 予定編集用の入力ページに遷移
			Application.Current.MainPage = new InputPage(schedule.StartDate,
				TableDiv.予定, PageDiv.スケジュール確認, ExecuteDiv.更新, this.scheduleId);
		}

		/// <summary>
		/// 削除クリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickDelete(object sender, EventArgs e) {

			Device.BeginInvokeOnMainThread(async () => {
				var result = await DisplayAlert(StringConst.DialogTitleConfirm, 
					string.Format(StringConst.MessageDeleteConfirm, StringConst.WordSchedule),
					StringConst.DialogAnswerPositive, StringConst.DialogAnswerNegative);

				if (result) {
					DeleteScheduleAndRelatedData(this.scheduleId);
					OnPageBack();
				}
			});
		}

		/// <summary>
		/// 前ページに戻る。
		/// </summary>
		private void OnPageBack() {
			Application.Current.MainPage = new TaskCardsMasterDetailPage(new DetailPage());
		}

		/// <summary>
		/// スケジュールとその関連データを削除する。
		/// </summary>
		/// <param name="scheduleId">スケジュールID</param>
		private void DeleteScheduleAndRelatedData(long scheduleId) {

			// スケジュールの削除
			var scheduleDao = new ScheduleDao();
			scheduleDao.Delete(scheduleId);

			// 全スケジュールメンバーの削除
			var scheduleMemberDao = new ScheduleMemberDao();
			scheduleMemberDao.DeleteAllByScheduleId(scheduleId);
		}
	}
}