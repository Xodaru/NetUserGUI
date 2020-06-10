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
        // Our method to parse the net user data returned.
        // Ensure that userData always includes at least a status message.
        public static bool ReadUserData(string data, UserData userData)
        {
           
            string[] userReturn = Regex.Split(data, Environment.NewLine); // Split our return into a string array.

            // If the username can't be found, lets just stop it right there.
            if (userReturn.FirstOrDefault(str => str.StartsWith("The user name could"))!=null) { userData.statusMessage = "User Name could not be found."; return false; }

            // The beauty about the following methods to search the string array is simple to understand and returns null if not found.
            string fn_line = userReturn.FirstOrDefault(str => str.StartsWith("Full Name"));
            string aa_line = userReturn.FirstOrDefault(str => str.StartsWith("Account active"));
            string ae_line = userReturn.FirstOrDefault(str => str.StartsWith("Account expires"));
            string pls_line = userReturn.FirstOrDefault(str => str.StartsWith("Password last set"));
            string pe_line = userReturn.FirstOrDefault(str => str.StartsWith("Password expires"));
            string ll_line = userReturn.FirstOrDefault(str => str.StartsWith("Last logon"));
            string ls_line = userReturn.FirstOrDefault(str => str.StartsWith("Logon script")); // added this, sometimes nice to know I think

            //Clean up trailing endline stuff - We could build this into the above, but for readability sake we won't
            // Date rows sometimes have question marks in them, character encoding issue (this is lazy way to fix) EG "?3/?06/?2020 2:31:26 PM"
            // Note to self, the below does not work if it's NULL - need to check on null values.
            fn_line = fn_line.Remove(0, 9).TrimStart().Replace("\n",""); 
            aa_line = aa_line.Remove(0,14).TrimStart().Replace("\n", "");
            ae_line = ae_line.Remove(0, 15).TrimStart().Replace("\n", "").Replace("?","");
            pls_line = pls_line.Remove(0, 17).TrimStart().Replace("\n", "").Replace("?", "");
            pe_line = pe_line.Remove(0, 16).TrimStart().Replace("\n", "").Replace("?", "");
            ll_line = ll_line.Remove(0, 10).TrimStart().Replace("\n", "").Replace("?", "");
            ls_line = ls_line.Remove(0, 12).TrimStart().Replace("\n", "");

            //Some checks on the data now to determine if we accept this. Not actually required as null values are fine, but added to demonstrate
            if (fn_line.Length < 1 || fn_line == null) { userData.statusMessage = "Full name seems wrong"; return false; }

            // Start filling out the userData structure.
            userData.fullName = fn_line;
            userData.accountActive = aa_line;
            userData.accountExpires = ae_line;
            userData.passLastSet = pls_line;
            userData.passExpire = pe_line;
            userData.lastLogon = ll_line;
            userData.logonScript = ls_line;
            userData.statusMessage = "Successfully parsed the output, you won't see this message except for debugging purposes :)"; 
            return true; 

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
            public string logonScript { get; set; }
            public string statusMessage { get; set; }

        }


    }
}
