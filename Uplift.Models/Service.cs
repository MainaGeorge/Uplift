using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uplift.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required, Display(Name = "Service Name")]
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }

        [Display(Name = "Description")]
        public string LongDescription { get; set; }

        [DataType(dataType: DataType.ImageUrl)]
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Required, Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required, Display(Name = "Frequency")]
        public int FrequencyId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [ForeignKey("FrequencyId")]
        public Frequency Frequency { get; set; }

    }
}
