using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BusinessLogicLayer;
using Ninject;

namespace HeroApp.WPF.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private bool _isBusy;
        private string _statusMessage;

        protected readonly IKernel Kernel;
        protected readonly Logic Logic;

        protected ViewModelBase()
        {
            Kernel = new StandardKernel(new SimpleConfigModule());
            Logic = Kernel.Get<Logic>();
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetField(ref _isBusy, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetField(ref _statusMessage, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected async Task ExecuteAsync(Func<Task> operation, string busyMessage = null)
        {
            try
            {
                IsBusy = true;
                StatusMessage = busyMessage ?? "Выполняется операция...";

                await operation();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка: {ex.Message}";
                // Здесь можно добавить логирование
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected async Task<T> ExecuteAsync<T>(Func<Task<T>> operation, string busyMessage = null)
        {
            try
            {
                IsBusy = true;
                StatusMessage = busyMessage ?? "Выполняется операция...";

                return await operation();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка: {ex.Message}";
                return default;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}