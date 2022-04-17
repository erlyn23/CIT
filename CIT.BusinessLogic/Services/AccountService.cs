using CIT.BusinessLogic.Contracts;
using CIT.DataAccess.Contracts;
using CIT.DataAccess.Models;
using CIT.Dtos.Requests;
using CIT.Dtos.Responses;
using CIT.Tools;
using Newtonsoft.Json;
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
        private readonly TokenCreator _tokenCreator;
        private readonly ILoginRepository _loginRepository;
        private readonly ILenderBusinessRepository _lenderBusinessRepository;
        private readonly AccountTools _accountTools;

        public AccountService(IUserRepository userRepository, TokenCreator tokenCreator, ILoginRepository loginRepository, ILenderBusinessRepository lenderBusinessRepository, AccountTools accountTools)
        {
            _userRepository = userRepository;
            _tokenCreator = tokenCreator;
            _loginRepository = loginRepository;
            _lenderBusinessRepository = lenderBusinessRepository;
            _accountTools = accountTools;
        }

        public async Task<AccountResponse> SignInAsync(string email, string password)
        {
            var encryptedPassword = Encryption.Encrypt(password);
            var login = await _loginRepository.FirstOrDefaultAsync(l => l.Email.Equals(email) && l.Password.Equals(encryptedPassword));


            if (login == null)
                throw new Exception("Usuario o contraseña incorrecta");
            if (login.Status == 0)
                throw new Exception("Usuario inactivo, por favor, verifique su correo electrónico y active su cuenta");



            var lenderBusiness = await _lenderBusinessRepository.FirstOrDefaultWithRelationsAsync(l => l.Email.Equals(email) && l.Password.Equals(encryptedPassword));

            if(lenderBusiness != null)
                return new AccountResponse() { Email = email, Token = _tokenCreator.BuildToken(lenderBusiness) };
            else
            {
               var user = await _userRepository.FirstOrDefaultWithRelationsAsync(u => u.Email.Equals(email) && u.Password.Equals(encryptedPassword));
                return new AccountResponse() { Email = email, Token = _tokenCreator.BuildToken(user) };
            }

        }

        public async Task<bool> ActivateAccountAsync(string email, string verificationNumber)
        {
            var user = await _loginRepository.FirstOrDefaultAsync(u => u.Email.Equals(email));

            var confirmationDataSaved = _accountTools.GetConfirmationDataFromJsonFile();

            var confirmationDataWanted = confirmationDataSaved.Where(e => e.UserEmail.Equals(user.Email)).FirstOrDefault();

            DateTime now = DateTime.UtcNow;
            TimeSpan difference = confirmationDataWanted.ExpireDate - now;
            int minutes = difference.Minutes;

            if (minutes <= 30)
            {
                if (string.Equals(verificationNumber, confirmationDataWanted.RandomCode))
                {
                    user.Status = 1;
                    _loginRepository.Update(user);
                    await _loginRepository.SaveChangesAsync();
                    int toDelete = confirmationDataSaved.IndexOf(confirmationDataWanted);
                    confirmationDataSaved.RemoveAt(toDelete);
                    string jsonString = JsonConvert.SerializeObject(confirmationDataSaved);
                    File.WriteAllText(_accountTools.FilePath, jsonString);
                    return true;
                }
                else
                {
                    throw new Exception("Este usuario no existe o no se ha registrado");
                }
            }
            else
            {
                throw new Exception("Este código ya expiró, reenvía la verificación");
            }
        }
    }
}
