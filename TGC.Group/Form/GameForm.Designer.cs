namespace TGC.Group.Form
{
    partial class GameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel3D = new System.Windows.Forms.Panel();
            this.menuUserControl1 = new TGC.Group.Form.MenuUserControl();
            this.gameOverUserControl1 = new TGC.Group.Form.GameOverUserControl();
            this.youSurvivedUserControl1 = new TGC.Group.Form.YouSurvivedUserControl();
            this.panel3D.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3D
            // 
            this.panel3D.Controls.Add(this.menuUserControl1);
            this.panel3D.Controls.Add(this.gameOverUserControl1);
            this.panel3D.Controls.Add(this.youSurvivedUserControl1);
            this.panel3D.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3D.Location = new System.Drawing.Point(0, 0);
            this.panel3D.Name = "panel3D";
            this.panel3D.Size = new System.Drawing.Size(784, 561);
            this.panel3D.TabIndex = 0;
            // 
            // menuUserControl1
            // 
            this.menuUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuUserControl1.Location = new System.Drawing.Point(0, 0);
            this.menuUserControl1.Name = "menuUserControl1";
            this.menuUserControl1.Size = new System.Drawing.Size(784, 561);
            this.menuUserControl1.TabIndex = 0;
            // 
            // gameOverUserControl1
            // 
            this.gameOverUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameOverUserControl1.Location = new System.Drawing.Point(0, 0);
            this.gameOverUserControl1.Name = "gameOverUserControl1";
            this.gameOverUserControl1.Size = new System.Drawing.Size(784, 561);
            this.gameOverUserControl1.TabIndex = 1;
            // 
            // youSurvivedUserControl1
            // 
            this.youSurvivedUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.youSurvivedUserControl1.Location = new System.Drawing.Point(0, 0);
            this.youSurvivedUserControl1.Name = "youSurvivedUserControl1";
            this.youSurvivedUserControl1.Size = new System.Drawing.Size(784, 561);
            this.youSurvivedUserControl1.TabIndex = 2;
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.panel3D);
            this.Name = "GameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameForm_FormClosing);
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.panel3D.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3D;
        private MenuUserControl menuUserControl1;
        private GameOverUserControl gameOverUserControl1;
        private YouSurvivedUserControl youSurvivedUserControl1;
    }
}

