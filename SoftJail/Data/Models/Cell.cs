using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models
{
    public class Cell
    {
        public int Id { get; set; }

        [Required]
        [Range(typeof(int), "1", "1000")]
        public int CellNumber { get; set; }

        [Required]
        public bool HasWindow { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public ICollection<Prisoner> Prisoners { get; set; }

        //        •	Id – integer, Primary Key
        //•	CellNumber – integer in the range[1, 1000] (required)
        //•	HasWindow – bool (required)
        //•	DepartmentId - integer, foreign key
        //•	Department – the cell's department (required)
        //•	Prisoners - collection of type Prisoner

    }
}