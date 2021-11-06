﻿using CIT.DataAccess.Contracts;
using CIT.DataAccess.DbContexts;
using CIT.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.DataAccess.Repositories
{
    public class LenderBusinessRepository : GenericRepository<LenderBusiness>,  ILenderBusinessRepository
    {
        public LenderBusinessRepository(CentroInversionesTecnocorpDbContext dbContext): base(dbContext)
        {

        }
    }
}