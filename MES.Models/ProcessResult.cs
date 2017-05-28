namespace MES.Models
{
    public enum ProcessResult
    {
        Generated,
        InProcess,
        Pass,
        Fail,
        Dismantled,
        BackJumped,
        Renamed,
        FailWrongProcessAttempt,
    }
}
