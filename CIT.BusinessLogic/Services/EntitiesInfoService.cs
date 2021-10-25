using CIT.BusinessLogic.Contracts;
using CIT.DataAccess.Contracts;
using CIT.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Services
{
    public class EntitiesInfoService : IEntitiesInfoService
    {
        private readonly IEntitesInfoRepository _entitiesInfoRepository;

        public EntitiesInfoService(IEntitesInfoRepository entitiesInfoRepository)
        {
            _entitiesInfoRepository = entitiesInfoRepository;
        }
        public async Task<Entitiesinfo> AddEntityInfoAsync(Entitiesinfo entitesInfo)
        {
            var savedEntityInfo = await _entitiesInfoRepository.AddAsync(entitesInfo);
            return savedEntityInfo;
        }

        public async Task<Entitiesinfo> UpdateEntityInfoAsync(Entitiesinfo entitiesInfo)
        {
            _entitiesInfoRepository.Update(entitiesInfo);
            await _entitiesInfoRepository.SaveChangesAsync();
            return entitiesInfo;
        }
    }
}
