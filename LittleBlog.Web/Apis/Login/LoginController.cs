using LittleBlog.Web.Areas.Identity.Data;
using LittleBlog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleBlog.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LittleBlog.Web.Apis.Login
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : BaseApiController
    {
        private readonly UserManager<LittleBlogIdentityUser> _userManager;
        private readonly SignInManager<LittleBlogIdentityUser> _signInManager;

        public LoginController(SignInManager<LittleBlogIdentityUser> signInManager,
            ILogger<LoginController> logger,
            UserManager<LittleBlogIdentityUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultModel> Post([FromBody] LoginViewModel loginViewModel)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: false);
                if(result.Succeeded)
                {
                    _logger.LogInformation("Admin logged in");
                    return Success();
                }
                if(result.IsLockedOut)
                {
                    return Fail(null, "Admin account locked out");
                }
                else
                {
                    return Fail(null, "Invalid login attempt");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Admin login failed");
                return Fail(ex, "Failed login attempt");
            }
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public async Task<ResultModel> Get()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Logout error");
                return Fail();
            }
        }
    }
}
