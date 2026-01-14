using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DietetykAPI.Models.Entities
{
    [Table("config")]
    public class Config
    {

        
        [Column("id")]
        [Key]
        public int id { get; set; }

        [Column("visit_duration")]
        public int visit_duration { get; set; }
    }
}
