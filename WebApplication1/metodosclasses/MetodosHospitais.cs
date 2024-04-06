using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data.Repositories
{
    public class Hospital : IHospitalRepository
    {
        private readonly DbContext _dbContext;

        public HospitalRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Hospital> ObterLista()
        {
            return _dbContext.Set<Hospital>().ToList();
        }

        public Hospital ObterPorId(int id)
        {
            return _dbContext.Set<Hospital>().Find(id);
        }

        public void Inserir(Hospital hospital)
        {
            _dbContext.Set<Hospital>().Add(hospital);
            _dbContext.SaveChanges();
        }

        public void Remover(int id)
        {
            var hospital = _dbContext.Set<Hospital>().Find(id);
            if (hospital != null)
            {
                _dbContext.Set<Hospital>().Remove(hospital);
                _dbContext.SaveChanges();
            }
        }
    }
}
