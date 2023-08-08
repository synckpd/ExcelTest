using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ExcelTest
{
    public sealed class BusyProgressDialog : ContentDialog
    {
        public BusyProgressDialog()
        {
            DefaultStyleKey = typeof(BusyProgressDialog);
        }


        public static readonly DependencyProperty DiameterProperty =
            DependencyProperty.Register(nameof(Diameter), typeof(double), typeof(BusyProgressDialog), new PropertyMetadata(0d));


        public double Diameter
        {
            get { return (double)GetValue(DiameterProperty); }
            set { SetValue(DiameterProperty, value); }
        }
    }
}
