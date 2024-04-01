namespace SNESDisassembler
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DisassemblyGroup = new System.Windows.Forms.GroupBox();
            this.SourceData = new System.Windows.Forms.DataGridView();
            this.DisassemblyGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SourceData)).BeginInit();
            this.SuspendLayout();
            // 
            // DisassemblyGroup
            // 
            this.DisassemblyGroup.Controls.Add(this.SourceData);
            this.DisassemblyGroup.Location = new System.Drawing.Point(9, 29);
            this.DisassemblyGroup.Name = "DisassemblyGroup";
            this.DisassemblyGroup.Size = new System.Drawing.Size(796, 614);
            this.DisassemblyGroup.TabIndex = 0;
            this.DisassemblyGroup.TabStop = false;
            this.DisassemblyGroup.Text = "Disassembly";
            // 
            // SourceData
            // 
            this.SourceData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SourceData.Location = new System.Drawing.Point(8, 22);
            this.SourceData.Name = "SourceData";
            this.SourceData.RowTemplate.Height = 25;
            this.SourceData.Size = new System.Drawing.Size(782, 586);
            this.SourceData.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 669);
            this.Controls.Add(this.DisassemblyGroup);
            this.Name = "Form1";
            this.Text = "Form1";
            this.DisassemblyGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SourceData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox DisassemblyGroup;
        private DataGridView SourceData;
    }
}