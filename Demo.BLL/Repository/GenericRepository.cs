using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {
        private protected readonly MVCAppDbContext _dbContext;

        public GenericRepository(MVCAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int Add(T Item)
        {
            //_dbContext.Set<T>().Add(Item);
            _dbContext.Add(Item);
            return _dbContext.SaveChanges();
        }

        public int Delete(T Item)
        {
           _dbContext.Set<T>().Remove(Item);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbContext.Set<T>().AsNoTracking().ToList();

        }

        public T GetById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public int Update(T Item)
        {
            _dbContext.Set<T>().Update(Item);
            return _dbContext.SaveChanges();
        }
    }
}
