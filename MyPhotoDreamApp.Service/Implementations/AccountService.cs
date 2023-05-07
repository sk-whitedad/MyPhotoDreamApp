using Microsoft.EntityFrameworkCore;
using MyPhotoDreamApp.DAL.Interfaces;
using MyPhotoDreamApp.DAL.Repositories;
using MyPhotoDreamApp.Domain.Entity;
using MyPhotoDreamApp.Domain.Enum;
using MyPhotoDreamApp.Domain.Helpers;
using MyPhotoDreamApp.Domain.Response;
using MyPhotoDreamApp.Domain.ViewModels.Account;
using MyPhotoDreamApp.Service.Interfaces;
using System.Security.Claims;

namespace MyPhotoDreamApp.Service.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Basket> _basketRepository;

        public AccountService(IBaseRepository<User> userRepository, IBaseRepository<Basket> basketRepository)
        {
            _userRepository = userRepository;
            _basketRepository = basketRepository;
        }

        public async Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.PhoneNumber == model.PhoneNumber);
                if (user == null)
                {
                    return new BaseResponse<ClaimsIdentity>
                    {
                        Description = "Пользователь не найден"
                    };
                }

                if (user.Password != HashPasswordHelper.HashPassowrd(model.Password))
                {
                    return new BaseResponse<ClaimsIdentity>
                    {
                        Description = "Не верный пароль"
                    };
                }

                var result = Authenticate(user);
                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.PhoneNumber == model.PhoneNumber);
                if (user != null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Пользователь с таким номером телефона уже есть",
                    };
                }

                user = new User()
                {
                    PhoneNumber = model.PhoneNumber,
                    Role = Role.User,
                    Password = HashPasswordHelper.HashPassowrd(model.Password),
                };

                await _userRepository.Create(user);

                var basket = new Basket()//создаем корзину для нового юзера
                {
                    UserId = user.Id
                };
                await _basketRepository.Create(basket);

                var result = Authenticate(user);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    Description = "Объект добавился",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        private ClaimsIdentity Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.PhoneNumber),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
            };
            return new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
    }
}
