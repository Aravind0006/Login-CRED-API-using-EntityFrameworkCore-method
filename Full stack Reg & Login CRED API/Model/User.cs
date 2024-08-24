using System.ComponentModel.DataAnnotations;

namespace Full_stack_Reg___Login_CRED_API.Model
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; } // In a real app, store hashed passwords

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
    }

    public class Login
    {
        [Required]
        [MaxLength(100)]
        public string Password { get; set; } // In a real app, store hashed passwords

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
    }
}
