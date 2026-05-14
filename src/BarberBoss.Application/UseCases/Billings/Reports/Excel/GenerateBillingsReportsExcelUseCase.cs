using ClosedXML.Excel;

namespace BarberBoss.Application.UseCases.Billings.Reports.Excel;

public class GenerateBillingsReportsExcelUseCase : IGenerateBillingsReportsExcelUseCase {
    public async Task<byte[]> Execute(DateOnly month) {
        var WorkBook = new XLWorkbook();
        ConfigureWorkBook(WorkBook);
        var worksheet = WorkBook.Worksheets.Add(month.ToString("Y"));
        InsertHeader(worksheet);

    }

    private void ConfigureWorkBook(XLWorkbook WorkBook) {
        WorkBook.Author = "BarberBoss";
        WorkBook.Style.Font.FontSize = 12;
        WorkBook.Style.Font.FontName = "Arial";  
    }

    private void InsertHeader(IXLWorksheet worksheet) {
        worksheet.Cell("A1").Value = "ServiceName";
        worksheet.Cell("B1").Value = "BarberName";
        worksheet.Cell("C1").Value = "Date";
        worksheet.Cell("D1").Value = "PaymentType";
        worksheet.Cell("E1").Value = "Amount";

        worksheet.Cells("A1:E1").Style.Font.Bold = true; //Deixa o cabeçalho em negrito
        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#F5C2B6"); //Define a cor de fundo do cabeçalho

        //Centraliza o texto do cabeçalho
        worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right); 

    }
}