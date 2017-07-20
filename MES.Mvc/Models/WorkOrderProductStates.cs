namespace MES.Mvc.Models
{
    public enum WorkOrderProductStates
    {
        Generated,
        Processed,
        Pass,
        Fail,
        Dismantled,
        PassAfterDismantle,
        FailAfterDismantle,
        FinalPass,
        FinalFail
    }
}