using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MES.Logics
{
    public interface ISetting
    {
        string GetDatabaseConnectionString();
        void SetEnableTraceability(bool enable);
        bool GetEnableTraceability();
        string MachineSerialNumber();
    }
}
