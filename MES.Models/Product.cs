using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MES.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Product Reference")]
        public string Reference { get; set; }
        [DisplayName("Article Number")]
        public string ArticleNumber { get; set; }
        [DisplayName("Sequence")]
        public int? SequenceId { get; set; }

        public virtual ProductSequence Sequence { get; set; }
    }
}
