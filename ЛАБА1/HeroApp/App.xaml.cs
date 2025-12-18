using System.Windows;
using HeroApp.WPF.View;
using HeroApp.WPF.ViewModel;

namespace HeroApp.WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Регистрируем связи между ViewModel и View
            ViewManager.Register<MainViewModel, MainView>();
            ViewManager.Register<AddHeroViewModel, AddHeroView>();
            ViewManager.Register<EditHeroViewModel, EditHeroView>();
            ViewManager.Register<HitHeroViewModel, HitHeroView>();
            ViewManager.Register<StatisticsViewModel, StatisticsView>();

            // Инициализируем базу данных
            //DatabaseCreator.InitializeDatabase();

            // Показываем главное окно
            var mainView = ViewManager.ResolveView<MainViewModel>();
            mainView.Show();
        }
    }
}