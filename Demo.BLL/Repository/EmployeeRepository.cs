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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MVCAppDbContext _dbContext;
        public EmployeeRepository(MVCAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int Add(Employee Employee)
        {
            _dbContext.Employees.Add(Employee);
            return _dbContext.SaveChanges();
        }

        public int Delete(Employee Employee)
        {
            _dbContext?.Employees.Remove(Employee);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<Employee> GetAll()
        {
           return _dbContext.Employees.AsNoTracking().ToList();
        }

        public Employee GetById(int id)
        {
            return _dbContext.Employees.Find(id);
        }

        public int Update(Employee Employee)
        {
            _dbContext.Employees.Update(Employee);
            return _dbContext.SaveChanges();
        }
    }
}
