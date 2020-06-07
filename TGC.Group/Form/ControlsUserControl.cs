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
    public partial class ControlsUserControl : UserControl
    {
        public ControlsUserControl()
        {
            InitializeComponent();

            Pipe3.Parent = pictureBox1;
            Pipe3.BackColor = Color.Transparent;

            returnButton.Parent = pictureBox1;
            returnButton.BackColor = Color.Transparent;
        }

        private void returnButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

    }
}
