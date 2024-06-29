using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MatchBall.Utilities;
using Microsoft.AspNetCore.Http;
using System;

namespace MatchBall.Filter
{
    public class JWTAuth : IActionFilter
    {
        private readonly IJWTUtil JwtUtil;
        private readonly IHttpContextAccessor HttpContextAccessor;
        private readonly IConfiguration Configuration;
        private readonly int ExpiryJWT;
        public JWTAuth(IJWTUtil JwtUtil, IHttpContextAccessor HttpContextAccessor, IConfiguration Configuration)
        {
            this.JwtUtil = JwtUtil;
            this.HttpContextAccessor = HttpContextAccessor;
            this.Configuration = Configuration;
            ExpiryJWT = int.Parse(Configuration["JWT:ExpiryJWT"]);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = HttpContextAccessor.HttpContext;
            var jwtToken = httpContext.Request.Cookies["Token"];
            var refreshToken = httpContext.Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(jwtToken))
            {
                RedirectToLogin(context);
                return;
            }

            if (JwtUtil.ValidateToken(jwtToken))
            {
                return;
            }

            if (!string.IsNullOrEmpty(refreshToken) && JwtUtil.ValidateRefreshToken(jwtToken, refreshToken))
            {
                var username = JwtUtil.FetchUsername().message;
                var newJwtResult = JwtUtil.GenerateToken(username, ExpiryJWT); 

                if (newJwtResult.Flag)
                {
                    httpContext.Response.Cookies.Append("JWTToken", newJwtResult.message);

                    return;
                }
            }

            RedirectToLogin(context);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        private void RedirectToLogin(ActionExecutingContext context)
        {
            context.Result = new RedirectToActionResult("LoginPage", "Login", null);
        }
    }
}