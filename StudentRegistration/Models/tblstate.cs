using System.ComponentModel.DataAnnotations;

namespace StudentRegistration.Models
{
    public class tblstate
    {
        [Key]
        public int sid { get; set; }
        public string sname { get; set; }
    }
}
