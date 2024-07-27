using System.ComponentModel.DataAnnotations;

namespace MVCPhonebook.Models
{
    public class Contact
    {
        public int ContactId { get; set; }

        [Required]
        [StringLength(15)]
        public string FirstName {  get; set; }

        [Required]
        [StringLength(15)]
        public string LastName { get; set; }

        [Required]
        [StringLength(15)]
        public string ContactNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(15)]
        public string Company { get; set; }

        public string FileName { get; set; }
    }
}
