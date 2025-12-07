using BusinessLogicLayer;
using Ninject;
using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class AddHeroPresenter
    {
        private readonly Logic _logic;

        public AddHeroPresenter()
        {
            IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());
            _logic = ninjectKernel.Get<Logic>();
        }

        public List<Species> GetAvailableSpecies()
        {
            return _logic.GetAllSpecies();
        }

        public void AddHero(string name, int speciesId, string genre, int strange, string damageType, double hp)
        {
            _logic.CreateHero(name, speciesId, genre, strange, damageType, hp);
        }
    }

}
