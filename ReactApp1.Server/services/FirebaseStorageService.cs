using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ReactApp1.Server.services
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly string _bucketName = "linhtinh-f9308.appspot.com";
        private readonly StorageClient _storageClient;

        public FirebaseStorageService()
        {
            // Đường dẫn file JSON credential
            string credentialPath = @"C:\Users\ADMIN\source\repos\ReactApp1\ReactApp1.Server\firebase-service-account.json";

            // Tạo GoogleCredential từ file JSON
            var credential = GoogleCredential.FromFile(credentialPath);

            // Tạo StorageClient với credential truyền vào
            _storageClient = StorageClient.Create(credential);
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var objectName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            using var stream = file.OpenReadStream();
            await _storageClient.UploadObjectAsync(_bucketName, objectName, file.ContentType, stream);

            var url = $"https://firebasestorage.googleapis.com/v0/b/{_bucketName}/o/{Uri.EscapeDataString(objectName)}?alt=media";
            return url;
        }
    }
}
