
using System.Collections.ObjectModel;

namespace FinalProject.Pages;

public partial class DataEntryPage : ContentPage


{
    public MainViewModel _viewModel { get; set; }
    private ObservableCollection<string> LeafList;
    public ObservableCollection<string> supplier_list { get; set; }



    public DataEntryPage()
    {
        InitializeComponent();
        
        

        // Cast the BindingContext to MainViewModel to access the database service
        _viewModel =   new MainViewModel();
        BindingContext = this;

        LoadLeafTypes();
        supplier_list = _viewModel.databaseService.GetSupplier_list();
        SupplierPicker.ItemsSource = supplier_list;
        // _viewModel = BindingContext as MainViewModel;

        //   if (_viewModel == null)
        //  {
        //     throw new InvalidOperationException("BindingContext is not set to MainViewModel."); }

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Refresh the supplier list every time the page appears
        RefreshSupplierList();
        LoadLeafTypes();
    }
    private void LoadLeafTypes()
    {
        LeafList = new ObservableCollection<string>(_viewModel.databaseService.GetLeafList());
        LeafPicker.ItemsSource = LeafList;  // Bind leaf list to the Picker
    }
    public void RefreshSupplierList()
    {
        var updatedSuppliers = _viewModel.databaseService.GetSupplier_list();
        supplier_list.Clear();
        foreach (var supplier in updatedSuppliers)
        {
            supplier_list.Add(supplier);
        }
    }


    private async void Submit_clicked(object sender, EventArgs e)

    {
        // Ensure the service is properly initialized
        var databaseService = _viewModel.databaseService;
        if (databaseService == null)
        {
            await DisplayAlert("Error", "Database service not initialized", "OK");
            return;
        }
        // Retrieve the values from the input fields
        var selectedDate = DateEntry.Date;
        var supplier = SupplierPicker.SelectedItem?.ToString();
        var leaf = LeafPicker.SelectedItem?.ToString();
        int quantity;
        int advance;

        // Validate and parse numeric values (Quantity and Advance)
        if (!int.TryParse(QuantityEntry.Text, out quantity) || !int.TryParse(AdvanceEntry.Text, out advance))
        {
            await DisplayAlert("Error", "Please enter valid numeric values for Quantity and Advance.", "OK");
            return;
        }

        // Ensure that all fields have been filled in
        if (string.IsNullOrWhiteSpace(supplier) || string.IsNullOrWhiteSpace(leaf))
        {
            await DisplayAlert("Error", "Please select Supplier and Leaf.", "OK");
            return;
        }
        var (baseRate, leafValue) = databaseService.GetLeafDetails(leaf);

        // Create a new Person object to represent the data
        var person = new Person
        {
            TransactionDate=selectedDate,
            Name = supplier,
            Particular = leaf,
            Quantity = quantity,
            BasicRate = baseRate,  // Set the retrieved base rate
            Value = leafValue,
            AdvancePaid = advance
        };

        // Insert the data into the database
        databaseService.AddPerson(person);

        // Show success message
        await DisplayAlert("Success", "Data entry successful", "OK");

        // Optionally clear the input fields after submission
        QuantityEntry.Text = string.Empty;
        AdvanceEntry.Text = string.Empty;
        SupplierPicker.SelectedIndex = -1;
        LeafPicker.SelectedIndex = -1;
    }

}