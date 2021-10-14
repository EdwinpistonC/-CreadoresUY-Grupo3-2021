using System.ComponentModel.DataAnnotations;

namespace Share.Dtos
{
    public class CreateUserDto
    {
        [Required]
        [StringLength(25)]
        public string Name { get; set; }

        [Required]
        [StringLength(40)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "La Password debe contener al menos 8 caracteres")]
        public string Password { get; set; }
    }

}
