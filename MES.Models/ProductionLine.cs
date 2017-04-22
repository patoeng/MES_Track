using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MES.Models
{
    public class ProductionLine
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Production Line")]
        public string Name { get; set; }
    }
}
