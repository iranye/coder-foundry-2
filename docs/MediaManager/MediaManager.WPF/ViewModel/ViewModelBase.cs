namespace MediaManager.WPF.ViewModel
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewModelBase.cs" company="IRANYE">
//   Copyright (c) IRANYE. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual Task LoadAsync() => Task.CompletedTask;
    }
}
