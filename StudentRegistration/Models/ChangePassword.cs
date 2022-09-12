using System.ComponentModel.DataAnnotations;

namespace StudentRegistration.Models
{
    public class ChangePassword
    {
        [Key]
        public int Id { get; set; }
        public string OldPass { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPass { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPass")]
        public string ConfirmPass { get; set; }
    }
}
