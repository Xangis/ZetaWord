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
    public partial class FindForm : Form
    {
        String txtFind = String.Empty;
        public FindForm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtFind = String.Empty;
            Close();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            txtFind = txtFindText.Text; 
            Close();
        }

        public string FindText
        {
            get
            {
                return txtFind;
            }
            set
            {
                txtFind = value;
                txtFindText.Text = value;
            }
        }

    }
}
