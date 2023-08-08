using System.Threading.Tasks;

namespace ExcelTest
{
    public interface IPdfExportService
    {
        Task<bool> ExportAsync(string excelFilePath, string pdfFilePath = null, bool openAfterPublish = false);
    }
}
