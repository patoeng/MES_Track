using System;
using System.ComponentModel.DataAnnotations;

namespace MES.Models
{
    public class MachineStatus
    {
        [Key]
        public int Id { get; set; }

        public DateTime? DateTime { get; set; }

        public MachineStatusType Status { get; set; }
    }
}
