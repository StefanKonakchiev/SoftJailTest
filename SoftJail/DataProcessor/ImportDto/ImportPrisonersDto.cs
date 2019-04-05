using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportPrisonersDto
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^The\s[A-Z][a-z]*$")]
        public string Nickname { get; set; }

        [Required]
        [Range(typeof(int), "18", "65")]
        public int Age { get; set; }


        public DateTime DateTime { get; set; }

        [Required]
        public string IncarcerationDate
        {
            get { return this.DateTime.ToString("dd/MM/yyyy"); }
            set
            {
                this.DateTime = DateTime.ParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
        }

        public DateTime DateTimeRelease { get; set; }

        [Required]
        public string ReleaseDate
        {
            get { return this.DateTimeRelease.ToString("dd/MM/yyyy"); }
            set
            {
                this.DateTimeRelease = DateTime.ParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
        }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Bail { get; set; }

        public int CellId { get; set; }

        public ImportMailsDto[] Mails { get; set; }
    }
}
