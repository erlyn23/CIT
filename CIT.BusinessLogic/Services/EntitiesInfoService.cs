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

        public async Task<Entitiesinfo> GetEntityInfoAsync(int entityInfoId)
        {
            return await _entitiesInfoRepository.FirstOrDefaultAsync(e => e.Id == entityInfoId);
        }

        public Entitiesinfo UpdateEntityInfo(Entitiesinfo entitiesInfo)
        {
            _entitiesInfoRepository.Update(entitiesInfo);
            return entitiesInfo;
        }

        public async Task DeleteEntityInfoAsync(int entityInfoId)
        {
            var entityInfo = await _entitiesInfoRepository.FirstOrDefaultAsync(e => e.Id == entityInfoId);
            if(entityInfo != null)
                _entitiesInfoRepository.Delete(entityInfo);
        }
    }
}
