namespace FollowMe.Autopilot
{
    partial class AutopilotCameraForm
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
            this.cameraPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // cameraPanel
            // 
            this.cameraPanel.Location = new System.Drawing.Point(12, 12);
            this.cameraPanel.Name = "cameraPanel";
            this.cameraPanel.Size = new System.Drawing.Size(833, 560);
            this.cameraPanel.TabIndex = 0;
            // 
            // AutopilotCameraForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(857, 584);
            this.Controls.Add(this.cameraPanel);
            this.Name = "AutopilotCameraForm";
            this.Text = "AutopilotCameraForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel cameraPanel;
    }
}