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
        public decimal CantidadArticulos { get; set; }
        public decimal Importe { get; set; }
        [Required]
        public DateTime FechaRegistro { get; set; }
        public int UsuarioId { get; set; }
        public string Usuario { get; set; }
        public OrderStatus EstatusOrden { get; set; }
        public List<OrderDetailDto> OrderDetail { get; set; }
    }
}
