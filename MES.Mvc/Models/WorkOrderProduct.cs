using System.ComponentModel;

namespace MES.Mvc.Models
{
    public class WorkOrderProduct
    {
        [DisplayName("Product Data Metrix")]
        public string DataMetrix { get; set; }
        [DisplayName("Work Order Number")]
        public string WorkOrderNumber { get; set; }
        [DisplayName("Product Status")]
        public WorkOrderProductStates State { get; set; }
        [DisplayName("Machine")]
        public string  Machine { get; set; }
    }
}