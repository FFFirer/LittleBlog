﻿using LittleBlog.Core.Models;
using LittleBlog.Web.Areas.Identity.Data;
using LittleBlog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LittleBlog.Web.Apis.Login
{
    [Route("api/user")]
    [ApiController]
    [AllowAnonymous]
    public class ApiUserController : BaseApiController
    {
        private readonly UserManager<LittleBlogIdentityUser> _userManager;
        private readonly SignInManager<LittleBlogIdentityUser> _signInManager;

        public ApiUserController(SignInManager<LittleBlogIdentityUser> signInManager,
            ILogger<ApiUserController> logger,
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
        [HttpPost("login")]
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
        /// <returns></returns
        [HttpGet("logout")]
        public async Task<ResultModel> Logout()
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
