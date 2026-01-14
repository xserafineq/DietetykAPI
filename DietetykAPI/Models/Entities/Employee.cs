using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DietetykAPI.Models.Entities
{
    [Table("employees")]
    public class Employee
    {
        [Key]
        [Column("employee_id")]
        public int EmployeeId {  get; set; }
        [Column("firstname")]
        public string firstName { get; set; }
        [Column("lastname")]
        public string lastName { get; set; }
        [Column("email")]
        public string email { get; set; }
        [Column("password")]
        public string password { get; set; }

        [Column("isadmin")]

        public Boolean isadmin { get; set; }

        public ICollection<Visit> Visits { get; set; } = new List<Visit>();

    }
}
