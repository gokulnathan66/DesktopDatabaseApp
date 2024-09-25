using FinalProject.Data;
using Syncfusion.Maui.Core.Carousel;
using System.Collections.ObjectModel;

namespace FinalProject.Pages;

public partial class GenericEntryPage : ContentPage
{
    private GenericReportData? entrydata;
    public MainViewModel _viewModel { get; set; }
    public ObservableCollection<string> supplier_list { get; set; }



    public GenericEntryPage()
	{
		InitializeComponent();
        BindingContext = new MainViewModel();
        //LoadSupplierData();
        _viewModel = new MainViewModel();

        supplier_list = _viewModel.databaseService.GetSupplier_list();
        SupplierofReport.ItemsSource = supplier_list;
    }

    private async void Submiting_date(object sender, EventArgs e)

    {
		string? Year = YearPicker.SelectedItem.ToString();
		string? Suppler = SupplierofReport.SelectedItem.ToString();
        entrydata = new GenericReportData(Year, Suppler);
        //GenericReportDataPage.Data = enertydata;
        var Service = new MainViewModel();
        await Navigation.PushAsync(new GenericReportDataPage(entrydata));
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Refresh the supplier list every time the page appears
        RefreshSupplierList();
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
    private void Button_Clicked(object sender, EventArgs e)
    {
        YearPicker.SelectedIndex = -1;
        SupplierofReport.SelectedIndex = -1;
    }

    /* private void LoadSupplierData()
     {
         // Initialize the DatabaseService
         var Service = new MainViewModel();
         var supplierList = Service.databaseService.GetSupplier_list();

         // Bind the supplier list to the Picker's ItemsSource
         SupplierofReport.ItemsSource = supplierList;
     }*/
    /* private void PopulateYearPicker()
     {
         int currentYear = DateTime.Now.Year;
         List<string> years = new List<string>();

         // Generate years from (current year - 4) to (current year + 4)
         for (int i = currentYear - 4; i <= currentYear + 4; i++)
         {
             years.Add(i.ToString());
         }

         // Set the years as the ItemSource of the YearPicker
         YearPicker.ItemsSource = years;
     }*/
}