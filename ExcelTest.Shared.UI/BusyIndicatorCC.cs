using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Permissions;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

namespace ExcelTest
{
    public sealed class BusyIndicatorCC : Control
    {
        public BusyIndicatorCC()
        {
            DefaultStyleKey = typeof(BusyIndicatorCC);
        }

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(BusyIndicatorCC), new PropertyMetadata(false, OnIsActivePropertyChanged));


        public static readonly DependencyProperty DiameterProperty =
            DependencyProperty.Register(nameof(Diameter), typeof(double), typeof(BusyIndicatorCC), new PropertyMetadata(90d));


        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(nameof(Description), typeof(object), typeof(BusyIndicatorCC), new PropertyMetadata(null));


        public static readonly DependencyProperty DescriptionTemplateProperty =
            DependencyProperty.Register(nameof(DescriptionTemplate), typeof(DataTemplate), typeof(BusyIndicatorCC), new PropertyMetadata(null));


        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }

        public double Diameter
        {
            get => (double)GetValue(DiameterProperty);
            set => SetValue(DiameterProperty, value);
        }

        public object Description
        {
            get => (object)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public DataTemplate DescriptionTemplate
        {
            get => (DataTemplate)GetValue(DescriptionTemplateProperty);
            set => SetValue(DescriptionTemplateProperty, value);
        }

        private static void OnIsActivePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BusyIndicatorCC)d).OnIsActivePropertyChanged((bool)e.NewValue);
        }

        private void OnIsActivePropertyChanged(bool isActive)
        {
            IsHitTestVisible = isActive;
            Visibility = isActive ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
