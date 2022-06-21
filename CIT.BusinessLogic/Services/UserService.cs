using AutoMapper;
using CIT.BusinessLogic.Contracts;
using CIT.DataAccess.Contracts;
using CIT.DataAccess.Models;
using CIT.Dtos.Requests;
using CIT.Dtos.Responses;
using CIT.Dtos.Validations;
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
        private readonly IUsersLenderBusinessesRepository _usersLenderBusinessesRepository;
        private readonly ILoanService _loanService;
        private readonly IPaymentService _paymentService;
        private readonly IVehicleAssignmentService _vehicleAssignmentService;
        private readonly ILoginService _loginService;
        private readonly IRoleService _roleService;
        private readonly TokenCreator _tokenCreator;
        private readonly AccountTools _accountTools;

        private const string USER_EXISTS_ERROR_MESSAGE = "Ya existe un usuario con este correo, teléfono o documento en este negocio prestamista, por favor, valida los datos.";

        public UserService(IUserRepository userRepository,
            IUsersLenderBusinessesRepository usersLenderBusinessesRepository,
            ILoanService loanService,
            IPaymentService paymentService,
            IVehicleAssignmentService vehicleAssignmentService,
            IRoleService roleService,
            IUserRoleService userRoleService,
            IEntitiesInfoService entitiesInfoService,
            IAddressService addressService,
            IUserAddressService userAddressService,
            ILoginService loginService,
            TokenCreator tokenCreator, 
            AccountTools accountTools)
        {
            _userRepository = userRepository;
            _tokenCreator = tokenCreator;
            _loginService = loginService;
            _userRoleService = userRoleService;
            _entitiesInfoService = entitiesInfoService;
            _addressService = addressService;
            _userAddressService = userAddressService;
            _usersLenderBusinessesRepository = usersLenderBusinessesRepository;
            _loanService = loanService;
            _paymentService = paymentService;
            _vehicleAssignmentService = vehicleAssignmentService;
            _accountTools = accountTools;
            _roleService = roleService;
        }
        public async Task<AccountResponse> RegisterUserAsync(UserDto userDto, int lenderBusinessId)
        {
            var isUserExists = await ValidateUserExistsAsync(new UserLenderBusinessValidateDto()
            {
                Email = userDto.Email,
                Phone = userDto.Phone,
                IdentificationDocument = userDto.IdentificationDocument,
                LenderBusinessId = lenderBusinessId
            });

            if (!isUserExists)
            {
                var userInDb = await _userRepository.FirstOrDefaultWithRelationsAsync(u => u.Email.Equals(userDto.Email) || u.IdentificationDocument.Equals(userDto.IdentificationDocument) || u.Phone.Equals(userDto.Phone));

                if(userInDb != null)
                {
                    await SaveUserLenderBusinessAsync(userInDb.Id, lenderBusinessId);

                    return new AccountResponse()
                    {
                        Email = userDto.Email,
                        Token = _tokenCreator.BuildToken(userInDb, lenderBusinessId)
                    };
                }
                else
                {
                    var userEntity = new User()
                    {
                        Name = userDto.Name,
                        LastName = userDto.LastName,
                        IdentificationDocument = userDto.IdentificationDocument,
                        Phone = userDto.Phone,
                        Email = userDto.Email,
                        Password = Encryption.Encrypt(userDto.Password)
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


                    await SaveUserLenderBusinessAsync(userEntity.Id, lenderBusinessId);
                    await _accountTools.SendEmailConfirmationAsync(userEntity.IdentificationDocument, userEntity.Email);
                    await _loginService.SaveLoginAsync(userEntity.Email, userEntity.Password);

                    return new AccountResponse()
                    {
                        Email = userDto.Email,
                        Token = _tokenCreator.BuildToken(userEntity, lenderBusinessId)
                    };
                }
            }
            throw new Exception(USER_EXISTS_ERROR_MESSAGE);
        }

        public async Task<UserDto> UpdateUserAsync(UserDto user, int lenderBusinessId)
        {
            var isUserExists = await ValidateUserExistsAsync(new UserLenderBusinessValidateDto()
            {
                Email = user.Email,
                IdentificationDocument = user.IdentificationDocument,
                Phone = user.Phone,
                UserId = user.Id,
                LenderBusinessId = lenderBusinessId
            });

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

        private async Task<bool> ValidateUserExistsAsync(UserLenderBusinessValidateDto validate)
        {
            if(validate.UserId == 0)
            {
                var userLenderBusinesses = await _usersLenderBusinessesRepository.GetAllWithFilterAsync(ul => ul.LenderBusinessId == validate.LenderBusinessId);

                foreach (var userLenderBusiness in userLenderBusinesses)
                {
                    var userInDb = await _userRepository.FirstOrDefaultAsync(u => u.Email.Equals(validate.Email) || u.IdentificationDocument.Equals(validate.IdentificationDocument) || u.Phone.Equals(validate.Phone) && u.Id == userLenderBusiness.UserId);

                    return userInDb != null;
                }
            }
            else
            {
                var userLenderBusiness = await _usersLenderBusinessesRepository.FirstOrDefaultAsync(ul => ul.LenderBusinessId == validate.LenderBusinessId && ul.UserId == validate.UserId);

                if(userLenderBusiness != null)
                {
                    var userInDb = await _userRepository.FirstOrDefaultAsync(u => u.Email.Equals(validate.Email) || u.IdentificationDocument.Equals(validate.IdentificationDocument) || u.Phone.Equals(validate.Phone) && u.Id != validate.UserId);

                    return userInDb != null;
                }
            }
            return false;
        }

        private async Task SaveUserLenderBusinessAsync(int userId, int lenderBusinessId)
        {
            await _usersLenderBusinessesRepository.AddAsync(new UsersLenderBusinesses()
            {
                UserId = userId,
                LenderBusinessId = lenderBusinessId
            });
            await _usersLenderBusinessesRepository.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int lenderBusinessId, int userId)
        {
            var user = await GetUserAsync(userId);
            
            if(user != null)
            {
                var loans = await _loanService.GetLoansByUserAsync(lenderBusinessId, userId);

                if (loans.Count > 0)
                    foreach (var loan in loans)
                        await _loanService.DeleteLoanAsync(loan.Id);

                var vehicleAssignment = await _vehicleAssignmentService.GetVehicleAssignmentByUserAsync(lenderBusinessId, userId);

                if (vehicleAssignment != null)
                    await _vehicleAssignmentService.DeleteAssignmentAsync(vehicleAssignment.Id);

                var userLenderBusiness = await _usersLenderBusinessesRepository.FirstOrDefaultAsync(ul => ul.LenderBusinessId == lenderBusinessId && ul.UserId == userId);
                if (userLenderBusiness != null)
                {
                    _usersLenderBusinessesRepository.Delete(userLenderBusiness);
                    await _usersLenderBusinessesRepository.SaveChangesAsync();
                }

                await _entitiesInfoService.UpdateEntityInfo(user.EntityInfo.Id, 0);
                await _loginService.DeleteLoginAsync(user.Email);
            }
            
        }

        public async Task<List<UserDto>> GetUsersAsync(int lenderBusinessId)
        {
            var usersLenderBusinesss = await _usersLenderBusinessesRepository.GetAllWithFilterAsync(ul => ul.LenderBusinessId == lenderBusinessId);
            var users = await _userRepository.GetAllWithFilterAndWithRelationsAsync(u => 
            usersLenderBusinesss.Select(ul => ul.UserId).ToArray().Contains(u.Id));
            return GetUsersResponse(users);
        }

        public async Task<List<UserDto>> GetUsersByNameAsync(int lenderBusinessId, string fullName)
        {
            var usersLenderBusiness = await _usersLenderBusinessesRepository.GetAllWithFilterAsync(ul => ul.LenderBusinessId == lenderBusinessId);

            var usersLenderBusinessArray = usersLenderBusiness.Select(ul => ul.UserId).ToArray();
            var users = await _userRepository.GetAllWithFilterAndWithRelationsAsync(u => 
            usersLenderBusinessArray.Contains(u.Id) && string.Concat(u.Name, " ", u.LastName).ToLower().Contains(fullName.ToLower()));
            return GetUsersResponse(users);
        }

        private List<UserDto> GetUsersResponse(List<User> users)
        {
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
                }
            };

            return userDto;
        }

    }
}
