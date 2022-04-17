using AutoMapper;
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
        private readonly IUserRoleService _userRoleService;
        private readonly IEntitiesInfoService _entitiesInfoService;
        private readonly IAddressService _addressService;
        private readonly IUserAddressService _userAddressService;
        private readonly TokenCreator _tokenCreator;
        private readonly AccountTools _accountTools;
        private readonly ILoginService _loginService;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;

        private const string USER_EXISTS_ERROR_MESSAGE = "Ya existe un usuario con este correo, cédula o teléfono, por favor, valida los datos";

        public UserService(IUserRepository userRepository, IRoleService roleService, IUserRoleService userRoleService, IEntitiesInfoService entitiesInfoService, IAddressService addressService, IUserAddressService userAddressService, TokenCreator tokenCreator, AccountTools accountTools, ILoginService loginService, IMapper mapper)
        {
            _userRepository = userRepository;
            _tokenCreator = tokenCreator;
            _loginService = loginService;
            _mapper = mapper;
            _userRoleService = userRoleService;
            _entitiesInfoService = entitiesInfoService;
            _addressService = addressService;
            _userAddressService = userAddressService;
            _accountTools = accountTools;
            _roleService = roleService;
        }
        public async Task<AccountResponse> RegisterUserAsync(UserDto userDto)
        {
            var isUserExists = await ValidateUserExistsAsync(userDto.Email, userDto.IdentificationDocument, userDto.Phone);

            if (!isUserExists)
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

                await _userRoleService.AddUserRoleAsync(new UserRoleDto
                {
                    RoleId = roleId,
                    UserId = userEntity.Id
                });

                await _accountTools.SendEmailConfirmationAsync(userEntity.Email);
                await _loginService.SaveLoginAsync(userEntity.Email, userEntity.Password);

                return new AccountResponse()
                {
                    Email = userDto.Email,
                    Token = _tokenCreator.BuildToken(userEntity)
                };
            }
            throw new Exception(USER_EXISTS_ERROR_MESSAGE);
        }

        public async Task<UserDto> UpdateUserAsync(UserDto user)
        {
            var isUserExists = await ValidateUserExistsAsync(user.Email, user.IdentificationDocument, user.Phone, user.Id);

            if (!isUserExists)
            {
                var userInDb = await _userRepository.FirstOrDefaultWithRelationsAsync(u => u.Id == user.Id);

                if (userInDb != null)
                {
                    userInDb.Name = user.Name;
                    userInDb.LastName = user.LastName;
                    userInDb.IdentificationDocument = user.IdentificationDocument;
                    if (!string.IsNullOrEmpty(user.Photo))
                        userInDb.Photo = await UploadPhoto.UploadProfilePhotoAsync($"user_profile_photo_{userInDb.Id}.jpg", user.Photo);
                    userInDb.Phone = user.Phone;
                    userInDb.Password = Encryption.Encrypt(user.Password);

                    await _addressService.UpdateAddressAsync(user.Address);

                    await _entitiesInfoService.UpdateEntityInfo(userInDb.EntityInfo.Id, 1);

                    var userRole = await _userRoleService.UpdateUserRoleAsync(new UserRoleDto
                    {
                        Id = userInDb.Userrole.Id,
                        UserId = userInDb.Id,
                        RoleId = user.UserRole.RoleId
                    });
                        

                    _userRepository.Update(userInDb);
                    await _userRepository.SaveChangesAsync();
                }
                return user;
            }

            throw new Exception(USER_EXISTS_ERROR_MESSAGE);
        }

        private async Task<bool> ValidateUserExistsAsync(string email, string identificationDocument, string phone, int userId = 0)
        {
            var userInDb = await _userRepository.FirstOrDefaultAsync(u => u.Email.Equals(email) || u.IdentificationDocument.Equals(identificationDocument) || u.Phone.Equals(phone));

            if (userId != 0)
            {
                userInDb = null;
                userInDb = await _userRepository.FirstOrDefaultAsync(u => u.Email.Equals(email) || u.IdentificationDocument.Equals(identificationDocument) || u.Phone.Equals(phone));

                if (userInDb.Id == userId)
                    userInDb = null;
            }
               

            if (userInDb != null)
                return true;

            return false;
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await GetUserAsync(userId);
            await _entitiesInfoService.UpdateEntityInfo(user.EntityInfo.Id, 0);
            await _loginService.DeleteLoginAsync(user.Email);
        }

        public async Task<List<UserDto>> GetUsersAsync(int lenderBusinessId)
        {
            var users = await _userRepository.GetAllWithFilterAndWithRelationsAsync(u => u.LenderBusinessId == lenderBusinessId);

            var usersDto = users.Select(u => MapUserAsync(u).Result).ToList();
            return usersDto.Where(u => u.EntityInfo.Status != 0).ToList();
        }

        public async Task<UserDto> GetUserAsync(int userId)
        {
            string errorMessage = "Este usuario no existe en la base de datos";
            var user = await _userRepository.FirstOrDefaultWithRelationsAsync(u => u.Id == userId);

            UserDto userDto = null;
            if (user != null)
                userDto = await MapUserAsync(user);

            if (userDto != null)
            {
                if (userDto.EntityInfo.Status != 0)
                    return userDto;
                else
                    throw new Exception(errorMessage);
            }
            else
                throw new Exception(errorMessage);
        }

        private async Task<UserDto> MapUserAsync(User user)
        {
            var entityInfo = await _entitiesInfoService.GetEntityInfoAsync(user.EntityInfoId);
            var address = await _addressService.GetAddressAsync(user.Useraddress.AddressId);
            var role = await _roleService.GetRoleByIdAsync(user.Userrole.RoleId);
            var userRoles = await _userRoleService.GetUserRolesAsync();
            var userRole = userRoles.FirstOrDefault(u => u.UserId == user.Id);

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
                    Id = userRole.Id,
                    UserId = user.Id,
                    RoleId = role.Id,
                    Role = role
                },
                EntityInfo = new EntityInfoDto()
                {
                    Id = entityInfo.Id,
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
