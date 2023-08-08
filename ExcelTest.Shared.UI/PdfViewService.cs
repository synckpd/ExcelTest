using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;

namespace ExcelTest
{
    public class PdfViewService : DependencyObject, IPdfViewService
    {
        public void Display(string pdfFilePath)
        {
            DispatcherQueue.TryEnqueue(() =>
            {
                var window = new PdfViewWindow(pdfFilePath);
                window.Activate();
            });
        }
    }
}
