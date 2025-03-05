using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using ProfileEditor.Models;

namespace ProfileEditor.Services.ImageStore
{
    public class AzureBlobImageStoreService : IImageStoreService
    {
        private readonly BlobContainerClient _containerClient;
        private const string ContainerName = "profile-images";
        private ILogger<AzureBlobImageStoreService> _logger;

        public AzureBlobImageStoreService(IOptions<AzureBlobImageStoreServiceConfig> config, ILogger<AzureBlobImageStoreService> logger)
        {
            _logger = logger;
            var blobServiceClient = new BlobServiceClient(config.Value.ConnectionString);
            _containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);
            _containerClient.CreateIfNotExists();
        }

        public async Task UploadImage(ProfileImage image)
        {
            _logger.LogDebug($"Uploading profile image for user {image.PersonId}, size: {image.ImageData.Length} bytes");
            var blobClient = _containerClient.GetBlobClient(GetBlobName(image.PersonId));
            
            using var stream = new MemoryStream(image.ImageData);
            await blobClient.UploadAsync(stream, new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders {
                    ContentType = image.ContentType
                }
            });
            _logger.LogInformation($"Uploaded profile image for user {image.PersonId}, size: {image.ImageData.Length} bytes");
        }

        public async Task DeleteImage(Guid personId)
        {
            _logger.LogDebug($"Deleting profile image for user {personId}");
            var blobClient = _containerClient.GetBlobClient(GetBlobName(personId));
            await blobClient.DeleteIfExistsAsync();
            _logger.LogInformation($"Deleted profile image for user {personId}");
        }

        public async Task<ProfileImage?> FetchImage(Guid personId)
        {
            _logger.LogTrace($"Fetching profile image for user {personId}");
            var blobClient = _containerClient.GetBlobClient(GetBlobName(personId));
            
            try
            {
                var response = await blobClient.DownloadContentAsync();
                var properties = await blobClient.GetPropertiesAsync();
                var profileImage = new ProfileImage(
                    PersonId: personId,
                    ImageData: response.Value.Content.ToArray(),
                    ContentType: properties.Value.ContentType);
                
                _logger.LogTrace($"Fetched profile image for user {personId}, size: {profileImage.ImageData.Length} bytes");
                return profileImage;
                
            }
            catch (Azure.RequestFailedException)
            {
                _logger.LogTrace($"Could not find image for user {personId}");
                return null;
            }
        }

        private static string GetBlobName(Guid personId) => $"{personId}.img";
    }
} 