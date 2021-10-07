using CIT.BusinessLogic.Contracts;
using CIT.DataAccess.Contracts;
using CIT.Dtos.Responses;
using CIT.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenCreator _tokenCreator;

        public AccountService(IUserRepository userRepository, TokenCreator tokenCreator)
        {
            _userRepository = userRepository;
            _tokenCreator = tokenCreator;
        }
        public async Task<AccountResponse> SignInAsync(string email, string password)
        {
            var encryptedPassword = Encryption.Encrypt(password);
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Email.Equals(email) && u.Password.Equals(encryptedPassword));

            if (user != null)
                return new AccountResponse() { Email = email, Token = _tokenCreator.BuildToken(user) };

            throw new Exception("Usuario o contraseña incorrecta");
        }
    }
}
