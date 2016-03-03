namespace lallouslab.FluentLib.WinForms.Dialogs
{
    partial class StringsPicker
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
            this.components = new System.ComponentModel.Container();
            this.ctxmenuMatchOptions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pnlAdd = new System.Windows.Forms.Panel();
            this.lblInsertText = new System.Windows.Forms.Label();
            this.txtInsertText = new System.Windows.Forms.TextBox();
            this.btnInsertText = new System.Windows.Forms.Button();
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.lblFilter = new System.Windows.Forms.Label();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.btnTextFilterOption = new System.Windows.Forms.Button();
            this.pnlOkCancel = new System.Windows.Forms.Panel();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlLV = new System.Windows.Forms.Panel();
            this.lvItems = new System.Windows.Forms.ListView();
            this.ctxmenuLV = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pnlFreeStyleValues = new System.Windows.Forms.Panel();
            this.lblFreeStyleValues = new System.Windows.Forms.Label();
            this.txtFreeStyleValues = new System.Windows.Forms.TextBox();
            this.pnlAdd.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.pnlOkCancel.SuspendLayout();
            this.pnlLV.SuspendLayout();
            this.pnlFreeStyleValues.SuspendLayout();
            this.SuspendLayout();
            // 
            // ctxmenuMatchOptions
            // 
            this.ctxmenuMatchOptions.Name = "ctxmenuMatchOptions";
            this.ctxmenuMatchOptions.Size = new System.Drawing.Size(61, 4);
            // 
            // pnlAdd
            // 
            this.pnlAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlAdd.Controls.Add(this.lblInsertText);
            this.pnlAdd.Controls.Add(this.txtInsertText);
            this.pnlAdd.Controls.Add(this.btnInsertText);
            this.pnlAdd.Location = new System.Drawing.Point(3, 131);
            this.pnlAdd.Name = "pnlAdd";
            this.pnlAdd.Size = new System.Drawing.Size(420, 32);
            this.pnlAdd.TabIndex = 0;
            // 
            // lblInsertText
            // 
            this.lblInsertText.AutoSize = true;
            this.lblInsertText.Location = new System.Drawing.Point(-2, 9);
            this.lblInsertText.Name = "lblInsertText";
            this.lblInsertText.Size = new System.Drawing.Size(36, 13);
            this.lblInsertText.TabIndex = 1;
            this.lblInsertText.Text = "Insert:";
            // 
            // txtInsertText
            // 
            this.txtInsertText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInsertText.Location = new System.Drawing.Point(36, 4);
            this.txtInsertText.Name = "txtInsertText";
            this.txtInsertText.Size = new System.Drawing.Size(347, 20);
            this.txtInsertText.TabIndex = 2;
            // 
            // btnInsertText
            // 
            this.btnInsertText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInsertText.Font = new System.Drawing.Font("Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnInsertText.Location = new System.Drawing.Point(389, 2);
            this.btnInsertText.Name = "btnInsertText";
            this.btnInsertText.Size = new System.Drawing.Size(32, 23);
            this.btnInsertText.TabIndex = 3;
            this.btnInsertText.Text = "Å";
            this.btnInsertText.UseVisualStyleBackColor = true;
            this.btnInsertText.Click += new System.EventHandler(this.btnInsertText_Click);
            // 
            // pnlFilter
            // 
            this.pnlFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFilter.Controls.Add(this.lblFilter);
            this.pnlFilter.Controls.Add(this.txtFilter);
            this.pnlFilter.Controls.Add(this.btnTextFilterOption);
            this.pnlFilter.Location = new System.Drawing.Point(3, 94);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(420, 31);
            this.pnlFilter.TabIndex = 1;
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(-2, 9);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(32, 13);
            this.lblFilter.TabIndex = 4;
            this.lblFilter.Text = "Filter:";
            // 
            // txtFilter
            // 
            this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilter.Location = new System.Drawing.Point(36, 4);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(347, 20);
            this.txtFilter.TabIndex = 5;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // btnTextFilterOption
            // 
            this.btnTextFilterOption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTextFilterOption.Font = new System.Drawing.Font("Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnTextFilterOption.Location = new System.Drawing.Point(389, 2);
            this.btnTextFilterOption.Name = "btnTextFilterOption";
            this.btnTextFilterOption.Size = new System.Drawing.Size(32, 23);
            this.btnTextFilterOption.TabIndex = 6;
            this.btnTextFilterOption.Text = "ß";
            this.btnTextFilterOption.UseVisualStyleBackColor = true;
            this.btnTextFilterOption.Click += new System.EventHandler(this.btnTextFilterOption_Click);
            // 
            // pnlOkCancel
            // 
            this.pnlOkCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlOkCancel.Controls.Add(this.btnOk);
            this.pnlOkCancel.Controls.Add(this.btnCancel);
            this.pnlOkCancel.Location = new System.Drawing.Point(3, 443);
            this.pnlOkCancel.Name = "pnlOkCancel";
            this.pnlOkCancel.Size = new System.Drawing.Size(420, 34);
            this.pnlOkCancel.TabIndex = 2;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(-4, -3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 36);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(77, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 33);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&CANCEL";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // pnlLV
            // 
            this.pnlLV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlLV.Controls.Add(this.lvItems);
            this.pnlLV.Location = new System.Drawing.Point(4, 165);
            this.pnlLV.Name = "pnlLV";
            this.pnlLV.Size = new System.Drawing.Size(420, 272);
            this.pnlLV.TabIndex = 7;
            // 
            // lvItems
            // 
            this.lvItems.ContextMenuStrip = this.ctxmenuLV;
            this.lvItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvItems.FullRowSelect = true;
            this.lvItems.Location = new System.Drawing.Point(0, 0);
            this.lvItems.Name = "lvItems";
            this.lvItems.Size = new System.Drawing.Size(420, 272);
            this.lvItems.TabIndex = 7;
            this.lvItems.UseCompatibleStateImageBehavior = false;
            this.lvItems.View = System.Windows.Forms.View.List;
            this.lvItems.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvItems_ItemChecked);
            // 
            // ctxmenuLV
            // 
            this.ctxmenuLV.Name = "contextMenuStrip1";
            this.ctxmenuLV.Size = new System.Drawing.Size(61, 4);
            // 
            // pnlFreeStyleValues
            // 
            this.pnlFreeStyleValues.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFreeStyleValues.Controls.Add(this.lblFreeStyleValues);
            this.pnlFreeStyleValues.Controls.Add(this.txtFreeStyleValues);
            this.pnlFreeStyleValues.Location = new System.Drawing.Point(4, 41);
            this.pnlFreeStyleValues.Name = "pnlFreeStyleValues";
            this.pnlFreeStyleValues.Size = new System.Drawing.Size(420, 47);
            this.pnlFreeStyleValues.TabIndex = 8;
            // 
            // lblFreeStyleValues
            // 
            this.lblFreeStyleValues.AutoSize = true;
            this.lblFreeStyleValues.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFreeStyleValues.Location = new System.Drawing.Point(-3, 7);
            this.lblFreeStyleValues.Name = "lblFreeStyleValues";
            this.lblFreeStyleValues.Size = new System.Drawing.Size(210, 13);
            this.lblFreeStyleValues.TabIndex = 1;
            this.lblFreeStyleValues.Text = "Enter additional values (comma separated):";
            // 
            // txtFreeStyleValues
            // 
            this.txtFreeStyleValues.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFreeStyleValues.Location = new System.Drawing.Point(0, 24);
            this.txtFreeStyleValues.Name = "txtFreeStyleValues";
            this.txtFreeStyleValues.Size = new System.Drawing.Size(420, 20);
            this.txtFreeStyleValues.TabIndex = 2;
            // 
            // StringsPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(426, 518);
            this.Controls.Add(this.pnlFreeStyleValues);
            this.Controls.Add(this.pnlAdd);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.pnlLV);
            this.Controls.Add(this.pnlOkCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StringsPicker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "StringsPicker";
            this.Load += new System.EventHandler(this.StaticItemsPicker_Load);
            this.pnlAdd.ResumeLayout(false);
            this.pnlAdd.PerformLayout();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            this.pnlOkCancel.ResumeLayout(false);
            this.pnlLV.ResumeLayout(false);
            this.pnlFreeStyleValues.ResumeLayout(false);
            this.pnlFreeStyleValues.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip ctxmenuMatchOptions;

        private System.Windows.Forms.Panel pnlAdd;
        private System.Windows.Forms.Button btnInsertText;
        private System.Windows.Forms.TextBox txtInsertText;
        private System.Windows.Forms.Label lblInsertText;
        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.Button btnTextFilterOption;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.Panel pnlOkCancel;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel pnlLV;
        private System.Windows.Forms.ListView lvItems;
        private System.Windows.Forms.ContextMenuStrip ctxmenuLV;
        private System.Windows.Forms.Panel pnlFreeStyleValues;
        private System.Windows.Forms.Label lblFreeStyleValues;
        private System.Windows.Forms.TextBox txtFreeStyleValues;
    }
}