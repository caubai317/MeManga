using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MeManga.Core.Common.Helpers;
using System;

namespace MeManga.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid CurrentUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BaseController()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionExecutingContext"></param>
        public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
        {
            var accessToken = actionExecutingContext.HttpContext.Request.Headers["x-access-token"];
            if (!string.IsNullOrEmpty(accessToken))
            {
                var jwtPayload = JwtHelper.ValidateToken(accessToken.ToString());
                if (jwtPayload != null)
                {
                    CurrentUserId = jwtPayload.UserId;
                }
            }

            base.OnActionExecuting(actionExecutingContext);
        }
    }
}
