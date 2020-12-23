using System.ComponentModel.DataAnnotations;

namespace EShopping.Dtos
{

    public class UserDto
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Name of the User")]
        public string Name { get; set; }
        [Display(Name = "LastName of the User")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Account")]
        public string Username { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Profile of the User")]
        public int PerfilId { get; set; }
        public string Profile { get; set; }
    }

}
