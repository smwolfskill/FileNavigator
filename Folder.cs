using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace FileExplorer
{ //general folders listed in the program
    public class Folder
    {
        public Label lbl;
        public ContextMenuStrip cms;
        public ToolStripMenuItem tsm1;
        public Folder next; //Instead of array of folders, use a linkedlist of them - more efficient with memory

        public Folder(string lblText, Point location, int frmWidth)
        {
            this.lbl = new Label();
            this.lbl.Text = lblText;
            this.lbl.Font = new Font("Calibri", 11.25f, FontStyle.Bold);
            //this.lbl.Font = new Font("Segoe UI", 9.75f, FontStyle.Bold);
            this.lbl.Location = location;
            this.lbl.AutoSize = false;
            this.lbl.Size = new Size(frmWidth - 14, 23);
            //this.lbl.BorderStyle = BorderStyle.FixedSingle;
            
            this.cms = new ContextMenuStrip();
            this.tsm1 = new ToolStripMenuItem();
            this.tsm1.Text = "Open Folder";
            this.cms.Items.Add(tsm1);
            
            this.lbl.ContextMenuStrip = this.cms;
        }

        public void lbl_Click(object sender, EventArgs e)
        {
            //was implemented in frm1
        }

        public void lbl_MouseEnter(object sender, EventArgs e)
        {
            /*this.lbl.BorderStyle = BorderStyle.FixedSingle;
            this.lbl.Location = new Point(this.lbl.Location.X - 1, this.lbl.Location.Y - 1);*/
        }

        public void lbl_MouseLeave(object sender, EventArgs e)
        {
            /*this.lbl.BorderStyle = BorderStyle.None;
            this.lbl.Location = new Point(this.lbl.Location.X + 1, this.lbl.Location.Y + 1);*/
        }
    }
}
