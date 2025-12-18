using System;
using System.Collections.Generic;
using System.Windows;
using Presenter.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace WpfView
{
    public class ViewManager : IViewManager
    {
        private readonly Dictionary<Type, Type> _map = new Dictionary<Type, Type>();

        public void Register<TViewModel, TView>() where TView : Window
        {
            _map[typeof(TViewModel)] = typeof(TView);
        }

        public void Show(object viewModel)
        {
            CreateWindow(viewModel).Show();
        }

        public void ShowDialog(object viewModel)
        {
            CreateWindow(viewModel).ShowDialog();
        }

        public void Close(object viewModel)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == viewModel)
                {
                    window.Close();
                    break;
                }
            }
        }

        private Window CreateWindow(object viewModel)
        {
            var viewType = _map[viewModel.GetType()];
            var window = (Window)Activator.CreateInstance(viewType);
            window.DataContext = viewModel;
            return window;
        }
    }
}