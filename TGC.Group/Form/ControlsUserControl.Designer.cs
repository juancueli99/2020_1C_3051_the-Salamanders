namespace TGC.Group.Form
{
    partial class ControlsUserControl
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlsUserControl));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.returnButton = new System.Windows.Forms.Button();
            this.Pipe3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1370, 705);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // returnButton
            // 
            this.returnButton.BackColor = System.Drawing.Color.Transparent;
            this.returnButton.FlatAppearance.BorderSize = 0;
            this.returnButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Maroon;
            this.returnButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.returnButton.Font = new System.Drawing.Font("Bahnschrift Condensed", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.returnButton.ForeColor = System.Drawing.Color.White;
            this.returnButton.Location = new System.Drawing.Point(33, 643);
            this.returnButton.Margin = new System.Windows.Forms.Padding(0);
            this.returnButton.Name = "returnButton";
            this.returnButton.Size = new System.Drawing.Size(135, 31);
            this.returnButton.TabIndex = 40;
            this.returnButton.Text = " > RETURN";
            this.returnButton.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.returnButton.UseVisualStyleBackColor = false;
            this.returnButton.Click += new System.EventHandler(this.returnButton_Click);
            // 
            // Pipe3
            // 
            this.Pipe3.AutoSize = true;
            this.Pipe3.Font = new System.Drawing.Font("Bahnschrift Condensed", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Pipe3.ForeColor = System.Drawing.Color.Maroon;
            this.Pipe3.Location = new System.Drawing.Point(16, 640);
            this.Pipe3.Name = "Pipe3";
            this.Pipe3.Size = new System.Drawing.Size(21, 35);
            this.Pipe3.TabIndex = 39;
            this.Pipe3.Text = "|";
            this.Pipe3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ControlsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.returnButton);
            this.Controls.Add(this.Pipe3);
            this.Controls.Add(this.pictureBox1);
            this.Name = "ControlsUserControl";
            this.Size = new System.Drawing.Size(1370, 705);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label Pipe3;
        private System.Windows.Forms.Button returnButton;
    }
}
