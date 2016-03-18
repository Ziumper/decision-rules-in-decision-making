namespace Algorytmy_wyliczania_reguł_decyzyjnych
{
    partial class AWRDForm
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
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.btnWczytajPlik = new System.Windows.Forms.Button();
            this.tbSciezka = new System.Windows.Forms.TextBox();
            this.rtbSystemDecyzyjny = new System.Windows.Forms.RichTextBox();
            this.lbZawartoscPliku = new System.Windows.Forms.Label();
            this.btnExhaustive = new System.Windows.Forms.Button();
            this.btnCovering = new System.Windows.Forms.Button();
            this.btnLem2 = new System.Windows.Forms.Button();
            this.rtbWyniki = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pBładowanie = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // ofd
            // 
            this.ofd.FileName = "openFileDialog1";
            // 
            // btnWczytajPlik
            // 
            this.btnWczytajPlik.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWczytajPlik.Location = new System.Drawing.Point(410, 29);
            this.btnWczytajPlik.Name = "btnWczytajPlik";
            this.btnWczytajPlik.Size = new System.Drawing.Size(35, 20);
            this.btnWczytajPlik.TabIndex = 0;
            this.btnWczytajPlik.Text = "...";
            this.btnWczytajPlik.UseVisualStyleBackColor = true;
            this.btnWczytajPlik.Click += new System.EventHandler(this.btnWczytajPlik_Click);
            // 
            // tbSciezka
            // 
            this.tbSciezka.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbSciezka.Location = new System.Drawing.Point(0, 0);
            this.tbSciezka.Name = "tbSciezka";
            this.tbSciezka.ReadOnly = true;
            this.tbSciezka.Size = new System.Drawing.Size(457, 20);
            this.tbSciezka.TabIndex = 1;
            // 
            // rtbSystemDecyzyjny
            // 
            this.rtbSystemDecyzyjny.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbSystemDecyzyjny.Location = new System.Drawing.Point(0, 87);
            this.rtbSystemDecyzyjny.Name = "rtbSystemDecyzyjny";
            this.rtbSystemDecyzyjny.ReadOnly = true;
            this.rtbSystemDecyzyjny.Size = new System.Drawing.Size(457, 153);
            this.rtbSystemDecyzyjny.TabIndex = 2;
            this.rtbSystemDecyzyjny.Text = "";
            // 
            // lbZawartoscPliku
            // 
            this.lbZawartoscPliku.AutoSize = true;
            this.lbZawartoscPliku.Location = new System.Drawing.Point(-3, 71);
            this.lbZawartoscPliku.Name = "lbZawartoscPliku";
            this.lbZawartoscPliku.Size = new System.Drawing.Size(93, 13);
            this.lbZawartoscPliku.TabIndex = 3;
            this.lbZawartoscPliku.Text = "System decyzyjny:";
            // 
            // btnExhaustive
            // 
            this.btnExhaustive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExhaustive.Enabled = false;
            this.btnExhaustive.Location = new System.Drawing.Point(237, 28);
            this.btnExhaustive.Name = "btnExhaustive";
            this.btnExhaustive.Size = new System.Drawing.Size(75, 23);
            this.btnExhaustive.TabIndex = 4;
            this.btnExhaustive.Text = "Exhaustive";
            this.btnExhaustive.UseVisualStyleBackColor = true;
            this.btnExhaustive.Click += new System.EventHandler(this.btnExhaustive_Click);
            // 
            // btnCovering
            // 
            this.btnCovering.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCovering.Enabled = false;
            this.btnCovering.Location = new System.Drawing.Point(318, 28);
            this.btnCovering.Name = "btnCovering";
            this.btnCovering.Size = new System.Drawing.Size(75, 23);
            this.btnCovering.TabIndex = 5;
            this.btnCovering.Text = "Covering";
            this.btnCovering.UseVisualStyleBackColor = true;
            this.btnCovering.Click += new System.EventHandler(this.btnCovering_Click);
            // 
            // btnLem2
            // 
            this.btnLem2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLem2.Enabled = false;
            this.btnLem2.Location = new System.Drawing.Point(156, 28);
            this.btnLem2.Name = "btnLem2";
            this.btnLem2.Size = new System.Drawing.Size(75, 23);
            this.btnLem2.TabIndex = 6;
            this.btnLem2.Text = "LEM2";
            this.btnLem2.UseVisualStyleBackColor = true;
            this.btnLem2.Click += new System.EventHandler(this.btnLem2_Click);
            // 
            // rtbWyniki
            // 
            this.rtbWyniki.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbWyniki.BackColor = System.Drawing.SystemColors.Control;
            this.rtbWyniki.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rtbWyniki.Location = new System.Drawing.Point(0, 327);
            this.rtbWyniki.Name = "rtbWyniki";
            this.rtbWyniki.ReadOnly = true;
            this.rtbWyniki.Size = new System.Drawing.Size(457, 233);
            this.rtbWyniki.TabIndex = 7;
            this.rtbWyniki.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-3, 300);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Wyniki:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(95, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Algorytmy:";
            // 
            // pBładowanie
            // 
            this.pBładowanie.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pBładowanie.Location = new System.Drawing.Point(0, 262);
            this.pBładowanie.Maximum = 1000;
            this.pBładowanie.Name = "pBładowanie";
            this.pBładowanie.Size = new System.Drawing.Size(457, 23);
            this.pBładowanie.Step = 3;
            this.pBładowanie.TabIndex = 10;
            // 
            // AWRDForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 555);
            this.Controls.Add(this.pBładowanie);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtbWyniki);
            this.Controls.Add(this.btnLem2);
            this.Controls.Add(this.btnCovering);
            this.Controls.Add(this.btnExhaustive);
            this.Controls.Add(this.lbZawartoscPliku);
            this.Controls.Add(this.rtbSystemDecyzyjny);
            this.Controls.Add(this.tbSciezka);
            this.Controls.Add(this.btnWczytajPlik);
            this.MinimumSize = new System.Drawing.Size(398, 578);
            this.Name = "AWRDForm";
            this.Text = "Algorytmy wyliczania reguł decyzyjnych";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.Button btnWczytajPlik;
        private System.Windows.Forms.TextBox tbSciezka;
        private System.Windows.Forms.RichTextBox rtbSystemDecyzyjny;
        private System.Windows.Forms.Label lbZawartoscPliku;
        private System.Windows.Forms.Button btnExhaustive;
        private System.Windows.Forms.Button btnCovering;
        private System.Windows.Forms.Button btnLem2;
        private System.Windows.Forms.RichTextBox rtbWyniki;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar pBładowanie;
    }
}

