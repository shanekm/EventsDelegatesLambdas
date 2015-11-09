using System;
using System.Threading;
using System.Windows.Forms;

namespace ThreadsAndDelegates
{
    public partial class UsingAThread : Form
    {
        private int _Max;

        public UsingAThread()
        {
            InitializeComponent();
        }

        [STAThread]
        private static void Main()
        {
            Application.Run(new UsingAThread());
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            _Max = 100;
            
            // ThreadStart is a delegate
            // Delegate functionality to StartProcess() method
            Thread t = new Thread(new ThreadStart(StartProcess));
            t.Start(); // Invoke
        }

        private void StartProcess()
        {
            if (this.pbStatus.InvokeRequired)
            {
                StartProcessHandler sph = new StartProcessHandler(StartProcess);
                this.Invoke(sph); // Form thread (Guid thread) will recall start process but on GUI thread
            }
            else
            {
                this.Refresh();
                this.pbStatus.Maximum = _Max;
                for (int i = 0; i <= _Max; i++)
                {
                    Thread.Sleep(10);
                    this.lblOutput.Text = i.ToString();
                    this.pbStatus.Value = i;
                }
            }
        }

        // Used for Guid thread check
        private delegate void StartProcessHandler();
    }
}