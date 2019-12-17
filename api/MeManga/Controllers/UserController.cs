using Microsoft.AspNetCore.Mvc;
using MeManga.Core.Business.Services;
using System;
using Microsoft.AspNetCore.Cors;
using MeManga.Core.Business.Models.Users;
using MeManga.Core.Business.Filters;
using System.Threading.Tasks;
using MeManga.Core.Common.Constants;
using MeManga.Core.Business.Models.Base;

namespace MeManga.Controllers
{
    [Route("api/users")]
    [EnableCors("CorsPolicy")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public UserController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(UserRequestListViewModel userRequestListViewModel)
        {
            var users = await _userService.ListUserAsync(userRequestListViewModel);
            return Ok(users);
        }

        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUser(BaseRequestGetAllViewModel baseRequestGetAllViewModel)
        {
            var users = await _userService.GetAllUserAsync(baseRequestGetAllViewModel);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var responseModel = await _userService.GetUserByIdAsync(id);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel);
            }
            else
            {
                return NotFound(new { message = responseModel.Message });
            }
        }

        [HttpPut("{id}")]
        [CustomAuthorize(Role = RoleConstants.ADMIN )]
        public async Task<IActionResult> Put(Guid id, [FromBody] UserUpdateProfileModel userUpdateProfileModel)
        {
            var responseModel = await _userService.UpdateProfileAsync(id, userUpdateProfileModel);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound("User không tồn tại trong hệ thống. Vui lòng kiểm tra lại!");
            }
            else
            {
                if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return Ok(responseModel.Data);
                }
                else
                {
                    return BadRequest(new { Message = responseModel.Message });
                }
            }
        }

        [HttpDelete("{id}")]
        [CustomAuthorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var responseModel = await _userService.DeleteUserAsync(id);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel.Data);
            }
            else
            {
                return BadRequest(new { Message = responseModel.Message });
            }
        }

        #region Other Methods

        [HttpGet("check-existing-email")]
        public async Task<IActionResult> ValidateExistEmail([FromBody] string email)
        {
            var user = await _userService.GetByEmailAsync(email);
            return Ok(user != null);
        }

        #endregion
    }
}
