using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MeManga.Core.Common.Constants;
using System;
using System.Net;
using System.Linq;
using MeManga.Core.Common.Helpers;
using System.Collections.Generic;

namespace MeManga.Core.Business.Filters
{
    /// <summary>
    /// 
    /// </summary>
	public class CustomAuthorizeAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Override OnActionExecuting to check Access Token and Role
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var accessToken = context.HttpContext.Request.Headers["x-access-token"].ToString();
            var jwtPayload = JwtHelper.ValidateToken(accessToken);

            if (jwtPayload == null)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new JsonResult(MessageConstants.INVALID_ACCESS_TOKEN);
            }
            else if (Role != null )
            {
                bool isUserInRole = IsUserInRole(Role, jwtPayload.RoleId);
                if (!isUserInRole)
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Result = new JsonResult("Unauthorized request");
                }
            }
            base.OnActionExecuting(context);
        }

        private bool IsUserInRole(string allowRoleId, Guid currentRoleId)
        {
            if (currentRoleId == null)
            {
                return false;
            }

            //foreach (var role in allowRoleId)
            //{
            //    var roleId = Guid.Parse(role);
            //    return currentRoleId.Any(x => x == roleId);
            //}
            return false;
        }
    }
}
