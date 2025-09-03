using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timednotesapppractice
{
    // ===== THE NOTE CLASS =====
    // Represents a single note item with automatic UI update capability
    public class Note : INotifyPropertyChanged
    {
        // ===== PRIVATE BACKING FIELDS =====
        // These store the actual data - the public properties use these
        private string _noteText;
        private DateTime _noteDate;
        private bool _completed;

        // ===== PUBLIC PROPERTIES WITH CHANGE NOTIFICATION =====
        
        // The text content of the note
        public string NoteText
        {
            get => _noteText;
            set
            {
                if (_noteText != value)  // Only update if value actually changed
                {
                    _noteText = value;
                    OnPropertyChanged(nameof(NoteText));  // Tell UI to update
                }
            }
        }

        // When the note was created
        public DateTime NoteDate
        {
            get => _noteDate;
            set
            {
                if (_noteDate != value)  // Only update if value actually changed
                {
                    _noteDate = value;
                    OnPropertyChanged(nameof(NoteDate));  // Tell UI to update
                }
            }
        }

        // Whether the note is completed (checkbox state)
        public bool Completed
        {
            get => _completed;
            set
            {
                if (_completed != value)  // Only update if value actually changed
                {
                    _completed = value;
                    OnPropertyChanged(nameof(Completed));  // Tell UI to update
                }
            }
        }

        // ===== CONSTRUCTORS =====
        
        // Empty constructor (creates blank note)
        public Note() { }

        // Full constructor (creates note with all values)
        public Note(string noteText, DateTime noteDate, bool completed)
        {
            _noteText = noteText;
            _noteDate = noteDate;
            _completed = completed;
        }

        // ===== CHANGE NOTIFICATION SYSTEM =====
        // This is what makes the DataGridView update automatically when properties change
        
        // Event that the UI subscribes to
        public event PropertyChangedEventHandler PropertyChanged;

        // Method to fire the event when a property changes
        protected virtual void OnPropertyChanged(string propertyName)
        {
            // The ?. means "only call if PropertyChanged is not null"
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
