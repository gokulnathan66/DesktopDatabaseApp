using FinalProject.Data;

namespace FinalProject.Pages;

public partial class MonthlyReport : ContentPage

{

    public Dictionary<string, string> monthMap = new Dictionary<string, string>()
    {
        {"JAN", "01"},
        {"FEB", "02"},
        {"MAR", "03"},
        {"APR", "04"},
        {"MAY", "05"},
        {"JUN", "06"},
        {"JUL", "07"},
        {"AUG", "08"},
        {"SEP", "09"},
        {"OCT", "10"},
        {"NOV", "11"},
        {"DEC", "12"}
    };


    public MonthlyReport()
	{
		InitializeComponent();
        BindingContext = new MainViewModel();
    }

    private async void Submiting_date(object sender, EventArgs e)

    {
        string? Year = year.SelectedItem.ToString();
        string? Month = month.SelectedItem.ToString();
        /*enertydata = new MonthlyData(Year, Month);
        MonthlyReportData.Data = enertydata;*/
        string monthNumber = monthMap[Month];



        await Navigation.PushAsync(new MonthlyReportData(Year,
            monthNumber));
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        year.SelectedIndex = -1;
        month.SelectedIndex = -1;
    }
}