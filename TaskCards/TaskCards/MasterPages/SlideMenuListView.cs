using Xamarin.Forms;
using System.Collections.Generic;

namespace TaskCards.MasterPages {

    public class SlideMenuListView : ListView {

		public SlideMenuListView() {

			List<SlideMenuItem> menuList = new SlideMenuListData();

			ItemsSource = menuList;
			VerticalOptions = LayoutOptions.FillAndExpand;
			BackgroundColor = Color.Transparent;
			SeparatorVisibility = SeparatorVisibility.None;

			var cell = new DataTemplate(typeof(SlideMenuCell));
			cell.SetBinding(SlideMenuCell.TextProperty, "Title");
			cell.SetBinding(SlideMenuCell.ImageSourceProperty, "IconSource");

			ItemTemplate = cell;

		}

    }
}
