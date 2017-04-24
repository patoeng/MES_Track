using System;
using System.Configuration;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MES.Logics
{
    public class Settings : ISetting
    {
        
            private readonly Configuration _appConfiguration;

            public Settings()
            {
                _appConfiguration = ConfigurationManager.OpenExeConfiguration(LogicsLocation());
            }

            [DispId(1)]
            public string LogicsLocation()
            {
                var location = Assembly.GetExecutingAssembly().Location;
                return location;
            }

            private object GetCreateSetting(string parameter, string value)
            {

                try
                {
                    return _appConfiguration.AppSettings.Settings[parameter].Value;
                }
                catch (Exception)
                {
                _appConfiguration.AppSettings.Settings.Add(new KeyValueConfigurationElement(parameter, value));
                 Save();
                return _appConfiguration.AppSettings.Settings[parameter].Value;
                }
            }
            private void SetCreateSetting(string parameter, string value)
            {
                try
                {
                    _appConfiguration.AppSettings.Settings[parameter].Value = value;
                }
                catch (Exception)
                {
                    _appConfiguration.AppSettings.Settings.Add(new KeyValueConfigurationElement(parameter, value));
                }
                Save();
            }
            private string GetSetting(string parameter, string defaultValue)
            {
                return GetCreateSetting(parameter, defaultValue).ToString();
            }
            private void SetSetting(string parameter, string parameterValue)
            {
                SetCreateSetting(parameter, parameterValue);
            }
            [DispId(2)]
            public void Save()
            {
                _appConfiguration.Save();
            }

        public string GetDatabaseConnectionString()
        {
            return GetSetting("DbConnection",
                "Data Source=localhost;Initial Catalog=MESTRAC;Persist Security Info=True;User ID=sa;Password=passwordsa;");
        }

        public void SetEnableTraceability(bool enable)
        {
            SetSetting("EnableTraceability",enable.ToString());
        }
        public bool GetEnableTraceability()
        {
            return GetSetting("EnableTraceability", "") == "True";
        }

        public string MachineSerialNumber()
        {
            return GetSetting("MachineSerialNumber", "XXX");
        }

    }
}
