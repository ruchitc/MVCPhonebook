using System.ComponentModel.DataAnnotations;

namespace MVCPhonebook.Models
{
    public class City
    {
        public int CityId { get; set; }

        [Required]
        [StringLength(20)]
        public string CityName { get; set; }

        [Required]
        public int StateId { get; set; }

        public State State { get; set; }
    }
}
