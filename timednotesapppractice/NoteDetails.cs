using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace timednotesapppractice
{
    // ===== ADD/EDIT NOTE DIALOG =====
    // This popup window lets users create new notes or edit existing ones
    public partial class NoteDetails : Form
    {
        // ===== THE RESULT PROPERTY =====
        // After user clicks Save, this contains the new note they created
        public Note NewNote { get; private set; }

        // ===== FORM INITIALIZATION =====
        public NoteDetails()
        {
            InitializeComponent();  // Create all the UI controls

            // Set up dialog behavior
            cancelbtn.DialogResult = DialogResult.Cancel;  // Cancel button closes with Cancel result
            this.CancelButton = cancelbtn;                 // ESC key triggers Cancel button
            this.AcceptButton = savebtn;                   // ENTER key triggers Save button
        }

        // ===== SET INITIAL TEXT FOR EDITING (NEWLY ADDED - COMMENTED OUT) =====
        public void SetInitialText(string noteText)
        {
            // THIS METHOD ALLOWS THE DIALOG TO BE USED FOR EDITING EXISTING NOTES
            // HOW IT WORKS:
            // 1. The Edit button calls this method before showing the dialog
            // 2. It pre-fills the text box with the current note text
            // 3. SelectAll() highlights the text so user can easily replace it
            
            
            if (txtnote != null)
            {
                txtnote.Text = noteText;       // Put existing text in the textbox
                txtnote.SelectAll();           // Highlight all text for easy editing
            }
            
            
           
        }

        // ===== SAVE BUTTON CLICKED =====
        private void savebtn_Click_1(object sender, EventArgs e)
        {
            // STEP 1: Validate user input
            if (string.IsNullOrWhiteSpace(txtnote.Text))
            {
                MessageBox.Show("Please enter a note.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtnote.Focus();    // Put cursor back in the text box
                return;             // Don't close dialog - let user fix the error
            }

            // STEP 2: Create the new note object
            NewNote = new Note
            {
                NoteText = txtnote.Text,    // What the user typed
                NoteDate = DateTime.Now     // Current date/time
                // Completed defaults to false (not completed)
            };

            // STEP 3: Close the dialog successfully
            this.DialogResult = DialogResult.OK;  // Tell caller we succeeded
            this.Close();                         // Close the dialog
        }

        // ===== UNUSED EVENT HANDLERS (LEFT FOR REFERENCE) =====
        private void NoteDetails_Load(object sender, EventArgs e)
        {
            // This fires when the dialog loads - currently unused
        }

        private void savebtn_Click(object sender, EventArgs e)
        {
            // This is unused - the real handler is savebtn_Click_1
        }
    }
}
