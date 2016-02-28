namespace WindowsFormsApplication1
{
    partial class SelectorForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.btnStringsPicker1 = new System.Windows.Forms.Button();
            this.btnStringsPicker2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 21);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Names color picker";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnStringsPicker1
            // 
            this.btnStringsPicker1.Location = new System.Drawing.Point(12, 50);
            this.btnStringsPicker1.Name = "btnStringsPicker1";
            this.btnStringsPicker1.Size = new System.Drawing.Size(118, 23);
            this.btnStringsPicker1.TabIndex = 1;
            this.btnStringsPicker1.Text = "Strings picker #1";
            this.btnStringsPicker1.UseVisualStyleBackColor = true;
            this.btnStringsPicker1.Click += new System.EventHandler(this.btnStringsPicker1_Click);
            // 
            // btnStringsPicker2
            // 
            this.btnStringsPicker2.Location = new System.Drawing.Point(12, 79);
            this.btnStringsPicker2.Name = "btnStringsPicker2";
            this.btnStringsPicker2.Size = new System.Drawing.Size(118, 23);
            this.btnStringsPicker2.TabIndex = 2;
            this.btnStringsPicker2.Text = "Strings picker #2";
            this.btnStringsPicker2.UseVisualStyleBackColor = true;
            this.btnStringsPicker2.Click += new System.EventHandler(this.btnStringsPicker2_Click);
            // 
            // SelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 297);
            this.Controls.Add(this.btnStringsPicker2);
            this.Controls.Add(this.btnStringsPicker1);
            this.Controls.Add(this.button1);
            this.Name = "SelectorForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.SelectorForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnStringsPicker1;
        private System.Windows.Forms.Button btnStringsPicker2;
    }
}