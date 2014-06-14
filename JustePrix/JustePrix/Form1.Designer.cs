namespace JustePrix
{
    partial class Serveur
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
            this.lbTitre = new System.Windows.Forms.Label();
            this.tbHistorique = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbTitre
            // 
            this.lbTitre.AutoSize = true;
            this.lbTitre.Font = new System.Drawing.Font("Monotype Corsiva", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitre.Location = new System.Drawing.Point(73, 26);
            this.lbTitre.Name = "lbTitre";
            this.lbTitre.Size = new System.Drawing.Size(156, 33);
            this.lbTitre.TabIndex = 0;
            this.lbTitre.Text = "Juste Prix !!!";
            this.lbTitre.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tbHistorique
            // 
            this.tbHistorique.Location = new System.Drawing.Point(12, 66);
            this.tbHistorique.Multiline = true;
            this.tbHistorique.Name = "tbHistorique";
            this.tbHistorique.ReadOnly = true;
            this.tbHistorique.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbHistorique.Size = new System.Drawing.Size(260, 163);
            this.tbHistorique.TabIndex = 1;
            // 
            // Serveur
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.tbHistorique);
            this.Controls.Add(this.lbTitre);
            this.Name = "Serveur";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FermetureServeur_Event);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbTitre;
        private System.Windows.Forms.TextBox tbHistorique;
                
    }
}

