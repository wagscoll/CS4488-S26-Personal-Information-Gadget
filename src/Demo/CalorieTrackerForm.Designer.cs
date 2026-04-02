namespace Demo_PIG_Tool
{
    partial class CalorieTrackerControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            mainPanel = new Panel();
            titleLabel = new Label();
            viewLogsButton = new Button();
            logTodayButton = new Button();
            editTipLabel = new Label();
            listView = new ListView();
            dateColumn = new ColumnHeader();
            weightColumn = new ColumnHeader();
            caloriesColumn = new ColumnHeader();
            editPanel = new Panel();
            logFormTitleLabel = new Label();
            dateLabel = new Label();
            dateTextBox = new TextBox();
            weightLabel = new Label();
            weightTextBox = new TextBox();
            caloriesLabel = new Label();
            caloriesTextBox = new TextBox();
            saveButton = new Button();
            cancelButton = new Button();
            editEntryButton = new Button();
            deleteEntryButton = new Button();
            mainPanel.SuspendLayout();
            editPanel.SuspendLayout();
            SuspendLayout();

            // mainPanel
            mainPanel.BackColor = Color.FromArgb(240, 240, 240);
            mainPanel.Dock = DockStyle.Top;
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(1000, 120);
            mainPanel.TabIndex = 0;
            mainPanel.Paint += mainPanel_Paint;

            // titleLabel
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            titleLabel.ForeColor = Color.FromArgb(0, 120, 215);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Name = "titleLabel";
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Health Tracking Tool";

            // viewLogsButton
            viewLogsButton.BackColor = Color.FromArgb(0, 20, 215);
            viewLogsButton.FlatStyle = FlatStyle.Flat;
            viewLogsButton.Font = new Font("Segoe UI", 10F);
            viewLogsButton.ForeColor = Color.White;
            viewLogsButton.Location = new Point(20, 65);
            viewLogsButton.Name = "viewLogsButton";
            viewLogsButton.Size = new Size(150, 35);
            viewLogsButton.TabIndex = 1;
            viewLogsButton.Text = "View Logs";
            viewLogsButton.UseVisualStyleBackColor = false;
            viewLogsButton.Click += ViewLogsButton_Click;

            // logTodayButton
            logTodayButton.BackColor = Color.FromArgb(0, 20, 215);
            logTodayButton.FlatStyle = FlatStyle.Flat;
            logTodayButton.Font = new Font("Segoe UI", 10F);
            logTodayButton.ForeColor = Color.White;
            logTodayButton.Location = new Point(175, 65);
            logTodayButton.Name = "logTodayButton";
            logTodayButton.Size = new Size(150, 35);
            logTodayButton.TabIndex = 2;
            logTodayButton.Text = "Log Today's Data";
            logTodayButton.UseVisualStyleBackColor = false;
            logTodayButton.Click += LogTodayButton_Click;

            // editEntryButton
            editEntryButton.BackColor = Color.ForestGreen;
            editEntryButton.FlatStyle = FlatStyle.Flat;
            editEntryButton.Font = new Font("Segoe UI", 10F);
            editEntryButton.ForeColor = Color.White;
            editEntryButton.Location = new Point(340, 65);
            editEntryButton.Name = "editEntryButton";
            editEntryButton.Size = new Size(120, 35);
            editEntryButton.TabIndex = 3;
            editEntryButton.Text = "Edit Entry";
            editEntryButton.UseVisualStyleBackColor = false;
            editEntryButton.Click += EditEntryButton_Click;

            // deleteEntryButton
            deleteEntryButton.BackColor = Color.OrangeRed;
            deleteEntryButton.FlatStyle = FlatStyle.Flat;
            deleteEntryButton.Font = new Font("Segoe UI", 10F);
            deleteEntryButton.ForeColor = Color.White;
            deleteEntryButton.Location = new Point(470, 65);
            deleteEntryButton.Name = "deleteEntryButton";
            deleteEntryButton.Size = new Size(130, 35);
            deleteEntryButton.TabIndex = 4;
            deleteEntryButton.Text = "Delete Entry";
            deleteEntryButton.UseVisualStyleBackColor = false;
            deleteEntryButton.Click += DeleteEntryButton_Click;

            // editTipLabel
            editTipLabel.AutoSize = true;
            editTipLabel.Font = new Font("Segoe UI", 8F);
            editTipLabel.ForeColor = Color.FromArgb(15, 15, 15);
            editTipLabel.Location = new Point(20, 100);
            editTipLabel.Name = "editTipLabel";
            editTipLabel.TabIndex = 0;
            editTipLabel.Text = "Your health log entries.";
            editTipLabel.Visible = false;

            // listView
            listView.Columns.AddRange(new ColumnHeader[] { dateColumn, weightColumn, caloriesColumn });
            listView.Dock = DockStyle.Fill;
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.Location = new Point(0, 120);
            listView.Name = "listView";
            listView.Size = new Size(1000, 583);
            listView.TabIndex = 0;
            listView.UseCompatibleStateImageBehavior = false;
            listView.View = View.Details;

            // dateColumn
            dateColumn.Text = "Date";
            dateColumn.Width = 160;

            // weightColumn
            weightColumn.Text = "Weight (lbs)";
            weightColumn.Width = 130;

            // caloriesColumn
            caloriesColumn.Text = "Calories";
            caloriesColumn.Width = 130;

            // editPanel
            editPanel.AutoScroll = true;
            editPanel.Controls.Add(cancelButton);
            editPanel.Controls.Add(saveButton);
            editPanel.Controls.Add(caloriesTextBox);
            editPanel.Controls.Add(caloriesLabel);
            editPanel.Controls.Add(weightTextBox);
            editPanel.Controls.Add(weightLabel);
            editPanel.Controls.Add(dateTextBox);
            editPanel.Controls.Add(dateLabel);
            editPanel.Controls.Add(logFormTitleLabel);
            editPanel.Dock = DockStyle.Fill;
            editPanel.Location = new Point(0, 120);
            editPanel.Name = "editPanel";
            editPanel.Size = new Size(1000, 583);
            editPanel.TabIndex = 1;
            editPanel.Visible = false;

            // logFormTitleLabel
            logFormTitleLabel.AutoSize = true;
            logFormTitleLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            logFormTitleLabel.Location = new Point(20, 20);
            logFormTitleLabel.Name = "logFormTitleLabel";
            logFormTitleLabel.TabIndex = 0;
            logFormTitleLabel.Text = "Log Today's Health Data";

            // dateLabel
            dateLabel.Location = new Point(20, 65);
            dateLabel.Name = "dateLabel";
            dateLabel.Size = new Size(150, 23);
            dateLabel.TabIndex = 1;
            dateLabel.Text = "Date (YYYY-MM-DD):";


            // dateTextBox
            dateTextBox.Location = new Point(180, 62);
            dateTextBox.Name = "dateTextBox";
            dateTextBox.Size = new Size(120, 23);
            dateTextBox.TabIndex = 2;

             
            // weightLabel
            weightLabel.Location = new Point(20, 100);
            weightLabel.Name = "weightLabel";
            weightLabel.Size = new Size(150, 23);
            weightLabel.TabIndex = 3;
            weightLabel.Text = "Weight (lbs):";

            // weightTextBox
            weightTextBox.Location = new Point(180, 97);
            weightTextBox.Name = "weightTextBox";
            weightTextBox.Size = new Size(300, 23);
            weightTextBox.TabIndex = 4;

            // caloriesLabel
            caloriesLabel.Location = new Point(20, 135);
            caloriesLabel.Name = "caloriesLabel";
            caloriesLabel.Size = new Size(150, 23);
            caloriesLabel.TabIndex = 5;
            caloriesLabel.Text = "Calories Consumed:";

            // caloriesTextBox
            caloriesTextBox.Location = new Point(180, 132);
            caloriesTextBox.Name = "caloriesTextBox";
            caloriesTextBox.Size = new Size(300, 23);
            caloriesTextBox.TabIndex = 6;

            // saveButton
            saveButton.BackColor = Color.FromArgb(0, 120, 215);
            saveButton.FlatStyle = FlatStyle.Flat;
            saveButton.ForeColor = Color.White;
            saveButton.Location = new Point(180, 185);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(120, 35);
            saveButton.TabIndex = 7;
            saveButton.Text = "Submit";
            saveButton.UseVisualStyleBackColor = false;
            saveButton.Click += saveButton_Click;

            // cancelButton
            cancelButton.BackColor = Color.Gray;
            cancelButton.FlatStyle = FlatStyle.Flat;
            cancelButton.ForeColor = Color.White;
            cancelButton.Location = new Point(310, 185);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(100, 35);
            cancelButton.TabIndex = 8;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = false;
            cancelButton.Click += cancelButton_Click;

            // mainPanel Controls
            mainPanel.Controls.Add(titleLabel);
            mainPanel.Controls.Add(viewLogsButton);
            mainPanel.Controls.Add(logTodayButton);
            mainPanel.Controls.Add(editEntryButton);
            mainPanel.Controls.Add(deleteEntryButton);
            mainPanel.Controls.Add(editTipLabel);

            // CalorieTrackerControl
            Controls.Add(editPanel);
            Controls.Add(listView);
            Controls.Add(mainPanel);
            Name = "CalorieTrackerControl";
            Size = new Size(1000, 703);
            mainPanel.ResumeLayout(false);
            editPanel.ResumeLayout(false);
            editPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }



        #endregion

        private Panel mainPanel;
        private Label titleLabel;
        private Label editTipLabel;
        private Button viewLogsButton;
        private Button logTodayButton;
        private ListView listView;
        private ColumnHeader dateColumn;
        private ColumnHeader weightColumn;
        private ColumnHeader caloriesColumn;
        private Panel editPanel;
        private Label logFormTitleLabel;
        private Label dateLabel;
        private TextBox dateTextBox;
        private TextBox weightTextBox;
        private TextBox caloriesTextBox;
        private Label weightLabel;
        private Label caloriesLabel;
        private Button saveButton;
        private Button cancelButton;
        private Button editEntryButton;
        private Button deleteEntryButton;
    }
}
