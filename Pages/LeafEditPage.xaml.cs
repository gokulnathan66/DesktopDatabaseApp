using System.Collections.ObjectModel;

namespace FinalProject.Pages
{
    public partial class LeafEditPage : ContentPage
    {
        private DatabaseService _databaseService;
        private ObservableCollection<string> LeafList;

        public LeafEditPage()
        {
            InitializeComponent();
            _databaseService = new MainViewModel().databaseService;
            LoadLeafTypes();
        }

        private void LoadLeafTypes()
        {
            LeafList = new ObservableCollection<string>(_databaseService.GetLeafList());
            LeafPicker.ItemsSource = LeafList;  // Bind leaf list to the Picker
        }

        private void OnAddLeafClicked(object sender, EventArgs e)
        {
            string newLeafType = LeafEntry.Text;
            int baseRate = int.Parse(BaseRate.Text);
            int value = int.Parse(Value.Text);

            if (!string.IsNullOrEmpty(newLeafType))
            {
                _databaseService.AddLeaf(newLeafType, baseRate, value);
                LeafList.Add(newLeafType);
                LeafEntry.Text = string.Empty;  // Clear the entry field
                BaseRate.Text = string.Empty;  // Clear the base rate field
                Value.Text = string.Empty;     // Clear the value field
            }
        }

        private void OnUpdateLeafClicked(object sender, EventArgs e)
        {
            string selectedLeafType = LeafPicker.SelectedItem as string;
            string newLeafName = LeafEntry.Text;
            int baseRate = int.Parse(BaseRate.Text);
            int value = int.Parse(Value.Text);

            if (!string.IsNullOrEmpty(selectedLeafType) && !string.IsNullOrEmpty(newLeafName))
            {
                _databaseService.UpdateLeaf(selectedLeafType, newLeafName, baseRate, value);
                int index = LeafList.IndexOf(selectedLeafType);
                LeafList[index] = newLeafName;  // Update the ObservableCollection
                LeafEntry.Text = string.Empty;  // Clear the entry field
                BaseRate.Text = string.Empty;  // Clear the base rate field
                Value.Text = string.Empty;     // Clear the value field
            }
        }

        private void OnDeleteLeafClicked(object sender, EventArgs e)
        {
            string selectedLeafType = LeafPicker.SelectedItem as string;

            if (!string.IsNullOrEmpty(selectedLeafType))
            {
                _databaseService.DeleteLeaf(selectedLeafType);
                LeafList.Remove(selectedLeafType);  // Remove from ObservableCollection
                LeafEntry.Text = string.Empty;  // Clear the entry field
                BaseRate.Text = string.Empty;  // Clear the base rate field
                Value.Text = string.Empty;     // Clear the value field
            }
        }
    }
}
