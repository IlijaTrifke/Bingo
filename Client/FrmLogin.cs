using System;
using System.Windows.Forms;

namespace Client
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
            Communication.Instance.Connect();
            btnPick.Enabled = false;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Popunite ime!");
            }
            else
            {
                Communication.Instance.PorukaReceived += ObradiPoruku;
                Communication.Instance.Login(textBox1.Text);

            }
        }

        private void ObradiPoruku(object sender, Common.Poruka e)
        {
            Invoke(new Action(() =>
            {
                switch (e.Operation)
                {
                    case Common.Operation.StartTheGame:
                        textBox1.Text = "";
                        button1.Enabled = false;
                        btnPick.Enabled = true;
                        break;
                    case Common.Operation.WinnerAnnouncement:
                        MessageBox.Show(e.Tekst);
                        break;
                    case Common.Operation.TimeExpired:
                        MessageBox.Show(e.Tekst);
                        break;
                    default:
                        break;
                }
            }));
        }

        private void btnPick_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Popunite broj!");
            }
            else
            {
                Communication.Instance.Pick(int.Parse(textBox1.Text));
            }
        }
    }
}
