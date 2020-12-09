using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel.Design;
using System.Globalization;

namespace LambdaText
{
    public partial class LambdaText : Form
    {
        String filename = String.Empty;
        String lastFind = String.Empty;
        PrintDocument printDocument = new PrintDocument();
        PageSettings pageSettings = new PageSettings();
        String textToPrint = String.Empty;
        int initialTextLength = 0;
        bool lineNumbersShown = false;

        public LambdaText(String file)
        {
            InitializeComponent();
            RefreshMRUList();
            this.SizeChanged += HandleSizeChange;
            if (!String.IsNullOrEmpty(file))
            {
                SetFilename(file);
                LoadFile();
            }
            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
            FormClosing += closeHandler;
            DragEnter += doDragEnter;
            DragDrop += doDragDrop;
            rtbDocument.AllowDrop = true;
            rtbDocument.DragEnter += doDragEnter;
            rtbDocument.DragDrop += doDragDrop;
        }

        /// <summary>
        /// Refreshes the most recently used file list using the current application settings.
        /// </summary>
        private void RefreshMRUList()
        {
            if (String.IsNullOrEmpty(Properties.Settings.Default.MRU1))
            {
                this.mru1ToolStripMenuItem.Visible = false;
            }
            else
            {
                this.mru1ToolStripMenuItem.Visible = true;
                this.mru1ToolStripMenuItem.Text = Properties.Settings.Default.MRU1;
            }
            if (String.IsNullOrEmpty(Properties.Settings.Default.MRU2))
            {
                this.mru2ToolStripMenuItem.Visible = false;
            }
            else
            {
                this.mru2ToolStripMenuItem.Visible = true;
                this.mru2ToolStripMenuItem.Text = Properties.Settings.Default.MRU2;
            }
            if (String.IsNullOrEmpty(Properties.Settings.Default.MRU3))
            {
                this.mru3ToolStripMenuItem.Visible = false;
            }
            else
            {
                this.mru3ToolStripMenuItem.Visible = true;
                this.mru3ToolStripMenuItem.Text = Properties.Settings.Default.MRU3;
            }
            if (String.IsNullOrEmpty(Properties.Settings.Default.MRU4))
            {
                this.mru4ToolStripMenuItem.Visible = false;
            }
            else
            {
                this.mru4ToolStripMenuItem.Visible = true;
                this.mru4ToolStripMenuItem.Text = Properties.Settings.Default.MRU4;
            }
            if (String.IsNullOrEmpty(Properties.Settings.Default.MRU5))
            {
                this.mru5ToolStripMenuItem.Visible = false;
            }
            else
            {
                this.mru5ToolStripMenuItem.Visible = true;
                this.mru5ToolStripMenuItem.Text = Properties.Settings.Default.MRU5;
            }
            if (String.IsNullOrEmpty(Properties.Settings.Default.MRU6))
            {
                this.mru6ToolStripMenuItem.Visible = false;
            }
            else
            {
                this.mru6ToolStripMenuItem.Visible = true;
                this.mru6ToolStripMenuItem.Text = Properties.Settings.Default.MRU6;
            }
            if (String.IsNullOrEmpty(Properties.Settings.Default.MRU7))
            {
                this.mru7ToolStripMenuItem.Visible = false;
            }
            else
            {
                this.mru7ToolStripMenuItem.Visible = true;
                this.mru7ToolStripMenuItem.Text = Properties.Settings.Default.MRU7;
            }
            if (String.IsNullOrEmpty(Properties.Settings.Default.MRU8))
            {
                this.mru8ToolStripMenuItem.Visible = false;
            }
            else
            {
                this.mru8ToolStripMenuItem.Visible = true;
                this.mru8ToolStripMenuItem.Text = Properties.Settings.Default.MRU8;
            }
        }

        /// <summary>
        /// Adds a file to the most recently used list if it's not already on it.  Cycles out the oldest file.
        /// </summary>
        /// <param name="filename"></param>
        private void AddMRU(String filename)
        {
            if( String.IsNullOrEmpty(filename))
            {
                return;
            }

            if (filename.Equals(Properties.Settings.Default.MRU1, StringComparison.CurrentCultureIgnoreCase) ||
                filename.Equals(Properties.Settings.Default.MRU2, StringComparison.CurrentCultureIgnoreCase) ||
                filename.Equals(Properties.Settings.Default.MRU3, StringComparison.CurrentCultureIgnoreCase) ||
                filename.Equals(Properties.Settings.Default.MRU4, StringComparison.CurrentCultureIgnoreCase) ||
                filename.Equals(Properties.Settings.Default.MRU5, StringComparison.CurrentCultureIgnoreCase) ||
                filename.Equals(Properties.Settings.Default.MRU6, StringComparison.CurrentCultureIgnoreCase) ||
                filename.Equals(Properties.Settings.Default.MRU7, StringComparison.CurrentCultureIgnoreCase) ||
                filename.Equals(Properties.Settings.Default.MRU8, StringComparison.CurrentCultureIgnoreCase))
            {
                return;
            }

            Properties.Settings.Default.MRU8 = Properties.Settings.Default.MRU7;
            Properties.Settings.Default.MRU7 = Properties.Settings.Default.MRU6;
            Properties.Settings.Default.MRU6 = Properties.Settings.Default.MRU5;
            Properties.Settings.Default.MRU5 = Properties.Settings.Default.MRU4;
            Properties.Settings.Default.MRU4 = Properties.Settings.Default.MRU3;
            Properties.Settings.Default.MRU3 = Properties.Settings.Default.MRU2;
            Properties.Settings.Default.MRU2 = Properties.Settings.Default.MRU1;
            Properties.Settings.Default.MRU1 = filename;
            Properties.Settings.Default.Save();

            RefreshMRUList();
        }

        private void doDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void doDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                rtbDocument.Text = (String)e.Data.GetData(DataFormats.Text);
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                {
                    filename = files[0];
                    LoadFile();
                }
            }
        }

        private void printDocument_PrintPage(Object sender, PrintPageEventArgs e)
        {
            int charactersOnPage = 0;
            int linesPerPage = 0;
            Font printFont = new Font("Courier New", 10);

            // Sets the value of charactersOnPage to the number of characters 
            // of stringToPrint that will fit within the bounds of the page.
            e.Graphics.MeasureString(textToPrint, printFont,
                e.MarginBounds.Size, StringFormat.GenericTypographic,
                out charactersOnPage, out linesPerPage);

            e.Graphics.DrawString(textToPrint, printFont, Brushes.Black, 
                e.MarginBounds, StringFormat.GenericTypographic);

            // Remove the portion of the string that has been printed.
            textToPrint = textToPrint.Substring(charactersOnPage);

            // Check to see if more pages are to be printed.
            e.HasMorePages = (textToPrint.Length > 0);
        }

        /// <summary>
        /// Requires the filename variable to be set to the filename to be
        /// loaded before proceeding.
        /// </summary>
        private void LoadFile()
        {
            if (String.IsNullOrEmpty(filename))
            {
                MessageBox.Show("No file to load.");
                return;
            }
            try
            {
                if (filename.Contains(".rtf"))
                {
                    rtbDocument.LoadFile(filename, RichTextBoxStreamType.RichText);
                }
                else
                {
                    rtbDocument.LoadFile(filename, RichTextBoxStreamType.PlainText);
                }
                initialTextLength = rtbDocument.Text.Length;
                AddMRU(filename);
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("File " + filename + " not found.");
                SetFilename(string.Empty);
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                MessageBox.Show("Directory containing " + filename + " not found.  Was the drive containing that file removed?");
                SetFilename(string.Empty);
            }

        }

        private void HandleSizeChange(object sender, EventArgs e)
        {
            if (!lineNumbersShown)
            {
                rtbDocument.Width = this.Width - 36;
            }
            else
            {
                // TODO: Figure out why scrollbars disappear when line numbers present.
                rtbDocument.Width = this.Width - 76;
            }
            rtbDocument.Height = this.Height - 90;
        }

        private void closeHandler(object sender, EventArgs e)
        {
            if (rtbDocument.Text.Length != initialTextLength)
            {
                if (MessageBox.Show("The original document was " + initialTextLength.ToString() + " bytes.  It is now " + rtbDocument.Text.Length.ToString() + " bytes.  Do you wish to save the changes?", "Save Changes?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (String.IsNullOrEmpty(filename))
                    {
                        saveAsToolStripMenuItem_Click(sender, e);
                    }
                    else
                    {
                        saveToolStripMenuItem_Click(sender, e);
                    }
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void closeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            rtbDocument.Clear();
            SetFilename(String.Empty);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SetFilename(dialog.FileName);
                LoadFile();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(filename))
            {
                MessageBox.Show("Document doesn't have a filename yet.  Try 'save as'.");
                return;
            }

            if (filename.Contains(".rtf"))
            {
                rtbDocument.SaveFile(filename, RichTextBoxStreamType.RichText);
            }
            else
            {
                rtbDocument.SaveFile(filename, RichTextBoxStreamType.PlainText);
            }
            initialTextLength = rtbDocument.Text.Length;

            AddMRU(filename);
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbDocument.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataObject dto = new DataObject();
            dto.SetText(rtbDocument.SelectedRtf, TextDataFormat.Rtf);
            dto.SetText(rtbDocument.SelectedText, TextDataFormat.UnicodeText);
            Clipboard.Clear();
            Clipboard.SetDataObject(dto);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbDocument.Paste();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbDocument.SelectAll();
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindForm form = new FindForm();
            form.FindText = lastFind;
            form.ShowDialog();
            if (!String.IsNullOrEmpty(form.FindText))
            {
                int startPos = 0;
                if (form.FindText == lastFind)
                {
                    startPos = rtbDocument.SelectionStart + 1;
                }
                rtbDocument.Find(form.FindText, startPos, RichTextBoxFinds.None);
                lastFind = form.FindText;
            }
        }

        private void wordCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            char[] chars = { ' ', '\t', '\r', '\n' };
            String[] words = rtbDocument.Text.Split(chars, StringSplitOptions.RemoveEmptyEntries);
            MessageBox.Show("This document has " + words.Length + " words and " +
                rtbDocument.Text.Length + " characters.", "Word count");
        }

        private void sortAlphabeticallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            char[] chars = { '\n' };
            String[] words = rtbDocument.Text.Split(chars, StringSplitOptions.RemoveEmptyEntries);
            List<String> wordList = new List<String>(words);
            wordList.Sort();
            rtbDocument.Clear();
            foreach( String item in wordList )
            {
                rtbDocument.Text += item + "\r\n";
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SetFilename(dlg.FileName);
                saveToolStripMenuItem_Click(sender, e);
            }
        }

        public void SetFilename(String name)
        {
            filename = name;
            if (!String.IsNullOrEmpty(name))
            {
                this.Text = "LambdaText - " + filename;
            }
            else
            {
                this.Text = "LambdaText";
            }
        }

        private void goToLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GotoForm form = new GotoForm();
            form.ShowDialog();
            if (form.GotoLine != -1)
            {
                int pos = rtbDocument.GetFirstCharIndexFromLine(form.GotoLine-1);
                rtbDocument.Select(pos, 0);
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rtbDocument.CanUndo)
            {
                rtbDocument.Undo();
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rtbDocument.CanRedo)
            {
                rtbDocument.Redo();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int position = rtbDocument.SelectionStart;
            if (rtbDocument.SelectionLength < 1)
            {
                rtbDocument.Text = rtbDocument.Text.Remove(rtbDocument.SelectionStart, 1);
            }
            else
            {
                rtbDocument.Text = rtbDocument.Text.Remove(rtbDocument.SelectionStart, rtbDocument.SelectionLength);
            }
            rtbDocument.SelectionStart = position;
            rtbDocument.SelectionLength = 0;
        }

        private void pageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PageSetupDialog dialog = new PageSetupDialog();
            dialog.PageSettings = pageSettings;
            dialog.AllowOrientation = true;
            dialog.AllowMargins = true;
            dialog.ShowDialog();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rtbDocument.Text.Length < 1)
            {
                MessageBox.Show("Can't print -- nothing to print.");
                return;
            }
            printDocument.DefaultPageSettings = pageSettings;
            PrintDialog dialog = new PrintDialog();
            dialog.Document = printDocument;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textToPrint = rtbDocument.Text;
                printDocument.Print();
            }
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplaceForm dialog = new ReplaceForm();
            dialog.ShowDialog();
            if (!String.IsNullOrEmpty(dialog.ReplaceText))
            {
                rtbDocument.Text = rtbDocument.Text.Replace(dialog.ReplaceText, dialog.ReplaceWith);
            }
        }

        private void findAgainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(lastFind))
            {
                int startPos = rtbDocument.SelectionStart + 1;
                rtbDocument.Find(lastFind, startPos, RichTextBoxFinds.None);
            }
        }

        private void insertDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int position = rtbDocument.SelectionStart;
            rtbDocument.Text = rtbDocument.Text.Insert(position, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            rtbDocument.SelectionStart = position + 19;
        }

        /// <summary>
        /// Wipes out the list of most recently used files.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.MRU1 = String.Empty;
            Properties.Settings.Default.MRU2 = String.Empty;
            Properties.Settings.Default.MRU3 = String.Empty;
            Properties.Settings.Default.MRU4 = String.Empty;
            Properties.Settings.Default.MRU5 = String.Empty;
            Properties.Settings.Default.MRU6 = String.Empty;
            Properties.Settings.Default.MRU7 = String.Empty;
            Properties.Settings.Default.MRU8 = String.Empty;
            RefreshMRUList();
        }

        private void mru1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filename = mru1ToolStripMenuItem.Text;
            LoadFile();
        }

        private void mru2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filename = mru2ToolStripMenuItem.Text;
            LoadFile();
        }

        private void mru3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filename = mru3ToolStripMenuItem.Text;
            LoadFile();
        }

        private void mru4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filename = mru4ToolStripMenuItem.Text;
            LoadFile();
        }

        private void mru5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filename = mru5ToolStripMenuItem.Text;
            LoadFile();
        }

        private void mru6ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filename = mru6ToolStripMenuItem.Text;
            LoadFile();
        }

        private void mru7ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filename = mru7ToolStripMenuItem.Text;
            LoadFile();
        }

        private void mru8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filename = mru8ToolStripMenuItem.Text;
            LoadFile();
        }

        private void toALLCAPSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbDocument.Text = rtbDocument.Text.ToUpper();
        }

        private void toAllLowercaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbDocument.Text = rtbDocument.Text.ToLower();
        }

        private void reverseLineOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            char[] chars = { '\n' };
            String[] words = rtbDocument.Text.Split(chars, StringSplitOptions.RemoveEmptyEntries);
            List<String> wordList = new List<String>(words);
            wordList.Reverse();
            rtbDocument.Clear();
            foreach (String item in wordList)
            {
                rtbDocument.Text += item + "\r\n";
            }
        }
    }
}
