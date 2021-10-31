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
        private readonly IOperationService _operationService;
        private readonly IPageService _pageService;
        private readonly TokenCreator _tokenCreator;
        private readonly IRoleService _roleService;

        public AccountService(IUserRepository userRepository, IRoleService roleService, IUserRoleRepository userRoleRepository, IEntitiesInfoService entitiesInfoService, IOperationService operationService, IPageService pageService, TokenCreator tokenCreator)
        {
            _userRepository = userRepository;
            _tokenCreator = tokenCreator;
            _userRoleRepository = userRoleRepository;
            _entitiesInfoService = entitiesInfoService;
            _operationService = operationService;
            _pageService = pageService;
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
                Password = Encryption.Encrypt(userDto.Password)
            };

            if (!userDto.Password.Equals(userDto.ConfirmPassword))
                throw new Exception("Las contraseñas no coinciden");


            var entitiesInfo = new Entitiesinfo()
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = 1
            };

            var savedEntityInfo = await _entitiesInfoService.AddEntityInfoAsync(entitiesInfo);

            userEntity.EntityInfoId = savedEntityInfo.Id;

            await _userRepository.AddAsync(userEntity);

            if (!string.IsNullOrEmpty(userDto.Photo))
                userEntity.Photo = await UploadProfilePhotoAsync(userDto.Photo, userEntity.Id);

            await _userRepository.SaveChangesAsync();

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
                var savedRole = await _roleService.CreateRoleAsync(administratorRole);
                savedRoleId = savedRole.RoleId;
            }


            var userRole = new Userrole()
            {
                RoleId = savedRoleId,
                UserId = userEntity.Id,
                EntityInfoId = entitiesInfo.Id
            };
            await _userRoleRepository.AddAsync(userRole);
            await _userRoleRepository.SaveChangesAsync();


            return new AccountResponse()
            {
                Email = userDto.Email,
                Token = _tokenCreator.BuildToken(userEntity)
            };
        }

        private async Task<string> UploadProfilePhotoAsync(string photo, int userId)
        {
            string imagePath = $"{Environment.CurrentDirectory}/ProfilePhotos";
            string[] imageSplitted = photo.Split(',');
            byte[] imageInBytes = Convert.FromBase64String(imageSplitted[1]);

            string fileName = $"profile_photo_{userId}.jpg";
            string path = Path.Combine(imagePath, fileName);


            using (var stream = new FileStream(path, FileMode.Create))
            {
                await stream.WriteAsync(imageInBytes, 0, imageInBytes.Length);
                await stream.FlushAsync();
            }

            return path;
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

        public async Task<AccountResponse> SignInAsync(string email, string password)
        {
            var encryptedPassword = Encryption.Encrypt(password);
            var user = await _userRepository.FirstOrDefaultWithRelationsAsync(u => u.Email.Equals(email) && u.Password.Equals(encryptedPassword));

            if (user != null)
            {
                if (user.EntityInfo.Status == 0)
                    throw new Exception("Este usuario ha sido deshabilitado o eliminado");

                return new AccountResponse() { Email = email, Token = _tokenCreator.BuildToken(user) };
            }
            

            throw new Exception("Usuario o contraseña incorrecta");
        }
    }
}
