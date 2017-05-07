using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace MES.Models
{
    public class Machine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Machine Code")]
        public string SerialNumber { get; set; }
       
        [Required]
        [DisplayName("Machine Name")]
        public string Name { get; set; }
        [DisplayName("Machine Family")]
        public int? MachineFamilyId { get; set; }
        [DisplayName("Production Line")]
        public int? ProductionLineId { get; set; }

        public MachineStatusType MachineStatus { get; set; }
        public TraceabilityStatusType TraceabilityStatus { get; set; }
        public TraceabilityPingStatus TraceabilityPingStatus { get; set; }

        public virtual MachineFamily MachineFamily { get; set; }

        public virtual ProductionLine ProductionLine { get; set; }

    }

   
}
