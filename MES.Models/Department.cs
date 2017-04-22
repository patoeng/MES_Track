using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MES.Models
{
   public class Department
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Department")]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }
    }
}
