using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DemoApp.Helpers
{
    public class Observable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        protected void SetProperty<T>(ref T storage, T value, [CallerMemberName]string propertyName = null) => Set(ref storage, value, propertyName);

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
