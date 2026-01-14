using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DietetykAPI.Models.Entities
{
    [Table("medical_recommendations")]
    public class MedicalRecomendations
    {
        [Key]
        [Column("visit_code")]
        public int MedicalRecomendationsId { get; set; }
        [Column("diet_id")]
        public int dietId { get; set; }

        [Column("note")]
        public string note {  get; set;}

        [Column("date")]
        public DateTimeOffset date { get; set; }

        public Visit Visit { get; set; }
        public Diet Diet { get; set; }

    }
}
