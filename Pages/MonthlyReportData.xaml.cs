using FinalProject.Data;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
namespace FinalProject.Pages;

public partial class MonthlyReportData : ContentPage
{
    private string Year { get; set; }
    private string Month { get; set; }

    public MonthlyReportData(string year, string month)
    {
        InitializeComponent();
        /*
        _ = DisplayAlert("Welcome", $"The year is {Data.Year} and the supplier name is {Data.Month}", "OK");
        financial_year.Text = Data.Year;
        Supplier_name.Text = Data.Month;*/
        Year = year; // Store the year
        Month = month; // Store the month
        financial_year.Text=year;
        Month_name.Text=month;
        var Service = new MainViewModel();
        BindingContext = this;

         var  monthlydata= Service.databaseService.GetMontylyReport(year,month);
        monthlygrid.ItemsSource = monthlydata;
    }
    private async void HomeButton_Clicked(object sender, EventArgs e)
    {
        // Navigate back to the previous page or to the home page
        await Navigation.PopAsync(); // Use this if you want to go back to the previous page

        // If you want to navigate to a specific home page, use:
        // await Navigation.PushAsync(new HomePage()); // Replace 'HomePage' with your actual home page class
    }
    private void PrintButton_Clicked(object sender, EventArgs e)
    {
        GeneratePDFAsync(Year, Month);
    }
    private async Task GeneratePDFAsync(string year, string month)
    {
        // Create a new PDF document
        PdfDocument document = new PdfDocument();
        document.Info.Title = "Monthly Report";

        // Create an empty page
        PdfPage page = document.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);

        // Set fonts
        XFont titleFont = new XFont("Verdana", 20, XFontStyle.Bold);
        XFont headerFont = new XFont("Verdana", 12, XFontStyle.Bold);
        XFont regularFont = new XFont("Verdana", 10, XFontStyle.Regular);

        // Define colors
        XBrush headerBackground = new XSolidBrush(XColor.FromArgb(140, 198, 63)); // Green for header background
        XBrush borderBrush = XBrushes.Black;
        XPen borderPen = new XPen(borderBrush, 2); // Border pen for table and page

        // Calculate the page width for centering
        double pageWidth = page.Width;
        double tableWidth = 500; // Set table width
        double tableStartX = (pageWidth - tableWidth) / 2; // Center table horizontally

        // Draw a page border
        gfx.DrawRectangle(borderPen, 20, 20, page.Width - 40, page.Height - 40);

        // Draw title
        gfx.DrawString("Generic  Report", titleFont, XBrushes.Black, new XRect(40, 60, page.Width - 80, 40), XStringFormats.Center);

        // Draw Financial Year
        gfx.DrawString($"Financial Year: {Year}", regularFont, XBrushes.Black, new XRect(40, 100, page.Width, page.Height), XStringFormats.TopLeft);

        // Draw Supplier Name
        gfx.DrawString($"Month: {Month}", regularFont, XBrushes.Black, new XRect(40, 120, page.Width, page.Height), XStringFormats.TopLeft);

        // Table Headers
        gfx.DrawRectangle(headerBackground, tableStartX, 160, tableWidth, 20); // Header background
        gfx.DrawString("Name", headerFont, XBrushes.White, new XRect(tableStartX, 160, 100, 20), XStringFormats.CenterLeft);
        gfx.DrawString("Particular", headerFont, XBrushes.White, new XRect(tableStartX + 100, 160, 100, 20), XStringFormats.CenterLeft);
        gfx.DrawString("Quantity", headerFont, XBrushes.White, new XRect(tableStartX + 200, 160, 100, 20), XStringFormats.CenterLeft);
        gfx.DrawString("Basic Rate", headerFont, XBrushes.White, new XRect(tableStartX + 300, 160, 100, 20), XStringFormats.CenterLeft);
        gfx.DrawString("Value", headerFont, XBrushes.White, new XRect(tableStartX + 400, 160, 50, 20), XStringFormats.CenterLeft);
        gfx.DrawString("Advance Paid", headerFont, XBrushes.White, new XRect(tableStartX + 450, 160, 50, 20), XStringFormats.CenterLeft);

        // Get DataGrid contents
        var data = monthlygrid.ItemsSource as List<Person>;

        // Draw table rows with alternating background colors and borders
        int yPos = 180; // Start position for the table rows
        bool alternateColor = false; // To alternate row color

        foreach (var item in data)
        {
            // Alternating row background
            if (alternateColor)
            {
                gfx.DrawRectangle(XBrushes.LightGray, tableStartX, yPos, tableWidth, 20);
            }

            // Draw each row's content
            gfx.DrawString(item.Name, regularFont, XBrushes.Black, new XRect(tableStartX, yPos, 100, 20), XStringFormats.CenterLeft);
            gfx.DrawString(item.Particular, regularFont, XBrushes.Black, new XRect(tableStartX + 100, yPos, 100, 20), XStringFormats.CenterLeft);
            gfx.DrawString(item.Quantity.ToString(), regularFont, XBrushes.Black, new XRect(tableStartX + 200, yPos, 100, 20), XStringFormats.CenterLeft);
            gfx.DrawString(item.BasicRate.ToString(), regularFont, XBrushes.Black, new XRect(tableStartX + 300, yPos, 100, 20), XStringFormats.CenterLeft);
            gfx.DrawString(item.Value.ToString(), regularFont, XBrushes.Black, new XRect(tableStartX + 400, yPos, 50, 20), XStringFormats.CenterLeft);
            gfx.DrawString(item.AdvancePaid.ToString(), regularFont, XBrushes.Black, new XRect(tableStartX + 450, yPos, 50, 20), XStringFormats.CenterLeft);

            // Draw borders for each row
            gfx.DrawRectangle(borderPen, tableStartX, yPos, tableWidth, 20);

            yPos += 20; // Move to next row
            alternateColor = !alternateColor; // Alternate the row color
        }

        // Save the document
        string filename = await DisplayPromptAsync("Save PDF", "Enter the file name:", "OK", "Cancel", "MonthlyReport");
        if (!string.IsNullOrWhiteSpace(filename))
        {
            // Create a path to save the PDF in the Documents folder
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"{filename}.pdf");

            // Save the document
            document.Save(path);
            document.Close();

            // Notify user
            await DisplayAlert("Success", $"PDF saved to {path}", "OK");
        }
        else
        {
            // Handle the case when the user cancels or enters an empty name
            await DisplayAlert("Cancelled", "PDF save operation was cancelled.", "OK");
        }
    }
}
