using Android.Content;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using TaskCards.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

//[assembly: ExportRenderer(typeof(TabbedPage), typeof(CustomTabbedPageRenderer))]
namespace TaskCards.Droid.Renderers {

	/// <summary>
	/// タブページのカスタムレンダラ―
	/// </summary>
	//public class CustomTabbedPageRenderer : TabbedRenderer {

	//	public CustomTabbedPageRenderer(Context context) : base(context) {
	//		AutoPackage = false;
	//	}

		//protected override void OnLayout(bool changed, int l, int t, int r, int b) {

		//	ViewGroup.ScaleY = -1;

		//	TabLayout tabLayout = null;
		//	ViewPager viewPager = null;

		//	for (int i = 0; i < ChildCount; ++i) {
		//		Android.Views.View view = (Android.Views.View)GetChildAt(i);
		//		if (view is TabLayout) tabLayout = (TabLayout)view;
		//		else if (view is ViewPager) viewPager = (ViewPager)view;
		//	}

		//	tabLayout.ScaleY = viewPager.ScaleY = -1;
		//	viewPager.SetPadding(0, -tabLayout.MeasuredHeight, 0, 0);
		//}
	//}
}