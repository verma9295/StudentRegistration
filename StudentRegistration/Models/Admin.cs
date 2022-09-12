using System.ComponentModel.DataAnnotations;

namespace StudentRegistration.Models
{
    public class Admin
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
