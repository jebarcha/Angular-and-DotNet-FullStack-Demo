using System.ComponentModel.DataAnnotations;

namespace EShopping.Dtos
{

    public class ProfileDto
    {
        public int Id { get; set; }
        [Display(Name = "Profile")]
        [Required(ErrorMessage = "The name of the profile is required")]
        public string Name { get; set; }
    }


}
