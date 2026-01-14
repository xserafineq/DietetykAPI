using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DietetykAPI.Models.Entities
{
    [Table("medical_results")]
    public class MedicalResult
    {

        [Key]
        [Column("visit_code")]
        public int MedicalResultId { get; set; }
        [Column("weight")]
        public double weight {  get; set; }
        [Column("height")]
        public double height { get; set; }
        [Column("waistline")]
        public double waistLine { get; set; }
        [Column("body_fat")]
        public double bodyFat { get; set; }
        [Column("sugar_level")]
        public double sugarLevel { get; set; }
        [Column("bmi")]
        public double bmi {  get; set; }
        [Column("date")]
        public DateTime date { get; set; } = DateTime.UtcNow;

        public Visit visit { get; set; }
    }
}
