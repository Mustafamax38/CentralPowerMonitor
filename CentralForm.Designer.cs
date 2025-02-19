namespace CentralPowerMonitor
{
    partial class CentralForm
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
        /// Required method for Designer support - do not modify the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lvwPcStatus = new System.Windows.Forms.ListView();
            this.pcName = new System.Windows.Forms.ColumnHeader();
            this.status = new System.Windows.Forms.ColumnHeader();
            this.time = new System.Windows.Forms.ColumnHeader();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.listBoxResults = new System.Windows.Forms.ListBox();
            this.SuspendLayout();

            // 
            // lvwPcStatus
            // 
            this.lvwPcStatus.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.pcName, this.status, this.time });
            this.lvwPcStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwPcStatus.Location = new System.Drawing.Point(0, 0);
            this.lvwPcStatus.Name = "lvwPcStatus";
            this.lvwPcStatus.Size = new System.Drawing.Size(1054, 514);
            this.lvwPcStatus.TabIndex = 0;
            this.lvwPcStatus.UseCompatibleStateImageBehavior = false;
            this.lvwPcStatus.View = System.Windows.Forms.View.Details;
            this.lvwPcStatus.SelectedIndexChanged += new System.EventHandler(this.lvwPcStatus_SelectedIndexChanged);

            // 
            // pcName
            // 
            this.pcName.Text = "PC Name";
            this.pcName.Width = 150;

            // 
            // status
            // 
            this.status.Text = "Status";
            this.status.Width = 200;

            // 
            // time
            // 
            this.time.Text = "Last Update";
            this.time.Width = 200;

            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(12, 259);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.PlaceholderText = "Power off history list";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(459, 243);
            this.txtLog.TabIndex = 1;

            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";

            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] { "192.168.1.20", "192.168.1.21", "192.168.1.22", "192.168.1.150", "192.168.1.160", "192.168.1.170", "192.168.1.180" });
            this.checkedListBox1.Location = new System.Drawing.Point(477, 256);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(201, 246);
            this.checkedListBox1.TabIndex = 6;

            // 
            // listBoxResults
            // 
            this.listBoxResults.FormattingEnabled = true;
            this.listBoxResults.Location = new System.Drawing.Point(684, 258);
            this.listBoxResults.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listBoxResults.Name = "listBoxResults";
            this.listBoxResults.Size = new System.Drawing.Size(370, 244);
            this.listBoxResults.TabIndex = 7;

            // 
            // CentralForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1054, 514);
            this.Controls.Add(this.listBoxResults);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lvwPcStatus);
            this.Name = "CentralForm";
            this.Text = "CentralForm";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListView lvwPcStatus;
        private System.Windows.Forms.ColumnHeader pcName;
        private System.Windows.Forms.ColumnHeader status;
        private System.Windows.Forms.ColumnHeader time;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.ListBox listBoxResults;
    }
}
