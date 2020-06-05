using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TGC.Group.Form
{
    public partial class CreditsUserControl : UserControl
    {
        public CreditsUserControl()
        {
            InitializeComponent();

            creditsLabel.Parent = pictureBox1;
            creditsLabel.BackColor = Color.Transparent;

            jjfLabel.Parent = pictureBox1;
            jjfLabel.BackColor = Color.Transparent;

            jfcLabel.Parent = pictureBox1;
            jfcLabel.BackColor = Color.Transparent;

            jmjLabel.Parent = pictureBox1;
            jmjLabel.BackColor = Color.Transparent;

            miunLabel.Parent = pictureBox1;
            miunLabel.BackColor = Color.Transparent;

            Pipe1.Parent = pictureBox1;
            Pipe1.BackColor = Color.Transparent;

            returnButton.Parent = pictureBox1;
            returnButton.BackColor = Color.Transparent;
        }

        private void returnButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
