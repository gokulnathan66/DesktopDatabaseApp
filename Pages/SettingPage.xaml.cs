namespace FinalProject.Pages
{
    public partial class SettingPage : ContentPage
    {
        public SettingPage()
        {
            InitializeComponent();
        }

        // Navigate to Supplier Edit Page
        private async void OnEditSupplierClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SupplierEditPage());
        }

        // Navigate to Leaf Edit Page
        private async void OnEditLeafClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LeafEditPage());
        }
    }
}
