using System.Windows;
using BusinessLogicLayer;
using Ninject;
using Presenter.Interfaces;
using Presenter.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace WpfView
{
    public partial class App : Application
    {
        private IKernel _kernel;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _kernel = new StandardKernel(new SimpleConfigModule());

            // Создаем и настраиваем ViewManager
            var viewManager = new ViewManager();
            viewManager.Register<MainViewModel, MainWindow>();

            // Регистрируем менеджер в контейнере
            _kernel.Bind<IViewManager>().ToConstant(viewManager);

            // Получаем ViewModel (ViewModelFirst)
            var mainViewModel = _kernel.Get<MainViewModel>();

            // Запускаем отображение через менеджер
            viewManager.Show(mainViewModel);
        }
    }
}