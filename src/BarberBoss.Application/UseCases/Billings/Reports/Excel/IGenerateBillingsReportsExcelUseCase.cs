namespace BarberBoss.Application.UseCases.Billings.Reports.Excel;

public interface IGenerateBillingsReportsExcelUseCase {
    Task<byte[]> Execute(DateOnly month);
}
