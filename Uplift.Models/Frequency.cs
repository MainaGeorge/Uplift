using System.ComponentModel.DataAnnotations;

namespace Uplift.Models
{
    public class Frequency
    {
        [Key]
        public int Id { get; set; }


        [Required, Display(Name = "Frequency Count")]
        public int FrequencyCount { get; set; }


        [Required]
        public string Name { get; set; }
    }
}
