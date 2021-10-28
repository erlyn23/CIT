﻿using CIT.DataAccess.Contracts;
using CIT.DataAccess.DbContexts;
using CIT.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CIT.DataAccess.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {

        public RoleRepository(CentroInversionesTecnocorpDbContext dbContext) : base(dbContext)
        {

        }
    }
}
