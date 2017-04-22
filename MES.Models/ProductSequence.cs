
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MES.Models
{
    public class ProductSequence
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Product Sequence")]
        public string Name { get; set; }
    }
}
