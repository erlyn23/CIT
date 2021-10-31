using CIT.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Contracts
{
    public interface IEntitiesInfoService
    {
        Task<Entitiesinfo> AddEntityInfoAsync(Entitiesinfo entitesInfo);
        Task<Entitiesinfo> GetEntityInfoAsync(int entityInfoId);
        Entitiesinfo UpdateEntityInfo(Entitiesinfo entitiesInfo);
        Task DeleteEntityInfoAsync(int entityInfoId);
    }
}
