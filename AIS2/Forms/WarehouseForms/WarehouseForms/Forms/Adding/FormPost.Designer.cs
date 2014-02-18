namespace WarehouseForms.Forms.Adding
{
    partial class FormPost
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPost));
            this.dataGridViewPosts = new System.Windows.Forms.DataGridView();
            this.ColumnId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPostName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTypeOfPost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCompanyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnWorkerSalary = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnHalfWorkerSalary = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.comboBoxTypeOfPost = new System.Windows.Forms.ComboBox();
            this.comboBoxNameOfCompany = new System.Windows.Forms.ComboBox();
            this.textBoxNameOfPost = new System.Windows.Forms.TextBox();
            this.textBoxWorkerSalary = new System.Windows.Forms.TextBox();
            this.textBoxHalfWorkerSalary = new System.Windows.Forms.TextBox();
            this.labelTypeOfPost = new System.Windows.Forms.Label();
            this.labelNameOfCompany = new System.Windows.Forms.Label();
            this.labelNameOfPost = new System.Windows.Forms.Label();
            this.labelWorkerSalary = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.dateTimePickerDate = new System.Windows.Forms.DateTimePicker();
            this.labelDateTimePicker = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPosts)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewPosts
            // 
            this.dataGridViewPosts.AllowUserToAddRows = false;
            this.dataGridViewPosts.AllowUserToDeleteRows = false;
            this.dataGridViewPosts.AllowUserToOrderColumns = true;
            this.dataGridViewPosts.AllowUserToResizeColumns = false;
            this.dataGridViewPosts.AllowUserToResizeRows = false;
            this.dataGridViewPosts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewPosts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewPosts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPosts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnId,
            this.ColumnPostName,
            this.ColumnTypeOfPost,
            this.ColumnCompanyName,
            this.ColumnDate,
            this.ColumnWorkerSalary,
            this.ColumnHalfWorkerSalary});
            resources.ApplyResources(this.dataGridViewPosts, "dataGridViewPosts");
            this.dataGridViewPosts.MultiSelect = false;
            this.dataGridViewPosts.Name = "dataGridViewPosts";
            this.dataGridViewPosts.ReadOnly = true;
            this.dataGridViewPosts.RowHeadersVisible = false;
            this.dataGridViewPosts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPosts.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewPosts_CellMouseDoubleClick);
            // 
            // ColumnId
            // 
            resources.ApplyResources(this.ColumnId, "ColumnId");
            this.ColumnId.Name = "ColumnId";
            this.ColumnId.ReadOnly = true;
            // 
            // ColumnPostName
            // 
            resources.ApplyResources(this.ColumnPostName, "ColumnPostName");
            this.ColumnPostName.Name = "ColumnPostName";
            this.ColumnPostName.ReadOnly = true;
            // 
            // ColumnTypeOfPost
            // 
            resources.ApplyResources(this.ColumnTypeOfPost, "ColumnTypeOfPost");
            this.ColumnTypeOfPost.Name = "ColumnTypeOfPost";
            this.ColumnTypeOfPost.ReadOnly = true;
            // 
            // ColumnCompanyName
            // 
            resources.ApplyResources(this.ColumnCompanyName, "ColumnCompanyName");
            this.ColumnCompanyName.Name = "ColumnCompanyName";
            this.ColumnCompanyName.ReadOnly = true;
            // 
            // ColumnDate
            // 
            resources.ApplyResources(this.ColumnDate, "ColumnDate");
            this.ColumnDate.Name = "ColumnDate";
            this.ColumnDate.ReadOnly = true;
            // 
            // ColumnWorkerSalary
            // 
            resources.ApplyResources(this.ColumnWorkerSalary, "ColumnWorkerSalary");
            this.ColumnWorkerSalary.Name = "ColumnWorkerSalary";
            this.ColumnWorkerSalary.ReadOnly = true;
            // 
            // ColumnHalfWorkerSalary
            // 
            resources.ApplyResources(this.ColumnHalfWorkerSalary, "ColumnHalfWorkerSalary");
            this.ColumnHalfWorkerSalary.Name = "ColumnHalfWorkerSalary";
            this.ColumnHalfWorkerSalary.ReadOnly = true;
            // 
            // comboBoxTypeOfPost
            // 
            this.comboBoxTypeOfPost.FormattingEnabled = true;
            resources.ApplyResources(this.comboBoxTypeOfPost, "comboBoxTypeOfPost");
            this.comboBoxTypeOfPost.Name = "comboBoxTypeOfPost";
            // 
            // comboBoxNameOfCompany
            // 
            this.comboBoxNameOfCompany.FormattingEnabled = true;
            resources.ApplyResources(this.comboBoxNameOfCompany, "comboBoxNameOfCompany");
            this.comboBoxNameOfCompany.Name = "comboBoxNameOfCompany";
            // 
            // textBoxNameOfPost
            // 
            resources.ApplyResources(this.textBoxNameOfPost, "textBoxNameOfPost");
            this.textBoxNameOfPost.Name = "textBoxNameOfPost";
            // 
            // textBoxWorkerSalary
            // 
            resources.ApplyResources(this.textBoxWorkerSalary, "textBoxWorkerSalary");
            this.textBoxWorkerSalary.Name = "textBoxWorkerSalary";
            // 
            // textBoxHalfWorkerSalary
            // 
            resources.ApplyResources(this.textBoxHalfWorkerSalary, "textBoxHalfWorkerSalary");
            this.textBoxHalfWorkerSalary.Name = "textBoxHalfWorkerSalary";
            // 
            // labelTypeOfPost
            // 
            resources.ApplyResources(this.labelTypeOfPost, "labelTypeOfPost");
            this.labelTypeOfPost.Name = "labelTypeOfPost";
            // 
            // labelNameOfCompany
            // 
            resources.ApplyResources(this.labelNameOfCompany, "labelNameOfCompany");
            this.labelNameOfCompany.Name = "labelNameOfCompany";
            // 
            // labelNameOfPost
            // 
            resources.ApplyResources(this.labelNameOfPost, "labelNameOfPost");
            this.labelNameOfPost.Name = "labelNameOfPost";
            // 
            // labelWorkerSalary
            // 
            resources.ApplyResources(this.labelWorkerSalary, "labelWorkerSalary");
            this.labelWorkerSalary.Name = "labelWorkerSalary";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // buttonAdd
            // 
            resources.ApplyResources(this.buttonAdd, "buttonAdd");
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonRemove
            // 
            resources.ApplyResources(this.buttonRemove, "buttonRemove");
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            // 
            // dateTimePickerDate
            // 
            resources.ApplyResources(this.dateTimePickerDate, "dateTimePickerDate");
            this.dateTimePickerDate.MaxDate = new System.DateTime(2014, 2, 18, 0, 0, 0, 0);
            this.dateTimePickerDate.Name = "dateTimePickerDate";
            this.dateTimePickerDate.Value = new System.DateTime(2014, 2, 18, 0, 0, 0, 0);
            // 
            // labelDateTimePicker
            // 
            resources.ApplyResources(this.labelDateTimePicker, "labelDateTimePicker");
            this.labelDateTimePicker.Name = "labelDateTimePicker";
            // 
            // FormPost
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelDateTimePicker);
            this.Controls.Add(this.dateTimePickerDate);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.labelWorkerSalary);
            this.Controls.Add(this.labelNameOfPost);
            this.Controls.Add(this.labelNameOfCompany);
            this.Controls.Add(this.labelTypeOfPost);
            this.Controls.Add(this.textBoxHalfWorkerSalary);
            this.Controls.Add(this.textBoxWorkerSalary);
            this.Controls.Add(this.textBoxNameOfPost);
            this.Controls.Add(this.comboBoxNameOfCompany);
            this.Controls.Add(this.comboBoxTypeOfPost);
            this.Controls.Add(this.dataGridViewPosts);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormPost";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPost_FormClosing);
            this.Load += new System.EventHandler(this.FormPost_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPosts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewPosts;
        private System.Windows.Forms.ComboBox comboBoxTypeOfPost;
        private System.Windows.Forms.ComboBox comboBoxNameOfCompany;
        private System.Windows.Forms.TextBox textBoxNameOfPost;
        private System.Windows.Forms.TextBox textBoxWorkerSalary;
        private System.Windows.Forms.TextBox textBoxHalfWorkerSalary;
        private System.Windows.Forms.Label labelTypeOfPost;
        private System.Windows.Forms.Label labelNameOfCompany;
        private System.Windows.Forms.Label labelNameOfPost;
        private System.Windows.Forms.Label labelWorkerSalary;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.DateTimePicker dateTimePickerDate;
        private System.Windows.Forms.Label labelDateTimePicker;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPostName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTypeOfPost;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCompanyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnWorkerSalary;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnHalfWorkerSalary;


    }
}