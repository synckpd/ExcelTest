using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT.Interop;

namespace ExcelTest
{
    public sealed partial class PdfViewWindow : Window
    {
        public PdfViewWindow(string pdfFilePath)
        {
            InitializeComponent();

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(TitleBarRect);

            AppWindow.GetFromWindowId(
                Win32Interop.GetWindowIdFromWindow(
                    WindowNative.GetWindowHandle(this)))
                .Resize(new Windows.Graphics.SizeInt32(1280, 720));

            Closed += OnClosed;
            InitializeAsync(pdfFilePath);
        }

        private void OnClosed(object sender, WindowEventArgs args)
        {
            Closed -= OnClosed;

            webView2.Close();
        }

        private async void InitializeAsync(string pdfFilePath)
        {
            if(string.IsNullOrEmpty(pdfFilePath))
            {
                throw new ArgumentNullException(nameof(pdfFilePath));
            }

            if(!File.Exists(pdfFilePath))
            {
                throw new FileNotFoundException(nameof(pdfFilePath));
            }

            await webView2.EnsureCoreWebView2Async();
            webView2.CoreWebView2.Navigate(pdfFilePath);
        }
    }
}
