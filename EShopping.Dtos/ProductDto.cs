using System.ComponentModel.DataAnnotations;

namespace EShopping.Dtos
{

    public class ProductDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The name of the product is required")]
        [Display(Name = "Product")]
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

}
