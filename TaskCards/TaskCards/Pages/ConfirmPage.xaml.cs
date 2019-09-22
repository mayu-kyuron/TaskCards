using System;
using System.Collections.Generic;
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
	/// 確認ページ
	/// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConfirmPage : ContentPage {

		private ConfirmViewModel viewModel;

		DateTime selectedDate = DateTime.Today; // 選択された日付
		TableDiv tableDiv; // データ登録先のテーブル区分
		PageDiv exPageDiv; // 元のページ区分
		long id; // ID

		public ConfirmPage() {
			Initialize();
		}

		public ConfirmPage(DateTime selectedDate, TableDiv tableDiv, PageDiv exPageDiv, long id) {
			this.selectedDate = selectedDate;
			this.tableDiv = tableDiv;
			this.exPageDiv = exPageDiv;
			this.id = id;
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

			var tgrAdd = new TapGestureRecognizer();
			tgrAdd.Tapped += (sender, e) => OnClickAdd(sender, e);
			gdAdd.GestureRecognizers.Add(tgrAdd);

			// 各コントロールの表示切り替え
			switch (this.tableDiv) {
				case TableDiv.タスク:
					break;
				case TableDiv.プロジェクト:
					gdSales.IsVisible = true;
					break;
			}
		}

		/// <summary>
		/// 画面サイズ変更イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void OnSizeChanged(object sender, EventArgs args) {
			this.viewModel = new ConfirmViewModel(this.tableDiv, this.id, Height, gdWorkTime, gdMember);
			BindingContext = this.viewModel;
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
		/// 右上ボタン（メニューボタン）クリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickTopRightButton(object sender, EventArgs e) {
			cvDialogBack.IsVisible = true;
		}

		/// <summary>
		/// 編集クリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickEdit(object sender, EventArgs e) {

			switch (this.tableDiv) {

				case TableDiv.タスク: {
						TaskDao taskDao = new TaskDao();
						Task task = taskDao.GetTaskById(this.id);

						// タスク編集用の入力ページに遷移
						Application.Current.MainPage = new InputPage(task.StartDate,
							TableDiv.タスク, PageDiv.確認, ExecuteDiv.更新, this.id);
					}
					break;

				case TableDiv.プロジェクト:
					break;
			}
		}

		/// <summary>
		/// 削除クリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickDelete(object sender, EventArgs e) {

			string messageArg0 = null;
			switch (this.tableDiv) {
				case TableDiv.タスク:
					messageArg0 = StringConst.WordTask;
					break;
				case TableDiv.プロジェクト:
					messageArg0 = StringConst.WordProject;
					break;
			}

			Device.BeginInvokeOnMainThread((async () => {
				var result = await DisplayAlert(StringConst.DialogTitleConfirm,
					String.Format(StringConst.MessageDeleteConfirm, messageArg0),
					StringConst.DialogAnswerPositive, StringConst.DialogAnswerNegative);

				if (result) {
					switch (this.tableDiv) {

						case TableDiv.タスク:
							DeleteTaskAndRelatedData();
							OnPageBack();
							break;

						case TableDiv.プロジェクト:
							ProjectDao projectDao = new ProjectDao();
							projectDao.Delete(this.id);
							OnPageBack();
							break;
					}
				}
			}));
		}

		/// <summary>
		/// 〜を追加するクリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickAdd(object sender, EventArgs e) {

			switch (this.tableDiv) {
				case TableDiv.タスク:
					// 作業記録入力ページに遷移
					Application.Current.MainPage = new InputWorkPage(this.id);
					break;
			}
		}

		/// <summary>
		/// 前ページに戻る。
		/// </summary>
		private void OnPageBack() {

			switch (this.exPageDiv) {
				case PageDiv.カレンダー:
					Application.Current.MainPage = new TaskCardsMasterDetailPage(new DetailPage());
					break;
			}
		}

		/// <summary>
		/// タスクとその関連データを削除する。
		/// </summary>
		private void DeleteTaskAndRelatedData() {

			// タスクの削除
			TaskDao taskDao = new TaskDao();
			taskDao.Delete(this.id);

			// 全タスクメンバーの削除
			TaskMemberDao taskMemberDao = new TaskMemberDao();
			List<TaskMember> taskMemberList = taskMemberDao.GetTaskMemberListByTaskId(this.id);
			taskMemberDao.DeleteAllByTaskId(this.id);

			// 全タスク進捗の削除
			TaskProgressDao taskProgressDao = new TaskProgressDao();
			foreach (TaskMember taskMember in taskMemberList) {
				taskProgressDao.DeleteAllByTaskMemberId(taskMember.Id);
			}
		}
	}
}