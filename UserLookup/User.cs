using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UserLookup
{
    class User
    {
        // Our method to read the data
        public static bool ReadUserData(string data, UserData userData)
        {
           
            string[] userReturn = Regex.Split(data, Environment.NewLine); // Split our return into a string array.

            // The beauty about the following methods to search the string array is simple to understand and returns null if not found.
            string fn_line = userReturn.FirstOrDefault(str => str.StartsWith("Full Name"));
            string aa_line = userReturn.FirstOrDefault(str => str.StartsWith("Account active"));
            string ae_line = userReturn.FirstOrDefault(str => str.StartsWith("Account expires"));
            string pls_line = userReturn.FirstOrDefault(str => str.StartsWith("Password last set"));
            string pe_line = userReturn.FirstOrDefault(str => str.StartsWith("Password expires"));
            string ll_line = userReturn.FirstOrDefault(str => str.StartsWith("Last logon"));

            fn_line = fn_line.Remove(0, 9).TrimStart().Replace("\n",""); // We could build this into the above, but for readability sake we won't
            aa_line = aa_line.Remove(0,14).TrimStart().Replace("\n", "");

            return false; 

        }


        // Structure for the data we want to read, if available.
        public class UserData
        {
            public string fullName { get; set; }
            public string accountActive { get; set; }
            public string accountExpires { get; set; }
            public string passLastSet { get; set; }
            public string passExpire { get; set; }
            public string lastLogon { get; set; }

        }


    }
}
