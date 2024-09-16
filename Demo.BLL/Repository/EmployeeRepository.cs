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
    public class EmployeeRepository : GenericRepository<Employee> , IEmployeeRepository

    {
        //private readonly MVCAppDbContext _dbContext;
        public EmployeeRepository(MVCAppDbContext dbContext):base(dbContext)
        {
            //_dbContext = dbContext;
        }

        public IQueryable<Employee> GetEmployeeByAddress(string address)
               => _dbContext.Employee.Where(E => E.Address.ToLower().Contains(address.ToLower()));

        public IQueryable<Employee> SearchEmployeeByName(string name)
               => _dbContext.Employee.Where(E => E.Name.ToLower().Contains(name.ToLower()));
        
    }
}
