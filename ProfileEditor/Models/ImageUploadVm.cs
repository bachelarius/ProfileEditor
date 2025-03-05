using ProfileEditor.Validation;
using System.ComponentModel.DataAnnotations;

namespace ProfileEditor.Models {
    public class ImageUploadVm {
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" })]
        [AllowedContentTypes(new string[] { "image/jpeg", "image/jpg", "image/png", "image/gif" })]
        [MaxFileSize(5 * 1024 * 1024)] // 5MB max
        [Required]
        public IFormFile? Image { get; set; }
    }
}
