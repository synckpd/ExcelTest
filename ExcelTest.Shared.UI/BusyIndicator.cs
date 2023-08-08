using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace ExcelTest
{
    public class BusyIndicator : Control
    {
        public BusyIndicator()
        {
            DefaultStyleKey = typeof(BusyIndicator);
            Visibility = Visibility.Collapsed;
        }


        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(BusyIndicator), new PropertyMetadata(false, OnIsActivePropertyChanged));


        public static readonly DependencyProperty DiameterProperty =
            DependencyProperty.Register(nameof(Diameter), typeof(double), typeof(BusyIndicator), new PropertyMetadata(60d, OnDiameterPropertyChanged));


        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(nameof(Description), typeof(object), typeof(BusyIndicator), new PropertyMetadata(null));


        public static readonly DependencyProperty DescriptionTemplateProperty =
            DependencyProperty.Register(nameof(DescriptionTemplate), typeof(DataTemplate), typeof(BusyIndicator), new PropertyMetadata(null));


        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public double Diameter
        {
            get { return (double)GetValue(DiameterProperty); }
            set { SetValue(DiameterProperty, value); }
        }

        public object Description
        {
            get { return (object)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public DataTemplate DescriptionTemplate
        {
            get { return (DataTemplate)GetValue(DescriptionTemplateProperty); }
            set { SetValue(DescriptionTemplateProperty, value); }
        }

        private static void OnIsActivePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BusyIndicator)d).OnIsActivePropertyChanged((bool)e.NewValue);
        }


        private static void OnDiameterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var busyIndicator = (BusyIndicator)d;
            var diameter = (double)e.NewValue;

            if(diameter > MaxDiameter)
            {
                busyIndicator.Diameter = MaxDiameter;
            }
            else if(diameter < MinDiameter)
            {
                busyIndicator.Diameter = MinDiameter;
            }
        }

        private async void OnIsActivePropertyChanged(bool isActive)
        {
            if(isActive)
            {
                if(_dialog != null)
                {
                    _dialog.Hide();
                    _dialog = null;
                }

                _dialog = CreateDialog();
                await _dialog.ShowAsync();
            }
            else
            {
                _dialog.Hide();
                _dialog = null;
            }
        }

        private BusyProgressDialog CreateDialog()
        {
            var dialog = new BusyProgressDialog();
            dialog.XamlRoot = XamlRoot;
            dialog.SetBinding(BusyProgressDialog.DiameterProperty, new Binding { Source = this, Path = new PropertyPath(nameof(Diameter)) });
            dialog.SetBinding(ContentDialog.ContentProperty, new Binding { Source = this, Path = new PropertyPath(nameof(Description)) });
            dialog.SetBinding(ContentDialog.ContentTemplateProperty, new Binding { Source = this, Path = new PropertyPath(nameof(DescriptionTemplate)) });
            
            return dialog;
        }

        private const double MaxDiameter = 200;
        private const double MinDiameter = 10;

        private BusyProgressDialog _dialog;
    }
}
