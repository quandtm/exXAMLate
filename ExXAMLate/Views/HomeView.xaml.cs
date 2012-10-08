using ExXAMLate.Models;

namespace ExXAMLate.Views
{
    public sealed partial class HomeView
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void itemGridView_ItemClick_1(object sender, Windows.UI.Xaml.Controls.ItemClickEventArgs e)
        {
            var item = ((HomeViewGroupItem)e.ClickedItem);
            if (item.Clicked != null)
                item.Clicked.Execute(null);
        }
    }
}
