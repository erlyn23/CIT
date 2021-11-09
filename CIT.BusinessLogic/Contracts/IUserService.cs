using CIT.Dtos.Requests;
using CIT.Dtos.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Contracts
{
    public interface IUserService
    {
        Task<AccountResponse> RegisterUserAsync(UserDto userDto);
        Task<List<UserDto>> GetUsersAsync(int lenderBusinessId);
        Task<UserDto> GetUserAsync(int userId);
        Task DeleteUserAsync(int userId);
    }
}
