using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DietetykAPI.Models.Entities
{
    [Table("visits")]
    public class Visit
    {
        [Key]
        [Column("visit_code")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VisitId { get; set; }
        [Column("date")]
        public DateTimeOffset Date { get; set; }

        [Column("employee_id")]
        public int EmployeeId { get; set; }


        [Column("customer_pesel")]
        public string CustomerPesel { get; set; }
        [Column("status")]
        public string status { get; set; } = "active";

        public Employee Employee { get; set; }

        public Customer Customer { get; set; }

        public Notification Notification { get; set; } 
        public MedicalResult Result { get; set; }
        public MedicalRecomendations Recomendation { get; set; }
    }
}