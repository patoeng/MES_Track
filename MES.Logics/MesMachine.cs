using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using MES.Common.Helper;
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
        private readonly ISetting _setting;
        private string _koneksi;

        [DispId(12)]
        public event MyEventHandlerWithInfo MesExceptionEvent;
        public MesMachine()
        {
            _setting = new Settings();
        }
      
        public void InitializeMachine()
        {
            string machineSerialNumber = _setting.MachineSerialNumber();
            InitializeMachine(machineSerialNumber);
        }
        public void InitializeMachine(string machineSerialNumber)
        {
            if (_setting.GetEnableTraceability())
            {
                try
                {


                    _koneksi = _setting.GetDatabaseConnectionString();


                    _data = new MesData(_koneksi);
                    if (!GetMachineBySerialNumber(machineSerialNumber))
                    {
                        _machine = new Machine();
                        MesExceptionEvent?.Invoke("Machine Serial Number Not Set Or Not Registered to Server");
                    }
                }
                catch (Exception exception)
                {
                    MesExceptionEvent?.Invoke(exception.StackTrace + "\r\n " + exception.Message + "\r\n" +
                                              exception.InnerException);
                }
            }
        }
        private bool GetMachineById(int machineId)
        {
            try
            {
                _machine = _data.Machines.GetById(machineId);
            }
            catch (Exception exception)
            {
                MesExceptionEvent?.Invoke(exception.Message);
            }
            return _machine != null;
        }

        private bool GetMachineBySerialNumber(string serialNumber)
        {
            try
            {
                _machine = _data.Machines.All().Where(m => m.SerialNumber == serialNumber).Take(1).FirstOrDefault();
            }
            catch (Exception exception)
            {
                MesExceptionEvent?.Invoke(exception.Message);
            }
            return _machine != null;
        }
        private bool NewProductProcess(Machine machine,Product product, Workorder workorder, string fullName, ProcessResult result)
        {
            if (IsTraceabilityEnabled())
            {
                try
                {
                    ProductProcess process = new ProductProcess
                    {
                        ProductId = product.Id,
                        WorkorderId = workorder.Id,
                        MachineId = machine.Id,
                        FullName = fullName,
                        DateTime = DateTime.Now,
                        Result = result
                    };
                    _data.ProductProcesses.Add(process);
                    _data.ProductProcesses.SaveChanges();
                    return true;
                }
                catch (Exception exception)
                {
                    MesExceptionEvent?.Invoke(exception.Message);
                }               
            }
            else
            {
                return true;
            }
            return false;
        }
        public bool StartProductProcess(string  workorderNumber, string fullName)
        {
            if (IsTraceabilityEnabled())
            {
                try
                {
                    ProductReference pr = new ProductReference(fullName);
                    Product product =
                        _data.Products.All().FirstOrDefault(m => m.Reference == pr.Parsed.ReferencePart);
                    Workorder workorder =
                        _data.Workorders.All().FirstOrDefault(m => m.Number == workorderNumber);

                    var checkForAlreadyProcessed =
                        _data.ProductProcesses.All().FirstOrDefault(m => m.FullName == fullName);

                    if (product != null && workorder != null && checkForAlreadyProcessed==null)
                    {
                        return NewProductProcess(_machine, product, workorder, fullName, ProcessResult.Generated);
                    }
                    if (product == null)
                    {
                        MesExceptionEvent?.Invoke("Product Reference Not Found");
                    }
                    if (workorder == null)
                    {
                        MesExceptionEvent?.Invoke("WorkOrder Number Not Found");
                    }
                    if (checkForAlreadyProcessed != null)
                    {
                        MesExceptionEvent?.Invoke("Product "+fullName+" has already been generated");
                    }
                }
                catch (Exception exception)
                {
                    MesExceptionEvent?.Invoke(exception.Message);
                }
            }
            else
            {
                return true;
            }
            return false;
        }
        public bool LoadProductToMachine(string productFullname)
        {
            if (IsTraceabilityEnabled())
            {
                try
                {
                    ProductReference pr = new ProductReference(productFullname);

                    var lastPosition = _data.ProductProcesses.All().Include(m => m.Product).Include(m => m.Machine)
                        .Where(x => x.FullName == pr.FullName)
                        .OrderByDescending(x => x.DateTime)
                        .FirstOrDefault();
                    if (lastPosition != null &&
                        (lastPosition.Result == ProcessResult.Pass || lastPosition.Result == ProcessResult.Dismantled))
                    {
                        var lastSequencetPosition = _data.ProductSequenceItems
                            .All()
                            .FirstOrDefault(m => m.MachineFamilyId == lastPosition.Machine.MachineFamilyId);

                        var next = _data.ProductSequenceItems.All()
                            .Where(
                                m =>
                                    m.ProductSequenceId == lastPosition.Product.SequenceId &&
                                    m.Level > lastSequencetPosition.Level)
                            .OrderBy(m => m.Level).FirstOrDefault();

                        if (next != null && next.MachineFamily.Id == _machine.MachineFamily.Id ||
                            lastPosition.Result == ProcessResult.Dismantled)
                        {
                            var product =
                                _data.Products.All()
                                    .FirstOrDefault(m => m.Reference == pr.Parsed.ReferencePart);
                            if (product != null)
                            {
                                return NewProductProcess(_machine, product, lastPosition.Workorder, pr.FullName,
                                    ProcessResult.InProcess);
                            }
                            MesExceptionEvent?.Invoke("Product Reference "+pr.Parsed.ReferencePart+" is not found");
                        }
                    }
                }
                catch (Exception exception)
                {
                    MesExceptionEvent?.Invoke(exception.Message);
                }
            }
            else
            {
                return true;
            }
            return false; 
        }

        public bool DismantleProduct(string productFullName)
        {
            if (IsTraceabilityEnabled())
            {
                try
                {
                    ProductReference pr = new ProductReference(productFullName);

                    var lastPosition = _data.ProductProcesses.All().Include(m => m.Product).Include(m => m.Machine)
                        .Where(x => x.FullName == pr.FullName)
                        .OrderByDescending(x => x.DateTime)
                        .Take(1).FirstOrDefault();
                    if (lastPosition != null)
                    {
                        return UpdateProductStatus(pr.FullName, ProcessResult.Dismantled, "");
                    }
                    MesExceptionEvent?.Invoke("Product process of " + productFullName + " is not found");
                }
                catch (Exception exception)
                {
                    MesExceptionEvent?.Invoke(exception.Message);
                }
            }
            else
            {
                return true;
            }
            return false;
        }
        private bool UpdateProductStatus(string productFullName, ProcessResult result, string remarks)
        {
            if (IsTraceabilityEnabled())
            {
                try
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
                        process.Remarks = remarks;
                        _data.ProductProcesses.Add(process);
                        _data.ProductProcesses.SaveChanges();
                        return true;
                    }
                    MesExceptionEvent?.Invoke("Product process of " + productFullName + " is not found");
                }
                catch (Exception exception)
                {
                    MesExceptionEvent?.Invoke(exception.Message);
                }
            }
            else
            {
                return true;
            }
            return false;
        }

        public bool UpdateProductStatusOk(string productFullName)
        {
            return UpdateProductStatus(productFullName, ProcessResult.Pass,"");
        }
        public bool UpdateProductStatusNok(string productFullName)
        {
            return UpdateProductStatus(productFullName, ProcessResult.Fail,"");
        }
        public bool UpdateProductStatusOk(string productFullName,string remarks)
        {
            return UpdateProductStatus(productFullName, ProcessResult.Pass, remarks);
        }
        public bool UpdateProductStatusNok(string productFullName,string remarks)
        {
            return UpdateProductStatus(productFullName, ProcessResult.Fail, remarks);
        }
        public bool NewWorkOrder(string workorderNumber, string reference, int target)
        {
            if (IsTraceabilityEnabled())
            {
                try
                {
                    Product product = _data.Products.All().FirstOrDefault(m => m.Reference == reference);
                    if (product != null)
                    {
                        Workorder workorder = new Workorder
                        {
                            Number = workorderNumber,
                            ReferenceId = product.Id,
                            Quantity = target,
                            DateTime = DateTime.Now,
                            EntryThroughMachineId = _machine.Id
                        };
                        _data.Workorders.Add(workorder);
                        _data.Workorders.SaveChanges();
                        return true;
                    }
                }
                catch (Exception exception)
                {
                    MesExceptionEvent?.Invoke(exception.Message);
                }
            }
            else
            {
                return true;
            }
            return false;
        }
        public bool WorkOrderIsExist(string workorderNumber)
        {
            Workorder workorder = _data.Workorders.All().FirstOrDefault(m => m.Number == workorderNumber);
            return  workorder!=null;
        }

        public void EnableTraceability()
        {
           _setting.SetEnableTraceability(true);
        }
        public void DisableTraceability()
        {
            _setting.SetEnableTraceability(false);
        }

        public bool IsTraceabilityEnabled()
        {
            return _setting.GetEnableTraceability();
        }

        public string CurrentConnectionString()
        {
            return _setting.GetDatabaseConnectionString();
        }
    }

   
}
