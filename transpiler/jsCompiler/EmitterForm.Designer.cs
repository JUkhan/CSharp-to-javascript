namespace jsEmitter
{
    partial class EmitterForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCopyTo = new System.Windows.Forms.Button();
            this.btnfilepath = new System.Windows.Forms.Button();
            this.txtJs = new System.Windows.Forms.TextBox();
            this.txtCs = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.lblMsg = new System.Windows.Forms.Label();
            this.chkAutobind = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "JS File";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Cs File ";
            // 
            // btnCopyTo
            // 
            this.btnCopyTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyTo.Location = new System.Drawing.Point(572, 45);
            this.btnCopyTo.Name = "btnCopyTo";
            this.btnCopyTo.Size = new System.Drawing.Size(75, 23);
            this.btnCopyTo.TabIndex = 12;
            this.btnCopyTo.Text = "Browse";
            this.btnCopyTo.UseVisualStyleBackColor = true;
            this.btnCopyTo.Click += new System.EventHandler(this.btnCopyTo_Click);
            // 
            // btnfilepath
            // 
            this.btnfilepath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnfilepath.Location = new System.Drawing.Point(572, 16);
            this.btnfilepath.Name = "btnfilepath";
            this.btnfilepath.Size = new System.Drawing.Size(75, 23);
            this.btnfilepath.TabIndex = 11;
            this.btnfilepath.Text = "Browse";
            this.btnfilepath.UseVisualStyleBackColor = true;
            this.btnfilepath.Click += new System.EventHandler(this.btnfilepath_Click);
            // 
            // txtJs
            // 
            this.txtJs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtJs.Location = new System.Drawing.Point(74, 44);
            this.txtJs.Name = "txtJs";
            this.txtJs.Size = new System.Drawing.Size(492, 20);
            this.txtJs.TabIndex = 10;
            // 
            // txtCs
            // 
            this.txtCs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCs.Location = new System.Drawing.Point(74, 18);
            this.txtCs.Name = "txtCs";
            this.txtCs.Size = new System.Drawing.Size(492, 20);
            this.txtCs.TabIndex = 9;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(136, 140);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(369, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "Compile";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Location = new System.Drawing.Point(77, 99);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(35, 13);
            this.lblMsg.TabIndex = 15;
            this.lblMsg.Text = "label3";
            // 
            // chkAutobind
            // 
            this.chkAutobind.AutoSize = true;
            this.chkAutobind.Checked = true;
            this.chkAutobind.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutobind.Location = new System.Drawing.Point(74, 71);
            this.chkAutobind.Name = "chkAutobind";
            this.chkAutobind.Size = new System.Drawing.Size(72, 17);
            this.chkAutobind.TabIndex = 16;
            this.chkAutobind.Text = "Auto Bind";
            this.chkAutobind.UseVisualStyleBackColor = true;
            // 
            // EmitterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 175);
            this.Controls.Add(this.chkAutobind);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCopyTo);
            this.Controls.Add(this.btnfilepath);
            this.Controls.Add(this.txtJs);
            this.Controls.Add(this.txtCs);
            this.Controls.Add(this.btnOK);
            this.Name = "EmitterForm";
            this.Text = "jsEmitter";
            this.Load += new System.EventHandler(this.EmitterForm_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.EmitterForm_MouseClick);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCopyTo;
        private System.Windows.Forms.Button btnfilepath;
        private System.Windows.Forms.TextBox txtJs;
        private System.Windows.Forms.TextBox txtCs;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.CheckBox chkAutobind;
    }
}