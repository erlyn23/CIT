using AutoMapper;
using CIT.BusinessLogic.Contracts;
using CIT.DataAccess.Contracts;
using CIT.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IMapper _mapper;

        public LoginService(ILoginRepository loginRepository, IMapper mapper)
        {
            _loginRepository = loginRepository;
            _mapper = mapper;
        }

        public async Task DeleteLoginAsync(string email)
        {
            var login = await _loginRepository.FirstOrDefaultAsync(l => l.Email.Equals(email));
            login.Status = 0;

            if(login != null)
                _loginRepository.Update(login);

            await _loginRepository.SaveChangesAsync();
        }

        public async Task<Login> SaveLoginAsync(string email, string password)
        {
            var login = new Login()
            {
                Email = email,
                Password = password,
                Status = 0
            };

            var savedLogin = await _loginRepository.AddAsync(login);
            await _loginRepository.SaveChangesAsync();

            return savedLogin;
        }
    }
}
