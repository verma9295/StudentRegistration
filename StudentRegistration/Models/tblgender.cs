using System.ComponentModel.DataAnnotations;

namespace StudentRegistration.Models
{
    public class tblgender
    {
        [Key]
        public int gid { get; set; }
        public string gname { get; set; }

    }
}
