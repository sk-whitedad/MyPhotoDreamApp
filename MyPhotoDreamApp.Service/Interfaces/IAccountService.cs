using MyPhotoDreamApp.Domain.Entity;
using MyPhotoDreamApp.Domain.Response;
using MyPhotoDreamApp.Domain.ViewModels.Account;
using System.Security.Claims;

namespace MyPhotoDreamApp.Service.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model);

        Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model);

        Task<IBaseResponse<List<User>>> GetUsers();

        Task<IBaseResponse<User>> GetUser(int id);

        Task<IBaseResponse<User>> EditUser(int id, UserViewModel model);

    }
}