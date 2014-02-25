namespace WarehouseForms.Forms.Adding
{
    partial class FormAddComanyAnadPostToWorker
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
            this.comboBoxCompanies = new System.Windows.Forms.ComboBox();
            this.comboBoxPosts = new System.Windows.Forms.ComboBox();
            this.dateTimePickerChangeDate = new System.Windows.Forms.DateTimePicker();
            this.labelCompany = new System.Windows.Forms.Label();
            this.labelPost = new System.Windows.Forms.Label();
            this.labelStartDate = new System.Windows.Forms.Label();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBoxCompanyies
            // 
            this.comboBoxCompanies.FormattingEnabled = true;
            this.comboBoxCompanies.Location = new System.Drawing.Point(25, 43);
            this.comboBoxCompanies.Name = "comboBoxCompanyies";
            this.comboBoxCompanies.Size = new System.Drawing.Size(200, 21);
            this.comboBoxCompanies.TabIndex = 0;
            this.comboBoxCompanies.SelectedIndexChanged += new System.EventHandler(this.comboBoxCompanyies_SelectedIndexChanged);
            // 
            // comboBoxPosts
            // 
            this.comboBoxPosts.FormattingEnabled = true;
            this.comboBoxPosts.Location = new System.Drawing.Point(25, 91);
            this.comboBoxPosts.Name = "comboBoxPosts";
            this.comboBoxPosts.Size = new System.Drawing.Size(200, 21);
            this.comboBoxPosts.TabIndex = 1;
            // 
            // dateTimePickerChangeDate
            // 
            this.dateTimePickerChangeDate.Location = new System.Drawing.Point(25, 137);
            this.dateTimePickerChangeDate.Name = "dateTimePickerChangeDate";
            this.dateTimePickerChangeDate.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerChangeDate.TabIndex = 2;
            // 
            // labelCompany
            // 
            this.labelCompany.AutoSize = true;
            this.labelCompany.Location = new System.Drawing.Point(22, 27);
            this.labelCompany.Name = "labelCompany";
            this.labelCompany.Size = new System.Drawing.Size(58, 13);
            this.labelCompany.TabIndex = 3;
            this.labelCompany.Text = "Компания";
            // 
            // labelPost
            // 
            this.labelPost.AutoSize = true;
            this.labelPost.Location = new System.Drawing.Point(22, 75);
            this.labelPost.Name = "labelPost";
            this.labelPost.Size = new System.Drawing.Size(65, 13);
            this.labelPost.TabIndex = 4;
            this.labelPost.Text = "Должность";
            // 
            // labelStartDate
            // 
            this.labelStartDate.AutoSize = true;
            this.labelStartDate.Location = new System.Drawing.Point(22, 121);
            this.labelStartDate.Name = "labelStartDate";
            this.labelStartDate.Size = new System.Drawing.Size(74, 13);
            this.labelStartDate.TabIndex = 5;
            this.labelStartDate.Text = "Дата приёма";
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(25, 176);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 6;
            this.buttonAdd.Text = "Добавить";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // FormAddComanyAnadPostToWorker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 227);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.labelStartDate);
            this.Controls.Add(this.labelPost);
            this.Controls.Add(this.labelCompany);
            this.Controls.Add(this.dateTimePickerChangeDate);
            this.Controls.Add(this.comboBoxPosts);
            this.Controls.Add(this.comboBoxCompanies);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormAddComanyAnadPostToWorker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Компании и должности";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAddComanyAnadPostToWorker_FormClosing);
            this.Load += new System.EventHandler(this.FormAddComanyAnadPostToWorker_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxCompanies;
        private System.Windows.Forms.ComboBox comboBoxPosts;
        private System.Windows.Forms.DateTimePicker dateTimePickerChangeDate;
        private System.Windows.Forms.Label labelCompany;
        private System.Windows.Forms.Label labelPost;
        private System.Windows.Forms.Label labelStartDate;
        private System.Windows.Forms.Button buttonAdd;
    }
}