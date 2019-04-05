using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models
{
    public class Mail
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Sender { get; set; }

        [Required]
        [RegularExpression(@"^[0-9A-Za-z ]*str.$")]
        public string Address { get; set; }

        [ForeignKey("Prisoner")]
        public int PrisonerId { get; set; }
        public Prisoner Prisoner { get; set; }


        //        •	Id – integer, Primary Key
        //•	Description– text(required)
        //•	Sender – text(required)
        //•	Address – text, consisting only of letters, spaces and numbers, which ends with “ str.” (required) (Example: “62 Muir Hill str.“)
        //•	PrisonerId - integer, foreign key
        //•	Prisoner – the mail's Prisoner (required)

    }
}