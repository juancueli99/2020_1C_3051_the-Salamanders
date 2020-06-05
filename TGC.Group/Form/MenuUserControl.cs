using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TGC.Group.Model;

namespace TGC.Group.Form
{
    public partial class MenuUserControl : UserControl
    {
        public MenuUserControl()
        {
            InitializeComponent();
            this.BringToFront();

            //Pipes Bordo

            Pipe1.Parent = pictureBox1;
            Pipe1.BackColor = Color.Transparent;

            Pipe2.Parent = pictureBox1;
            Pipe2.BackColor = Color.Transparent;

            Pipe3.Parent = pictureBox1;
            Pipe3.BackColor = Color.Transparent;

            Pipe4.Parent = pictureBox1;
            Pipe4.BackColor = Color.Transparent;

            startGameButton.Parent = pictureBox1;
            startGameButton.BackColor = Color.Transparent;

            optionsButton.Parent = pictureBox1;
            optionsButton.BackColor = Color.Transparent;

            creditsButton.Parent = pictureBox1;
            creditsButton.BackColor = Color.Transparent;

            exitButton.Parent = pictureBox1;
            exitButton.BackColor = Color.Transparent;

            intoTheMistLabel.Parent = pictureBox1;
            intoTheMistLabel.BackColor = Color.Transparent;
        }

        private void startGameButton_Click(object sender, EventArgs e)
        {
            //GameModel.estoyCorriendo = true;
            this.Hide();
            //GameForm gameForm = new GameForm();
            //gameForm.ShowDialog();
            //System.Windows.Forms.Form mainMenu = (System.Windows.Forms.Form)this.Parent;
            //mainMenu.Close();
            //System.Windows.Forms.Application.ExitThread();
        }

        private void optionsButton_Click(object sender, EventArgs e)
        {
            optionsUserControl1.BringToFront();
            optionsUserControl1.Show();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void creditsButton_Click(object sender, EventArgs e)
        {
            creditsUserControl1.BringToFront();
            creditsUserControl1.Show();
        }
    }
}
