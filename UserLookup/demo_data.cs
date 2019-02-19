// Class for simulating return data for testing.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserLookup
{
    class demo_data
    {

        String net_user_do_valid = @"The request will be processed at a domain controller for domain domain.company.com.



User name jsmith

Full Name                    Smith, John

Comment                      --- 07/21/2018 11:40:18 - John gave Kevin an Apple --

User's comment               

Country/region code          000 (System Default)

Account active Yes

Account expires              Never



Password last set            19/01/2019 9:18:17 AM

Password expires             15/05/2019 9:18:17 AM

Password changeable          19/01/2019 9:18:17 AM

Password required No

User may change password     Yes



Workstations allowed All

Logon script                 company_logon.bat

User profile

Home directory               \\domain.company.com\australia\users\jsmith

Last logon                   10/02/2019 7:00:00 AM



Logon hours allowed          All



Local Group Memberships

Global Group memberships* TES_FI_COMP_SOFT_SEA*TES_FI_COMP_SOFT_GENE

                             * Local_Admin_COMP.READ* SQL_SRVR_SYSTEM-TEST

                             *System_Dev* TEST_FI_COMP_SOFT-Test

The command completed successfully.";


        String net_user_do_invalid = @"The request will be processed at a domain controller for domain domain.company.com.



The user name could not be found.



More help is available by typing NET HELPMSG 2221.







";






    }
}
