using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportCellsDto
    {
        [Required]
        [Range(typeof(int), "1", "1000")]
        public int CellNumber { get; set; }

        [Required]
        public bool HasWindow { get; set; }
        
    }
}
