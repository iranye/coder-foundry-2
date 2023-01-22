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
            this.SuspendLayout();
            // 
            // txtImport
            // 
            this.txtImport.Location = new System.Drawing.Point(176, 32);
            this.txtImport.Multiline = true;
            this.txtImport.Name = "txtImport";
            this.txtImport.Size = new System.Drawing.Size(612, 265);
            this.txtImport.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(176, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Paste Text";
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(176, 459);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(612, 265);
            this.txtOutput.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(176, 436);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Output";
            // 
            // btnImportHtml
            // 
            this.btnImportHtml.Location = new System.Drawing.Point(176, 303);
            this.btnImportHtml.Name = "btnImportHtml";
            this.btnImportHtml.Size = new System.Drawing.Size(110, 29);
            this.btnImportHtml.TabIndex = 4;
            this.btnImportHtml.Text = "Import Html";
            this.btnImportHtml.UseVisualStyleBackColor = true;
            this.btnImportHtml.Click += new System.EventHandler(this.btnImportHtml_Click);
            // 
            // btnImportJson
            // 
            this.btnImportJson.Location = new System.Drawing.Point(292, 303);
            this.btnImportJson.Name = "btnImportJson";
            this.btnImportJson.Size = new System.Drawing.Size(110, 29);
            this.btnImportJson.TabIndex = 5;
            this.btnImportJson.Text = "Import JSON";
            this.btnImportJson.UseVisualStyleBackColor = true;
            this.btnImportHtml.Click += new System.EventHandler(this.btnImportJson_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 736);
            this.Controls.Add(this.btnImportJson);
            this.Controls.Add(this.btnImportHtml);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtImport);
            this.Name = "MainForm";
            this.Text = "Form1";
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
    }
}