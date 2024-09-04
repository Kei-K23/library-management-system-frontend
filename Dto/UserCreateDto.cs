using System.ComponentModel.DataAnnotations;
using LibraryManagementSystemApp.Enum;

namespace LibraryManagementSystemApp.Dto
{
    public class UserCreateDto
    {
        [StringLength(100, MinimumLength = 3)]
        [Required]
        public string? UserName { get; set; }

        [StringLength(80, MinimumLength = 3)]
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(255, MinimumLength = 8)]
        [Required]
        public string? Password { get; set; }

        public string? ProfilePicture { get; set; }
        [Required]
        public UserRole UserRole { get; set; }
    }
}