using DataAccessLayer.Dapper;
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
            Bind<IHeroRepository>().To<DapperHeroRepository>().InSingletonScope();
            Bind<ISpeciesRepository>().To<DapperSpeciesRepository>().InSingletonScope();
            Bind<IHeroService>().To<HeroService>().InSingletonScope();
            Bind<Logic>().ToSelf();
        }
    }
}
