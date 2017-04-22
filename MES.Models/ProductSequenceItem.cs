
using System.ComponentModel.DataAnnotations;

namespace MES.Models
{
    public class ProductSequenceItem
    {
        [Key]
        public int Id { get; set; }
        public int? Level { get; set; }
        public int? MachineFamilyId { get; set; }
        public int? ProductSequenceId { get; set; }

        public virtual MachineFamily MachineFamily { get; set; }

        public virtual ProductSequence ProductSequence { get; set; }
    }
}
