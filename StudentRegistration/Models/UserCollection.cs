using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentRegistration.Models
{
    public class UserCollection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Range(15, 60, ErrorMessage = "The age field is required(15 to 60)")]
        public int Age { get; set; }
        public int Gender { get; set; }
        public string Mobile { get; set; }
        [EmailAddress]
        [Remote(action: "UserNameValidate",controller:"UserLogin")]
        public string Username { get; set; }
        public List<tblgender> tblgenders { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

    }
}
