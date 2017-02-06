using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace FileExplorer
{ //general files listed in the program
    public class Item
    {
        public Label lbl;
        public ContextMenuStrip cms;
        public ToolStripMenuItem tsm1;
        public Item next; //Instead of array of folders, use a linkedlist of them - more efficient with memory

        public Item(string lblText, Point location, int frmWidth)
        {
            this.lbl = new Label();
            this.lbl.Text = lblText;
            this.lbl.Font = new Font("Calibri", 11.25f, FontStyle.Regular);
            //this.lbl.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular);
            this.lbl.Location = location;
            this.lbl.AutoSize = false;
            this.lbl.Size = new Size(frmWidth - 14, 23);
            //this.lbl.BorderStyle = BorderStyle.FixedSingle;
            
            this.cms = new ContextMenuStrip();
            this.tsm1 = new ToolStripMenuItem();
            this.tsm1.Text = "Open";
            this.cms.Items.Add(tsm1);
            
            this.lbl.ContextMenuStrip = this.cms;
        }

        public void lbl_Click(object sender, EventArgs e)
        {
            //C:\Users\davyj_000\Desktop\Course Registration Date.txt
            //System.Diagnostics.Process.Start("C:\\Users\\davyj_000\\Desktop\\Course Registration Date.txt");
        }

        public void lbl_MouseEnter(object sender, EventArgs e)
        {

        }

        public void lbl_MouseLeave(object sender, EventArgs e)
        {

        }
    }
}
