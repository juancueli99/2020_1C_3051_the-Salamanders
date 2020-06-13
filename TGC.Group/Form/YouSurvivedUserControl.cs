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
    public partial class YouSurvivedUserControl : UserControl
    {
        public static YouSurvivedUserControl instancia = null;

        public MenuUserControl menuPrincipalPosta;
        public YouSurvivedUserControl()
        {
            InitializeComponent();

            //menuPrincipalPosta = menuPrincipal;

            Pipe1.Parent = pictureBox1;
            Pipe1.BackColor = Color.Transparent;

            returnButton.Parent = pictureBox1;
            returnButton.BackColor = Color.Transparent;

            YouSurvivedUserControl.instancia = this;
        }

        private void returnButton_Click(object sender, EventArgs e)
        {
            //menuPrincipalPosta.BringToFront();
            //menuPrincipalPosta.Show();

            System.Environment.Exit(0);
        }

    }
}
