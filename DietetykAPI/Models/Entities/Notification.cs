using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DietetykAPI.Models.Entities
{
    [Table("notifications")]
    public class Notification
    {
        [Key]
        [Column("visit_code")]
        public int NotificationId { get; set; }
        [Column("status")]
        public string status { get; set; }

        public Visit Visit { get; set; }
    }
}
