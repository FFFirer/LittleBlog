using LittleBlog.Core.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LittleBlog.Core.Services
{
    public interface IFileService
    {
        Task<UploadResult> SaveAsync(UploadInfo info, IFormFile file);
    }
}
