using System.ComponentModel.DataAnnotations;

namespace StudentRegistration.Models
{
    public class tblcity
    {
        [Key]
        public int cid { get; set; }
        public int sid { get; set; }
        public string cname { get; set; }

    }
}
