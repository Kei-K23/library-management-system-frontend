using System.ComponentModel.DataAnnotations;
using LibraryManagementSystemApp.Enum;

namespace LibraryManagementSystemApp.Dto
{
    public class UserUpdateDto
    {
        [StringLength(100, MinimumLength = 3)]
        public string? UserName { get; set; }

        [StringLength(80, MinimumLength = 3)]
        [EmailAddress]
        public string? Email { get; set; }

        public string? ProfilePicture { get; set; }
        [Required]
        public UserRole UserRole { get; set; }
    }
}