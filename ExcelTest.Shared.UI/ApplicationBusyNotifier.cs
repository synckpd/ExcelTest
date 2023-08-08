using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace ExcelTest
{
    public class ApplicationBusyNotifier : IApplicationBusyNotifier
    {
        private static readonly PropertyChangedEventArgs IsBusyPropertyChangedEventArgs = new(nameof(IsBusy));

        /// <summary>
        /// property changed event handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private int ProcessCounter { get; set; }

        private Subject<bool> IsBusySubject { get; } = new Subject<bool>();

        private object LockObject { get; } = new object();

        private bool isBusy;

        /// <summary>
        /// Is process running.
        /// </summary>
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if(isBusy == value) { return; }
                isBusy = value;
                PropertyChanged?.Invoke(this, IsBusyPropertyChangedEventArgs);
                IsBusySubject.OnNext(isBusy);
            }
        }

        /// <summary>
        /// Process Start.
        /// </summary>
        /// <returns>Call dispose method when process end.</returns>
        public IDisposable Start()
        {
            lock(LockObject)
            {
                ProcessCounter++;
                IsBusy = ProcessCounter != 0;
                return Disposable.Create(() =>
                {
                    lock(LockObject)
                    {
                        ProcessCounter--;
                        IsBusy = ProcessCounter != 0;
                    }
                });
            }
        }

        /// <summary>
        /// Subscribe busy.
        /// </summary>
        /// <param name="observer">observer</param>
        /// <returns>disposable</returns>
        public IDisposable Subscribe(IObserver<bool> observer)
        {
            observer.OnNext(IsBusy);
            return IsBusySubject.Subscribe(observer);
        }
    }
}
