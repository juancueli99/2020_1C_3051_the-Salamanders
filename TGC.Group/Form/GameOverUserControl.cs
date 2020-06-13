using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using TGC.Group.Model;
using TGC.Core.Example;

namespace TGC.Group.Form
{
    public partial class GameOverUserControl : UserControl
    {
        public static GameOverUserControl instancia = null;

        public MenuUserControl menuPrincipalPosta;
        public GameForm formActual;
        public GameOverUserControl()
        {
            InitializeComponent();

            /*menuPrincipalPosta = menuPrincipal;
            formActual = gameForm;*/

            Pipe1.Parent = pictureBox1;
            Pipe1.BackColor = Color.Transparent;

            returnButton.Parent = pictureBox1;
            returnButton.BackColor = Color.Transparent;

            GameOverUserControl.instancia = this;
        }

        private void returnButton_Click(object sender, EventArgs e)
        {
            //menuPrincipalPosta.BringToFront();
            //menuPrincipalPosta.Show();

            System.Environment.Exit(0);

            /*GameModel.estoyEnElMenu = true;

            GameForm nuevoJuego = new GameForm();
            nuevoJuego.BringToFront();
            nuevoJuego.Show();

            formActual.Modelo.Dispose();
            formActual.Close();*/


        }
    }
}
