using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FileExplorer
{
    public partial class frm1 : Form
    {
        // Author: Scott Wolfskill 
        // Created:      04/13/2015
        // Last edited:  04/15/2015
        /* A barebones file navigator for when Windows Explorer is being very slow or non-functioning.
         * Click on a folder to open it, and a file to attempt to open it.  */
        string winDir = System.Environment.GetEnvironmentVariable("windir");
        string userProfile = Environment.GetEnvironmentVariable("UserProfile");
        string currentDirectory;

        public frm1()
        {
            InitializeComponent();
            /*if (winDir.Equals("C:\\Windows") == true)
                changeDirectory("C:\\"); //Just "C:\", but since \ is escape char, must use \\ to denote \
            else
                changeDirectory(winDir); //to do: on startup, load from a txt file somewhere that has saved previous directory and frm1.size
            */
            changeDirectory(userProfile);
            setSizes();
        }

        private void frm1_SizeChanged(object sender, EventArgs e)
        {
            setSizes();
        }

        public void setSizes()
        {
            this.lblCurrentDirectory.Size = new Size(this.Width - 31, 27);
            this.txtCurrentDirectory.Size = this.lblCurrentDirectory.Size;
        }

        public void changeDirectory(string newDirectory)
        {
            //Documents -> C:\Users\davyj_000\Documents -> userProfile + "\\Documents"
            currentDirectory = newDirectory;
            this.lblCurrentDirectory.Text = newDirectory;
            this.txtCurrentDirectory.Text = newDirectory;

            cleanFolders(folders);
            cleanItems(items);
            listItems("", listFolders(""));
            //MessageBox.Show("Changed Directory");
        }

        public void cleanFolders(Folder folderRef) //remove folders from frm1 using recursion
        {
            //MessageBox.Show("Run cleanFolders");
            this.Controls.Remove(folderRef.lbl); //remove the lbl
            if (folderRef.next == null) return; //done
            cleanFolders(folderRef.next); //not null, so continue
        }

        public void cleanItems(Item itemRef) //remove files from frm1 using recursion
        {
            //MessageBox.Show("Run cleanItems");
            this.Controls.Remove(itemRef.lbl);
            if (itemRef.next == null) return; //done
            cleanItems(itemRef.next); //not null, so continue
        }

        private void lblCurrentDirectory_Click(object sender, EventArgs e)
        { //switch to txt
            this.txtCurrentDirectory.Visible = true;
            this.txtCurrentDirectory.BringToFront();
            this.txtCurrentDirectory.Select();
        }

        private void txtCurrentDirectory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) //switch to lbl
            {
                if (Directory.Exists(this.txtCurrentDirectory.Text) == true && this.txtCurrentDirectory.Text.Equals(currentDirectory) == false)
                {
                    changeDirectory(this.txtCurrentDirectory.Text);
                }
                this.txtCurrentDirectory.Visible = false;
                this.lblCurrentDirectory.BringToFront();
            }
        }

        private void lblUp_Click(object sender, EventArgs e) //Go up one directory; set to do nothing if not valid
        {
            if (currentDirectory.Length > 2)
            {
                int index = -1;
                for (int i = currentDirectory.Length - 2; i >= 0; i--) //start at end - more efficient
                {
                    if (currentDirectory.Substring(i, 1).Equals("\\") == true)
                    {

                        index = i;
                        break;
                    }
                }
                //MessageBox.Show("endIndex = " + index.ToString());
                if (index > 0)
                {
                    //MessageBox.Show(currentDirectory.Substring(0, index + 1));
                    changeDirectory(currentDirectory.Substring(0, index + 1));
                }
            }
        }

        private void btnTest1_Click(object sender, EventArgs e)
        {
            listItems("", listFolders(""));
        }

        public static Folder folders = new Folder("", new Point(0, 0), 420);
        public int listFolders(string folder) // returns # folders in current directory. listFolders("") denotes in currentFolder
        {
            //Maximum file length & folder name length: 255 chars
            if (folder.Equals("") == true) folder = currentDirectory;
            //MessageBox.Show(folder);
            string[] folderList = Directory.GetDirectories(folder);

            int modifier = 1;
            if (folder.Substring(folder.Length - 1).Equals("\\") == true) modifier = 0; //don't want to display the "\"
            if (folderList.Length > 0)
            {
                if (1 == 0) //For testing w/ a MessageBox
                {
                    string msg = "";
                    for (int i = 0; i < folderList.Length; i++)
                    {
                        if (i > 0) msg += "\n";
                        msg += folderList[i].Substring(folder.Length + modifier); //folder.Length + 1 excludes the prefix of the current folder + "\"
                    }
                    MessageBox.Show(msg);
                }
                else //Normal function - list folder names in frm1
                {
                    //this.Controls.Remove(folders.lbl);
                    folders = new Folder(folderList[0].Substring(folder.Length + modifier), new Point(1, 29), this.Width);
                    this.Controls.Add(folders.lbl);
                    folders.lbl.Click += new EventHandler(folders.lbl_Click);
                    folders.lbl.Click += new EventHandler(folder_Click);
                    folders.lbl.MouseEnter += new EventHandler(folders.lbl_MouseEnter);
                    folders.lbl.MouseLeave += new EventHandler(folders.lbl_MouseLeave);

                    Folder folderRef = folders; //pointer
                    //MessageBox.Show(Object.ReferenceEquals(folderRef, folders).ToString()); //to make sure pointer working
                    if (folderList.Length > 1) //start at next
                    {
                        for (int i = 1; i < folderList.Length; i++)
                        {
                            folderRef.next = new Folder(folderList[i].Substring(folder.Length + modifier), new Point(1, 29 + i * 24), this.Width); //works! :D
                            //MessageBox.Show(Object.ReferenceEquals(folderRef, folders).ToString()); //to make sure pointer working
                            this.Controls.Add(folderRef.next.lbl);
                            folderRef.next.lbl.Click += new EventHandler(folderRef.next.lbl_Click);
                            folderRef.next.lbl.Click += new EventHandler(folder_Click);
                            folderRef.next.lbl.MouseEnter += new EventHandler(folderRef.next.lbl_MouseEnter);
                            folderRef.next.lbl.MouseLeave += new EventHandler(folderRef.next.lbl_MouseLeave);

                            folderRef = folderRef.next; //Go on
                        }
                    }
                }
            }
            return folderList.Length;
        }

        public void folder_Click(object sender, EventArgs e) //generic - applies to every Folder object
        {
            //have to go through each Folder to figure out which one was clicked
            Folder folderRef = folders;
            while (folderRef.next != null)
            {
                if (sender.Equals(folderRef.lbl))
                    break; //found it
                folderRef = folderRef.next; //keep going
            }
            //MessageBox.Show(currentDirectory);
            if (currentDirectory.Substring(currentDirectory.Length - 1, 1).Equals("\\") == true)
            {
                //MessageBox.Show(currentDirectory + folderRef.lbl.Text);
                changeDirectory(currentDirectory + folderRef.lbl.Text);
            }
            else
            {
                //MessageBox.Show(currentDirectory + "\\" + folderRef.lbl.Text);
                changeDirectory(currentDirectory + "\\" + folderRef.lbl.Text);
            }
            //MessageBox.Show(currentDirectory);
        }

        public static Item items = new Item("", new Point(0, 0), 420);
        public int listItems(string folder, int numFolders) // returns # items in current directory. listItems("") denotes in currentFolder
        { //I call them 'Item' to avoid confusion with built-in type 'File'
            //Maximum file length & folder name length: 255 chars
            if (folder.Equals("") == true) folder = currentDirectory;
            //MessageBox.Show(folder);
            string[] itemList = Directory.GetFiles(folder);

            int modifier = 1;
            if (folder.Substring(folder.Length - 1).Equals("\\") == true) modifier = 0; //don't want to display the "\"
            if (itemList.Length > 0)
            {
                //this.Controls.Remove(items.lbl);
                items = new Item(itemList[0].Substring(folder.Length + modifier), new Point(1, 29 + numFolders * 24), this.Width);
                this.Controls.Add(items.lbl);
                items.lbl.Click += new EventHandler(items.lbl_Click);
                items.lbl.Click += new EventHandler(item_Click);
                items.lbl.MouseEnter += new EventHandler(items.lbl_MouseEnter);
                items.lbl.MouseLeave += new EventHandler(items.lbl_MouseLeave);

                Item itemRef = items; //pointer
                //MessageBox.Show(Object.ReferenceEquals(folderRef, folders).ToString()); //to make sure pointer working
                if (itemList.Length > 1) //start at next
                {
                    for (int i = 1; i < itemList.Length; i++)
                    {
                        itemRef.next = new Item(itemList[i].Substring(folder.Length + modifier), new Point(1, 29 + ((i + numFolders) * 24)), this.Width); //works! :D
                        //MessageBox.Show(Object.ReferenceEquals(folderRef, folders).ToString()); //to make sure pointer working
                        this.Controls.Add(itemRef.next.lbl);
                        itemRef.next.lbl.Click += new EventHandler(itemRef.next.lbl_Click);
                        itemRef.next.lbl.Click += new EventHandler(item_Click);
                        itemRef.next.lbl.MouseEnter += new EventHandler(itemRef.next.lbl_MouseEnter);
                        itemRef.next.lbl.MouseLeave += new EventHandler(itemRef.next.lbl_MouseLeave);

                        itemRef = itemRef.next; //Go on
                        }
                    }
            }
            return itemList.Length;
        }

        public void item_Click(object sender, EventArgs e) //generic - applies to every Item object
        {
            //have to go through each Item to figure out which one was clicked
            Item itemRef = items;
            while (itemRef.next != null)
            {
                if (sender.Equals(itemRef.lbl))
                    break; //found it
                itemRef = itemRef.next; //keep going
            }
            
            if (currentDirectory.Substring(currentDirectory.Length - 1, 1).Equals("\\") == true)
            {
                System.Diagnostics.Process.Start(currentDirectory + itemRef.lbl.Text);
            }
            else
            {
                System.Diagnostics.Process.Start(currentDirectory + "\\" + itemRef.lbl.Text);
            }
        }

        private void lbl_MouseEnter(Label lblUsing) 
        {
            lblUsing.BorderStyle = BorderStyle.FixedSingle;
            lblUsing.Location = new Point(-1, lblUsing.Location.Y);
        }

        private void lbl_MouseLeave(Label lblUsing)
        {
            lblUsing.BorderStyle = BorderStyle.None;
            lblUsing.Location = new Point(0, lblUsing.Location.Y);
        }

        private void lbl1_MouseEnter(object sender, EventArgs e)
        {
            lbl_MouseEnter(this.lbl1);
        }

        private void lbl1_MouseLeave(object sender, EventArgs e)
        {
            lbl_MouseLeave(this.lbl1);
        }

        private void lbl2_MouseEnter(object sender, EventArgs e)
        {
            lbl_MouseEnter(this.lbl2);
        }

        private void lbl2_MouseLeave(object sender, EventArgs e)
        {
            lbl_MouseLeave(this.lbl2);
        }

        

        


        

       


        

    }
}
