using System;
using System.Threading;
using System.Windows.Forms;

namespace ServerForma
{
    public partial class FrmServer : Form
    {
        Server server;
        System.Windows.Forms.Timer timer;
        System.Windows.Forms.Timer timer1;
        public FrmServer()
        {
            InitializeComponent();
            btnStop.Enabled = false;
            btnStartTheGame.Enabled = false;
        }

        private void btnStart_Click(object sender, System.EventArgs e)
        {
            server = new Server();
            if (server.Start())
            {
                btnStart.Enabled = false;
                server.Listen();
                btnStartTheGame.Enabled = true;
                timer = new System.Windows.Forms.Timer();
                timer.Interval = 500;
                timer.Tick += (s, a) =>
                {
                    label1.Text = new Random().Next(1, 100).ToString();
                };
                timer.Start();
            }
            else
            {
                MessageBox.Show("Neuspesno startovanje servera!");
            }
        }
        private void btnStop_Click(object sender, System.EventArgs e)
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            timer.Stop();
            server.Stop();
        }

        private void btnStartTheGame_Click(object sender, System.EventArgs e)
        {
            timer.Stop();
            Server.winnerNumber = int.Parse(label1.Text);
            server.StartTheGame();
            timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 20000;
            timer1.Tick += (s, a) =>
            {
                server.EndAnnouncement();
                timer1.Stop();
            };
            timer1.Start();

            Thread nit = new Thread(server.WinnerAnnouncement);
            nit.Start();
            btnStop.Enabled = true;

        }
    }
}
