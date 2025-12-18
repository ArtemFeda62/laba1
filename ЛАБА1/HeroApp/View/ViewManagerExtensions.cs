using HeroApp.WPF.ViewModel;
using System.Windows;

namespace HeroApp.WPF.View
{
    public static class ViewManagerExtensions
    {
        public static void CloseDialog(ViewModelBase viewModel, bool? dialogResult = null)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == viewModel)
                {
                    if (dialogResult.HasValue)
                    {
                        window.DialogResult = dialogResult;
                    }
                    window.Close();
                    return;
                }
            }
        }
    }
}