using System.ComponentModel;

namespace PhonebookClient.ViewModels
{
    public class CountryViewModel
    {
        [DisplayName("Country id")]
        public int CountryId { get; set; }
        
        [DisplayName("Country name")]
        public string CountryName { get; set; }
    }
}
