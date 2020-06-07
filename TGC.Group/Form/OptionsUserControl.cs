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
    public partial class OptionsUserControl : UserControl
    {
        private bool setDifficultyButtonWasClicked = false;
        private bool monstersAvailableButtonWasClicked = false;
        public OptionsUserControl()
        {
            InitializeComponent();

            //Pipes

            Pipe1.Parent = pictureBox1;
            Pipe1.BackColor = Color.Transparent;

            Pipe2.Parent = pictureBox1;
            Pipe2.BackColor = Color.Transparent;

            Pipe3.Parent = pictureBox1;
            Pipe3.BackColor = Color.Transparent;

            Pipe57.Parent = pictureBox1;
            Pipe57.BackColor = Color.Transparent;

            //Buttons

            controlsButton.Parent = pictureBox1;
            controlsButton.BackColor = Color.Transparent;

            setDifficultyButton.Parent = pictureBox1;
            setDifficultyButton.BackColor = Color.Transparent;

            monstersAvailableButton.Parent = pictureBox1;
            monstersAvailableButton.BackColor = Color.Transparent;

            returnButton.Parent = pictureBox1;
            returnButton.BackColor = Color.Transparent;

        }

        private void returnButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void setDifficultyButton_Click(object sender, EventArgs e)
        {
            if (monstersAvailableButtonWasClicked)
            {
                monstersAvailableButtonWasClicked = false;
                this.hideMonstersButtons();
            }

            setDifficultyButton.BackColor = Color.Maroon;

            Pipe4.Parent = pictureBox1;
            Pipe4.BackColor = Color.Transparent;

            Pipe5.Parent = pictureBox1;
            Pipe5.BackColor = Color.Transparent;

            Pipe6.Parent = pictureBox1;
            Pipe6.BackColor = Color.Transparent;

            easyButton.Parent = pictureBox1;
            easyButton.BackColor = Color.Transparent;

            normalButton.Parent = pictureBox1;
            normalButton.BackColor = Color.Transparent;

            impossibleButton.Parent = pictureBox1;
            impossibleButton.BackColor = Color.Transparent;

            Pipe4.Show();
            Pipe5.Show();
            Pipe6.Show();

            easyButton.Show();
            normalButton.Show();
            impossibleButton.Show();

            setDifficultyButtonWasClicked = true;
        }

        private void easyButton_Click(object sender, EventArgs e)
        {
            //Guardar la opcion elegida
            this.setDifficultyTo("easy");
            this.hideDifficultyButtons();
        }

        private void normalButton_Click(object sender, EventArgs e)
        {
            //Guardar la opcion elegida
            this.setDifficultyTo("normal");
            this.hideDifficultyButtons();
        }

        private void impossibleButton_Click(object sender, EventArgs e)
        {
            //Guardar la opcion elegida
            this.setDifficultyTo("impossible");
            this.hideDifficultyButtons();
        }

        private void setDifficultyTo(string level)
        {
            if (level.Equals("easy"))
            {
                //este seria el default
                GameModel.notasParaGanar = 4;
                GameModel.TiempoDeAdvertencia = 4000;
                GameModel.TiempoDeGameOver = 5000;
                GameModel.TiempoSinAdvertencia = 3500;
            }
            if (level.Equals("normal"))
            {
                GameModel.notasParaGanar = 4;
                GameModel.TiempoDeAdvertencia = 2500;
                GameModel.TiempoDeGameOver = 3000;
                GameModel.TiempoSinAdvertencia = 2000;
            }
            if (level.Equals("impossible")) 
            {
                GameModel.notasParaGanar = 6;
                GameModel.TiempoDeAdvertencia=1500;
                GameModel.TiempoDeGameOver=2000;
                GameModel.TiempoSinAdvertencia=1000;
            }
        }

        private void hideDifficultyButtons()
        {
            Pipe4.Hide();
            Pipe5.Hide();
            Pipe6.Hide();
            easyButton.Hide();
            normalButton.Hide();
            impossibleButton.Hide();
            setDifficultyButton.BackColor = Color.Transparent;
        }

        private void monstersAvailableButton_Click(object sender, EventArgs e)
        {
            if (setDifficultyButtonWasClicked)
            {
                setDifficultyButtonWasClicked = false;
                this.hideDifficultyButtons();
            }

            monstersAvailableButton.BackColor = Color.Maroon;

            Pipe7.Parent = pictureBox1;
            Pipe7.BackColor = Color.Transparent;

            Pipe8.Parent = pictureBox1;
            Pipe8.BackColor = Color.Transparent;

            Pipe9.Parent = pictureBox1;
            Pipe9.BackColor = Color.Transparent;

            ghostButton.Parent = pictureBox1;
            ghostButton.BackColor = Color.Transparent;

            demonButton.Parent = pictureBox1;
            demonButton.BackColor = Color.Transparent;

            alienButton.Parent = pictureBox1;
            alienButton.BackColor = Color.Transparent;

            Pipe7.Show();
            Pipe8.Show();
            Pipe9.Show();

            ghostButton.Show();
            demonButton.Show();
            alienButton.Show();

            monstersAvailableButtonWasClicked = true;

        }

        private void ghostButton_Click(object sender, EventArgs e)
        {
            //Guardar la opcion elegida
            GameModel.monstruoActual = monstruos.CLOWN;//hay que cambiarle el nombre de ghost a Clown
            this.hideMonstersButtons();
            
        }

        private void demonButton_Click(object sender, EventArgs e)
        {
            //Guardar la opcion elegida

            GameModel.monstruoActual = monstruos.SECTARIAN;//hay que cambiarle el nombre de demon a sectarian
            this.hideMonstersButtons();
        }

        private void alienButton_Click(object sender, EventArgs e)
        {
            //Guardar la opcion elegida
            GameModel.monstruoActual = monstruos.ALIEN;
            this.hideMonstersButtons();
        }

        private void hideMonstersButtons()
        {
            Pipe7.Hide();
            Pipe8.Hide();
            Pipe9.Hide();
            ghostButton.Hide();
            demonButton.Hide();
            alienButton.Hide();
            monstersAvailableButton.BackColor = Color.Transparent;
        }

        private void controlsButton_Click(object sender, EventArgs e)
        {
            //Aca va el UserControl de los controles
            controlsUserControl1.BringToFront();
            controlsUserControl1.Show();
        }
    }
}
