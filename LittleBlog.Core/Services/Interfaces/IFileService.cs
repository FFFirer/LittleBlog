using LittleBlog.Core.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LittleBlog.Core.Services
{
    public interface IFileService
    {
        Task<UploadResult> SaveFileAsync(UploadInfo info, IFormFile file);
    }
}
