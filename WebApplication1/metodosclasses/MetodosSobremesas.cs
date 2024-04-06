using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data.Repositories
{
    //public class SobremesaRepository : ISobremesaRepository
    public class SobremesaRepository
    {
        private readonly DbContext _dbContext;

        public SobremesaRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Sobremesa> ObterLista()
        {
            return _dbContext.Set<Sobremesa>().ToList();
        }

        public Sobremesa ObterPorId(int id)
        {
            return _dbContext.Set<Sobremesa>().Find(id);
        }

        public void Inserir(Sobremesa sobremesa)
        {
            _dbContext.Set<Sobremesa>().Add(sobremesa);
            _dbContext.SaveChanges();
        }

        public void Atualizar(Sobremesa sobremesa)
        {
            _dbContext.Entry(sobremesa).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void Remover(int id)
        {
            var sobremesa = _dbContext.Set<Sobremesa>().Find(id);
            if (sobremesa != null)
            {
                _dbContext.Set<Sobremesa>().Remove(sobremesa);
                _dbContext.SaveChanges();
            }
        }
    }
}
