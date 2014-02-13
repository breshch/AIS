namespace WarehouseForms.Forms.Adding
{
    partial class FormNameOfCompany
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
            this.textBoxNameOfCompany = new System.Windows.Forms.TextBox();
            this.listBoxNameOfCompanies = new System.Windows.Forms.ListBox();
            this.comboBoxTypeOfCompanies = new System.Windows.Forms.ComboBox();
            this.labelTypeOfCompany = new System.Windows.Forms.Label();
            this.labelNameOfCompany = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(153, 243);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(119, 23);
            this.buttonRemove.TabIndex = 11;
            this.buttonRemove.Text = "Удалить";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(35, 243);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(112, 23);
            this.buttonAdd.TabIndex = 10;
            this.buttonAdd.Text = "Добавить";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // textBoxNameOfCompany
            // 
            this.textBoxNameOfCompany.Location = new System.Drawing.Point(35, 217);
            this.textBoxNameOfCompany.Name = "textBoxNameOfCompany";
            this.textBoxNameOfCompany.Size = new System.Drawing.Size(237, 20);
            this.textBoxNameOfCompany.TabIndex = 9;
            this.textBoxNameOfCompany.TextAlignChanged += new System.EventHandler(this.textBoxNameOfCompany_TextChanged);
            // 
            // listBoxNameOfCompanies
            // 
            this.listBoxNameOfCompanies.FormattingEnabled = true;
            this.listBoxNameOfCompanies.Location = new System.Drawing.Point(35, 39);
            this.listBoxNameOfCompanies.Name = "listBoxNameOfCompanies";
            this.listBoxNameOfCompanies.Size = new System.Drawing.Size(237, 95);
            this.listBoxNameOfCompanies.TabIndex = 8;
            // 
            // comboBoxTypeOfCompanies
            // 
            this.comboBoxTypeOfCompanies.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTypeOfCompanies.FormattingEnabled = true;
            this.comboBoxTypeOfCompanies.Location = new System.Drawing.Point(35, 168);
            this.comboBoxTypeOfCompanies.Name = "comboBoxTypeOfCompanies";
            this.comboBoxTypeOfCompanies.Size = new System.Drawing.Size(237, 21);
            this.comboBoxTypeOfCompanies.TabIndex = 12;
            // 
            // labelTypeOfCompany
            // 
            this.labelTypeOfCompany.AutoSize = true;
            this.labelTypeOfCompany.Location = new System.Drawing.Point(32, 152);
            this.labelTypeOfCompany.Name = "labelTypeOfCompany";
            this.labelTypeOfCompany.Size = new System.Drawing.Size(79, 13);
            this.labelTypeOfCompany.TabIndex = 13;
            this.labelTypeOfCompany.Text = "Тип компании";
            // 
            // labelNameOfCompany
            // 
            this.labelNameOfCompany.AutoSize = true;
            this.labelNameOfCompany.Location = new System.Drawing.Point(32, 201);
            this.labelNameOfCompany.Name = "labelNameOfCompany";
            this.labelNameOfCompany.Size = new System.Drawing.Size(110, 13);
            this.labelNameOfCompany.TabIndex = 14;
            this.labelNameOfCompany.Text = "Название компании";
            // 
            // FormNameOfCompany
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(319, 287);
            this.Controls.Add(this.labelNameOfCompany);
            this.Controls.Add(this.labelTypeOfCompany);
            this.Controls.Add(this.comboBoxTypeOfCompanies);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.textBoxNameOfCompany);
            this.Controls.Add(this.listBoxNameOfCompanies);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormNameOfCompany";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Название компании";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormNameOfCompany_FormClosing);
            this.Load += new System.EventHandler(this.FormNameOfCompany_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.TextBox textBoxNameOfCompany;
        private System.Windows.Forms.ListBox listBoxNameOfCompanies;
        private System.Windows.Forms.ComboBox comboBoxTypeOfCompanies;
        private System.Windows.Forms.Label labelTypeOfCompany;
        private System.Windows.Forms.Label labelNameOfCompany;
    }
}