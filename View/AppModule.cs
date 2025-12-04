using BusinessLogic.Services;
using ConsoleInterface;
using DataAccess.EntityFramework;
using Domain.Repositories;
using Ninject.Modules;
using Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View
{
    public class AppModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IHeroRepository>().To<EntityHeroRepository>().InSingletonScope();
            Bind<ISpeciesRepository>().To<EntitySpeciesRepository>().InSingletonScope();

            Bind<IHeroLogicService>().To<HeroLogicService>().InSingletonScope();
            Bind<IHeroService>().To<HeroService>().InSingletonScope();
    
            Bind<HeroPresenter>().ToSelf().InTransientScope();
            Bind<ConsoleInterface.ConsoleInterface>().ToSelf().InSingletonScope();
        }
    }
}
