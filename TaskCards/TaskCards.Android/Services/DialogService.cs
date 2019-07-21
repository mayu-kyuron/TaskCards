using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using System;
using TaskCards.Consts;
using TaskCards.Droid.Services;
using TaskCards.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(DialogService))]
namespace TaskCards.Droid.Services {

	/// <summary>
	/// ダイアログサービス
	/// </summary>
	public class DialogService : IDialogService {

		/// <summary>
		/// 日付タップ時のダイアログを表示する。
		/// </summary>
		/// <param name="selectedDate">選択日付</param>
		public void ShowDateTappedDialog(DateTime selectedDate) {
			Context context = Forms.Context;

			var layoutBase = new LinearLayout(context) { Orientation = Orientation.Vertical };
			layoutBase.SetGravity(GravityFlags.Left);

			// 選択日付
			layoutBase.AddView(new TextView(context) { Text = selectedDate.ToString() });

			// スケジュールタイトルと追加ボタン
			var layoutSide1 = new LinearLayout(context) {
				Orientation = Orientation.Horizontal,
				LayoutParameters = new ViewGroup.LayoutParams(
					ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent),
			};
			layoutSide1.SetGravity(GravityFlags.Center);

			layoutSide1.AddView(new TextView(context) { Text = StringConst.WordSchedule });

			var viewCenter1 = new Android.Views.View(context);
			LinearLayout.LayoutParams viewCenter1Params = new LinearLayout.LayoutParams(0, 1);
			viewCenter1Params.Weight = 1;
			viewCenter1.LayoutParameters = viewCenter1Params;
			layoutSide1.AddView(viewCenter1);

			var btnAddSchedule = new Android.Widget.Button(context);
			btnAddSchedule.SetBackgroundResource(Resource.Drawable.btn_add);
			layoutSide1.AddView(btnAddSchedule);

			layoutBase.AddView(layoutSide1);

			var dlg = new AlertDialog.Builder(context);
			dlg.SetTitle("タイトル");
			dlg.SetView(layoutBase);
			dlg.SetPositiveButton("OK", (s, a) 
				=> Toast.MakeText(context, 
							"テスト中",
							//string.Format("{0}:{1}", textUser.Text, textPass.Text),
							ToastLength.Short)
						.Show());
			dlg.Create().Show();
		}
	}
}