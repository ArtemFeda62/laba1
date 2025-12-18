using System.Windows;
using HeroApp.WPF.ViewModel;

namespace HeroApp.WPF.View
{
    public abstract class ViewBase : Window
    {
        protected ViewBase()
        {
            Loaded += (s, e) =>
            {
                if (DataContext == null)
                {
                    var viewModel = ViewManager.ResolveViewModel(this.GetType());
                    if (viewModel != null)
                    {
                        DataContext = viewModel;
                    }
                }
            };
        }
    }
}