using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace jsEmitter
{
    public partial class EmitterForm : Form
    {
        public EmitterForm()
        {
            InitializeComponent();
        }

        private void btnfilepath_Click(object sender, EventArgs e)
        {
            openFileDialog1.Reset();
            openFileDialog1.InitialDirectory = jsEmitter.Properties.Settings.Default.CSDirectory;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtCs.Text = openFileDialog1.FileName;
            }
        }

        private void btnCopyTo_Click(object sender, EventArgs e)
        {

            saveFileDialog1.Reset();
            saveFileDialog1.InitialDirectory = jsEmitter.Properties.Settings.Default.JSDirectory;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtJs.Text = saveFileDialog1.FileName;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCs.Text)) return;
            jsEmitter.Properties.Settings.Default.CS = txtCs.Text;
            jsEmitter.Properties.Settings.Default.JS = txtJs.Text;
            jsEmitter.Properties.Settings.Default.CSDirectory = txtCs.Text.Substring(0, txtCs.Text.LastIndexOf('\\'));
            if (!string.IsNullOrEmpty(txtJs.Text))
                jsEmitter.Properties.Settings.Default.JSDirectory = txtJs.Text.Substring(0, txtJs.Text.LastIndexOf('\\'));
            jsEmitter.Properties.Settings.Default.Save();

            try
            {
                string res = JCompiler.Utilities.Compiler.Compile(txtCs.Text, chkAutobind.Checked);
                if (!string.IsNullOrEmpty(txtJs.Text))
                    File.WriteAllText(txtJs.Text, res);
                else Console.WriteLine(res);
                lblMsg.Text = "Successfull!!";
                Console.Beep();
            }
            catch (Exception ex)
            {

                lblMsg.Text = ex.Message;
            }
        }

        private void EmitterForm_Load(object sender, EventArgs e)
        {
            txtCs.Text = jsEmitter.Properties.Settings.Default.CS;
            txtJs.Text = jsEmitter.Properties.Settings.Default.JS;
            lblMsg.Text = "";

        }

        private void EmitterForm_MouseClick(object sender, MouseEventArgs e)
        {
            lblMsg.Text = "";
        }
    }
}
