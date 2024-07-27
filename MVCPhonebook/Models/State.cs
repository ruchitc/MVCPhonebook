using System.ComponentModel.DataAnnotations;

namespace MVCPhonebook.Models
{
    public class State
    {
        public int StateId { get; set; }

        [Required]
        [StringLength(20)]
        public string StateName {  get; set; }

        public ICollection<City> Cities { get; set; }
    }
}
