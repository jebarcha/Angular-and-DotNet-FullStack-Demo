using EShopping.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EShopping.Dtos
{

    public class OrderDto
    {
        public OrderDto()
        {
            OrderDetail = new List<OrderDetailDto>();
        }

        public int Id { get; set; }
        public decimal ItemsQuantity { get; set; }
        public decimal Amount { get; set; }
        //[Required]
        public DateTime? DateAdded { get; set; }
        public int UserId { get; set; }
        public string User { get; set; }
        //public OrderStatus OrderStatus { get; set; }
        public List<OrderDetailDto> OrderDetail { get; set; }
    }
}
