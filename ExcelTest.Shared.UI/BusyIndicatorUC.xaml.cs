using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;

namespace ExcelTest
{
    public sealed partial class BusyIndicatorUC : UserControl
    {
        private const double MaxIndicatorSize = 200d;
        private const double MinIndicatorSize = 30d;

        private static readonly Dictionary<double, double> _sizeDictionary;

        static BusyIndicatorUC()
        {
            _sizeDictionary = new Dictionary<double, double>
            {
                { 30, 18 },
                { 60, 20 },
                { 90, 22 },
                { 120, 26 },
                { 150, 30 },
                { 180, 34 }
            };
        }

        public BusyIndicatorUC()
        {
            InitializeComponent();
            IsHitTestVisible = false;
            Visibility = Visibility.Collapsed;
        }


        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(BusyIndicatorUC), new PropertyMetadata(false, OnIsActivePropertyChanged));


        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(nameof(Description), typeof(string), typeof(BusyIndicatorUC), new PropertyMetadata(null, OnDescriptionPropertyChanged));


        public static readonly DependencyProperty IndicatorSizeProperty =
            DependencyProperty.Register(nameof(IndicatorSize), typeof(double), typeof(BusyIndicatorUC), new PropertyMetadata(90d, OnIndicatorSizePropertyChanged));

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public double IndicatorSize
        {
            get { return (double)GetValue(IndicatorSizeProperty); }
            set { SetValue(IndicatorSizeProperty, value); }
        }

        private static void OnIsActivePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var busyIndicator = (BusyIndicatorUC)d;
            var isBusy = (bool)e.NewValue;

            busyIndicator.ProgressRing.IsActive = isBusy;
            busyIndicator.IsHitTestVisible = isBusy;
            busyIndicator.Visibility = isBusy ? Visibility.Visible : Visibility.Collapsed;
        }

        private static void OnDescriptionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var busyIndicator = (BusyIndicatorUC)d;
            var description = e.NewValue as string;
            var descriptionBlock = busyIndicator.DescriptionBlock;

            descriptionBlock.Text = description;
            descriptionBlock.Visibility = string.IsNullOrEmpty(description) ? Visibility.Collapsed : Visibility.Visible;
        }

        private static void OnIndicatorSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var busyIndicator = (BusyIndicatorUC)d;
            var size = (double)e.NewValue;

            if(size > MaxIndicatorSize)
            {
                busyIndicator.IndicatorSize = MaxIndicatorSize;
            }
            else if(size < MinIndicatorSize)
            {
                busyIndicator.IndicatorSize = MinIndicatorSize;
            }
            else
            {
                var coreRing = busyIndicator.ProgressRing;
                var descriptionBlock = busyIndicator.DescriptionBlock;

                coreRing.Height = size;
                coreRing.Width = size;

                descriptionBlock.FontSize = GetFontSizeFromIndicatorSize(size);
            }
        }

        private static double GetFontSizeFromIndicatorSize(double indicatorSize)
        {
            switch(indicatorSize)
            {
                case double num when num >= 180: return _sizeDictionary[180];
                case double num when num >= 150: return _sizeDictionary[150];
                case double num when num >= 120: return _sizeDictionary[120];
                case double num when num >= 90: return _sizeDictionary[90];
                case double num when num >= 60: return _sizeDictionary[60];
                case double num when num >= 30: return _sizeDictionary[30];
                default: return _sizeDictionary[30];
            }
        }
    }
}
