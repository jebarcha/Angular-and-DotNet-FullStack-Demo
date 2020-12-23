using EShopping.Models.Enum;
using System;
using System.Collections.Generic;

#nullable disable

namespace EShopping.Models
{
    public class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ProductStatus Status { get; set; }
        public DateTime DateAdded { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
