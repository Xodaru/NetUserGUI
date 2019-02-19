// APP ICON FROM https://visualpharm.com/free-icons/search-595b40b85ba036ed117daa18


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
            lookupOutput.Text = null; // Wipe Existing data

            if (userName.Text != "")
            {
                var user = userName.Text;

                if (user.All(x => char.IsLetterOrDigit(x) || char.IsWhiteSpace(x)) == false)
                    
                {
                    lookupOutput.Text = "Username should be alpha-numerical only";
                    return;
                }


                lookupOutput.Text = null;
                sb.Clear();

                Process process = new Process();
                
                
                process.EnableRaisingEvents = true;
                process.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(process_OutputDataReceived);
                process.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(process_ErrorDataReceived);
                process.Exited += new System.EventHandler(process_Exited);

                process.StartInfo.FileName = "net.exe";
                process.StartInfo.Arguments = "user " + user + " /do";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
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


                //We want to run some code after the process has exited so we WaitForExit
                process.WaitForExit();
                lookupOutput.Text = null; // Clear the output so Please Wait... is gone.
                user = null;
                if (sb != null)
                {
                    //  lookupOutput.Text = lookupData;
                    lookupOutput.Text = sb.ToString();

                }
                //   }
            } else
            {
                lookupOutput.Text = "Please enter a username";
                //MessageBox.Show("Enter a username");
            }

        }

        void process_Exited(object sender, EventArgs e)
        {
          //  Console.WriteLine(string.Format("process exited with code {0}\n", process.ExitCode.ToString()));
            //sb.AppendLine(string.Format(e.Data + "\n"));
        }

        void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
         //   Console.WriteLine(e.Data + "\n");
            sb.AppendLine(string.Format(e.Data + "\n"));
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {

            //    Debug.Print(e.Data + "\n");
            
            sb.AppendLine(string.Format(e.Data + "\n"));

            

        }


        }
    }
