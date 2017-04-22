using System;
using System.IO;
using System.Xml;

namespace MES.Datalog
{
    public class DataLog :IDataLog
    {
        private readonly char _separator;
        private readonly string _datalogFilePath;
        private readonly string _machine;
        public DataLog(string datalogFilePath, char separator, string machine)
        {
            _separator = separator;
            _datalogFilePath = datalogFilePath;
            _machine = machine;
        }
        
        public bool WriteLog(string message)
        {
            string path = _datalogFilePath + "\\MES_" + _machine + "_" + DateTime.Now.ToString("yymmdd") + ".log";
            using (StreamWriter sw = File.AppendText(path))
            {
                var line = DateTime.Now.ToString("f") + _separator + message ;
                sw.WriteLine(line);
            }

            return false;
        }
    }
}
