// APP ICON FROM https://visualpharm.com/free-icons/search-595b40b85ba036ed117daa18
//
// TO DO:
// * Parse results

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserLookup
{
    public partial class mainWindow : Form


    {
        User process = new User();
        static StringBuilder sb = new StringBuilder();

        public mainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            WipeValues(); // Wipe our screen values for a fresh start.
            if (userName.Text != "")
            {
                var user = userName.Text;
                // Check that the username contains no invalid characters
                if (user.All(x => char.IsLetterOrDigit(x) || char.IsWhiteSpace(x)) == false)
                {
                    lookupOutput.Text = "Username should be alpha-numerical only";
                    return;
                }

                // Do some clearing before starting
                sb.Clear();

                // Create our new process for the net.exe client
                Process process = new Process();
                process.EnableRaisingEvents = true;
                process.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(process_OutputDataReceived); // Output handler
                process.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(process_ErrorDataReceived); // Error handler
                process.Exited += new System.EventHandler(process_Exited); // Exit handler 
                process.StartInfo.FileName = "net.exe";
                process.StartInfo.Arguments = "user " + user + " /do";  // Hardcoded to execute /do for now
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;  // hide the process, user does not need to see the black CMD screen.
                lookupOutput.Text = "Please Wait..";  // This is only useful if net user takes ages to return the data, otherwise it's usually instant.
                process.Start();
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();

                // ALTERNATIVE METHOD TO LOOK INTO instead of process_OutputDataReceived 
                // This is a synchronous method, not 'normally' the best idea.
                // string copiedFileName;
                // while ((copiedFileName = xcopy.StandardOutput.ReadLine()) != null)
                // {
                //    output_list.Items.Add(copiedFileName);
                // }

                // We want to run some code after the process has exited so we WaitForExit.
                // Note in some situations this can make the app appear frozen.
                process.WaitForExit();
                //Note to self : create asynchronous method, I'm just lazy for now as the response is very quick anyway.

                // Some quick spring cleaning
                lookupOutput.Text = null;

                // Show the raw data
                lookupOutput.Text = sb.ToString();

                user = null;
                if (sb != null)
                {

                    // We have a Class to parse the return from net user into a user object
                    // For more information on that refer to User.cs

                    // Set up our structure.
                    User.UserData ourUser = new User.UserData();

                    // Parse the data. Returns false is there was a problem 
                    if (!User.ReadUserData(sb.ToString(), ourUser)) {
                        // Problem was encoutnered so just show the message and finish up
                        lookupOutput.Text = ourUser.statusMessage;
                        return; // Do nothign more, lets add proper error handling sometime
                    }

                    // No error, so we assume ourUser is filled with lots of goodies for us to use.
                    nameBox.Text = ourUser.fullName;
                    if (ourUser.accountActive == "Yes") { checkBox1.Checked = true; } else { checkBox1.Checked = false; };
                    // I was planning to use datepick controls but ran into some complications with dates that return as a string "Never". Will look into this
                    aeBox.Text = ourUser.accountExpires;
                    plsBox.Text = ourUser.passLastSet;
                    peBox.Text = ourUser.passExpire;
                    llBox.Text = ourUser.lastLogon;
                    lsBox.Text = ourUser.logonScript;

                }
            } else
            {
                // Username is empty.
                lookupOutput.Text = "Please enter a username";
            }
        }

        void process_Exited(object sender, EventArgs e)
        {
            // We dont need anything here for now
            //sb.AppendLine(string.Format(e.Data + "\n"));
        }

        void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            // Display the error output
            sb.AppendLine(string.Format(e.Data + "\n"));
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            // Our data output
            sb.AppendLine(string.Format(e.Data + "\n"));
        }

        private void mainWindow_Load(object sender, EventArgs e)
        {
            // Set default view to exclude the raw output

            this.Width = 269;

        }
        // Small method to wipe the values
        private void WipeValues()
        {
            lookupOutput.Text = null;
            nameBox.Text = "";
            checkBox1.Checked = false;
            aeBox.Text = "";
            plsBox.Text = "";
            peBox.Text = "";
            llBox.Text = "";
            lsBox.Text = "";

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true) { this.Width = 826; } else { this.Width = 269;}
        }
    }
}
