using System;
using System.ComponentModel.DataAnnotations;

namespace ProfileEditor.Data
{
    public record Person
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public required string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public required string LastName { get; set; }

        [Required]
        [EmailAddress]
        public required string EmailAddress { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }
    }
}
