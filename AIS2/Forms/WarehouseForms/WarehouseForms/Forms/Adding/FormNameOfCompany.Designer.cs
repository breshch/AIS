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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.comboBoxTypeOfCompanies = new System.Windows.Forms.ComboBox();
            this.labelTypeOfCompany = new System.Windows.Forms.Label();
            this.dataGridViewNameOfCompanies = new System.Windows.Forms.DataGridView();
            this.ColumnId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnNameOfCompany = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTypeOfCompany = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelNameOfCompany = new System.Windows.Forms.Label();
            this.textBoxNameOfCompany = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewNameOfCompanies)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(153, 246);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(119, 23);
            this.buttonRemove.TabIndex = 11;
            this.buttonRemove.Text = "Удалить";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(35, 246);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(112, 23);
            this.buttonAdd.TabIndex = 10;
            this.buttonAdd.Text = "Добавить";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // comboBoxTypeOfCompanies
            // 
            this.comboBoxTypeOfCompanies.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTypeOfCompanies.FormattingEnabled = true;
            this.comboBoxTypeOfCompanies.Location = new System.Drawing.Point(35, 209);
            this.comboBoxTypeOfCompanies.Name = "comboBoxTypeOfCompanies";
            this.comboBoxTypeOfCompanies.Size = new System.Drawing.Size(237, 21);
            this.comboBoxTypeOfCompanies.TabIndex = 12;
            // 
            // labelTypeOfCompany
            // 
            this.labelTypeOfCompany.AutoSize = true;
            this.labelTypeOfCompany.Location = new System.Drawing.Point(32, 193);
            this.labelTypeOfCompany.Name = "labelTypeOfCompany";
            this.labelTypeOfCompany.Size = new System.Drawing.Size(79, 13);
            this.labelTypeOfCompany.TabIndex = 13;
            this.labelTypeOfCompany.Text = "Тип компании";
            // 
            // dataGridViewNameOfCompanies
            // 
            this.dataGridViewNameOfCompanies.AllowUserToAddRows = false;
            this.dataGridViewNameOfCompanies.AllowUserToDeleteRows = false;
            this.dataGridViewNameOfCompanies.AllowUserToOrderColumns = true;
            this.dataGridViewNameOfCompanies.AllowUserToResizeColumns = false;
            this.dataGridViewNameOfCompanies.AllowUserToResizeRows = false;
            this.dataGridViewNameOfCompanies.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewNameOfCompanies.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewNameOfCompanies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewNameOfCompanies.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnId,
            this.ColumnNameOfCompany,
            this.ColumnTypeOfCompany});
            this.dataGridViewNameOfCompanies.Location = new System.Drawing.Point(35, 23);
            this.dataGridViewNameOfCompanies.MultiSelect = false;
            this.dataGridViewNameOfCompanies.Name = "dataGridViewNameOfCompanies";
            this.dataGridViewNameOfCompanies.ReadOnly = true;
            this.dataGridViewNameOfCompanies.RowHeadersVisible = false;
            this.dataGridViewNameOfCompanies.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewNameOfCompanies.Size = new System.Drawing.Size(240, 116);
            this.dataGridViewNameOfCompanies.TabIndex = 15;
            // 
            // ColumnId
            // 
            this.ColumnId.HeaderText = "id";
            this.ColumnId.Name = "ColumnId";
            this.ColumnId.ReadOnly = true;
            this.ColumnId.Visible = false;
            // 
            // ColumnNameOfCompany
            // 
            this.ColumnNameOfCompany.HeaderText = "Название компании";
            this.ColumnNameOfCompany.Name = "ColumnNameOfCompany";
            this.ColumnNameOfCompany.ReadOnly = true;
            // 
            // ColumnTypeOfCompany
            // 
            this.ColumnTypeOfCompany.HeaderText = "Тип компании";
            this.ColumnTypeOfCompany.Name = "ColumnTypeOfCompany";
            this.ColumnTypeOfCompany.ReadOnly = true;
            // 
            // labelNameOfCompany
            // 
            this.labelNameOfCompany.AutoSize = true;
            this.labelNameOfCompany.Location = new System.Drawing.Point(32, 152);
            this.labelNameOfCompany.Name = "labelNameOfCompany";
            this.labelNameOfCompany.Size = new System.Drawing.Size(110, 13);
            this.labelNameOfCompany.TabIndex = 17;
            this.labelNameOfCompany.Text = "Название компании";
            // 
            // textBoxNameOfCompany
            // 
            this.textBoxNameOfCompany.Location = new System.Drawing.Point(35, 168);
            this.textBoxNameOfCompany.Name = "textBoxNameOfCompany";
            this.textBoxNameOfCompany.Size = new System.Drawing.Size(237, 20);
            this.textBoxNameOfCompany.TabIndex = 16;
            // 
            // FormNameOfCompany
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(319, 319);
            this.Controls.Add(this.labelNameOfCompany);
            this.Controls.Add(this.textBoxNameOfCompany);
            this.Controls.Add(this.dataGridViewNameOfCompanies);
            this.Controls.Add(this.labelTypeOfCompany);
            this.Controls.Add(this.comboBoxTypeOfCompanies);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonAdd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormNameOfCompany";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Название компании";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormNameOfCompany_FormClosing);
            this.Load += new System.EventHandler(this.FormNameOfCompany_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewNameOfCompanies)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.ComboBox comboBoxTypeOfCompanies;
        private System.Windows.Forms.Label labelTypeOfCompany;
        private System.Windows.Forms.DataGridView dataGridViewNameOfCompanies;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNameOfCompany;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTypeOfCompany;
        private System.Windows.Forms.Label labelNameOfCompany;
        private System.Windows.Forms.TextBox textBoxNameOfCompany;
    }
}