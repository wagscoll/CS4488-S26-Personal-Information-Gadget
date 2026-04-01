using Demo_PIG_Tool.Utils;

namespace Demo_PIG_Tool
{
    partial class TaskTrackerControl : UserControl
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            mainPanel = new Panel();
            titleLabel = new Label();
            viewEditButton = new Button();
            createTaskButton = new Button();
            createProjectButton = new Button();
            editTipLabel = new Label();
            listPanel = new Panel();
            listView = new ListView();
            idColumn = new ColumnHeader();
            typeColumn = new ColumnHeader();
            nameColumn = new ColumnHeader();
            importantColumn = new ColumnHeader();
            urgentColumn = new ColumnHeader();
            dueDateColumn = new ColumnHeader();
            hoursColumn = new ColumnHeader();
            projectIdColumn = new ColumnHeader();
            editPanel = new Panel();
            schedulePanel = new Panel();
            scheduleTextBox = new TextBox();
            twoWeeksButton = new Button();
            mainPanel.SuspendLayout();
            schedulePanel.SuspendLayout();
            SuspendLayout();
            // 
            // mainPanel
            // 
            mainPanel.BackColor = Color.FromArgb(240, 240, 240);
            mainPanel.Controls.Add(twoWeeksButton);
            mainPanel.Dock = DockStyle.Top;
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(1000, 120);
            mainPanel.TabIndex = 0;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            titleLabel.ForeColor = Color.FromArgb(0, 120, 215);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(415, 32);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Project and Task Management Tool";
            // 
            // viewEditButton
            // 
            viewEditButton.BackColor = Color.FromArgb(0, 20, 215);
            viewEditButton.FlatStyle = FlatStyle.Flat;
            viewEditButton.Font = new Font("Segoe UI", 10F);
            viewEditButton.ForeColor = Color.White;
            viewEditButton.Location = new Point(20, 60);
            viewEditButton.Name = "viewEditButton";
            viewEditButton.Size = new Size(150, 35);
            viewEditButton.TabIndex = 1;
            viewEditButton.Text = "View All";
            viewEditButton.UseVisualStyleBackColor = false;
            viewEditButton.Click += ViewEditButton_Click;
            // 
            // createTaskButton
            // 
            createTaskButton.BackColor = Color.FromArgb(0, 120, 215);
            createTaskButton.FlatStyle = FlatStyle.Flat;
            createTaskButton.Font = new Font("Segoe UI", 10F);
            createTaskButton.ForeColor = Color.White;
            createTaskButton.Location = new Point(165, 60);
            createTaskButton.Name = "createTaskButton";
            createTaskButton.Size = new Size(150, 35);
            createTaskButton.TabIndex = 2;
            createTaskButton.Text = "Create Task";
            createTaskButton.UseVisualStyleBackColor = false;
            createTaskButton.Click += CreateTaskButton_Click;
            // 
            // createProjectButton
            // 
            createProjectButton.BackColor = Color.FromArgb(0, 120, 215);
            createProjectButton.FlatStyle = FlatStyle.Flat;
            createProjectButton.Font = new Font("Segoe UI", 10F);
            createProjectButton.ForeColor = Color.White;
            createProjectButton.Location = new Point(310, 60);
            createProjectButton.Name = "createProjectButton";
            createProjectButton.Size = new Size(150, 35);
            createProjectButton.TabIndex = 3;
            createProjectButton.Text = "Create Project";
            createProjectButton.UseVisualStyleBackColor = false;
            createProjectButton.Click += CreateProjectButton_Click;
            // 
            // editTipLabel
            // 
            editTipLabel.AutoSize = true;
            editTipLabel.Font = new Font("Segoe UI", 8F);
            editTipLabel.ForeColor = Color.FromArgb(15, 15, 15);
            editTipLabel.Location = new Point(20, 100);
            editTipLabel.Name = "editTipLabel";
            editTipLabel.Size = new Size(196, 13);
            editTipLabel.TabIndex = 0;
            editTipLabel.Text = "Double click an item below to edit it.";
            // 
            // listPanel
            // 
            listPanel.Dock = DockStyle.Fill;
            listPanel.Location = new Point(0, 120);
            listPanel.Name = "listPanel";
            listPanel.Size = new Size(1000, 583);
            listPanel.TabIndex = 1;
            // 
            // listView
            // 
            listView.Columns.AddRange(new ColumnHeader[] { idColumn, typeColumn, nameColumn, importantColumn, urgentColumn, dueDateColumn, hoursColumn, projectIdColumn });
            listView.Dock = DockStyle.Fill;
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.Location = new Point(0, 120);
            listView.Name = "listView";
            listView.Size = new Size(1000, 583);
            listView.TabIndex = 0;
            listView.UseCompatibleStateImageBehavior = false;
            listView.View = View.Details;
            listView.DoubleClick += ListView_DoubleClick;
            // 
            // idColumn
            // 
            idColumn.Text = "ID";
            idColumn.Width = 50;
            // 
            // typeColumn
            // 
            typeColumn.Text = "Type";
            typeColumn.Width = 80;
            // 
            // nameColumn
            // 
            nameColumn.Text = "Name";
            nameColumn.Width = 200;
            // 
            // importantColumn
            // 
            importantColumn.Text = "Important";
            importantColumn.Width = 80;
            // 
            // urgentColumn
            // 
            urgentColumn.Text = "Urgent";
            urgentColumn.Width = 80;
            // 
            // dueDateColumn
            // 
            dueDateColumn.Text = "Due Date";
            dueDateColumn.Width = 150;
            // 
            // hoursColumn
            // 
            hoursColumn.Text = "Est. Hours";
            hoursColumn.Width = 80;
            // 
            // projectIdColumn
            // 
            projectIdColumn.Text = "Project ID";
            projectIdColumn.Width = 80;
            // 
            // editPanel
            // 
            editPanel.AutoScroll = true;
            editPanel.Dock = DockStyle.Fill;
            editPanel.Location = new Point(0, 120);
            editPanel.Name = "editPanel";
            editPanel.Size = new Size(1000, 583);
            editPanel.TabIndex = 2;
            editPanel.Visible = false;
            // 
            // schedulePanel
            // 
            schedulePanel.Controls.Add(scheduleTextBox);
            schedulePanel.Dock = DockStyle.Fill;
            schedulePanel.Location = new Point(0, 120);
            schedulePanel.Name = "schedulePanel";
            schedulePanel.Size = new Size(1000, 583);
            schedulePanel.TabIndex = 0;
            schedulePanel.Visible = false;
            // 
            // scheduleTextBox
            // 
            scheduleTextBox.Dock = DockStyle.Fill;
            scheduleTextBox.Location = new Point(0, 0);
            scheduleTextBox.Multiline = true;
            scheduleTextBox.Name = "scheduleTextBox";
            scheduleTextBox.ReadOnly = true;
            scheduleTextBox.ScrollBars = ScrollBars.Vertical;
            scheduleTextBox.Size = new Size(1000, 583);
            scheduleTextBox.TabIndex = 0;
            scheduleTextBox.WordWrap = false;
            // 
            // twoWeeksButton
            // 
            twoWeeksButton.BackColor = Color.FromArgb(0, 120, 215);
            twoWeeksButton.FlatStyle = FlatStyle.Flat;
            twoWeeksButton.Font = new Font("Segoe UI", 10F);
            twoWeeksButton.ForeColor = Color.White;
            twoWeeksButton.Location = new Point(458, 60);
            twoWeeksButton.Name = "twoWeeksButton";
            twoWeeksButton.Size = new Size(150, 35);
            twoWeeksButton.TabIndex = 4;
            twoWeeksButton.Text = "Two Week Schedule";
            twoWeeksButton.UseVisualStyleBackColor = false;
            twoWeeksButton.Click += twoWeeksButton_Click;
            // 
            // TaskTrackerControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 703);
            Controls.Add(schedulePanel);
            Controls.Add(editPanel);
            Controls.Add(listView);
            Controls.Add(listPanel);
            Controls.Add(createProjectButton);
            Controls.Add(createTaskButton);
            Controls.Add(viewEditButton);
            Controls.Add(titleLabel);
            Controls.Add(editTipLabel);
            Controls.Add(mainPanel);
            Name = "TaskTrackerControl";
            Text = "Project and Task Manager";
            mainPanel.ResumeLayout(false);
            schedulePanel.ResumeLayout(false);
            schedulePanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel mainPanel;
        private Label titleLabel;
        private Label editTipLabel;
        private Button viewEditButton;
        private Button createTaskButton;
        private Button createProjectButton;
        private Panel listPanel;
        private ListView listView;
        private ColumnHeader idColumn;
        private ColumnHeader typeColumn;
        private ColumnHeader nameColumn;
        private ColumnHeader importantColumn;
        private ColumnHeader urgentColumn;
        private ColumnHeader dueDateColumn;
        private ColumnHeader hoursColumn;
        private ColumnHeader projectIdColumn;
        private Panel editPanel;
        private Panel schedulePanel;
        private TextBox scheduleTextBox;
        private Button twoWeeksButton;
    }
}