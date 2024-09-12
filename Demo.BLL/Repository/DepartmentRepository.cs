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
    public class DepartmentRepository : GenericRepository<Department> , IDepartmentRepository
    {
        //private readonly MVCAppDbContext _dbContext;
        public DepartmentRepository(MVCAppDbContext dbContext):base(dbContext) // => Ask Clr For Creating Obj from DbContext
        {
            //_dbContext = dbContext;   
        }
        
    }
}
