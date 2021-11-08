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

        public async Task<Entitiesinfo> AddEntityInfoAsync()
        {
            var entitiesInfo = new Entitiesinfo()
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = 1
            };
            var savedEntityInfo = await _entitiesInfoRepository.AddAsync(entitiesInfo);
            await _entitiesInfoRepository.SaveChangesAsync();
            return savedEntityInfo;
        }

        public async Task<Entitiesinfo> GetEntityInfoAsync(int entityInfoId)
        {
            return await _entitiesInfoRepository.FirstOrDefaultAsync(e => e.Id == entityInfoId);
        }

        public async Task<Entitiesinfo> UpdateEntityInfo(Entitiesinfo entitiesInfo)
        {
            _entitiesInfoRepository.Update(entitiesInfo);
            await _entitiesInfoRepository.SaveChangesAsync();
            return entitiesInfo;
        }

        public async Task DeleteEntityInfoAsync(int entityInfoId)
        {
            var entityInfo = await _entitiesInfoRepository.FirstOrDefaultAsync(e => e.Id == entityInfoId);
            if(entityInfo != null)
                _entitiesInfoRepository.Delete(entityInfo);

            await _entitiesInfoRepository.SaveChangesAsync();
        }
    }
}
