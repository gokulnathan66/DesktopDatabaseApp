using System.Collections.ObjectModel;

namespace FinalProject;
public class MainViewModel

{
    public List<string> Years { get; set; }
    public List<Person> People { get; set; }
    public ObservableCollection<string> supplier_list { get; set; }
    public DatabaseService databaseService { get; set; }

    public MainViewModel()
    {
        // Use the local app data folder for storing the database
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "database2.sqlite3");

        // Initialize the database service with the dynamic path
        databaseService = new DatabaseService(dbPath);        //People = databaseService.GetPeople();
        supplier_list = databaseService.GetSupplier_list();
        GenerateYears();
    }
    private void RefreshSupplierList()
    {
        var updatedSuppliers = databaseService.GetSupplier_list();
        supplier_list.Clear();
        foreach (var supplier in updatedSuppliers)
        {
            supplier_list.Add(supplier);
        }
    }
    private void GenerateYears()
    {
        Years = new List<string>();
        int currentYear = DateTime.Now.Year;
        for (int i = currentYear - 4; i <= currentYear + 4; i++)
        {
            Years.Add(i.ToString());
        }
    }
}
