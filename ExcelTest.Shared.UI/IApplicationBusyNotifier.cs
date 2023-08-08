using System;
using System.ComponentModel;

namespace ExcelTest
{
    public interface IApplicationBusyNotifier : INotifyPropertyChanged, IObservable<bool>
    {
        bool IsBusy { get; }

        IDisposable Start();
    }
}
