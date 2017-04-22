using MES.Models;
namespace MES.Data
{
    public interface IMesData
    {

       
        IRepository<Machine> Machines { get; }
        IRepository<MachineFamily> MachineFamilies { get; }
        IRepository<Department> Departments { get; }
        IRepository<MachineStatus> MachineStatuses { get; }
        IRepository<ProductionLine> ProductionLines { get; }
        IRepository<Workorder> Workorders { get; }
        IRepository<ProductProcess> ProductProcesses { get; }
        IRepository<Product> Products { get; }
        IRepository<ProductSequenceItem>ProductSequenceItems { get; }
        IRepository<ProductSequence> ProductSequences { get; }

        int SaveChanges();
    }
}
