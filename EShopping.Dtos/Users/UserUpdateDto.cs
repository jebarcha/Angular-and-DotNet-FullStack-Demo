using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EShopping.Dtos
{
    public class UserUpdateDto
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
    }
}
