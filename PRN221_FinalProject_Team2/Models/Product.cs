using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PRN221_FinalProject_Team2.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }
        [Required(ErrorMessage = "Product name cannot be empty")]
        public string ProductName { get; set; } = null!;
        public int? CategoryId { get; set; }
        [Required(ErrorMessage = "Quantity for each unit cannot be empty")]
        public string? QuantityPerUnit { get; set; }
        [Required(ErrorMessage = "Price cannot be empty")]
        [Range(0, int.MaxValue, ErrorMessage = "Price cannot be below zero")]
        public decimal? UnitPrice { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be below zero")]
        public short? UnitsInStock { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Order cannot be below zero")]
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }

        public string? ImageUrl { get; set; } // Add this line

        public virtual Category? Category { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
