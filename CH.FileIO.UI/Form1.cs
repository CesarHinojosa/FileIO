using System.DirectoryServices;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace CH.FileIO.UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Global Variables
        bool FirstSave = true;
        //file  name uknown due to no file being created or opened
        string FILE_NAME = "Untitled";
        //starting file path unknown due to no file being created or opened
        string FILE_PATH = "";
        

        //copied from code worked together
        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtDocument.WordWrap = mnuWordWrap.Checked;
        }

        //it works but it doesn't as well
        //the no button has to be selected twice
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            if (staFile.Text != "")
            {
                FirstSave = true;
                Application.Exit();

            }
            else if (staFile.Text == "")
            {
                DialogResult result = MessageBox.Show("Would you like to save the changes?", "Alert Box", MessageBoxButtons.YesNoCancel);

                //if the user does want to save changes it calls the mnuSaveAs_Click function 
                if (result == DialogResult.Yes)
                {

                    mnuSaveAs_Click(sender, e);
                    FirstSave = true;
                }
                //if cancel it closes the form 
                else if (result == DialogResult.Cancel)
                {
                    FirstSave = true;
                    
                }
                else if (result == DialogResult.No)
                {
                    txtDocument.Clear();
                    this.Text = "Untitled";
                    staFile.Text = "";
                }
            }
        }

        //when the form is loaded the following happens
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = FILE_NAME;
            staDateTime.Text = DateTime.Now.ToString();
        }

        
        private void mnuNew_Click(object sender, EventArgs e)
        {
            //if the first save IS NOT TRUE
            if (!FirstSave)
            {
                //Ask the user if they would like to save
                DialogResult dlgResult = MessageBox.Show("Would you like to save?", "Unsaved Changed", MessageBoxButtons.YesNo);
                if (dlgResult == DialogResult.Yes)
                {
                    mnuSaveAs_Click(sender, e);
                    FirstSave = true;
                }
                //if user says no it clears everything
                if (dlgResult == DialogResult.No)
                {
                    txtDocument.Clear();
                    this.Text = "Untitled";
                    staFile.Text = "";
       
                }
            } 
            //if the file is saved and the user exits it clears everything 
            else if (FirstSave)
            {
                txtDocument.Clear();
                this.Text = "Untitled";
                staFile.Text = "";
            }

        }
        private void mnuOpen_Click(object sender, EventArgs e)
        {

            //this creates a new File Dialog
            OpenFileDialog dlgOpen = new OpenFileDialog();

            //The filter on the bottom right
            dlgOpen.Filter = "Text Files|*.txt|AllFiles|*.*";

            //This shows the Open Result File
            DialogResult dlgResult = dlgOpen.ShowDialog();

            if (dlgResult == DialogResult.OK)
            {

                using (StreamReader reader = new StreamReader(dlgOpen.FileName))
                {
                    txtDocument.Text = reader.ReadToEnd();
                    //the file path is converted to the absolute directory 
                    //THIS IS NEEDED FOR IT TO NOT CREATE A NEW FILE EVERYTIME YOU OPEN IT AND TRY TO SAVE IT
                    FILE_PATH = dlgOpen.FileName;

                    FILE_NAME = dlgOpen.SafeFileName;

                    //The Form will be called to whatever the the user saved the FILE_NAME as
                    this.Text = FILE_NAME;
                    //this ties back to the File_path, the status strip that contains the file outputs a string of the File Path.
                    staFile.Text = FILE_PATH;
                    FirstSave = true;

                }
            }
        }
        private void mnuSave_Click(object sender, EventArgs e)
        {   
            //if the FILE_PATH 'IS NOT' a string 
            if (FILE_PATH != "") 
            {
                //then the following text will be passed through and saved onto the FILE_PATH (which has already been created)
                //The file path helps the computer know "hey this file exists ill just save the changes to the new file" instead of trying to create a new file 
                using (StreamWriter writer = new StreamWriter(FILE_PATH))
                {
                    writer.Write(txtDocument.Text);
                    //The file name of the text 
                    //whatever the user put the file name in the (SAVE_AS method) it will be outputed onto the form
                    this.Text = FILE_NAME;
                    //the FILE_PATH that was taken from (SAVE_AS method) will be inserted onto the status strip file
                    staFile.Text = FILE_PATH;
                    //due to the user manually saving this and the file already created (the First Save is true)
                    FirstSave = true;          
                }  
            }
            //if the user has not saved the following text then it will push it to the Save As Method
            else
            {
                mnuSaveAs_Click(sender, e);
            }
        }

        //This saves the text to a file onto your computer
        private void mnuSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlgSaveAs = new SaveFileDialog();
            dlgSaveAs.Filter = "Text Files|*.txt|AllFiles|*.*";
            DialogResult dlgResult = dlgSaveAs.ShowDialog();

            if (dlgResult == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(dlgSaveAs.FileName))
                {
                    writer.Write(txtDocument.Text);

                    FILE_PATH = dlgSaveAs.FileName;

                    FILE_NAME = Path.GetFileName(dlgSaveAs.FileName);

                    this.Text = FILE_NAME;
                    staFile.Text = FILE_PATH;
                    FirstSave = true;
                }
            }
        }

        private void txtDocument_TextChanged(object sender, EventArgs e)
        {
            FirstSave = false; 
        }
    }
}