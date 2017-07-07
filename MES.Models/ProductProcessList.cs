
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MES.Models
{
    public class ProductProcessList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Complete Reference")]
        public string FullName { get; set; }
        public int? WorkorderId { get; set; }
        public virtual Workorder Workorder { get; set; }
        [DisplayName("Date")]
        public DateTime DateTime { get; set; }
        public int? ProductId { get; set; }
        public virtual Product Product { get; set; }
        public string Remarks { get; set; }
        public int? UseProductSequenceId { get; set; }
        public virtual ProductSequence UserProductSequence { get; set; }
    }
}
