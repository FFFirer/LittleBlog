using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Constants
{
    public static class ArticleOperationRequirements
    {
        public static OperationAuthorizationRequirement Details
            = new OperationAuthorizationRequirement { Name = ArticleOperationNames.DetailsOperationName };
    }

    public static class ArticleOperationNames
    {
        public static readonly string DetailsOperationName = "Details";
    }
}
