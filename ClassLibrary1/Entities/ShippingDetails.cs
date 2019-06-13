using System.ComponentModel.DataAnnotations;

namespace TechRent.Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Second name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "EMail")]
        public string EMail { get; set; }


    }
}