namespace timednotesapppractice
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvNotes = new System.Windows.Forms.DataGridView();
            this.addbtn = new System.Windows.Forms.Button();
            this.deletebtn = new System.Windows.Forms.Button();
            this.chkHideCompleted = new System.Windows.Forms.CheckBox();
            this.editbtn = new System.Windows.Forms.Button();
            this.markAllCompletebtn = new System.Windows.Forms.Button();
            this.clearCompletedbtn = new System.Windows.Forms.Button();
            this.refreshbtn = new System.Windows.Forms.Button();
            this.exportbtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotes)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvNotes
            // 
            this.dgvNotes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNotes.Location = new System.Drawing.Point(105, 51);
            this.dgvNotes.Name = "dgvNotes";
            this.dgvNotes.Size = new System.Drawing.Size(308, 231);
            this.dgvNotes.TabIndex = 0;
            this.dgvNotes.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvNotes_CellContentClick);
            // 
            // addbtn
            // 
            this.addbtn.Location = new System.Drawing.Point(482, 51);
            this.addbtn.Name = "addbtn";
            this.addbtn.Size = new System.Drawing.Size(75, 23);
            this.addbtn.TabIndex = 1;
            this.addbtn.Text = "Add";
            this.addbtn.UseVisualStyleBackColor = true;
            this.addbtn.Click += new System.EventHandler(this.addbtn_Click_1);
            // 
            // deletebtn
            // 
            this.deletebtn.Location = new System.Drawing.Point(482, 110);
            this.deletebtn.Name = "deletebtn";
            this.deletebtn.Size = new System.Drawing.Size(75, 23);
            this.deletebtn.TabIndex = 2;
            this.deletebtn.Text = "Delete";
            this.deletebtn.UseVisualStyleBackColor = true;
            this.deletebtn.Click += new System.EventHandler(this.deletebtn_Click_1);
            // 
            // chkHideCompleted
            // 
            this.chkHideCompleted.AutoSize = true;
            this.chkHideCompleted.Location = new System.Drawing.Point(105, 25);
            this.chkHideCompleted.Name = "chkHideCompleted";
            this.chkHideCompleted.Size = new System.Drawing.Size(100, 17);
            this.chkHideCompleted.TabIndex = 3;
            this.chkHideCompleted.Text = "Hide Completed";
            this.chkHideCompleted.UseVisualStyleBackColor = true;
            this.chkHideCompleted.CheckedChanged += new System.EventHandler(this.chkHideCompleted_CheckedChanged);
            // 
            // editbtn
            // 
            this.editbtn.Location = new System.Drawing.Point(482, 80);
            this.editbtn.Name = "editbtn";
            this.editbtn.Size = new System.Drawing.Size(75, 23);
            this.editbtn.TabIndex = 4;
            this.editbtn.Text = "Edit";
            this.editbtn.UseVisualStyleBackColor = true;
            this.editbtn.Click += new System.EventHandler(this.editbtn_Click);
            // 
            // markAllCompletebtn
            // 
            this.markAllCompletebtn.Location = new System.Drawing.Point(482, 170);
            this.markAllCompletebtn.Name = "markAllCompletebtn";
            this.markAllCompletebtn.Size = new System.Drawing.Size(75, 23);
            this.markAllCompletebtn.TabIndex = 5;
            this.markAllCompletebtn.Text = "Mark All";
            this.markAllCompletebtn.UseVisualStyleBackColor = true;
            this.markAllCompletebtn.Click += new System.EventHandler(this.markAllCompletebtn_Click);
            // 
            // clearCompletedbtn
            // 
            this.clearCompletedbtn.Location = new System.Drawing.Point(482, 200);
            this.clearCompletedbtn.Name = "clearCompletedbtn";
            this.clearCompletedbtn.Size = new System.Drawing.Size(75, 23);
            this.clearCompletedbtn.TabIndex = 6;
            this.clearCompletedbtn.Text = "Clear Done";
            this.clearCompletedbtn.UseVisualStyleBackColor = true;
            this.clearCompletedbtn.Click += new System.EventHandler(this.clearCompletedbtn_Click);
            // 
            // refreshbtn
            // 
            this.refreshbtn.Location = new System.Drawing.Point(482, 260);
            this.refreshbtn.Name = "refreshbtn";
            this.refreshbtn.Size = new System.Drawing.Size(75, 23);
            this.refreshbtn.TabIndex = 7;
            this.refreshbtn.Text = "Refresh";
            this.refreshbtn.UseVisualStyleBackColor = true;
            this.refreshbtn.Click += new System.EventHandler(this.refreshbtn_Click);
            // 
            // exportbtn
            // 
            this.exportbtn.Location = new System.Drawing.Point(482, 290);
            this.exportbtn.Name = "exportbtn";
            this.exportbtn.Size = new System.Drawing.Size(75, 23);
            this.exportbtn.TabIndex = 8;
            this.exportbtn.Text = "Export";
            this.exportbtn.UseVisualStyleBackColor = true;
            this.exportbtn.Click += new System.EventHandler(this.exportbtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.exportbtn);
            this.Controls.Add(this.refreshbtn);
            this.Controls.Add(this.clearCompletedbtn);
            this.Controls.Add(this.markAllCompletebtn);
            this.Controls.Add(this.editbtn);
            this.Controls.Add(this.chkHideCompleted);
            this.Controls.Add(this.deletebtn);
            this.Controls.Add(this.addbtn);
            this.Controls.Add(this.dgvNotes);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvNotes;
        private System.Windows.Forms.Button addbtn;
        private System.Windows.Forms.Button deletebtn;
        private System.Windows.Forms.CheckBox chkHideCompleted;
        private System.Windows.Forms.Button editbtn;
        private System.Windows.Forms.Button markAllCompletebtn;
        private System.Windows.Forms.Button clearCompletedbtn;
        private System.Windows.Forms.Button refreshbtn;
        private System.Windows.Forms.Button exportbtn;
    }
}

