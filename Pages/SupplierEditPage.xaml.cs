using System.Collections.ObjectModel;

namespace FinalProject.Pages;

public partial class SupplierEditPage : ContentPage
{
    private DatabaseService _databaseService;
    private ObservableCollection<string> SupplierList;

    public SupplierEditPage()
    {
        InitializeComponent();
        BindingContext = this;


        // Instantiate MainViewModel and get the database service
        var service = new MainViewModel();
        _databaseService = service.databaseService;  // Assign databaseService from MainViewModel

        LoadSuppliers();  // Load supplier list into the UI
    }

    // Load suppliers from the database and bind to the DataGrid
    private void LoadSuppliers()
    {
        SupplierList = new ObservableCollection<string>(_databaseService.GetSupplier_list());
        SupplierDataGrid.ItemsSource = SupplierList;  // Bind supplier list to the DataGrid
    }

    // Add Supplier
    private void OnAddSupplierClicked(object sender, EventArgs e)
    {
        string newSupplier = SupplierEntry.Text;
        if (!string.IsNullOrEmpty(newSupplier))
        {
            _databaseService.AddSupplier(newSupplier);
            SupplierList.Add(newSupplier);
            SupplierEntry.Text = string.Empty;  // Clear the entry field
        }
    }

    // Update Supplier
    private void OnUpdateSupplierClicked(object sender, EventArgs e)
    {
        string selectedSupplier = SupplierDataGrid.SelectedItem as string;
        string newSupplierName = SupplierEntry.Text;

        if (!string.IsNullOrEmpty(selectedSupplier) && !string.IsNullOrEmpty(newSupplierName))
        {
            _databaseService.UpdateSupplier(selectedSupplier, newSupplierName);
            int index = SupplierList.IndexOf(selectedSupplier);
            SupplierList[index] = newSupplierName;  // Update the ObservableCollection
            SupplierEntry.Text = string.Empty;  // Clear the entry field
        }
    }

    // Delete Supplier
    private void OnDeleteSupplierClicked(object sender, EventArgs e)
    {
        string selectedSupplier = SupplierDataGrid.SelectedItem as string;

        if (!string.IsNullOrEmpty(selectedSupplier))
        {
            _databaseService.DeleteSupplier(selectedSupplier);
            SupplierList.Remove(selectedSupplier);  // Remove from ObservableCollection
            SupplierEntry.Text = string.Empty;  // Clear the entry field
        }
    }
}
