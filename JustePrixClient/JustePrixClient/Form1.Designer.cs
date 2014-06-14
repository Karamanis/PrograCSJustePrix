namespace JustePrixClient
{
    partial class InterfaceJoueur
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.pbVoyage = new System.Windows.Forms.PictureBox();
            this.lbHorloge = new System.Windows.Forms.Label();
            this.pbMobilier = new System.Windows.Forms.PictureBox();
            this.pbVoiture = new System.Windows.Forms.PictureBox();
            this.btEnvoyer = new System.Windows.Forms.Button();
            this.tbRecvMesg = new System.Windows.Forms.TextBox();
            this.tbEnvoiMsg = new System.Windows.Forms.TextBox();
            this.lbVoiture = new System.Windows.Forms.Label();
            this.lbMobilier = new System.Windows.Forms.Label();
            this.lbVoyage = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbVoyage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMobilier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbVoiture)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Monotype Corsiva", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(266, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(244, 28);
            this.label1.TabIndex = 15;
            this.label1.Text = "Gagnez ces 3 cadeaux !!!";
            // 
            // pbVoyage
            // 
            this.pbVoyage.Location = new System.Drawing.Point(522, 103);
            this.pbVoyage.Name = "pbVoyage";
            this.pbVoyage.Size = new System.Drawing.Size(204, 153);
            this.pbVoyage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbVoyage.TabIndex = 14;
            this.pbVoyage.TabStop = false;
            // 
            // lbHorloge
            // 
            this.lbHorloge.AutoSize = true;
            this.lbHorloge.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbHorloge.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbHorloge.ForeColor = System.Drawing.Color.Red;
            this.lbHorloge.Location = new System.Drawing.Point(556, 273);
            this.lbHorloge.Name = "lbHorloge";
            this.lbHorloge.Size = new System.Drawing.Size(106, 73);
            this.lbHorloge.TabIndex = 13;
            this.lbHorloge.Text = "60";
            // 
            // pbMobilier
            // 
            this.pbMobilier.Location = new System.Drawing.Point(271, 103);
            this.pbMobilier.Name = "pbMobilier";
            this.pbMobilier.Size = new System.Drawing.Size(204, 153);
            this.pbMobilier.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbMobilier.TabIndex = 12;
            this.pbMobilier.TabStop = false;
            // 
            // pbVoiture
            // 
            this.pbVoiture.Location = new System.Drawing.Point(12, 103);
            this.pbVoiture.Name = "pbVoiture";
            this.pbVoiture.Size = new System.Drawing.Size(204, 153);
            this.pbVoiture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbVoiture.TabIndex = 11;
            this.pbVoiture.TabStop = false;
            // 
            // btEnvoyer
            // 
            this.btEnvoyer.Location = new System.Drawing.Point(716, 391);
            this.btEnvoyer.Name = "btEnvoyer";
            this.btEnvoyer.Size = new System.Drawing.Size(75, 23);
            this.btEnvoyer.TabIndex = 10;
            this.btEnvoyer.Text = "Envoyer";
            this.btEnvoyer.UseVisualStyleBackColor = true;
            this.btEnvoyer.Click += new System.EventHandler(this.btEnvoyer_Click);
            // 
            // tbRecvMesg
            // 
            this.tbRecvMesg.Location = new System.Drawing.Point(12, 273);
            this.tbRecvMesg.Multiline = true;
            this.tbRecvMesg.Name = "tbRecvMesg";
            this.tbRecvMesg.ReadOnly = true;
            this.tbRecvMesg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbRecvMesg.Size = new System.Drawing.Size(324, 141);
            this.tbRecvMesg.TabIndex = 9;
            // 
            // tbEnvoiMsg
            // 
            this.tbEnvoiMsg.Location = new System.Drawing.Point(416, 394);
            this.tbEnvoiMsg.Name = "tbEnvoiMsg";
            this.tbEnvoiMsg.Size = new System.Drawing.Size(284, 20);
            this.tbEnvoiMsg.TabIndex = 8;
            this.tbEnvoiMsg.TextChanged += new System.EventHandler(this.checkChar);
            this.tbEnvoiMsg.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Enter_Pressed);
            // 
            // lbVoiture
            // 
            this.lbVoiture.AutoSize = true;
            this.lbVoiture.Font = new System.Drawing.Font("Monotype Corsiva", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbVoiture.ForeColor = System.Drawing.Color.Blue;
            this.lbVoiture.Location = new System.Drawing.Point(13, 84);
            this.lbVoiture.Name = "lbVoiture";
            this.lbVoiture.Size = new System.Drawing.Size(59, 18);
            this.lbVoiture.TabIndex = 16;
            this.lbVoiture.Text = "Voiture";
            // 
            // lbMobilier
            // 
            this.lbMobilier.AutoSize = true;
            this.lbMobilier.Font = new System.Drawing.Font("Monotype Corsiva", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMobilier.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lbMobilier.Location = new System.Drawing.Point(271, 83);
            this.lbMobilier.Name = "lbMobilier";
            this.lbMobilier.Size = new System.Drawing.Size(65, 18);
            this.lbMobilier.TabIndex = 17;
            this.lbMobilier.Text = "Mobilier";
            // 
            // lbVoyage
            // 
            this.lbVoyage.AutoSize = true;
            this.lbVoyage.Font = new System.Drawing.Font("Monotype Corsiva", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbVoyage.ForeColor = System.Drawing.Color.ForestGreen;
            this.lbVoyage.Location = new System.Drawing.Point(522, 82);
            this.lbVoyage.Name = "lbVoyage";
            this.lbVoyage.Size = new System.Drawing.Size(55, 18);
            this.lbVoyage.TabIndex = 18;
            this.lbVoyage.Text = "Voyage";
            // 
            // InterfaceJoueur
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(803, 426);
            this.Controls.Add(this.lbVoyage);
            this.Controls.Add(this.lbMobilier);
            this.Controls.Add(this.lbVoiture);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbVoyage);
            this.Controls.Add(this.lbHorloge);
            this.Controls.Add(this.pbMobilier);
            this.Controls.Add(this.pbVoiture);
            this.Controls.Add(this.btEnvoyer);
            this.Controls.Add(this.tbRecvMesg);
            this.Controls.Add(this.tbEnvoiMsg);
            this.Name = "InterfaceJoueur";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Closing_Event);
            ((System.ComponentModel.ISupportInitialize)(this.pbVoyage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMobilier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbVoiture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pbVoyage;
        private System.Windows.Forms.Label lbHorloge;
        private System.Windows.Forms.PictureBox pbMobilier;
        private System.Windows.Forms.PictureBox pbVoiture;
        private System.Windows.Forms.Button btEnvoyer;
        private System.Windows.Forms.TextBox tbRecvMesg;
        private System.Windows.Forms.TextBox tbEnvoiMsg;
        private System.Windows.Forms.Label lbVoiture;
        private System.Windows.Forms.Label lbMobilier;
        private System.Windows.Forms.Label lbVoyage;

    }
}

