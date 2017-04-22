using System;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using MES.Common.Helper;
using MES.Common;
using MES.Models;
using MES.Data;
using MES.Data.Migrations;

namespace MES.Logics
{
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("MES.Logics.MesMachine")]
    [Guid("9629D25F-C37C-4A04-B1AE-9A136CD4D2A7")]
    [ComSourceInterfaces(typeof(IMesMachineEvent))]
    public class MesMachine :IMesMachine
    {
        private Machine _machine;
        private IMesData _data;

        public MesMachine(int machineId)
        {
            _data = new MesData();
            if (!GetMachineById(machineId))
            {
                _machine = new Machine();
            }
        }
        public MesMachine(string machineSerialNumber)
        {
            _data = new MesData();
            if (!GetMachineBySerialNumber(machineSerialNumber))
            {
                _machine = new Machine();
            }
        }
        private bool GetMachineById(int machineId)
        {
            _machine = _data.Machines.GetById(machineId);
            return _machine != null;
        }

        private bool GetMachineBySerialNumber(string serialNumber)
        {
            _machine = _data.Machines.All().Where(m => m.SerialNumber == serialNumber).Take(1).FirstOrDefault();
            return _machine != null;
        }
        private bool NewProductProcess(Machine machine,Product product, Workorder workorder, string fullName, ProcessResult result)
        {
            ProductProcess process = new ProductProcess
            {
                ProductId = product.Id,
                WorkorderId = workorder.Id,
                MachineId = machine.Id,
                FullName = fullName,
                DateTime = DateTime.Now,
                Result =result
            };
            _data.ProductProcesses.Add(process);
            _data.ProductProcesses.SaveChanges();
            return true;
        }
        public bool StartProductProcess(string  workorderNumber, string fullName)
        {
            ProductReference pr= new ProductReference(fullName);
            Product product =
                _data.Products.All().FirstOrDefault(m => m.Reference == pr.Parsed.ReferencePart);
            Workorder workorder =
                _data.Workorders.All().FirstOrDefault(m => m.Number == workorderNumber);
            if (product != null && workorder != null)
            {  
                return NewProductProcess(_machine, product, workorder, fullName, ProcessResult.Ok);
            }
            return false;
        }
        public bool LoadProductToMachine(string productFullname)
        {
            ProductReference pr = new ProductReference(productFullname);
           
            var lastPosition = _data.ProductProcesses.All().Include(m=>m.Product).Include(m=>m.Machine)
                .Where(x => x.FullName== pr.FullName)
                .OrderByDescending(x => x.DateTime)
                .FirstOrDefault();
            if (lastPosition != null && (lastPosition.Result==ProcessResult.Ok || lastPosition.Result == ProcessResult.Dismantled))
            {
                var lastSequencetPosition = _data.ProductSequenceItems
                    .All()
                    .FirstOrDefault(m => m.MachineFamilyId == lastPosition.Machine.MachineFamilyId);

                var next = _data.ProductSequenceItems.All()
                    .Where(
                        m =>
                            m.ProductSequenceId == lastPosition.Product.SequenceId &&
                            m.Level > lastSequencetPosition.Level)
                            .OrderBy(m=>m.Level).FirstOrDefault();

                if (next != null && next.MachineFamily.Id == _machine.MachineFamily.Id || lastPosition.Result == ProcessResult.Dismantled)
                {
                    var product =
                        _data.Products.All()
                            .FirstOrDefault(m => m.Reference == pr.Parsed.ReferencePart);
                    if (product != null)
                    {
                        return NewProductProcess(_machine, product, lastPosition.Workorder, pr.FullName,ProcessResult.InProcess);
                    }
                }
            }
            return false; 
        }

        public bool DismantleProduct(string productFullName)
        {
            ProductReference pr = new ProductReference(productFullName);

            var lastPosition = _data.ProductProcesses.All().Include(m => m.Product).Include(m => m.Machine)
                .Where(x => x.FullName == pr.FullName && x.Result==ProcessResult.NOk)
                .OrderByDescending(x => x.DateTime)
                .Take(1).FirstOrDefault();
            if (lastPosition != null)
            {
                
                return UpdateProductStatus(pr.FullName, ProcessResult.Dismantled);
            }
            return false;
        }
        private bool UpdateProductStatus(string productFullName, ProcessResult result)
        {
            ProductProcess process =
                _data.ProductProcesses.All()
                    .Where(m => m.FullName == productFullName && m.MachineId == _machine.Id)
                    .Take(1)
                    .FirstOrDefault();
            if (process != null)
            {
                process.DateTime = DateTime.Now;
                process.Result = result;
                _data.ProductProcesses.Add(process);
                _data.ProductProcesses.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateProductStatusOk(string productFullName)
        {
            return UpdateProductStatus(productFullName, ProcessResult.Ok);
        }
        public bool UpdateProductStatusNok(string productFullName)
        {
            return UpdateProductStatus(productFullName, ProcessResult.NOk);
        }

        public bool NewWorkOrder(string workorderNumber, string reference, int target)
        {
            Product product = _data.Products.All().FirstOrDefault(m => m.Reference == reference);
            if (product != null)
            {
                Workorder workorder = new Workorder
                {
                    Number = workorderNumber,
                    ReferenceId = product.Id,
                    Quantity = target,
                    DateTime =  DateTime.Now,
                    EntryThroughMachineId = _machine.Id
                };
                _data.Workorders.Add(workorder);
                _data.Workorders.SaveChanges();
                return true;
            }
            return false;
        }
        public bool WorkOrderIsExist(string workorderNumber)
        {
            Workorder workorder = _data.Workorders.All().FirstOrDefault(m => m.Number == workorderNumber);
            return  workorder!=null;
        }
    }

   
}
