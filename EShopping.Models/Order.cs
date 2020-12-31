using EShopping.Models.Enum;
using System;
using System.Collections.Generic;

#nullable disable

namespace EShopping.Models
{
    public class Order
    {
        public Order()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public decimal ItemsQuantity { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int UserId { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
