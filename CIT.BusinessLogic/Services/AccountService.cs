using CIT.BusinessLogic.Contracts;
using CIT.DataAccess.Contracts;
using CIT.DataAccess.Models;
using CIT.Dtos.Requests;
using CIT.Dtos.Responses;
using CIT.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IEntitiesInfoService _entitiesInfoService;
        private readonly IAddressService _addressService;
        private readonly IUserAddressService _userAddressService;
        private readonly TokenCreator _tokenCreator;
        private readonly ILoginRepository _loginRepository;
        private readonly IRoleService _roleService;
        private readonly ILenderBusinessRepository _lenderBusinessRepository;

        public AccountService(IUserRepository userRepository, IRoleService roleService, IUserRoleRepository userRoleRepository, IEntitiesInfoService entitiesInfoService, IAddressService addressService, IUserAddressService userAddressService, TokenCreator tokenCreator, ILoginRepository loginRepository, ILenderBusinessRepository lenderBusinessRepository)
        {
            _userRepository = userRepository;
            _tokenCreator = tokenCreator;
            _loginRepository = loginRepository;
            _lenderBusinessRepository = lenderBusinessRepository;
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
                userEntity.Photo = await UploadPhoto.UploadProfilePhotoAsync($"user_profile_photo_{userEntity.Id}.jpg", userDto.Photo);

            _userRepository.Update(userEntity);
            await _userRepository.SaveChangesAsync();

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
            return usersDto;
        }

        public async Task<UserDto> GetUserAsync(int userId)
        {
            var user = await _userRepository.FirstOrDefaultWithRelationsAsync(u => u.Id == userId);
            var userDto = await MapUserAsync(user);

            return userDto;
        }

        private async Task<UserDto> MapUserAsync(User user)
        {
            var entityInfo = await _entitiesInfoService.GetEntityInfoAsync(user.EntityInfoId);
            var address = await _addressService.GetAddressAsync(user.UserAddress.AddressId);
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
                LenderBusinessId = user.LenderBusinessId
            };

            return userDto;
        }

        public async Task<AccountResponse> SignInAsync(string email, string password)
        {
            var encryptedPassword = Encryption.Encrypt(password);
            var login = await _loginRepository.FirstOrDefaultAsync(l => l.Email.Equals(email) && l.Password.Equals(encryptedPassword));


            if (login == null)
                throw new Exception("Usuario o contraseña incorrecta");
            if (login.Status == 0)
                throw new Exception("Usuario o negocio inactivo o eliminado");



            var lenderBusiness = await _lenderBusinessRepository.FirstOrDefaultWithRelationsAsync(l => l.Email.Equals(email) && l.Password.Equals(encryptedPassword));

            if(lenderBusiness != null)
                return new AccountResponse() { Email = email, Token = _tokenCreator.BuildToken(lenderBusiness) };
            else
            {
               var user = await _userRepository.FirstOrDefaultWithRelationsAsync(u => u.Email.Equals(email) && u.Password.Equals(encryptedPassword));
                return new AccountResponse() { Email = email, Token = _tokenCreator.BuildToken(user) };
            }

        }
    }
}
