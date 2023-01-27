namespace Bookmarket.Winforms
{
    partial class MainForm
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
            this.txtImport = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnImportHtml = new System.Windows.Forms.Button();
            this.btnImportJson = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAddTag = new System.Windows.Forms.Button();
            this.txtNewTag = new System.Windows.Forms.TextBox();
            this.clbTags = new System.Windows.Forms.CheckedListBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtImport
            // 
            this.txtImport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtImport.Location = new System.Drawing.Point(154, 24);
            this.txtImport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtImport.Multiline = true;
            this.txtImport.Name = "txtImport";
            this.txtImport.Size = new System.Drawing.Size(536, 200);
            this.txtImport.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(154, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Paste Text";
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Location = new System.Drawing.Point(154, 344);
            this.txtOutput.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(536, 200);
            this.txtOutput.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(154, 327);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Output";
            // 
            // btnImportHtml
            // 
            this.btnImportHtml.Location = new System.Drawing.Point(154, 227);
            this.btnImportHtml.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnImportHtml.Name = "btnImportHtml";
            this.btnImportHtml.Size = new System.Drawing.Size(96, 22);
            this.btnImportHtml.TabIndex = 4;
            this.btnImportHtml.Text = "Import Html";
            this.btnImportHtml.UseVisualStyleBackColor = true;
            this.btnImportHtml.Click += new System.EventHandler(this.btnImportJson_Click);
            // 
            // btnImportJson
            // 
            this.btnImportJson.Location = new System.Drawing.Point(256, 227);
            this.btnImportJson.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnImportJson.Name = "btnImportJson";
            this.btnImportJson.Size = new System.Drawing.Size(96, 22);
            this.btnImportJson.TabIndex = 5;
            this.btnImportJson.Text = "Import JSON";
            this.btnImportJson.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.btnAddTag);
            this.groupBox1.Controls.Add(this.txtNewTag);
            this.groupBox1.Controls.Add(this.clbTags);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(147, 552);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tags";
            // 
            // btnAddTag
            // 
            this.btnAddTag.Location = new System.Drawing.Point(7, 51);
            this.btnAddTag.Name = "btnAddTag";
            this.btnAddTag.Size = new System.Drawing.Size(134, 23);
            this.btnAddTag.TabIndex = 2;
            this.btnAddTag.Text = "Add";
            this.btnAddTag.UseVisualStyleBackColor = true;
            this.btnAddTag.Click += new System.EventHandler(this.btnAddTag_Click);
            // 
            // txtNewTag
            // 
            this.txtNewTag.Location = new System.Drawing.Point(7, 22);
            this.txtNewTag.Name = "txtNewTag";
            this.txtNewTag.Size = new System.Drawing.Size(134, 23);
            this.txtNewTag.TabIndex = 1;
            // 
            // clbTags
            // 
            this.clbTags.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clbTags.CheckOnClick = true;
            this.clbTags.FormattingEnabled = true;
            this.clbTags.Location = new System.Drawing.Point(3, 91);
            this.clbTags.Name = "clbTags";
            this.clbTags.Size = new System.Drawing.Size(141, 454);
            this.clbTags.TabIndex = 0;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(358, 227);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(88, 23);
            this.btnTest.TabIndex = 7;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 552);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnImportJson);
            this.Controls.Add(this.btnImportHtml);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtImport);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txtImport;
        private Label label1;
        private TextBox txtOutput;
        private Label label2;
        private Button btnImportHtml;
        private Button btnImportJson;
        private GroupBox groupBox1;
        private CheckedListBox clbTags;
        private Button btnAddTag;
        private TextBox txtNewTag;
        private Button btnTest;
    }
}