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
        Task<AccountResponse> RegisterUserAsync(UserDto userDto, int lenderBusinessId);
        Task<UserDto> UpdateUserAsync(UserDto user, int lenderBusinessId);
        Task<List<UserDto>> GetUsersAsync(int lenderBusinessId);
        Task<List<UserDto>> GetUsersByNameAsync(int lenderBusinessId, string userFullName);
        Task<UserDto> GetUserAsync(int userId);
        Task DeleteUserAsync(int userId);
    }
}
