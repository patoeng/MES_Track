using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace MES.Common.Helper
{
    public static class SettingsHelper
    {

        public static string SqlDatabaseConnectionBuild( string database, string catalog, string password)
        {
            return "OK";
        }

        public static string PasswordEncrypt(string password, string salt)
        {
            return "";
        }

        public static string PasswordDecrypt(string encryptedPassword, string salt)
        {
            return "";
        }

        public static bool PasswordTest(string encryptedPassword, string passwordEntry)
        {
            return true;
        }

    }
}
