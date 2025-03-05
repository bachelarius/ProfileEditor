using ProfileEditor.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using ProfileEditor.Validation;

namespace ProfileEditor.Models {
    public record PersonVm {

        public Guid Id { get; set; } = new Guid();

        [MaxLength(50)]
        public required string FirstName { get; set; }

        [MaxLength(50)]
        public required string LastName { get; set; }

        [EmailAddress]
        public required string EmailAddress { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" })]
        [AllowedContentTypes(new string[] { "image/jpeg", "image/jpg", "image/png", "image/gif" })]
        [MaxFileSize(5 * 1024 * 1024)] // 5MB max
        public IFormFile? ProfilePicture { get; set; }

        public byte[]? ProfilePictureData { get; set; }

        public Person toEf() {
            return new Person {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                EmailAddress = EmailAddress,
                PhoneNumber = PhoneNumber,
                DateOfBirth = DateOfBirth ?? DateTime.MinValue,
                Gender = Gender
            };
        }

        public static PersonVm fromEf(Person person) {
            return new PersonVm {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                EmailAddress = person.EmailAddress,
                PhoneNumber = person.PhoneNumber,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender
            };
        }

        public Person UpdateEf(Person entity) {
            if (entity.Id != Id) {
                throw new InvalidOperationException($"Attempting to update user id {entity.Id} with content from user id {Id}");
            }

            entity.FirstName = FirstName;
            entity.LastName = LastName;
            entity.EmailAddress = EmailAddress;
            entity.PhoneNumber = PhoneNumber;
            entity.DateOfBirth = DateOfBirth ?? DateTime.MinValue;
            entity.Gender = Gender;
            
            return entity;
        }
    }
}
