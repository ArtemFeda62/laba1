using DataAccessLayer.EntityFramework;
using Ninject.Modules;
using Shared.Interfases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class SimpleConfigModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IHeroRepository>().To<EntityHeroRepository>().InSingletonScope();
            Bind<ISpeciesRepository>().To<EntitySpeciesRepository>().InSingletonScope();
            Bind<IHeroService>().To<HeroService>().InSingletonScope();
            Bind<Logic>().ToSelf();
        }
    }
}
