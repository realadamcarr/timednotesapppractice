using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace timednotesapppractice
{
    public partial class Form1 : Form
    {
        // ===== THE THREE MAIN DATA STRUCTURES =====
        // Think of these as three containers that work together:
        
        // 1. MASTER STORAGE: Contains ALL notes (never filtered)
        private readonly List<Note> _all = new List<Note>();
        
        // 2. FILTERED VIEW: Contains only notes that should be visible (filtered copy of _all)
        private readonly BindingList<Note> _view = new BindingList<Note>();
        
        // 3. BINDING CONNECTOR: Connects the filtered view to the DataGridView
        private readonly BindingSource _bs = new BindingSource();

        // ===== CSV FILE CONFIGURATION =====
        // Save file in user's Documents folder: Documents/TimedNotesApp/notes.csv
        private readonly string _dataFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "TimedNotesApp",
            "notes.csv"
        );

        // ===== FORM INITIALIZATION =====
        public Form1()
        {
            InitializeComponent();  // Creates all the UI controls (buttons, grid, etc.)
        }

        // ===== WHEN THE FORM LOADS (HAPPENS ONCE) =====
        private void Form1_Load(object sender, EventArgs e)
        {
            // STEP 1: Try to load saved data, or create sample data if no file exists
            LoadNotesFromFile();

            // STEP 2: Set up the DataGridView columns
            SetupDataGridView();

            // STEP 3: Connect the data structures together
            ConnectDataBinding();

            // STEP 4: Fill the grid with data
            ApplyFilters();
        }

        // ===== LOAD NOTES FROM CSV FILE =====
        private void LoadNotesFromFile()
        {
            try
            {
                // Check if the data file exists
                if (File.Exists(_dataFilePath))
                {
                    // Read all lines from the CSV file
                    string[] lines = File.ReadAllLines(_dataFilePath);
                    
                    // Clear existing notes
                    _all.Clear();
                    
                    // Process each line (skip header if present)
                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line) || line.StartsWith("NoteText,")) continue;
                        
                        // Parse CSV line: NoteText,NoteDate,Completed
                        string[] parts = ParseCsvLine(line);
                        if (parts.Length >= 3)
                        {
                            string noteText = parts[0];
                            DateTime noteDate = DateTime.Parse(parts[1]);
                            bool completed = bool.Parse(parts[2]);
                            
                            _all.Add(new Note(noteText, noteDate, completed));
                        }
                    }
                    
                    // Update window title to show load status
                    this.Text = $"Timed Notes App - Loaded {_all.Count} notes";
                }
                else
                {
                    // No saved file exists, create sample data for first-time users
                    CreateSampleData();
                    this.Text = "Timed Notes App - New Session";
                }
            }
            catch (Exception ex)
            {
                // If loading fails, show error and create sample data as fallback
                MessageBox.Show($"Error loading notes: {ex.Message}\nStarting with sample data.", 
                    "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CreateSampleData();
                this.Text = "Timed Notes App - Load Failed";
            }
        }

        // ===== SAVE NOTES TO CSV FILE =====
        private void SaveNotesToFile()
        {
            try
            {
                // Ensure the directory exists (create if needed)
                string directory = Path.GetDirectoryName(_dataFilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Create CSV content
                var csvLines = new List<string>();
                csvLines.Add("NoteText,NoteDate,Completed"); // Header
                
                foreach (var note in _all)
                {
                    // Escape quotes and commas in note text
                    string escapedText = EscapeCsvField(note.NoteText);
                    string dateString = note.NoteDate.ToString("yyyy-MM-dd HH:mm:ss");
                    string completedString = note.Completed.ToString();
                    
                    csvLines.Add($"{escapedText},{dateString},{completedString}");
                }

                // Write CSV to file
                File.WriteAllLines(_dataFilePath, csvLines);

                // Update window title to show save status
                this.Text = $"Timed Notes App - Saved {_all.Count} notes";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving notes: {ex.Message}", 
                    "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===== CSV HELPER METHODS =====
        private string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field)) return "\"\"";
            
            // If field contains comma, quote, or newline, wrap in quotes and escape quotes
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
            {
                return "\"" + field.Replace("\"", "\"\"") + "\"";
            }
            return field;
        }

        private string[] ParseCsvLine(string line)
        {
            var result = new List<string>();
            bool inQuotes = false;
            string currentField = "";
            
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                
                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        // Escaped quote
                        currentField += '"';
                        i++; // Skip next quote
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(currentField);
                    currentField = "";
                }
                else
                {
                    currentField += c;
                }
            }
            
            result.Add(currentField); // Add the last field
            return result.ToArray();
        }

        // ===== CREATE SAMPLE DATA (FALLBACK) =====
        private void CreateSampleData()
        {
            // Clear any existing data
            _all.Clear();
            
            // Create test notes to see how the app works
            _all.Add(new Note("lab 1", DateTime.Now.AddDays(-30), false));
            _all.Add(new Note("Completed task", DateTime.Now.AddDays(-25), true));
            _all.Add(new Note("Another note", DateTime.Now.AddDays(-20), false));
            _all.Add(new Note("Done item", DateTime.Now.AddDays(-15), true));
            _all.Add(new Note("Current task", DateTime.Now.AddDays(-10), false));
        }

        // ===== AUTO-SAVE WHEN APPLICATION CLOSES =====
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Automatically save all notes when user closes the application
            SaveNotesToFile();
            base.OnFormClosing(e);
        }

        // ===== SET UP THE DATAGRIDVIEW COLUMNS =====
        private void SetupDataGridView()
        {
            dgvNotes.Columns.Clear();           // Remove any existing columns
            dgvNotes.AutoGenerateColumns = false;  // We want manual control
            dgvNotes.AllowUserToAddRows = false;   // Disable the "new row" at bottom
            dgvNotes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;  // Select entire rows
            dgvNotes.MultiSelect = false;          // Only one row at a time

            // COLUMN 1: Note Text (what the user typed)
            dgvNotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Note.NoteText),    // Which property to show
                HeaderText = "Note",                         // Column title
                Width = 150                                  // How wide
            });

            // COLUMN 2: Completed checkbox
            dgvNotes.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(Note.Completed),   // Which property to show
                HeaderText = "Completed",                    // Column title
                Width = 90                                   // How wide
            });
        }

        // ===== CONNECT DATA STRUCTURES TO DATAGRIDVIEW =====
        private void ConnectDataBinding()
        {
            // Think of this as connecting pipes:
            // _all → (filter) → _view → _bs → dgvNotes
            
            _bs.DataSource = _view;        // BindingSource watches the filtered view
            dgvNotes.DataSource = _bs;     // DataGridView watches the BindingSource
        }

        // ===== THE FILTERING ENGINE =====
        // This copies notes from _all to _view based on filter rules
        private void ApplyFilters(IEnumerable<Note> source = null)
        {
            // STEP 1: Apply filters and sorting
            var items = (source ?? _all)   // Use provided source or default to _all
                .Where(a => !chkHideCompleted.Checked || !a.Completed)  // Hide completed if checkbox checked
                .OrderBy(a => a.NoteDate)  // Sort by date (oldest first)
                .ToList();

            // STEP 2: Update the filtered view
            _view.RaiseListChangedEvents = false;  // Pause notifications for performance
            _view.Clear();                         // Remove old items
            
            foreach (var note in items)            // Add filtered items
            {
                _view.Add(note);
            }
            
            _view.RaiseListChangedEvents = true;   // Resume notifications
            _view.ResetBindings();                 // Tell the grid to refresh
        }

        // ===== HELPER: GET NOTE FROM GRID ROW =====
        private Note GetNoteAtRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvNotes.Rows.Count)
                return null;

            // Each grid row is connected to a Note object
            return dgvNotes.Rows[rowIndex].DataBoundItem as Note;
        }

        // ===== HANDLE CLICKING ON GRID CELLS =====
        private void dgvNotes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if user clicked the checkbox column (column 1)
            if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                var note = GetNoteAtRow(e.RowIndex);
                if (note != null)
                {
                    // Toggle the completion status
                    note.Completed = !note.Completed;
                    
                    // Auto-save immediately after any change
                    SaveNotesToFile();
                    
                    // If we just completed a note and "hide completed" is on, refresh the view
                    if (chkHideCompleted.Checked && note.Completed)
                    {
                        ApplyFilters();
                    }
                }
            }
        }

        // ===== HANDLE "HIDE COMPLETED" CHECKBOX =====
        private void chkHideCompleted_CheckedChanged(object sender, EventArgs e)
        {
            // When checkbox changes, re-filter the data
            ApplyFilters();
        }

        // ===== ADD NEW NOTE BUTTON =====
        private void addbtn_Click_1(object sender, EventArgs e)
        {
            // Open the "Add Note" dialog
            using (var dlg = new NoteDetails())
            {
                dlg.Text = "Add New Note";
                
                // Show dialog and wait for user action
                if (dlg.ShowDialog(this) == DialogResult.OK && dlg.NewNote != null)
                {
                    // User clicked Save - add the new note
                    _all.Add(dlg.NewNote);     // Add to master list
                    SaveNotesToFile();         // Auto-save immediately after adding
                    ApplyFilters();            // Refresh the grid
                }
                // If user clicked Cancel, nothing happens
            }
        }

        // ===== DELETE SELECTED NOTE BUTTON =====
        private void deletebtn_Click_1(object sender, EventArgs e)
        {
            // STEP 1: Make sure a row is selected
            if (dgvNotes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a note to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // STEP 2: Get the selected note
            var selectedNote = GetNoteAtRow(dgvNotes.SelectedRows[0].Index);
            if (selectedNote == null)
            {
                MessageBox.Show("Unable to get the selected note.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // STEP 3: Ask for confirmation
            var result = MessageBox.Show(
                $"Are you sure you want to delete the note: \"{selectedNote.NoteText}\"?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            // STEP 4: Delete if confirmed
            if (result == DialogResult.Yes)
            {
                _all.Remove(selectedNote);  // Remove from master list
                SaveNotesToFile();          // Auto-save immediately after deletion
                ApplyFilters();             // Refresh the grid
                MessageBox.Show("Note deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ===== EDIT SELECTED NOTE BUTTON (NEWLY ADDED - COMMENTED OUT) =====
        private void editbtn_Click(object sender, EventArgs e)
        {
            // THIS IS NEW FUNCTIONALITY - Let's break it down step by step:
            
            // ===== STEP 1: VALIDATION =====
            // Just like delete button, we need to make sure user selected something
            
            if (dgvNotes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a note to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            

            // ===== STEP 2: GET THE SELECTED NOTE OBJECT =====
            // This gets the actual Note object from the selected grid row
           
            var selectedNote = GetNoteAtRow(dgvNotes.SelectedRows[0].Index);
            if (selectedNote == null)
            {
                MessageBox.Show("Unable to get the selected note.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            

            // ===== STEP 3: OPEN EDIT DIALOG =====
            // This is the interesting part - we reuse the same dialog as "Add Note"
            // but we pre-populate it with the existing note's text
            
            using (var dlg = new NoteDetails())
            {
                dlg.Text = "Edit Note";                           // Set dialog title
                dlg.SetInitialText(selectedNote.NoteText);        // Pre-fill with current text
                
                // Show dialog and wait for user action
                if (dlg.ShowDialog(this) == DialogResult.OK && dlg.NewNote != null)
                {
                    // ===== STEP 4: UPDATE THE EXISTING NOTE =====
                    // Instead of adding a new note, we modify the existing one
                    selectedNote.NoteText = dlg.NewNote.NoteText;  // Update text
                    selectedNote.NoteDate = DateTime.Now;          // Update timestamp
                    
                    SaveNotesToFile();         // Auto-save the changes
                    ApplyFilters();            // Refresh display
                }
            }
            
            
           
        }

        // ===== MARK ALL VISIBLE NOTES AS COMPLETE (NEWLY ADDED - COMMENTED OUT) =====
        private void markAllCompletebtn_Click(object sender, EventArgs e)
        {
            // THIS IS BULK OPERATION FUNCTIONALITY
            
            // ===== STEP 1: ASK FOR CONFIRMATION =====
            // Always confirm bulk operations to prevent accidents
            
            var result = MessageBox.Show(
                "Mark all visible notes as completed?",
                "Confirm Mark All",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // ===== STEP 2: GET VISIBLE NOTES =====
                // _view contains only the filtered/visible notes
                var visibleNotes = _view.ToList();
                int markedCount = 0;

                // ===== STEP 3: MARK EACH UNCOMPLETED NOTE =====
                foreach (var note in visibleNotes)
                {
                    if (!note.Completed)  // Only mark if not already completed
                    {
                        note.Completed = true;
                        markedCount++;
                    }
                }

                // ===== STEP 4: SAVE AND REFRESH =====
                SaveNotesToFile();         // Save changes
                ApplyFilters();            // Refresh display
                
                MessageBox.Show($"Marked {markedCount} notes as completed.", "Mark Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            
          
        }

        // ===== CLEAR ALL COMPLETED NOTES (NEWLY ADDED - COMMENTED OUT) =====
        private void clearCompletedbtn_Click(object sender, EventArgs e)
        {
            // THIS IS DESTRUCTIVE BULK OPERATION - BE VERY CAREFUL
            
            // ===== STEP 1: COUNT COMPLETED NOTES FIRST =====
            // Always show user what will be affected
            
            int completedCount = _all.Count(n => n.Completed);
            
            if (completedCount == 0)
            {
                MessageBox.Show("No completed notes to clear.", "No Completed Notes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // ===== STEP 2: STRONG CONFIRMATION WARNING =====
            var result = MessageBox.Show(
                $"Delete all {completedCount} completed notes? This cannot be undone.",
                "Confirm Clear Completed",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // ===== STEP 3: REMOVE ALL COMPLETED NOTES =====
                // RemoveAll is a powerful LINQ method that removes items matching a condition
                _all.RemoveAll(n => n.Completed);

                // ===== STEP 4: SAVE AND REFRESH =====
                SaveNotesToFile();         // Save the changes
                ApplyFilters();            // Refresh the display
                
                MessageBox.Show($"Deleted {completedCount} completed notes.", "Clear Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            
            
        }

        // ===== REFRESH DATA FROM FILE (NEWLY ADDED - COMMENTED OUT) =====
        private void refreshbtn_Click(object sender, EventArgs e)
        {
            // THIS RELOADS DATA FROM THE CSV FILE
            
            // ===== WHY WOULD YOU NEED THIS? =====
            // 1. If someone edited the CSV file externally
            // 2. If you want to undo unsaved changes (though we auto-save)
            // 3. If data got corrupted in memory
            
            
            var result = MessageBox.Show(
                "Reload notes from file? Any unsaved changes will be lost.",
                "Confirm Refresh",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // ===== RELOAD FROM FILE =====
                LoadNotesFromFile();       // This method already exists - we reuse it!
                ApplyFilters();            // Refresh the display
                
                MessageBox.Show("Notes refreshed from file.", "Refresh Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            
            
        }

        // ===== EXPORT NOTES TO CUSTOM LOCATION (NEWLY ADDED - COMMENTED OUT) =====
        private void exportbtn_Click(object sender, EventArgs e)
        {
            // THIS LETS USER SAVE NOTES TO A DIFFERENT FILE
            
            // ===== WHY EXPORT? =====
            // 1. Backup to different location
            // 2. Share notes with someone else
            // 3. Import into Excel or other programs
            
            
            // ===== STEP 1: SHOW FILE SAVE DIALOG =====
            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Title = "Export Notes";
                saveDialog.Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                saveDialog.DefaultExt = "csv";
                saveDialog.FileName = $"notes_export_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // ===== STEP 2: CREATE CSV CONTENT =====
                        // This is almost identical to SaveNotesToFile(), but saves to user's chosen location
                        var csvLines = new List<string>();
                        csvLines.Add("NoteText,NoteDate,Completed"); // Header
                        
                        foreach (var note in _all)
                        {
                            string escapedText = EscapeCsvField(note.NoteText);
                            string dateString = note.NoteDate.ToString("yyyy-MM-dd HH:mm:ss");
                            string completedString = note.Completed.ToString();
                            
                            csvLines.Add($"{escapedText},{dateString},{completedString}");
                        }

                        // ===== STEP 3: WRITE TO CHOSEN FILE =====
                        File.WriteAllLines(saveDialog.FileName, csvLines);

                        MessageBox.Show($"Successfully exported {_all.Count} notes to:\n{saveDialog.FileName}", 
                            "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error exporting notes: {ex.Message}", 
                            "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            
         
        }

        // ===== UNUSED EVENT HANDLERS (LEFT FOR REFERENCE) =====
        private void addbtn_Click(object sender, EventArgs e)
        {
            // This is unused - the real handler is addbtn_Click_1
        }

        private void deletebtn_Click(object sender, EventArgs e)
        {
            // This is unused - the real handler is deletebtn_Click_1
        }
    }
}
