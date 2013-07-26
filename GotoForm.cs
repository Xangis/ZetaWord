using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZetaWord
{
    public partial class GotoForm : Form
    {
        int gotoLine;

        public GotoForm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            gotoLine = -1;
            Close();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            gotoLine = -1;
            Int32.TryParse(txtGotoText.Text, out gotoLine);
            Close();
        }

        public int GotoLine
        {
            get
            {
                return gotoLine;
            }
            set
            {
                gotoLine = value;
                txtGotoText.Text = gotoLine.ToString();
            }
        }
    }
}
