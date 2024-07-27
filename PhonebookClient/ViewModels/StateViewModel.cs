using System.ComponentModel;

namespace PhonebookClient.ViewModels
{
    public class StateViewModel
    {
        [DisplayName("State id")]
        public int StateId { get; set; }
        
        [DisplayName("State name")]
        public string StateName { get; set; }
        
        [DisplayName("Country id")]
        public int CountryId { get; set; }
    }
}
