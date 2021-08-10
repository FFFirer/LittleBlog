using LittleBlog.Web.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LittleBlog.Web.Services
{
    public interface IFileService
    {
        Task<UploadResult> SaveFileAsync(UploadInfo info, IFormFile file);
    }
}
