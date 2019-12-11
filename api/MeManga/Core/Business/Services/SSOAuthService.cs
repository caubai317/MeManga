using MeManga.Core.Business.Models.Users;
using MeManga.Core.Common.Utilities;
using MeManga.Core.DataAccess.Repositories.Base;
using MeManga.Core.Entities;
using System.Linq;
using System.Threading.Tasks;
using MeManga.Core.Common.Helpers;

namespace MeManga.Core.Business.Services
{
    public interface ISSOAuthService
    {
        Task<ResponseModel> LoginAsync(UserLoginModel userLoginModel);

        ResponseModel VerifyTokenAsync(string token);
    }

    public class SSOAuthService : ISSOAuthService
    {
        private readonly IUserService _userService;

        public SSOAuthService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ResponseModel> LoginAsync(UserLoginModel userLoginModel)
        {
            var user = await _userService.GetByEmailAsync(userLoginModel.Email);
            if (user != null)
            {
                var result = PasswordUtilities.ValidatePass(user.Password, userLoginModel.Password, user.PasswordSalt);
                if (result)
                {
                    var jwtPayload = new JwtPayload()
                    {
                        UserId = user.Id,
                        Mobile = user.Mobile,
                        Name = user.Name,
                        RoleId = user.RoleId 
                    };

                    var token = JwtHelper.GenerateToken(jwtPayload);

                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Data = token
                    };
                }
                else
                {
                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Số điện thoại hoặc mật khẩu không đúng. Vui lòng thử lại!"// TODO: multi language
                    };
                }
            }
            else
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Số điện thoại chưa được đăng kí!"// TODO: multi language
                };
            }
        }

        public ResponseModel VerifyTokenAsync(string token)
        {
            var jwtPayload = JwtHelper.ValidateToken(token);

            if (jwtPayload == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Unauthorized request"
                };
            }
            else
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = jwtPayload
                };
            }
        }
    }
}
