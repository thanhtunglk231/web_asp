using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Newtonsoft.Json;
using ReactApp1.Server.Configurations;
using ReactApp1.Server.Models;
using RestSharp;

namespace ReactApp1.Server.services
{
    public class SupabaseService : ISupabaseService
    {
        private readonly RestClient _client;

        public SupabaseService()
        {
            _client = new RestClient(SupabaseConfig.SupabaseUrl);
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            var request = new RestRequest("products", RestSharp.Method.Get);
            request.AddHeader("apikey", SupabaseConfig.SupabaseApiKey);
            request.AddHeader("Authorization", $"Bearer {SupabaseConfig.SupabaseApiKey}");
            request.AddHeader("Accept", "application/json");

            var response = await _client.ExecuteAsync(request);
            return JsonConvert.DeserializeObject<List<Product>>(response.Content);
        }

        public async Task<string?> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var fileName = $"products/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var uploadUrl = $"{SupabaseConfig.SupabaseUrl}/storage/v1/object/{SupabaseConfig.StorageBucket}/{fileName}";

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            var request = new RestRequest(uploadUrl, RestSharp.Method.Put);
            request.AddHeader("apikey", SupabaseConfig.SupabaseApiKey);
            request.AddHeader("Authorization", $"Bearer {SupabaseConfig.SupabaseApiKey}");
            request.AddHeader("Content-Type", file.ContentType);

            // Với RestSharp cũ
            request.AddParameter(file.ContentType, memoryStream.ToArray(), ParameterType.RequestBody);

            var response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var publicUrl = $"{SupabaseConfig.SupabaseUrl}/storage/v1/object/public/{SupabaseConfig.StorageBucket}/{fileName}";
                return publicUrl;
            }
            else
            {
                Console.WriteLine("Supabase Upload Error: " + response.Content);
                return null;
            }
        }


    }
}
