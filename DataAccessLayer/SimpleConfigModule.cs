using Ninject.Modules;
using DataAccessLayer;
using DataAccessLayer.EntityFramework;
using BusinessLogicLayer.Services;
using ЛАБА1;

namespace BusinessLogicLayer
{
    public class SimpleConfigModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRepository<Hero>>().To<EntityRepository<Hero>>().InSingletonScope();
            Bind<IHeroService>().To<HeroService>().InSingletonScope();
            Bind<Logic>().ToSelf();
        }
    }
}