using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentRegistration.Models
{
    public class RegForm
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string fName { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string regno { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public int Gender { get; set; }
        [Required]
        public int State { get; set; }
        [Required]
        public int City { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [DisplayName("Product Photo")]
        public string ProfileImage { get; set; }
    }
}
