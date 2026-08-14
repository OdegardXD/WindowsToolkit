namespace WindowsToolkit
{
    partial class MainForm
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
            LogBox = new RichTextBox();
            panel1 = new Panel();
            FlushDNSCheckBox = new CheckBox();
            UILabel3 = new Label();
            UILabel2 = new Label();
            UILabel1 = new Label();
            ClearRecycleBinCheckBox = new CheckBox();
            ClearLogBox = new Button();
            DISMCleanupCheckBox = new CheckBox();
            HelpButton = new Button();
            CheckDiskCheckBox = new CheckBox();
            DISMCheckBox = new CheckBox();
            SFCCheckBox = new CheckBox();
            DeleteTempFilesCheckBox = new CheckBox();
            RunButton = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // LogBox
            // 
            LogBox.Location = new Point(303, 12);
            LogBox.MaximumSize = new Size(592, 426);
            LogBox.MinimumSize = new Size(592, 426);
            LogBox.Name = "LogBox";
            LogBox.ReadOnly = true;
            LogBox.Size = new Size(592, 426);
            LogBox.TabIndex = 0;
            LogBox.Text = "";
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(FlushDNSCheckBox);
            panel1.Controls.Add(UILabel3);
            panel1.Controls.Add(UILabel2);
            panel1.Controls.Add(UILabel1);
            panel1.Controls.Add(ClearRecycleBinCheckBox);
            panel1.Controls.Add(ClearLogBox);
            panel1.Controls.Add(DISMCleanupCheckBox);
            panel1.Controls.Add(HelpButton);
            panel1.Controls.Add(CheckDiskCheckBox);
            panel1.Controls.Add(DISMCheckBox);
            panel1.Controls.Add(SFCCheckBox);
            panel1.Controls.Add(DeleteTempFilesCheckBox);
            panel1.Controls.Add(RunButton);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(285, 426);
            panel1.TabIndex = 1;
            // 
            // FlushDNSCheckBox
            // 
            FlushDNSCheckBox.AutoSize = true;
            FlushDNSCheckBox.Location = new Point(3, 200);
            FlushDNSCheckBox.Name = "FlushDNSCheckBox";
            FlushDNSCheckBox.Size = new Size(80, 19);
            FlushDNSCheckBox.TabIndex = 12;
            FlushDNSCheckBox.Text = "Flush DNS";
            FlushDNSCheckBox.UseVisualStyleBackColor = true;
            // 
            // UILabel3
            // 
            UILabel3.AutoSize = true;
            UILabel3.Location = new Point(3, 182);
            UILabel3.Name = "UILabel3";
            UILabel3.Size = new Size(95, 15);
            UILabel3.TabIndex = 11;
            UILabel3.Text = "-- Networking --";
            // 
            // UILabel2
            // 
            UILabel2.AutoSize = true;
            UILabel2.Location = new Point(3, 67);
            UILabel2.Name = "UILabel2";
            UILabel2.Size = new Size(157, 15);
            UILabel2.TabIndex = 10;
            UILabel2.Text = "-- Builit-In Windows Tools --";
            // 
            // UILabel1
            // 
            UILabel1.AutoSize = true;
            UILabel1.Location = new Point(3, 2);
            UILabel1.Name = "UILabel1";
            UILabel1.Size = new Size(92, 15);
            UILabel1.TabIndex = 9;
            UILabel1.Text = "-- File System --";
            // 
            // ClearRecycleBinCheckBox
            // 
            ClearRecycleBinCheckBox.AutoSize = true;
            ClearRecycleBinCheckBox.Location = new Point(3, 45);
            ClearRecycleBinCheckBox.Name = "ClearRecycleBinCheckBox";
            ClearRecycleBinCheckBox.Size = new Size(116, 19);
            ClearRecycleBinCheckBox.TabIndex = 8;
            ClearRecycleBinCheckBox.Text = "Clear Recycle Bin";
            ClearRecycleBinCheckBox.UseVisualStyleBackColor = true;
            // 
            // ClearLogBox
            // 
            ClearLogBox.Location = new Point(84, 398);
            ClearLogBox.Name = "ClearLogBox";
            ClearLogBox.Size = new Size(75, 23);
            ClearLogBox.TabIndex = 7;
            ClearLogBox.Text = "Clear Log";
            ClearLogBox.UseVisualStyleBackColor = true;
            ClearLogBox.Click += ClearLogBox_Click;
            // 
            // DISMCleanupCheckBox
            // 
            DISMCleanupCheckBox.AutoSize = true;
            DISMCleanupCheckBox.Location = new Point(3, 135);
            DISMCleanupCheckBox.Name = "DISMCleanupCheckBox";
            DISMCleanupCheckBox.Size = new Size(168, 19);
            DISMCleanupCheckBox.TabIndex = 6;
            DISMCleanupCheckBox.Text = "DISM Component Cleanup";
            DISMCleanupCheckBox.UseVisualStyleBackColor = true;
            // 
            // HelpButton
            // 
            HelpButton.Location = new Point(3, 398);
            HelpButton.Name = "HelpButton";
            HelpButton.Size = new Size(75, 23);
            HelpButton.TabIndex = 5;
            HelpButton.Text = "Help!";
            HelpButton.UseVisualStyleBackColor = true;
            HelpButton.Click += HelpButton_Click;
            // 
            // CheckDiskCheckBox
            // 
            CheckDiskCheckBox.AutoSize = true;
            CheckDiskCheckBox.Location = new Point(3, 160);
            CheckDiskCheckBox.Name = "CheckDiskCheckBox";
            CheckDiskCheckBox.Size = new Size(194, 19);
            CheckDiskCheckBox.TabIndex = 4;
            CheckDiskCheckBox.Text = "Check Disk (REQUIRES RESTART)";
            CheckDiskCheckBox.UseVisualStyleBackColor = true;
            CheckDiskCheckBox.CheckedChanged += CHKDSK_Restart_warn;
            // 
            // DISMCheckBox
            // 
            DISMCheckBox.AutoSize = true;
            DISMCheckBox.Location = new Point(3, 85);
            DISMCheckBox.Name = "DISMCheckBox";
            DISMCheckBox.Size = new Size(134, 19);
            DISMCheckBox.TabIndex = 3;
            DISMCheckBox.Text = "DISM Restore Health";
            DISMCheckBox.UseVisualStyleBackColor = true;
            // 
            // SFCCheckBox
            // 
            SFCCheckBox.AutoSize = true;
            SFCCheckBox.Location = new Point(3, 110);
            SFCCheckBox.Name = "SFCCheckBox";
            SFCCheckBox.Size = new Size(106, 19);
            SFCCheckBox.TabIndex = 2;
            SFCCheckBox.Text = "\"sfc /scannow\"";
            SFCCheckBox.UseVisualStyleBackColor = true;
            // 
            // DeleteTempFilesCheckBox
            // 
            DeleteTempFilesCheckBox.AutoSize = true;
            DeleteTempFilesCheckBox.Location = new Point(3, 20);
            DeleteTempFilesCheckBox.Name = "DeleteTempFilesCheckBox";
            DeleteTempFilesCheckBox.Size = new Size(117, 19);
            DeleteTempFilesCheckBox.TabIndex = 1;
            DeleteTempFilesCheckBox.Text = "Delete Temp Files";
            DeleteTempFilesCheckBox.UseVisualStyleBackColor = true;
            // 
            // RunButton
            // 
            RunButton.Location = new Point(205, 398);
            RunButton.Name = "RunButton";
            RunButton.Size = new Size(75, 23);
            RunButton.TabIndex = 0;
            RunButton.Text = "Run";
            RunButton.UseVisualStyleBackColor = true;
            RunButton.Click += RunButton_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(907, 450);
            Controls.Add(panel1);
            Controls.Add(LogBox);
            MaximizeBox = false;
            MinimumSize = new Size(829, 489);
            Name = "MainForm";
            ShowIcon = false;
            Text = "WindowsToolkit";
            FormClosing += MainForm_FormClosing;
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox LogBox;
        private Panel panel1;
        private Button HelpButton;
        private CheckBox CheckDiskCheckBox;
        private CheckBox DISMCheckBox;
        private CheckBox SFCCheckBox;
        private CheckBox DeleteTempFilesCheckBox;
        private Button RunButton;
        private CheckBox DISMCleanupCheckBox;
        private Button ClearLogBox;
        private CheckBox ClearRecycleBinCheckBox;
        private Label UILabel3;
        private Label UILabel2;
        private Label UILabel1;
        private CheckBox FlushDNSCheckBox;
    }
}
