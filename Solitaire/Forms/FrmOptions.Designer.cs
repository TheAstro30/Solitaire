using Solitaire.Controls.TrackBar;

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
            this.chkEffects = new System.Windows.Forms.CheckBox();
            this.chkSave = new System.Windows.Forms.CheckBox();
            this.chkExit = new System.Windows.Forms.CheckBox();
            this.chkNew = new System.Windows.Forms.CheckBox();
            this.gbConfirm = new System.Windows.Forms.GroupBox();
            this.gbGeneral = new System.Windows.Forms.GroupBox();
            this.chkHighlight = new System.Windows.Forms.CheckBox();
            this.chkTips = new System.Windows.Forms.CheckBox();
            this.chkProgress = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.gbSound = new System.Windows.Forms.GroupBox();
            this.lblMusicVol = new System.Windows.Forms.Label();
            this.tbMusic = new Solitaire.Controls.TrackBar.TrackBarEx();
            this.lblFxVol = new System.Windows.Forms.Label();
            this.tbEffects = new Solitaire.Controls.TrackBar.TrackBarEx();
            this.chkMusic = new System.Windows.Forms.CheckBox();
            this.chkAutoTurn = new System.Windows.Forms.CheckBox();
            this.gbConfirm.SuspendLayout();
            this.gbGeneral.SuspendLayout();
            this.gbSound.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbMusic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbEffects)).BeginInit();
            this.SuspendLayout();
            // 
            // chkEffects
            // 
            this.chkEffects.AutoSize = true;
            this.chkEffects.BackColor = System.Drawing.Color.Transparent;
            this.chkEffects.Location = new System.Drawing.Point(6, 21);
            this.chkEffects.Name = "chkEffects";
            this.chkEffects.Size = new System.Drawing.Size(118, 17);
            this.chkEffects.TabIndex = 0;
            this.chkEffects.Text = "Play sound effects";
            this.chkEffects.UseVisualStyleBackColor = false;
            // 
            // chkSave
            // 
            this.chkSave.AutoSize = true;
            this.chkSave.BackColor = System.Drawing.Color.Transparent;
            this.chkSave.Location = new System.Drawing.Point(6, 90);
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
            // gbConfirm
            // 
            this.gbConfirm.BackColor = System.Drawing.Color.Transparent;
            this.gbConfirm.Controls.Add(this.chkExit);
            this.gbConfirm.Controls.Add(this.chkNew);
            this.gbConfirm.Location = new System.Drawing.Point(12, 243);
            this.gbConfirm.Name = "gbConfirm";
            this.gbConfirm.Size = new System.Drawing.Size(274, 73);
            this.gbConfirm.TabIndex = 4;
            this.gbConfirm.TabStop = false;
            this.gbConfirm.Text = "Confirm:";
            // 
            // gbGeneral
            // 
            this.gbGeneral.BackColor = System.Drawing.Color.Transparent;
            this.gbGeneral.Controls.Add(this.chkAutoTurn);
            this.gbGeneral.Controls.Add(this.chkHighlight);
            this.gbGeneral.Controls.Add(this.chkTips);
            this.gbGeneral.Controls.Add(this.chkProgress);
            this.gbGeneral.Controls.Add(this.chkSave);
            this.gbGeneral.Location = new System.Drawing.Point(12, 12);
            this.gbGeneral.Name = "gbGeneral";
            this.gbGeneral.Size = new System.Drawing.Size(274, 141);
            this.gbGeneral.TabIndex = 5;
            this.gbGeneral.TabStop = false;
            this.gbGeneral.Text = "General:";
            // 
            // chkHighlight
            // 
            this.chkHighlight.AutoSize = true;
            this.chkHighlight.BackColor = System.Drawing.Color.Transparent;
            this.chkHighlight.Location = new System.Drawing.Point(6, 113);
            this.chkHighlight.Name = "chkHighlight";
            this.chkHighlight.Size = new System.Drawing.Size(205, 17);
            this.chkHighlight.TabIndex = 5;
            this.chkHighlight.Text = "Show card highlight when dragging";
            this.chkHighlight.UseVisualStyleBackColor = false;
            // 
            // chkTips
            // 
            this.chkTips.AutoSize = true;
            this.chkTips.Location = new System.Drawing.Point(6, 67);
            this.chkTips.Name = "chkTips";
            this.chkTips.Size = new System.Drawing.Size(137, 17);
            this.chkTips.TabIndex = 4;
            this.chkTips.Text = "Show tips in statusbar";
            this.chkTips.UseVisualStyleBackColor = true;
            // 
            // chkProgress
            // 
            this.chkProgress.AutoSize = true;
            this.chkProgress.BackColor = System.Drawing.Color.Transparent;
            this.chkProgress.Location = new System.Drawing.Point(6, 44);
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
            this.btnOk.Location = new System.Drawing.Point(176, 334);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(110, 28);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            // 
            // gbSound
            // 
            this.gbSound.BackColor = System.Drawing.Color.Transparent;
            this.gbSound.Controls.Add(this.lblMusicVol);
            this.gbSound.Controls.Add(this.tbMusic);
            this.gbSound.Controls.Add(this.lblFxVol);
            this.gbSound.Controls.Add(this.tbEffects);
            this.gbSound.Controls.Add(this.chkMusic);
            this.gbSound.Controls.Add(this.chkEffects);
            this.gbSound.Location = new System.Drawing.Point(12, 159);
            this.gbSound.Name = "gbSound";
            this.gbSound.Size = new System.Drawing.Size(274, 78);
            this.gbSound.TabIndex = 7;
            this.gbSound.TabStop = false;
            this.gbSound.Text = "Sound:";
            // 
            // lblMusicVol
            // 
            this.lblMusicVol.AutoSize = true;
            this.lblMusicVol.Location = new System.Drawing.Point(145, 46);
            this.lblMusicVol.Name = "lblMusicVol";
            this.lblMusicVol.Size = new System.Drawing.Size(48, 13);
            this.lblMusicVol.TabIndex = 5;
            this.lblMusicVol.Text = "Volume:";
            // 
            // tbMusic
            // 
            this.tbMusic.Location = new System.Drawing.Point(192, 44);
            this.tbMusic.Maximum = 100;
            this.tbMusic.Name = "tbMusic";
            this.tbMusic.Size = new System.Drawing.Size(76, 45);
            this.tbMusic.TabIndex = 4;
            this.tbMusic.Tag = "MUSIC";
            this.tbMusic.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // lblFxVol
            // 
            this.lblFxVol.AutoSize = true;
            this.lblFxVol.Location = new System.Drawing.Point(3, 46);
            this.lblFxVol.Name = "lblFxVol";
            this.lblFxVol.Size = new System.Drawing.Size(48, 13);
            this.lblFxVol.TabIndex = 3;
            this.lblFxVol.Text = "Volume:";
            // 
            // tbEffects
            // 
            this.tbEffects.Location = new System.Drawing.Point(50, 44);
            this.tbEffects.Maximum = 100;
            this.tbEffects.Name = "tbEffects";
            this.tbEffects.Size = new System.Drawing.Size(76, 45);
            this.tbEffects.TabIndex = 2;
            this.tbEffects.Tag = "EFFECTS";
            this.tbEffects.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // chkMusic
            // 
            this.chkMusic.AutoSize = true;
            this.chkMusic.BackColor = System.Drawing.Color.Transparent;
            this.chkMusic.Location = new System.Drawing.Point(148, 21);
            this.chkMusic.Name = "chkMusic";
            this.chkMusic.Size = new System.Drawing.Size(79, 17);
            this.chkMusic.TabIndex = 1;
            this.chkMusic.Text = "Play music";
            this.chkMusic.UseVisualStyleBackColor = false;
            // 
            // chkAutoTurn
            // 
            this.chkAutoTurn.AutoSize = true;
            this.chkAutoTurn.BackColor = System.Drawing.Color.Transparent;
            this.chkAutoTurn.Location = new System.Drawing.Point(6, 21);
            this.chkAutoTurn.Name = "chkAutoTurn";
            this.chkAutoTurn.Size = new System.Drawing.Size(187, 17);
            this.chkAutoTurn.TabIndex = 6;
            this.chkAutoTurn.Text = "Automatically turn hidden cards";
            this.chkAutoTurn.UseVisualStyleBackColor = false;
            // 
            // FrmOptions
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 368);
            this.Controls.Add(this.gbSound);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.gbGeneral);
            this.Controls.Add(this.gbConfirm);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmOptions";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.gbConfirm.ResumeLayout(false);
            this.gbConfirm.PerformLayout();
            this.gbGeneral.ResumeLayout(false);
            this.gbGeneral.PerformLayout();
            this.gbSound.ResumeLayout(false);
            this.gbSound.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbMusic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbEffects)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkEffects;
        private System.Windows.Forms.CheckBox chkSave;
        private System.Windows.Forms.CheckBox chkExit;
        private System.Windows.Forms.CheckBox chkNew;
        private System.Windows.Forms.GroupBox gbConfirm;
        private System.Windows.Forms.GroupBox gbGeneral;
        private System.Windows.Forms.CheckBox chkProgress;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox gbSound;
        private System.Windows.Forms.CheckBox chkMusic;
        private TrackBarEx tbEffects;
        private System.Windows.Forms.Label lblFxVol;
        private System.Windows.Forms.Label lblMusicVol;
        private TrackBarEx tbMusic;
        private System.Windows.Forms.CheckBox chkTips;
        private System.Windows.Forms.CheckBox chkHighlight;
        private System.Windows.Forms.CheckBox chkAutoTurn;
    }
}