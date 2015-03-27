namespace FollowMe
{
    partial class CameraForm
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
            this.ExternalCameraPanel = new System.Windows.Forms.Panel();
            this.ExternalCameraPanelTrackingPreview = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // ExternalCameraPanel
            // 
            this.ExternalCameraPanel.Location = new System.Drawing.Point(12, 12);
            this.ExternalCameraPanel.Name = "ExternalCameraPanel";
            this.ExternalCameraPanel.Size = new System.Drawing.Size(666, 507);
            this.ExternalCameraPanel.TabIndex = 0;
            // 
            // ExternalCameraPanelTrackingPreview
            // 
            this.ExternalCameraPanelTrackingPreview.Location = new System.Drawing.Point(708, 12);
            this.ExternalCameraPanelTrackingPreview.Name = "ExternalCameraPanelTrackingPreview";
            this.ExternalCameraPanelTrackingPreview.Size = new System.Drawing.Size(666, 507);
            this.ExternalCameraPanelTrackingPreview.TabIndex = 1;
            // 
            // CameraForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1382, 519);
            this.Controls.Add(this.ExternalCameraPanelTrackingPreview);
            this.Controls.Add(this.ExternalCameraPanel);
            this.Name = "CameraForm";
            this.Text = "Live AR.Drone Frontkamera";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel ExternalCameraPanel;
    }
}