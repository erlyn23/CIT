using CIT.BusinessLogic.Contracts;
using CIT.DataAccess.Contracts;
using CIT.DataAccess.Models;
using CIT.Dtos.Requests;
using CIT.Dtos.Responses;
using CIT.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Services
{
    public class LenderBusinessService : ILenderBusinessService
    {
        private readonly ILenderBusinessRepository _lenderBusinessRepository;
        private readonly IEntitiesInfoService _entitiesInfoService;
        private readonly IAddressService _addressService;
        private readonly IOperationService _operationService;
        private readonly IPageService _pageService;
        private readonly IRoleService _roleService;
        private readonly ILenderAddressService _lenderAddressService;
        private readonly ILenderRoleService _lenderRoleService;
        private readonly TokenCreator _tokenCreator;

        public LenderBusinessService(IEntitiesInfoService entitiesInfoService,
            IAddressService addressService, ILenderBusinessRepository lenderBusinessRepository, IOperationService operationService, 
            IPageService pageService,
            IRoleService roleService,
            ILenderAddressService lenderAddressService,
            ILenderRoleService lenderRoleService,
            TokenCreator tokenCreator)
        {
            _entitiesInfoService = entitiesInfoService;
            _addressService = addressService;
            _lenderBusinessRepository = lenderBusinessRepository;
            _operationService = operationService;
            _pageService = pageService;
            _roleService = roleService;
            _lenderAddressService = lenderAddressService;
            _lenderRoleService = lenderRoleService;
            _tokenCreator = tokenCreator;
        }
        public async Task<AccountResponse> CreateLenderBusinessAsync(LenderBusinessDto lenderBusiness)
        {
            var lenderBusinessEntity = new LenderBusiness()
            {
                BusinessName = lenderBusiness.BusinessName,
                Email = lenderBusiness.Email,
                Phone = lenderBusiness.Phone,
                Rnc = lenderBusiness.Rnc,
                Password = Encryption.Encrypt(lenderBusiness.Password)
            };

            if (!lenderBusiness.Password.Equals(lenderBusiness.ConfirmPassword))
                throw new Exception("Las contraseñas no coinciden");

            var savedEntityInfo = await _entitiesInfoService.AddEntityInfoAsync();

            lenderBusinessEntity.EntityInfoId = savedEntityInfo.Id;

            await _lenderBusinessRepository.AddAsync(lenderBusinessEntity);

            await _lenderBusinessRepository.SaveChangesAsync();

            if (!string.IsNullOrEmpty(lenderBusiness.Photo))
                await UploadPhoto.UploadProfilePhotoAsync($"business_profile_photo_{lenderBusinessEntity.Id}", lenderBusiness.Photo);

            _lenderBusinessRepository.Update(lenderBusinessEntity);
            await _lenderBusinessRepository.SaveChangesAsync();

            var savedAddress = await _addressService.CreateAddressAsync(lenderBusiness.Address);

            await _lenderAddressService.SaveLenderAddressAsync(lenderBusinessEntity.Id, savedAddress.Id);

            int roleId = await SaveAdminRole(lenderBusinessEntity.Id);
            var lenderRole = new LenderRole()
            {
                RoleId = roleId,
                LenderBusinessId = lenderBusinessEntity.Id
            };

            var savedLenderRole = await _lenderRoleService.SaveLenderRoleAsync(lenderRole);
            lenderBusinessEntity.LenderRole = savedLenderRole;

            return new AccountResponse()
            {
                Email = lenderBusinessEntity.Email,
                Token = _tokenCreator.BuildToken(lenderBusinessEntity)
            };
        }

        private async Task<int> SaveAdminRole(int lenderBusinessId)
        {
            var rolePermissions = await SetAdminRolePermissions();
            var administratorRole = await _roleService.GetRoleByNameAsync("Administrador");
            var savedRoleId = (administratorRole != null) ? administratorRole.RoleId : 0;
            if (administratorRole == null)
            {
                administratorRole = new RoleDto()
                {
                    Role = "Administrador",
                    RolePermissions = rolePermissions
                };
                var savedRole = await _roleService.CreateRoleAsync(administratorRole, lenderBusinessId);
                savedRoleId = savedRole.RoleId;
            }
            return savedRoleId;
        }

        private async Task<List<RolePermissionDto>> SetAdminRolePermissions()
        {
            var operations = await _operationService.GetOperationsAsync();
            var pages = await _pageService.GetPagesAsync();

            var rolePermissions = new List<RolePermissionDto>();

            foreach (var page in pages)
            {
                foreach (var operation in operations)
                {
                    rolePermissions.Add(new RolePermissionDto()
                    {
                        PageId = page.PageId,
                        OperationId = operation.OperationId
                    });
                }
            }

            return rolePermissions;
        }

        public async Task<LenderBusinessDto> GetLenderBusinessAsync(int lenderBusinessId)
        {
            var lenderBusiness = await _lenderBusinessRepository.FirstOrDefaultAsync(l => l.Id == lenderBusinessId);
            var lenderBusinessDto = MapLenderBusiness(lenderBusiness);
            return lenderBusinessDto; 
        }

        private LenderBusinessDto MapLenderBusiness(LenderBusiness lenderBusiness)
        {
            var lenderBusinessDto = new LenderBusinessDto()
            {
                Id = lenderBusiness.Id,
                BusinessName = lenderBusiness.BusinessName,
                Rnc = lenderBusiness.Rnc,
                Email = lenderBusiness.Email,
                Phone = lenderBusiness.Phone,
                Photo = lenderBusiness.Photo
            };

            return lenderBusinessDto;
        }

    }
}
