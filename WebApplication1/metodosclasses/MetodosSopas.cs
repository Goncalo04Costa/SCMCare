using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data.Repositories
{
    public class MetodosSopas : ISopaRepository
    {
        private readonly DbContext _dbContext;

        public SopaRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Sopa> ObterLista()
        {
            return _dbContext.Set<Sopa>().ToList();
        }

        public Sopa ObterPorId(int id)
        {
            return _dbContext.Set<Sopa>().Find(id);
        }

        public void Inserir(Sopa sopa)
        {
            _dbContext.Set<Sopa>().Add(sopa);
            _dbContext.SaveChanges();
        }

        // Aqui você pode adicionar métodos para atualizar e remover sopa, se necessário.
    }
}
