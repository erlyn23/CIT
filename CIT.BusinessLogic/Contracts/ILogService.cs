﻿using CIT.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Contracts
{
    public interface ILogService
    {
        Task<LogDto> SaveLogAsync(LogDto log);
    }
}