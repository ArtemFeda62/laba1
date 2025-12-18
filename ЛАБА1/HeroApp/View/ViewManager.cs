using System;
using System.Collections.Generic;
using HeroApp.WPF.ViewModel;

namespace HeroApp.WPF.View
{
    public static class ViewManager
    {
        private static readonly Dictionary<Type, Type> _viewModelToView = new();
        private static readonly Dictionary<Type, object> _viewModelInstances = new();

        public static void Register<TViewModel, TView>()
            where TViewModel : ViewModelBase
            where TView : ViewBase
        {
            _viewModelToView[typeof(TViewModel)] = typeof(TView);
        }

        public static ViewBase ResolveView<TViewModel>(TViewModel viewModel = null)
            where TViewModel : ViewModelBase
        {
            var viewModelType = typeof(TViewModel);

            if (_viewModelToView.TryGetValue(viewModelType, out var viewType))
            {
                var view = (ViewBase)Activator.CreateInstance(viewType);

                if (viewModel != null)
                {
                    view.DataContext = viewModel;
                }
                else if (_viewModelInstances.TryGetValue(viewModelType, out var existingViewModel))
                {
                    view.DataContext = existingViewModel;
                }

                return view;
            }

            throw new InvalidOperationException($"No view registered for {viewModelType.Name}");
        }

        public static ViewModelBase ResolveViewModel(Type viewType)
        {
            foreach (var kvp in _viewModelToView)
            {
                if (kvp.Value == viewType)
                {
                    if (!_viewModelInstances.TryGetValue(kvp.Key, out var viewModel))
                    {
                        viewModel = (ViewModelBase)Activator.CreateInstance(kvp.Key);
                        _viewModelInstances[kvp.Key] = viewModel;
                    }
                    return (ViewModelBase)viewModel;
                }
            }

            throw new InvalidOperationException($"No view model registered for {viewType.Name}");
        }

        public static void Show<TViewModel>(TViewModel viewModel = null)
            where TViewModel : ViewModelBase
        {
            var view = ResolveView(viewModel);
            view.Show();
        }

        public static bool? ShowDialog<TViewModel>(TViewModel viewModel = null)
            where TViewModel : ViewModelBase
        {
            var view = ResolveView(viewModel);
            return view.ShowDialog();
        }
    }
}