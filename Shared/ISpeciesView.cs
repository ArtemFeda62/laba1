using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface ISpeciesView
    {
        event EventHandler LoadSpeciesRequested;
        event EventHandler AddSpeciesRequested;
        event EventHandler<int> DeleteSpeciesRequested;
        event EventHandler<(int id, string name, string description)> UpdateSpeciesRequested;
        void DisplaySpecies(List<Species> species);
    }
}
