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
    public class OperationRepository : GenericRepository<Operation>, IOperationRepository
    {
        public OperationRepository(CentroInversionesTecnocorpDbContext dbContext) : base(dbContext)
        {

        }
    }
}
