﻿namespace WindowsFormsApplication1
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
            this.btnStringsPicker3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
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
            // btnStringsPicker3
            // 
            this.btnStringsPicker3.Location = new System.Drawing.Point(12, 108);
            this.btnStringsPicker3.Name = "btnStringsPicker3";
            this.btnStringsPicker3.Size = new System.Drawing.Size(118, 23);
            this.btnStringsPicker3.TabIndex = 3;
            this.btnStringsPicker3.Text = "Strings picker #3";
            this.btnStringsPicker3.UseVisualStyleBackColor = true;
            this.btnStringsPicker3.Click += new System.EventHandler(this.btnStringsPicker3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(136, 21);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(118, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "ListView Test";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(136, 50);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(118, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "TreeView Test";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // SelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 297);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnStringsPicker3);
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
        private System.Windows.Forms.Button btnStringsPicker3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}