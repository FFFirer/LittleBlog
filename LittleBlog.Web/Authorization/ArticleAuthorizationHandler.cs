using LittleBlog.Web.Constants;
using LittleBlog.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Authorization
{
    public class ArticleAuthorizationHandler
        : AuthorizationHandler<OperationAuthorizationRequirement, Article>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Article resource)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            if (requirement.Name != ArticleOperationNames.DetailsOperationName)
            {
                return Task.CompletedTask;
            }

            // 已发布的所有角色都可以查看
            if (resource.IsPublished)
            {
                context.Succeed(requirement);
            }

            // 未发布的只有[Administrator]可以查看
            if (context.User.IsInRole(RoleNames.AdministratorRoleName) && !resource.IsPublished)
            {
                context.Succeed(requirement);
            }


            return Task.CompletedTask;
        }
    }
}
