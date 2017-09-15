namespace Pictaurus
{
    partial class ScreenForm
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
            this.displayPanel = new System.Windows.Forms.Panel();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonResume = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // displayPanel
            // 
            this.displayPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.displayPanel.BackColor = System.Drawing.Color.Transparent;
            this.displayPanel.Location = new System.Drawing.Point(12, 12);
            this.displayPanel.Name = "displayPanel";
            this.displayPanel.Size = new System.Drawing.Size(260, 238);
            this.displayPanel.TabIndex = 0;
            this.displayPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.displayPanel_Paint);
            this.displayPanel.Layout += new System.Windows.Forms.LayoutEventHandler(this.displayPanel_Layout);
            this.displayPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.displayPanel_MouseClick);
            this.displayPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.displayPanel_MouseMove);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClose.Location = new System.Drawing.Point(265, 0);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(0);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(19, 20);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "X";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            this.buttonClose.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ScreenForm_KeyDown);
            // 
            // buttonResume
            // 
            this.buttonResume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonResume.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonResume.Location = new System.Drawing.Point(0, 243);
            this.buttonResume.Margin = new System.Windows.Forms.Padding(0);
            this.buttonResume.Name = "buttonResume";
            this.buttonResume.Size = new System.Drawing.Size(61, 20);
            this.buttonResume.TabIndex = 0;
            this.buttonResume.Text = "Resume";
            this.buttonResume.UseVisualStyleBackColor = true;
            this.buttonResume.Visible = false;
            this.buttonResume.Click += new System.EventHandler(this.buttonResume_Click);
            this.buttonResume.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ScreenForm_KeyDown);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(70, 243);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(61, 20);
            this.button1.TabIndex = 1;
            this.button1.Text = "Resume";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(140, 243);
            this.button2.Margin = new System.Windows.Forms.Padding(0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(61, 20);
            this.button2.TabIndex = 2;
            this.button2.Text = "Resume";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            // 
            // ScreenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonResume);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.displayPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ScreenForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.ScreenForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ScreenForm_KeyDown);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ScreenForm_MouseClick);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel displayPanel;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonResume;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

