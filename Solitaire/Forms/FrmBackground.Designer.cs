namespace Solitaire.Forms
{
    partial class FrmBackground
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
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.lvFiles = new libolv.ObjectListView();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lvFiles)).BeginInit();
            this.SuspendLayout();
            // 
            // pbPreview
            // 
            this.pbPreview.BackColor = System.Drawing.Color.White;
            this.pbPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbPreview.Location = new System.Drawing.Point(179, 12);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(368, 267);
            this.pbPreview.TabIndex = 1;
            this.pbPreview.TabStop = false;
            // 
            // lvFiles
            // 
            this.lvFiles.HideSelection = false;
            this.lvFiles.Location = new System.Drawing.Point(12, 12);
            this.lvFiles.MultiSelect = false;
            this.lvFiles.Name = "lvFiles";
            this.lvFiles.Size = new System.Drawing.Size(161, 267);
            this.lvFiles.TabIndex = 2;
            this.lvFiles.UseCompatibleStateImageBehavior = false;
            this.lvFiles.View = System.Windows.Forms.View.Details;
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = global::Solitaire.Properties.Resources.button_cancel;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(321, 298);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 28);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnOk
            // 
            this.btnOk.BackgroundImage = global::Solitaire.Properties.Resources.button_ok;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Location = new System.Drawing.Point(437, 298);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(110, 28);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            // 
            // FrmBackground
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(559, 332);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lvFiles);
            this.Controls.Add(this.pbPreview);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBackground";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose Background";
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lvFiles)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pbPreview;
        private libolv.ObjectListView lvFiles;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
    }
}