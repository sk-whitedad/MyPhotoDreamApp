using Microsoft.EntityFrameworkCore;
using MyPhotoDreamApp.DAL.Interfaces;
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

        public async Task<IBaseResponse<List<User>>> GetUsers()
        {
            try
            {
                var users = _userRepository.GetAll().ToList();
                if (users == null)
                {
                    return new BaseResponse<List<User>>()
                    {
                        Description = "Клиенты не найдены",
                        StatusCode = StatusCode.UserNotFound
                    };
                }
                return new BaseResponse<List<User>>()
                {
                    Data = users,
                    Description = "Клиенты найдены",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<User>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<User>> GetUser(int id)
        {
            try
            {
                var users = _userRepository.GetAll();
                if (users == null)
                {
                    return new BaseResponse<User>()
                    {
                        Description = "Клиенты не найдены",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var user = users.FirstOrDefault(x => x.Id == id);
                return new BaseResponse<User>()
                {
                    Data = user,
                    Description = "Клиент найден",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<User>> EditUser(UserViewModel model)
        {
            try
            {
                var userDuble = _userRepository.GetAll().FirstOrDefault(x => x.PhoneNumber == model.PhoneNumber && x.Id != model.Id);
                if (userDuble != null)
                {
                    return new BaseResponse<User>()
                    {
                        Description = "Пользователь с таким номером телефона уже есть",
                    };
                }
                var _user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == model.Id);
                if (_user == null)
                {
                    return new BaseResponse<User>()
                    {
                        Description = "Клиент не найдены",
                        StatusCode = StatusCode.UserNotFound
                    };
                }
                User user = new User();
                if (model.Password != null)
                {
                    user.Id = model.Id;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Role = model.Role;
                    user.Password = HashPasswordHelper.HashPassowrd(model.Password);
                }
                else
                {
                    user.Id = model.Id;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Role = model.Role;
                }
                await _userRepository.Update(user);
                return new BaseResponse<User>()
                {
                    Data = user,
                    Description = "Клиент изменен",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }


    }
}
