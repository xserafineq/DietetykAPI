using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DietetykAPI.Models.Entities
{
    [Table("diets")]
    public class Diet
    {
        [Column("diet_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int dietId { get; set; }
        [Column("type")]
        public string type { get; set; }
        [Column("kcal_deficit")]
        public double kcalDeficit { get; set; }
        [Column("pdf")]
        public byte[] pdf { get; set; }

    }
}
