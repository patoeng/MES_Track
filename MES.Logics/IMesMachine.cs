using System.Runtime.InteropServices;

namespace MES.Logics
{
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [Guid("74E0EAEF-512D-4B69-9496-835C7A72FE1C")]
    public interface IMesMachine
    {
        bool StartProductProcess(string  workorderNumber, string fullName);
        bool LoadProductToMachine(string productFullname);
        bool DismantleProduct(string productFullName);
        bool UpdateProductStatusOk(string productFullName);
        bool UpdateProductStatusNok(string productFullName);
        bool NewWorkOrder(string workorderNumber, string reference, int target);
        bool WorkOrderIsExist(string workorderNumber);
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [Guid("CDCE3A57-E48C-4612-BA5D-BE55AA0FB37F")]
    public interface IMesMachineEvent
    {
    }
}
