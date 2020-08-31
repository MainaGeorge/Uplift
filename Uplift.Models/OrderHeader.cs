using System;
using System.ComponentModel.DataAnnotations;

namespace Uplift.Models
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } 
        
        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string ZipCode { get; set; }

        public DateTime DateOrdered { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }    
    }
}
