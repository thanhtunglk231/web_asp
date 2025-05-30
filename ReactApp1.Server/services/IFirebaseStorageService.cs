
namespace ReactApp1.Server.services
{
    public interface IFirebaseStorageService
    {
        Task<string> UploadFileAsync(IFormFile file);
    }
}