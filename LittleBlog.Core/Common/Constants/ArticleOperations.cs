using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace LittleBlog.Core.Common
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
