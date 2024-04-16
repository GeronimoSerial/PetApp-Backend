using System.ComponentModel.DataAnnotations;

namespace BusinessAccessLayer.Dto
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
     
    }
}
