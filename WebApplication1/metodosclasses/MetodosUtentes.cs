using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ObjetosNegocio;

namespace Data.Repositories
{
    public class UtentesRepository : IUtentesRepository
    {
        private readonly DbContext _dbContext;

        public UtentesRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Utentes> ObterLista()
        {
            return _dbContext.Set<Utentes>().ToList();
        }

        public Utentes ObterPorId(int id)
        {
            return _dbContext.Set<Utentes>().Find(id);
        }

        public void Inserir(Utentes utente)
        {
            _dbContext.Set<Utentes>().Add(utente);
            _dbContext.SaveChanges();
        }

        public void Atualizar(Utentes utente)
        {
            _dbContext.Entry(utente).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void Remover(int id)
        {
            var utente = _dbContext.Set<Utentes>().Find(id);
            if (utente != null)
            {
                _dbContext.Set<Utentes>().Remove(utente);
                _dbContext.SaveChanges();
            }
        }
    }
}
