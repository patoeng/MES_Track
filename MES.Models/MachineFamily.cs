using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MES.Models
{
    public class MachineFamily
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Machine Family")]
        public string Name { get; set; }
    }
}
