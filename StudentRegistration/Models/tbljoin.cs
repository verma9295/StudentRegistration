using System.ComponentModel;

namespace StudentRegistration.Models
{
    public class tbljoin
    {
        public int Id { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("S/O")]
        public string fName { get; set; }
        public string Category { get; set; }
        [DisplayName("Registration")]
        public string regno { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        [DisplayName("Student photo")]
        public string ProfileImage { get; set; }

    }
}
