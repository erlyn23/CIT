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
        private readonly TokenCreator _tokenCreator;
        private readonly IRoleRepository _roleRepository;

        public AccountService(IUserRepository userRepository, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository, IEntitiesInfoService entitiesInfoService, TokenCreator tokenCreator)
        {
            _userRepository = userRepository;
            _tokenCreator = tokenCreator;
            _userRoleRepository = userRoleRepository;
            _entitiesInfoService = entitiesInfoService;
            _roleRepository = roleRepository;
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


            var administratorRole = await _roleRepository.FirstOrDefaultAsync(r => r.RoleName.Equals("Administrador"));

            if (administratorRole == null)
            {
                administratorRole = new Role()
                {
                    RoleName = "Administrador",
                    EntityInfoId = entitiesInfo.Id
                };
                await _roleRepository.AddAsync(administratorRole);
            }


            var userRole = new Userrole()
            {
                RoleId = administratorRole.Id,
                UserId = userEntity.Id,
                EntityInfoId = entitiesInfo.Id
            };
            await _userRoleRepository.AddAsync(userRole);
            await _userRepository.SaveChangesAsync();


            return new AccountResponse()
            {
                Email = userDto.Email,
                Token = _tokenCreator.BuildToken(userEntity)
            };
        }

        private async Task<string> UploadProfilePhotoAsync(string photo, string userId)
        {
            string imagePath = $"{Environment.CurrentDirectory}/ProfilePhotos";
            string[] imageSplitted = photo.Split(',');
            byte[] imageInBytes = Convert.FromBase64String(imageSplitted[1]);

            string fileName = $"profile_photo_{userId.Trim('-')}.jpg";
            string path = Path.Combine(imagePath, fileName);


            using (var stream = new FileStream(path, FileMode.Create))
            {
                await stream.WriteAsync(imageInBytes, 0, imageInBytes.Length);
                await stream.FlushAsync();
            }

            return path;
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
