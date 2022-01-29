using CIT.BusinessLogic.Contracts;
using CIT.DataAccess.Contracts;
using CIT.DataAccess.Models;
using CIT.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }
        public async Task<LogDto> SaveLogAsync(LogDto log)
        {

            var logEntity = new Log()
            {
                Operation = log.Operation,
                LenderBusinessId = log.LenderBusinessId,
                ResultMessageOrObject = log.ResultMessageOrObject,
                LogDate = log.LogDate
            };

            await _logRepository.AddAsync(logEntity);
            await _logRepository.SaveChangesAsync();
            log.Id = logEntity.Id;
            return log;
        }
    }
}
