using System.ComponentModel.DataAnnotations;

namespace BusinessAccessLayer.Dto
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime? Birthday { get; set; }
        //public string ConfirmPassword { get; set; }
    }
}
