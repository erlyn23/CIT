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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IEntitiesInfoService _entitiesInfoService;
        private readonly IAddressService _addressService;
        private readonly IUserAddressService _userAddressService;
        private readonly TokenCreator _tokenCreator;
        private readonly ILoginRepository _loginRepository;
        private readonly IRoleService _roleService;

        public UserService(IUserRepository userRepository, IRoleService roleService, IUserRoleRepository userRoleRepository, IEntitiesInfoService entitiesInfoService, IAddressService addressService, IUserAddressService userAddressService, TokenCreator tokenCreator, ILoginRepository loginRepository)
        {
            _userRepository = userRepository;
            _tokenCreator = tokenCreator;
            _loginRepository = loginRepository;
            _userRoleRepository = userRoleRepository;
            _entitiesInfoService = entitiesInfoService;
            _addressService = addressService;
            _userAddressService = userAddressService;
            _roleService = roleService;
        }
        public async Task<AccountResponse> RegisterUserAsync(UserDto userDto)
        {
            var userEntity = new User()
            {
                Name = userDto.Name,
                LastName = userDto.LastName,
                IdentificationDocument = userDto.IdentificationDocument,
                Phone = userDto.Phone,
                Email = userDto.Email,
                Password = Encryption.Encrypt(userDto.Password),
                LenderBusinessId = userDto.LenderBusinessId
            };

            if (!userDto.Password.Equals(userDto.ConfirmPassword))
                throw new Exception("Las contraseñas no coinciden");


            var savedEntityInfo = await _entitiesInfoService.AddEntityInfoAsync();
            userEntity.EntityInfoId = savedEntityInfo.Id;


            await _userRepository.AddAsync(userEntity);
            await _userRepository.SaveChangesAsync();

            if (!string.IsNullOrEmpty(userDto.Photo))
            {
                userEntity.Photo = await UploadPhoto.UploadProfilePhotoAsync($"user_profile_photo_{userEntity.Id}.jpg", userDto.Photo);

                _userRepository.Update(userEntity);
                await _userRepository.SaveChangesAsync();
            }

            var savedAddress = await _addressService.CreateAddressAsync(userDto.Address);
            await _userAddressService.CreateUserAddress(userEntity.Id, savedAddress.Id);

            int roleId = userDto.UserRole.RoleId;

            var userRole = new Userrole()
            {
                RoleId = roleId,
                UserId = userEntity.Id,
                EntityInfoId = savedEntityInfo.Id
            };
            await _userRoleRepository.AddAsync(userRole);
            await _userRoleRepository.SaveChangesAsync();


            return new AccountResponse()
            {
                Email = userDto.Email,
                Token = _tokenCreator.BuildToken(userEntity)
            };
        }

        public async Task<List<UserDto>> GetUsersAsync(int lenderBusinessId)
        {
            var users = await _userRepository.GetAllWithFilterAndWithRelationsAsync(u => u.LenderBusinessId == lenderBusinessId);

            var usersDto = users.Select(u => MapUserAsync(u).Result).ToList();
            return usersDto.Where(u => u.EntityInfo.Status != 0).ToList();
        }

        public async Task<UserDto> GetUserAsync(int userId)
        {
            var user = await _userRepository.FirstOrDefaultWithRelationsAsync(u => u.Id == userId);
            
            UserDto userDto = null;
            if(user != null)
                userDto = await MapUserAsync(user);

            if (userDto != null)
            {
                if (userDto.EntityInfo.Status != 0)
                    return userDto;
                else
                    throw new Exception("Este usuario no existe en la base de datos");
            }
            else
                throw new Exception("Este usuario no existe en la base de datos");
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await GetUserAsync(userId);

            var entityInfo = await _entitiesInfoService.GetEntityInfoAsync(user.EntityInfo.Id);
            entityInfo.UpdatedAt = DateTime.Now;
            entityInfo.Status = 0;
            await _entitiesInfoService.UpdateEntityInfo(entityInfo);
            
            
            var login = await _loginRepository.FirstOrDefaultAsync(l => l.Email.Equals(user.Email));
            login.Status = 0;
            _loginRepository.Update(login);
            await _loginRepository.SaveChangesAsync();
        }

        private async Task<UserDto> MapUserAsync(User user)
        {
            var entityInfo = await _entitiesInfoService.GetEntityInfoAsync(user.EntityInfoId);
            var address = await _addressService.GetAddressAsync(user.Useraddress.AddressId);
            var role = await _roleService.GetRoleByIdAsync(user.Userrole.RoleId);
            var userRole = await _userRoleRepository.FirstOrDefaultAsync(ur => ur.UserId == user.Id);

            var userDto = new UserDto()
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                IdentificationDocument = user.IdentificationDocument,
                Phone = user.Phone,
                Email = user.Email,
                Photo = user.Photo,
                Address = address,
                UserRole = new UserRoleDto()
                {
                    UserRoleId = userRole.Id,
                    UserId = user.Id,
                    RoleId = role.RoleId,
                    Role = role
                },
                EntityInfo = new EntityInfoDto()
                {
                    CreatedAt = entityInfo.CreatedAt,
                    UpdatedAt = entityInfo.UpdatedAt,
                    Status = entityInfo.Status
                },
                LenderBusinessId = user.LenderBusinessId
            };

            return userDto;
        }
    }
}
