using LittleBlog.Web.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Areas.Identity.Data
{
    public static class SeedData
    {
        public static async Task Initialze(IServiceProvider serviceProvider, string userPwd, string userName)
        {
            var AdminId = await EnsureUser(serviceProvider, userPwd, userName);
            await EnsureRole(serviceProvider, AdminId, RoleNames.AdministratorRoleName);
        }

        /// <summary>
        /// 确认用户
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="userPwd"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        private static async Task<string> EnsureUser(IServiceProvider serviceProvider, string userPwd, string userName)
        {
            var userManager = serviceProvider.GetService<UserManager<LittleBlogIdentityUser>>();

            var user = await userManager.FindByNameAsync(userName);

            if(user == null)
            {
                user = new LittleBlogIdentityUser
                {
                    UserName = userName,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, userPwd);
            }

            if(user == null)
            {
                throw new Exception("管理员密码强度不够！");
            }

            return user.Id;
        }


        /// <summary>
        /// 确认角色
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider, string userId, string role)
        {
            IdentityResult IR = null;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if(roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            if(!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<LittleBlogIdentityUser>>();

            var user = await userManager.FindByIdAsync(userId);

            if(user == null)
            {
                throw new Exception("用户密码强度不够！");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }
    }
}
