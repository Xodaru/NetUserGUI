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
    public partial class Form1 : Form


    {
        Process process = new Process();
        static StringBuilder sb = new StringBuilder();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lookupOutput.Text = null; 
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
                lookupOutput.Text = null;
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

                // Some quick spring cleaning
                lookupOutput.Text = null; 
                user = null;
                if (sb != null)
                {
                    // Show the data
                    lookupOutput.Text = sb.ToString();
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


    }
}
