namespace Solitaire.Forms
{
    partial class FrmOptions
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
            this.chkSound = new System.Windows.Forms.CheckBox();
            this.chkSave = new System.Windows.Forms.CheckBox();
            this.chkExit = new System.Windows.Forms.CheckBox();
            this.chkNew = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkProgress = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkSound
            // 
            this.chkSound.AutoSize = true;
            this.chkSound.BackColor = System.Drawing.Color.Transparent;
            this.chkSound.Location = new System.Drawing.Point(6, 21);
            this.chkSound.Name = "chkSound";
            this.chkSound.Size = new System.Drawing.Size(101, 17);
            this.chkSound.TabIndex = 0;
            this.chkSound.Text = "Play all sounds";
            this.chkSound.UseVisualStyleBackColor = false;
            // 
            // chkSave
            // 
            this.chkSave.AutoSize = true;
            this.chkSave.BackColor = System.Drawing.Color.Transparent;
            this.chkSave.Location = new System.Drawing.Point(6, 44);
            this.chkSave.Name = "chkSave";
            this.chkSave.Size = new System.Drawing.Size(262, 17);
            this.chkSave.TabIndex = 1;
            this.chkSave.Text = "Save current game on exit and resume on start";
            this.chkSave.UseVisualStyleBackColor = false;
            // 
            // chkExit
            // 
            this.chkExit.AutoSize = true;
            this.chkExit.BackColor = System.Drawing.Color.Transparent;
            this.chkExit.Location = new System.Drawing.Point(6, 44);
            this.chkExit.Name = "chkExit";
            this.chkExit.Size = new System.Drawing.Size(143, 17);
            this.chkExit.TabIndex = 2;
            this.chkExit.Text = "Closing the application";
            this.chkExit.UseVisualStyleBackColor = false;
            // 
            // chkNew
            // 
            this.chkNew.AutoSize = true;
            this.chkNew.BackColor = System.Drawing.Color.Transparent;
            this.chkNew.Location = new System.Drawing.Point(6, 21);
            this.chkNew.Name = "chkNew";
            this.chkNew.Size = new System.Drawing.Size(258, 17);
            this.chkNew.TabIndex = 3;
            this.chkNew.Text = "When starting a new game or loading a game";
            this.chkNew.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.chkExit);
            this.groupBox1.Controls.Add(this.chkNew);
            this.groupBox1.Location = new System.Drawing.Point(12, 111);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(324, 71);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Confirm:";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.chkProgress);
            this.groupBox2.Controls.Add(this.chkSound);
            this.groupBox2.Controls.Add(this.chkSave);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(324, 93);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "General:";
            // 
            // chkProgress
            // 
            this.chkProgress.AutoSize = true;
            this.chkProgress.BackColor = System.Drawing.Color.Transparent;
            this.chkProgress.Location = new System.Drawing.Point(6, 67);
            this.chkProgress.Name = "chkProgress";
            this.chkProgress.Size = new System.Drawing.Size(221, 17);
            this.chkProgress.TabIndex = 3;
            this.chkProgress.Text = "Show completed progress in statusbar";
            this.chkProgress.UseVisualStyleBackColor = false;
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.White;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(226, 204);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(110, 28);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            // 
            // FrmOptions
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 238);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmOptions";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkSound;
        private System.Windows.Forms.CheckBox chkSave;
        private System.Windows.Forms.CheckBox chkExit;
        private System.Windows.Forms.CheckBox chkNew;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkProgress;
        private System.Windows.Forms.Button btnOk;
    }
}