namespace FollowMe
{
    partial class HuePickerForm
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
            this.huePicker1 = new AForge.Controls.HuePicker();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.btnUebernehmen = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.hueMinTextBox = new System.Windows.Forms.TextBox();
            this.hueMaxTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // huePicker1
            // 
            this.huePicker1.Location = new System.Drawing.Point(38, 26);
            this.huePicker1.Name = "huePicker1";
            this.huePicker1.Size = new System.Drawing.Size(209, 209);
            this.huePicker1.TabIndex = 12;
            this.huePicker1.Text = "huePicker";
            this.huePicker1.Type = AForge.Controls.HuePicker.HuePickerType.Range;
            // 
            // btnUebernehmen
            // 
            this.btnUebernehmen.Location = new System.Drawing.Point(302, 211);
            this.btnUebernehmen.Name = "btnUebernehmen";
            this.btnUebernehmen.Size = new System.Drawing.Size(115, 46);
            this.btnUebernehmen.TabIndex = 13;
            this.btnUebernehmen.Text = "übernehmen";
            this.btnUebernehmen.UseVisualStyleBackColor = true;
            this.btnUebernehmen.Click += new System.EventHandler(this.btnUebernehmen_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(302, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Min";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(302, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Max";
            // 
            // hueMinTextBox
            // 
            this.hueMinTextBox.Location = new System.Drawing.Point(347, 53);
            this.hueMinTextBox.Name = "hueMinTextBox";
            this.hueMinTextBox.ReadOnly = true;
            this.hueMinTextBox.Size = new System.Drawing.Size(70, 20);
            this.hueMinTextBox.TabIndex = 16;
            // 
            // hueMaxTextBox
            // 
            this.hueMaxTextBox.Location = new System.Drawing.Point(347, 85);
            this.hueMaxTextBox.Name = "hueMaxTextBox";
            this.hueMaxTextBox.ReadOnly = true;
            this.hueMaxTextBox.Size = new System.Drawing.Size(70, 20);
            this.hueMaxTextBox.TabIndex = 17;
            // 
            // HuePickerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 278);
            this.Controls.Add(this.hueMaxTextBox);
            this.Controls.Add(this.hueMinTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnUebernehmen);
            this.Controls.Add(this.huePicker1);
            this.Name = "HuePickerForm";
            this.Text = "Hue Picker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AForge.Controls.HuePicker huePicker1;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private System.Windows.Forms.Button btnUebernehmen;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox hueMinTextBox;
        private System.Windows.Forms.TextBox hueMaxTextBox;
    }
}