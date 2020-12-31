using System.ComponentModel.DataAnnotations;

namespace EShopping.Dtos
{
    public class UserRegisterDto
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Name of the user")]
        public string Name { get; set; }
        [Display(Name = "Lastname of the user")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Profile of the user")]
        public int ProfileId { get; set; }
        public string Profile { get; set; }
    }

}
