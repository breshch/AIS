namespace WarehouseForms.Forms.Adding
{
    partial class FormTypeOfCompany
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
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.textBoxTypeOfCompany = new System.Windows.Forms.TextBox();
            this.listBoxTypeOfCompanies = new System.Windows.Forms.ListBox();
            this.labelTypeOfCompany = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(151, 173);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(121, 23);
            this.buttonRemove.TabIndex = 7;
            this.buttonRemove.Text = "Удалить";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(12, 173);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(121, 23);
            this.buttonAdd.TabIndex = 6;
            this.buttonAdd.Text = "Добавить";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // textBoxTypeOfCompany
            // 
            this.textBoxTypeOfCompany.Location = new System.Drawing.Point(12, 140);
            this.textBoxTypeOfCompany.Name = "textBoxTypeOfCompany";
            this.textBoxTypeOfCompany.Size = new System.Drawing.Size(260, 20);
            this.textBoxTypeOfCompany.TabIndex = 5;
            // 
            // listBoxTypeOfCompanies
            // 
            this.listBoxTypeOfCompanies.FormattingEnabled = true;
            this.listBoxTypeOfCompanies.Location = new System.Drawing.Point(12, 12);
            this.listBoxTypeOfCompanies.Name = "listBoxTypeOfCompanies";
            this.listBoxTypeOfCompanies.Size = new System.Drawing.Size(260, 95);
            this.listBoxTypeOfCompanies.TabIndex = 4;
            // 
            // labelTypeOfCompany
            // 
            this.labelTypeOfCompany.AutoSize = true;
            this.labelTypeOfCompany.Location = new System.Drawing.Point(12, 124);
            this.labelTypeOfCompany.Name = "labelTypeOfCompany";
            this.labelTypeOfCompany.Size = new System.Drawing.Size(79, 13);
            this.labelTypeOfCompany.TabIndex = 8;
            this.labelTypeOfCompany.Text = "Тип компании";
            // 
            // FormTypeOfCompany
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 224);
            this.Controls.Add(this.labelTypeOfCompany);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.textBoxTypeOfCompany);
            this.Controls.Add(this.listBoxTypeOfCompanies);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormTypeOfCompany";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Тип компаний";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTypeOfCompany_FormClosing);
            this.Load += new System.EventHandler(this.FormTypeOfCompany_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.TextBox textBoxTypeOfCompany;
        private System.Windows.Forms.ListBox listBoxTypeOfCompanies;
        private System.Windows.Forms.Label labelTypeOfCompany;
    }
}