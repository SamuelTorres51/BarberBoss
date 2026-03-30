using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;

namespace BarberBoss.Application.UseCases.Billings.Reports.Excel;

public class GenerateBillingsReportsExcelUseCase : IGenerateBillingsReportsExcelUseCase {
    public async Task<byte[]> Execute(DateOnly month) {
        var WorkBook = new XLWorkbook();
        ConfigureWorkSheet(month, WorkBook);
        var workSheet = WorkBook.Worksheets.Add(month.ToString("Y"));

    }

    private void ConfigureWorkSheet(DateOnly month, XLWorkbook WorkBook) {
        WorkBook.Author = "BarberBoss";
        WorkBook.Style.Font.FontSize = 12;
        WorkBook.Style.Font.FontName = "Arial";  
    }
}