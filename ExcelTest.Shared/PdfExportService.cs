using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelTest
{
    public class PdfExportService : IPdfExportService
    {
        private static readonly string _pdfExtension = ".pdf";
        private static readonly string[] _extensions = new string[] { ".xls", ".xlsx", ".xlsm" };

        public async Task<bool> ExportAsync(string excelFilePath, string pdfFilePath = null, bool openAfterPublish = false)
        {
            if(string.IsNullOrEmpty(excelFilePath))
            {
                throw new ArgumentException($"'The file path cannot be null or empty.", nameof(excelFilePath));
            }

            var extension = Path.GetExtension(excelFilePath);
            if(string.IsNullOrEmpty(excelFilePath) || !_extensions.Contains(extension))
            {
                throw new ArgumentException("The file path must end with an 'xls' or 'xlsx' extension.", nameof(excelFilePath));
            }

            if(!File.Exists(excelFilePath))
            {
                throw new FileNotFoundException("The file not found.", nameof(excelFilePath));
            }

            return await Task.Run(() =>
            {
                bool result = false;
                Excel.Application excel = null;
                Excel.Workbooks books = null;
                Excel.Workbook book = null;

                try
                {
                    var exportFilePath = string.IsNullOrEmpty(pdfFilePath) ? excelFilePath : pdfFilePath;
                    exportFilePath = ConvertToPdfFilePath(exportFilePath);

                    excel = new Excel.Application();
                    books = excel.Workbooks;
                    book = books.Open(excelFilePath);

                    book.ExportAsFixedFormat2(
                        Excel.XlFixedFormatType.xlTypePDF,
                        Filename: exportFilePath,
                        Quality: Excel.XlFixedFormatQuality.xlQualityStandard,
                        IncludeDocProperties: false,
                        IgnorePrintAreas: true,
                        OpenAfterPublish: openAfterPublish);

                    book.Close();
                    excel.Quit();
                    result = true;
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    Debug.WriteLine(ex.Message);

                    if(book != null)
                    {
                        book.Close();
                    }

                    if(excel != null)
                    {
                        excel.Quit();
                    }
                }
                finally
                {
                    FinalReleaseComObject(book);
                    FinalReleaseComObject(books);
                    FinalReleaseComObject(excel);
                    book = null;
                    books = null;
                    excel = null;
                }

                return result;
            });
        }

        private string ConvertToPdfFilePath(string source)
        {
            var extension = Path.GetExtension(source);
            if(string.IsNullOrEmpty(extension))
            {
                return $"{source}{_pdfExtension}";
            }
            else if(string.Compare(extension, _pdfExtension, StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                return source.Replace(extension, _pdfExtension, StringComparison.InvariantCultureIgnoreCase);
            }

            return source;
        }

        private void FinalReleaseComObject(object obj)
        {
            if(obj != null)
            {
                Marshal.FinalReleaseComObject(obj);
            }
        }
    }
}
