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
        public void Add(T Item)
        {
            _dbContext.Add(Item);
        }

        public void Delete(T Item)
        {
           _dbContext.Set<T>().Remove(Item);
        }

        public IEnumerable<T> GetAll()
        {
            if (typeof(T)==typeof(Employee))
            {
                return (IEnumerable<T>) _dbContext.Employee.Include(E => E.Department).AsNoTracking().ToList();
            }
            else
            {
                return _dbContext.Set<T>().AsNoTracking().ToList();
            }
           

        }

        public T GetById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public void Update(T Item)
        {
            _dbContext.Set<T>().Update(Item);
        }
    }
}
