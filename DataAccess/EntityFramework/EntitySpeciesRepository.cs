using Domain;
using Domain.Models;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.EntityFramework
{
    public class EntitySpeciesRepository : ISpeciesRepository
    {
        private readonly HeroContext _context;

        public EntitySpeciesRepository()
        {
            _context = new HeroContext();
        }

        public void Add(Species entity)
        {
            _context.Species.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var species = _context.Species.FirstOrDefault(s => s.Id == id);
            if (species != null)
            {
                _context.Species.Remove(species);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Species> ReadAll()
        {
            return _context.Species.ToList();
        }

        public Species ReadById(int id)
        {
            return _context.Species.FirstOrDefault(s => s.Id == id);
        }

        public void Update(Species entity)
        {
            var existingSpecies = _context.Species.Find(entity.Id);
            if (existingSpecies != null)
            {
                _context.Entry(existingSpecies).CurrentValues.SetValues(entity);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Species> GetAllOrderedByName()
        {
            return _context.Species.OrderBy(s => s.Name).ToList();
        }
    }
}