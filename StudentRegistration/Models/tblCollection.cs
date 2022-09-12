using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentRegistration.Models
{
    public class tblCollection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string fName { get; set; }
        public string Category { get; set; }
        public string regno { get; set; }
        [Range(15, 60, ErrorMessage = "The age field is required(15 to 60)")]
        public int Age { get; set; }
        public int Gender { get; set; }
        public int State { get; set; }
        public int City { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string ProfileImage { get; set; }
        public List<tblstate> tblstates { get; set; }
        public List<tblcity> tblcities { get; set; }
        public List<tblgender> tblgenders { get; set; }



    }
}
