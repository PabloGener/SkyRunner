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
            this.lblOriginIcao = new System.Windows.Forms.Label();
            this.lblDestination = new System.Windows.Forms.Label();
            this.lblDistance = new System.Windows.Forms.Label();
            this.lblPax = new System.Windows.Forms.Label();
            this.lblWeight = new System.Windows.Forms.Label();
            this.lblJobType = new System.Windows.Forms.Label();
            this.cmdTake = new System.Windows.Forms.Button();
            this.lblCurrentJob = new System.Windows.Forms.Label();
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
            // lblOriginIcao
            // 
            this.lblOriginIcao.AutoSize = true;
            this.lblOriginIcao.Enabled = false;
            this.lblOriginIcao.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOriginIcao.Location = new System.Drawing.Point(8, 331);
            this.lblOriginIcao.Name = "lblOriginIcao";
            this.lblOriginIcao.Size = new System.Drawing.Size(54, 20);
            this.lblOriginIcao.TabIndex = 6;
            this.lblOriginIcao.Text = "Origin:";
            // 
            // lblDestination
            // 
            this.lblDestination.AutoSize = true;
            this.lblDestination.Enabled = false;
            this.lblDestination.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDestination.Location = new System.Drawing.Point(8, 351);
            this.lblDestination.Name = "lblDestination";
            this.lblDestination.Size = new System.Drawing.Size(90, 20);
            this.lblDestination.TabIndex = 7;
            this.lblDestination.Text = "Destination";
            // 
            // lblDistance
            // 
            this.lblDistance.AutoSize = true;
            this.lblDistance.Enabled = false;
            this.lblDistance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDistance.Location = new System.Drawing.Point(8, 371);
            this.lblDistance.Name = "lblDistance";
            this.lblDistance.Size = new System.Drawing.Size(76, 20);
            this.lblDistance.TabIndex = 8;
            this.lblDistance.Text = "Distance:";
            // 
            // lblPax
            // 
            this.lblPax.AutoSize = true;
            this.lblPax.Enabled = false;
            this.lblPax.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPax.Location = new System.Drawing.Point(391, 371);
            this.lblPax.Name = "lblPax";
            this.lblPax.Size = new System.Drawing.Size(39, 20);
            this.lblPax.TabIndex = 11;
            this.lblPax.Text = "Pax:";
            // 
            // lblWeight
            // 
            this.lblWeight.AutoSize = true;
            this.lblWeight.Enabled = false;
            this.lblWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWeight.Location = new System.Drawing.Point(391, 351);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(63, 20);
            this.lblWeight.TabIndex = 10;
            this.lblWeight.Text = "Weight:";
            // 
            // lblJobType
            // 
            this.lblJobType.AutoSize = true;
            this.lblJobType.Enabled = false;
            this.lblJobType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblJobType.Location = new System.Drawing.Point(391, 331);
            this.lblJobType.Name = "lblJobType";
            this.lblJobType.Size = new System.Drawing.Size(47, 20);
            this.lblJobType.TabIndex = 9;
            this.lblJobType.Text = "Type:";
            // 
            // cmdTake
            // 
            this.cmdTake.Location = new System.Drawing.Point(703, 314);
            this.cmdTake.Name = "cmdTake";
            this.cmdTake.Size = new System.Drawing.Size(85, 57);
            this.cmdTake.TabIndex = 12;
            this.cmdTake.Text = "Take this job";
            this.cmdTake.UseVisualStyleBackColor = true;
            this.cmdTake.Click += new System.EventHandler(this.cmdTake_Click);
            // 
            // lblCurrentJob
            // 
            this.lblCurrentJob.AutoSize = true;
            this.lblCurrentJob.Enabled = false;
            this.lblCurrentJob.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentJob.Location = new System.Drawing.Point(8, 311);
            this.lblCurrentJob.Name = "lblCurrentJob";
            this.lblCurrentJob.Size = new System.Drawing.Size(96, 20);
            this.lblCurrentJob.TabIndex = 13;
            this.lblCurrentJob.Text = "Current Job:";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblCurrentJob);
            this.Controls.Add(this.cmdTake);
            this.Controls.Add(this.lblPax);
            this.Controls.Add(this.lblWeight);
            this.Controls.Add(this.lblJobType);
            this.Controls.Add(this.lblDistance);
            this.Controls.Add(this.lblDestination);
            this.Controls.Add(this.lblOriginIcao);
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
        private System.Windows.Forms.Label lblOriginIcao;
        private System.Windows.Forms.Label lblDestination;
        private System.Windows.Forms.Label lblDistance;
        private System.Windows.Forms.Label lblPax;
        private System.Windows.Forms.Label lblWeight;
        private System.Windows.Forms.Label lblJobType;
        private System.Windows.Forms.Button cmdTake;
        private System.Windows.Forms.Label lblCurrentJob;
    }
}

