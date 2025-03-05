using ProfileEditor.Models;

namespace ProfileEditor.Services.ImageStore {
    public interface IImageStoreService {
        public Task UploadImage(ProfileImage image);
        public Task DeleteImage(Guid PersonId);
        public Task<ProfileImage?> FetchImage(Guid PersonId);
    }
}
