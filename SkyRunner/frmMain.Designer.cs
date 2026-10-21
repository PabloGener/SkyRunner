namespace SkyRunner
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.cmdConnect = new System.Windows.Forms.Button();
            this.tmrPoll = new System.Windows.Forms.Timer(this.components);
            this.lblCurrentAirport = new System.Windows.Forms.Label();
            this.txtCurrentAirport = new System.Windows.Forms.TextBox();
            this.cmdGenerateJobs = new System.Windows.Forms.Button();
            this.trkRadius = new System.Windows.Forms.TrackBar();
            this.lstAvailableJobs = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.trkRadius)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdConnect
            // 
            this.cmdConnect.Location = new System.Drawing.Point(12, 12);
            this.cmdConnect.Name = "cmdConnect";
            this.cmdConnect.Size = new System.Drawing.Size(85, 55);
            this.cmdConnect.TabIndex = 0;
            this.cmdConnect.Text = "Connect";
            this.cmdConnect.UseVisualStyleBackColor = true;
            this.cmdConnect.Click += new System.EventHandler(this.cmdConnect_Click);
            // 
            // lblCurrentAirport
            // 
            this.lblCurrentAirport.AutoSize = true;
            this.lblCurrentAirport.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentAirport.Location = new System.Drawing.Point(99, 18);
            this.lblCurrentAirport.Name = "lblCurrentAirport";
            this.lblCurrentAirport.Size = new System.Drawing.Size(117, 20);
            this.lblCurrentAirport.TabIndex = 1;
            this.lblCurrentAirport.Text = "Current Airport:";
            // 
            // txtCurrentAirport
            // 
            this.txtCurrentAirport.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrentAirport.Location = new System.Drawing.Point(103, 41);
            this.txtCurrentAirport.Name = "txtCurrentAirport";
            this.txtCurrentAirport.Size = new System.Drawing.Size(346, 26);
            this.txtCurrentAirport.TabIndex = 2;
            // 
            // cmdGenerateJobs
            // 
            this.cmdGenerateJobs.Location = new System.Drawing.Point(12, 73);
            this.cmdGenerateJobs.Name = "cmdGenerateJobs";
            this.cmdGenerateJobs.Size = new System.Drawing.Size(85, 45);
            this.cmdGenerateJobs.TabIndex = 3;
            this.cmdGenerateJobs.Text = "Find jobs here";
            this.cmdGenerateJobs.UseVisualStyleBackColor = true;
            this.cmdGenerateJobs.Click += new System.EventHandler(this.cmdGenerateJobs_Click);
            // 
            // trkRadius
            // 
            this.trkRadius.LargeChange = 50;
            this.trkRadius.Location = new System.Drawing.Point(112, 73);
            this.trkRadius.Maximum = 500;
            this.trkRadius.Minimum = 50;
            this.trkRadius.Name = "trkRadius";
            this.trkRadius.Size = new System.Drawing.Size(337, 45);
            this.trkRadius.TabIndex = 4;
            this.trkRadius.TickFrequency = 50;
            this.trkRadius.Value = 50;
            // 
            // lstAvailableJobs
            // 
            this.lstAvailableJobs.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstAvailableJobs.FormattingEnabled = true;
            this.lstAvailableJobs.ItemHeight = 20;
            this.lstAvailableJobs.Location = new System.Drawing.Point(12, 124);
            this.lstAvailableJobs.Name = "lstAvailableJobs";
            this.lstAvailableJobs.Size = new System.Drawing.Size(776, 184);
            this.lstAvailableJobs.TabIndex = 5;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lstAvailableJobs);
            this.Controls.Add(this.trkRadius);
            this.Controls.Add(this.cmdGenerateJobs);
            this.Controls.Add(this.txtCurrentAirport);
            this.Controls.Add(this.lblCurrentAirport);
            this.Controls.Add(this.cmdConnect);
            this.Name = "frmMain";
            this.Text = "SkyRunner";
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trkRadius)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdConnect;
        private System.Windows.Forms.Timer tmrPoll;
        private System.Windows.Forms.Label lblCurrentAirport;
        private System.Windows.Forms.TextBox txtCurrentAirport;
        private System.Windows.Forms.Button cmdGenerateJobs;
        private System.Windows.Forms.TrackBar trkRadius;
        private System.Windows.Forms.ListBox lstAvailableJobs;
    }
}

