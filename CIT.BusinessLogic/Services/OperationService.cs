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
    public class OperationService : IOperationService
    {
        private readonly IOperationRepository _operationRepository;

        public OperationService(IOperationRepository operationRepository)
        {
            _operationRepository = operationRepository;
        }
        public async Task<List<OperationDto>> GetOperationsAsync()
        {
            var operations = await _operationRepository.GetAllAsync();
            return operations.Select(o => MapOperation(o)).ToList();
        }

        private OperationDto MapOperation(Operation operation)
        {
            return new OperationDto()
            {
                Id = operation.Id,
                OperationName = operation.OperationName
            };
        }
    }
}
