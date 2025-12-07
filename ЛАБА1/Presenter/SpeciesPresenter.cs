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
    public class SpeciesPresenter
    {
        private readonly Logic _logic;

        public SpeciesPresenter()
        {
            IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());
            _logic = ninjectKernel.Get<Logic>();
        }

        public List<Species> GetAllSpecies()
        {
            return _logic.GetAllSpecies();
        }

        public void AddSpecies(string name, string description)
        {
            _logic.AddSpecies(name, description);
        }

        public void UpdateSpecies(Species species)
        {
            _logic.UpdateSpecies(species);
        }

        public void DeleteSpecies(int id)
        {
            _logic.DeleteSpecies(id);
        }
    }
}
