using System;
using System.Collections.Generic;
using System.Data.Entity;
using MES.Models;


namespace MES.Data.Migrations
{
    public class MesData: IMesData
    {
        private readonly DbContext _context;

        private readonly IDictionary<Type, object> _repositories;
        public MesData()
            : this(new ApplicationDbContext())
        {
        }
        public MesData(string connection)
           : this(new ApplicationDbContext(connection))
        {
        }
        public MesData(DbContext context)
        {
            this._context = context;
            this._repositories = new Dictionary<Type, object>();
        }


       
        public IRepository<Machine> Machines => this.GetRepository<Machine>();
        public IRepository<MachineFamily> MachineFamilies => this.GetRepository<MachineFamily>();
        public IRepository<Department> Departments => this.GetRepository<Department>();
        public IRepository<MachineStatus> MachineStatuses => this.GetRepository<MachineStatus>();
        public IRepository<ProductionLine> ProductionLines => this.GetRepository<ProductionLine>();
        public IRepository<Workorder> Workorders => this.GetRepository<Workorder>();
        public IRepository<ProductProcess> ProductProcesses => this.GetRepository<ProductProcess>();
        public IRepository<Product> Products => this.GetRepository<Product>();
        public IRepository<ProductSequenceItem> ProductSequenceItems => this.GetRepository<ProductSequenceItem>();
        public IRepository<ProductSequence> ProductSequences => this.GetRepository<ProductSequence>();

        public int SaveChanges()
        {
            return this._context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this._repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(EfRepository<T>);
                this._repositories.Add(typeof(T), Activator.CreateInstance(type, this._context));
            }

            return (IRepository<T>)this._repositories[typeof(T)];
        }
    }
}
