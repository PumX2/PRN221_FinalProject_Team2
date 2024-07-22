using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PRN221_FinalProject_Team2.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public string? CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? OrderDate { get; set; }
        [Required(ErrorMessage ="Must input date")]
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal? Freight { get; set; }
        [Required(ErrorMessage = "Must input ")]
        public string? ShipName { get; set; }
        [Required(ErrorMessage = "Must input ")]
        public string? ShipAddress { get; set; }
        [Required(ErrorMessage = "Must input ")]
        public string? ShipCity { get; set; }
        public string? ShipRegion { get; set; }
        public string? ShipPostalCode { get; set; }
        [Required(ErrorMessage = "Must input")]
        public string? ShipCountry { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Employee? Employee { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
