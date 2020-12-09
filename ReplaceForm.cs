using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LambdaText
{
    public partial class ReplaceForm : Form
    {
        String txtReplace = String.Empty;
        String txtReplaceWith = String.Empty;
        public ReplaceForm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtReplace = String.Empty;
            Close();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            txtReplace = txtReplaceText.Text;
            txtReplaceWith = txtReplaceWithBox.Text;
            Close();
        }

        public string ReplaceText
        {
            get
            {
                return txtReplace;
            }
            set
            {
                txtReplace = value;
                txtReplaceText.Text = value;
            }
        }

        public string ReplaceWith
        {
            get
            {
                return txtReplaceWith;
            }
            set
            {
                txtReplaceWith = value;
                txtReplaceWithBox.Text = value;
            }
        }
    }
}
