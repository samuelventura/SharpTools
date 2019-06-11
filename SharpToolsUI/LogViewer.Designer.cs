namespace SharpToolsUI
{
    partial class LogViewer
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Disposes resources used by the control.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.linkLabel = new System.Windows.Forms.LinkLabel();
            this.followTailCheckBox = new System.Windows.Forms.CheckBox();
            this.showDebugCheckBox = new System.Windows.Forms.CheckBox();
            this.clearButton = new System.Windows.Forms.Button();
            this.rtb = new System.Windows.Forms.RichTextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.controlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // controlPanel
            // 
            this.controlPanel.BackColor = System.Drawing.SystemColors.Control;
            this.controlPanel.Controls.Add(this.linkLabel);
            this.controlPanel.Controls.Add(this.followTailCheckBox);
            this.controlPanel.Controls.Add(this.showDebugCheckBox);
            this.controlPanel.Controls.Add(this.clearButton);
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.controlPanel.Location = new System.Drawing.Point(0, 0);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(638, 28);
            this.controlPanel.TabIndex = 11;
            // 
            // linkLabel
            // 
            this.linkLabel.AutoSize = true;
            this.linkLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.linkLabel.LinkColor = System.Drawing.Color.ForestGreen;
            this.linkLabel.Location = new System.Drawing.Point(555, 0);
            this.linkLabel.Name = "linkLabel";
            this.linkLabel.Size = new System.Drawing.Size(83, 13);
            this.linkLabel.TabIndex = 5;
            this.linkLabel.TabStop = true;
            this.linkLabel.Text = "info@yeico.com";
            this.toolTip.SetToolTip(this.linkLabel, "Click to copy to clipboard");
            this.linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelLinkClicked);
            // 
            // followTailCheckBox
            // 
            this.followTailCheckBox.AutoSize = true;
            this.followTailCheckBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.followTailCheckBox.Location = new System.Drawing.Point(166, 0);
            this.followTailCheckBox.Name = "followTailCheckBox";
            this.followTailCheckBox.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.followTailCheckBox.Size = new System.Drawing.Size(79, 28);
            this.followTailCheckBox.TabIndex = 4;
            this.followTailCheckBox.Text = "Follow Tail";
            this.followTailCheckBox.UseVisualStyleBackColor = true;
            this.followTailCheckBox.CheckedChanged += new System.EventHandler(this.FollowTailCheckBoxCheckedChanged);
            // 
            // showDebugCheckBox
            // 
            this.showDebugCheckBox.AutoSize = true;
            this.showDebugCheckBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.showDebugCheckBox.Location = new System.Drawing.Point(75, 0);
            this.showDebugCheckBox.Name = "showDebugCheckBox";
            this.showDebugCheckBox.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.showDebugCheckBox.Size = new System.Drawing.Size(91, 28);
            this.showDebugCheckBox.TabIndex = 3;
            this.showDebugCheckBox.Text = "Show Debug";
            this.showDebugCheckBox.UseVisualStyleBackColor = true;
            // 
            // clearButton
            // 
            this.clearButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.clearButton.Location = new System.Drawing.Point(0, 0);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 28);
            this.clearButton.TabIndex = 1;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.ClearButtonClick);
            // 
            // rtb
            // 
            this.rtb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(24)))), ((int)(((byte)(34)))));
            this.rtb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtb.ForeColor = System.Drawing.Color.White;
            this.rtb.Location = new System.Drawing.Point(0, 28);
            this.rtb.Name = "rtb";
            this.rtb.ReadOnly = true;
            this.rtb.Size = new System.Drawing.Size(638, 363);
            this.rtb.TabIndex = 13;
            this.rtb.Text = "";
            // 
            // LogViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rtb);
            this.Controls.Add(this.controlPanel);
            this.Name = "LogViewer";
            this.Size = new System.Drawing.Size(638, 391);
            this.controlPanel.ResumeLayout(false);
            this.controlPanel.PerformLayout();
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.LinkLabel linkLabel;
        private System.Windows.Forms.CheckBox followTailCheckBox;
        private System.Windows.Forms.CheckBox showDebugCheckBox;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.RichTextBox rtb;
    }
}
