namespace WarehouseForms.Forms.Adding
{
    partial class FormWorker
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
            this.textBoxLastName = new System.Windows.Forms.TextBox();
            this.textBoxFirstName = new System.Windows.Forms.TextBox();
            this.textBoxMidName = new System.Windows.Forms.TextBox();
            this.dateTimePickerBirthDay = new System.Windows.Forms.DateTimePicker();
            this.radioButtonMale = new System.Windows.Forms.RadioButton();
            this.radioButtonFemale = new System.Windows.Forms.RadioButton();
            this.groupBoxSex = new System.Windows.Forms.GroupBox();
            this.textBoxAddress = new System.Windows.Forms.TextBox();
            this.dateTimePickerDateOfStart = new System.Windows.Forms.DateTimePicker();
            this.dataGridViewCompanyAndPost = new System.Windows.Forms.DataGridView();
            this.ColumnId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCompany = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSalary = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnHalfSalary = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonAddCompanyAndPost = new System.Windows.Forms.Button();
            this.pictureBoxPicture = new System.Windows.Forms.PictureBox();
            this.labelLastName = new System.Windows.Forms.Label();
            this.labelFirstName = new System.Windows.Forms.Label();
            this.labelMidName = new System.Windows.Forms.Label();
            this.labelSex = new System.Windows.Forms.Label();
            this.labelBirthDay = new System.Windows.Forms.Label();
            this.labelAddress = new System.Windows.Forms.Label();
            this.labelCellPhone = new System.Windows.Forms.Label();
            this.labelHomePhone = new System.Windows.Forms.Label();
            this.labelDateOfStart = new System.Windows.Forms.Label();
            this.groupBoxCompanyAndPost = new System.Windows.Forms.GroupBox();
            this.buttonRemoveCompanyAndPost = new System.Windows.Forms.Button();
            this.buttonAddPicture = new System.Windows.Forms.Button();
            this.buttonRemovePicture = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.textBoxCellPhone = new System.Windows.Forms.TextBox();
            this.textBoxHomePhone = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBoxSex.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCompanyAndPost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPicture)).BeginInit();
            this.groupBoxCompanyAndPost.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxLastName
            // 
            this.textBoxLastName.Location = new System.Drawing.Point(50, 34);
            this.textBoxLastName.Name = "textBoxLastName";
            this.textBoxLastName.Size = new System.Drawing.Size(100, 20);
            this.textBoxLastName.TabIndex = 0;
            // 
            // textBoxFirstName
            // 
            this.textBoxFirstName.Location = new System.Drawing.Point(50, 83);
            this.textBoxFirstName.Name = "textBoxFirstName";
            this.textBoxFirstName.Size = new System.Drawing.Size(100, 20);
            this.textBoxFirstName.TabIndex = 1;
            // 
            // textBoxMidName
            // 
            this.textBoxMidName.Location = new System.Drawing.Point(50, 128);
            this.textBoxMidName.Name = "textBoxMidName";
            this.textBoxMidName.Size = new System.Drawing.Size(100, 20);
            this.textBoxMidName.TabIndex = 2;
            // 
            // dateTimePickerBirthDay
            // 
            this.dateTimePickerBirthDay.Location = new System.Drawing.Point(50, 268);
            this.dateTimePickerBirthDay.Name = "dateTimePickerBirthDay";
            this.dateTimePickerBirthDay.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerBirthDay.TabIndex = 3;
            // 
            // radioButtonMale
            // 
            this.radioButtonMale.AutoSize = true;
            this.radioButtonMale.Checked = true;
            this.radioButtonMale.Location = new System.Drawing.Point(15, 13);
            this.radioButtonMale.Name = "radioButtonMale";
            this.radioButtonMale.Size = new System.Drawing.Size(34, 17);
            this.radioButtonMale.TabIndex = 4;
            this.radioButtonMale.TabStop = true;
            this.radioButtonMale.Text = "М";
            this.radioButtonMale.UseVisualStyleBackColor = true;
            // 
            // radioButtonFemale
            // 
            this.radioButtonFemale.AutoSize = true;
            this.radioButtonFemale.Location = new System.Drawing.Point(58, 13);
            this.radioButtonFemale.Name = "radioButtonFemale";
            this.radioButtonFemale.Size = new System.Drawing.Size(36, 17);
            this.radioButtonFemale.TabIndex = 5;
            this.radioButtonFemale.Text = "Ж";
            this.radioButtonFemale.UseVisualStyleBackColor = true;
            // 
            // groupBoxSex
            // 
            this.groupBoxSex.Controls.Add(this.radioButtonMale);
            this.groupBoxSex.Controls.Add(this.radioButtonFemale);
            this.groupBoxSex.Location = new System.Drawing.Point(50, 167);
            this.groupBoxSex.Name = "groupBoxSex";
            this.groupBoxSex.Size = new System.Drawing.Size(115, 37);
            this.groupBoxSex.TabIndex = 6;
            this.groupBoxSex.TabStop = false;
            // 
            // textBoxAddress
            // 
            this.textBoxAddress.Location = new System.Drawing.Point(65, 320);
            this.textBoxAddress.Multiline = true;
            this.textBoxAddress.Name = "textBoxAddress";
            this.textBoxAddress.Size = new System.Drawing.Size(100, 84);
            this.textBoxAddress.TabIndex = 7;
            // 
            // dateTimePickerDateOfStart
            // 
            this.dateTimePickerDateOfStart.Location = new System.Drawing.Point(65, 549);
            this.dateTimePickerDateOfStart.Name = "dateTimePickerDateOfStart";
            this.dateTimePickerDateOfStart.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerDateOfStart.TabIndex = 10;
            // 
            // dataGridViewCompanyAndPost
            // 
            this.dataGridViewCompanyAndPost.AllowUserToAddRows = false;
            this.dataGridViewCompanyAndPost.AllowUserToDeleteRows = false;
            this.dataGridViewCompanyAndPost.AllowUserToOrderColumns = true;
            this.dataGridViewCompanyAndPost.AllowUserToResizeColumns = false;
            this.dataGridViewCompanyAndPost.AllowUserToResizeRows = false;
            this.dataGridViewCompanyAndPost.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewCompanyAndPost.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewCompanyAndPost.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCompanyAndPost.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnId,
            this.ColumnCompany,
            this.ColumnPost,
            this.ColumnSalary,
            this.ColumnHalfSalary,
            this.ColumnDate});
            this.dataGridViewCompanyAndPost.Location = new System.Drawing.Point(6, 17);
            this.dataGridViewCompanyAndPost.MultiSelect = false;
            this.dataGridViewCompanyAndPost.Name = "dataGridViewCompanyAndPost";
            this.dataGridViewCompanyAndPost.ReadOnly = true;
            this.dataGridViewCompanyAndPost.RowHeadersVisible = false;
            this.dataGridViewCompanyAndPost.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewCompanyAndPost.Size = new System.Drawing.Size(598, 95);
            this.dataGridViewCompanyAndPost.TabIndex = 12;
            // 
            // ColumnId
            // 
            this.ColumnId.HeaderText = "Id";
            this.ColumnId.Name = "ColumnId";
            this.ColumnId.ReadOnly = true;
            this.ColumnId.Visible = false;
            // 
            // ColumnCompany
            // 
            this.ColumnCompany.HeaderText = "Компания";
            this.ColumnCompany.Name = "ColumnCompany";
            this.ColumnCompany.ReadOnly = true;
            // 
            // ColumnPost
            // 
            this.ColumnPost.HeaderText = "Должность";
            this.ColumnPost.Name = "ColumnPost";
            this.ColumnPost.ReadOnly = true;
            // 
            // ColumnSalary
            // 
            this.ColumnSalary.HeaderText = "Оклад";
            this.ColumnSalary.Name = "ColumnSalary";
            this.ColumnSalary.ReadOnly = true;
            // 
            // ColumnHalfSalary
            // 
            this.ColumnHalfSalary.HeaderText = "Совместительство";
            this.ColumnHalfSalary.Name = "ColumnHalfSalary";
            this.ColumnHalfSalary.ReadOnly = true;
            // 
            // ColumnDate
            // 
            this.ColumnDate.HeaderText = "Дата";
            this.ColumnDate.Name = "ColumnDate";
            this.ColumnDate.ReadOnly = true;
            // 
            // buttonAddCompanyAndPost
            // 
            this.buttonAddCompanyAndPost.Location = new System.Drawing.Point(6, 118);
            this.buttonAddCompanyAndPost.Name = "buttonAddCompanyAndPost";
            this.buttonAddCompanyAndPost.Size = new System.Drawing.Size(303, 23);
            this.buttonAddCompanyAndPost.TabIndex = 13;
            this.buttonAddCompanyAndPost.Text = "Добавить компанию и должность";
            this.buttonAddCompanyAndPost.UseVisualStyleBackColor = true;
            this.buttonAddCompanyAndPost.Click += new System.EventHandler(this.buttonAddCompanyAndPost_Click);
            // 
            // pictureBoxPicture
            // 
            this.pictureBoxPicture.Location = new System.Drawing.Point(318, 34);
            this.pictureBoxPicture.Name = "pictureBoxPicture";
            this.pictureBoxPicture.Size = new System.Drawing.Size(326, 254);
            this.pictureBoxPicture.TabIndex = 15;
            this.pictureBoxPicture.TabStop = false;
            // 
            // labelLastName
            // 
            this.labelLastName.AutoSize = true;
            this.labelLastName.Location = new System.Drawing.Point(47, 18);
            this.labelLastName.Name = "labelLastName";
            this.labelLastName.Size = new System.Drawing.Size(56, 13);
            this.labelLastName.TabIndex = 16;
            this.labelLastName.Text = "Фамилия";
            // 
            // labelFirstName
            // 
            this.labelFirstName.AutoSize = true;
            this.labelFirstName.Location = new System.Drawing.Point(50, 67);
            this.labelFirstName.Name = "labelFirstName";
            this.labelFirstName.Size = new System.Drawing.Size(29, 13);
            this.labelFirstName.TabIndex = 17;
            this.labelFirstName.Text = "Имя";
            // 
            // labelMidName
            // 
            this.labelMidName.AutoSize = true;
            this.labelMidName.Location = new System.Drawing.Point(50, 112);
            this.labelMidName.Name = "labelMidName";
            this.labelMidName.Size = new System.Drawing.Size(54, 13);
            this.labelMidName.TabIndex = 18;
            this.labelMidName.Text = "Отчество";
            // 
            // labelSex
            // 
            this.labelSex.AutoSize = true;
            this.labelSex.Location = new System.Drawing.Point(53, 157);
            this.labelSex.Name = "labelSex";
            this.labelSex.Size = new System.Drawing.Size(27, 13);
            this.labelSex.TabIndex = 19;
            this.labelSex.Text = "Пол";
            // 
            // labelBirthDay
            // 
            this.labelBirthDay.AutoSize = true;
            this.labelBirthDay.Location = new System.Drawing.Point(50, 252);
            this.labelBirthDay.Name = "labelBirthDay";
            this.labelBirthDay.Size = new System.Drawing.Size(86, 13);
            this.labelBirthDay.TabIndex = 20;
            this.labelBirthDay.Text = "Дата рождения";
            // 
            // labelAddress
            // 
            this.labelAddress.AutoSize = true;
            this.labelAddress.Location = new System.Drawing.Point(62, 304);
            this.labelAddress.Name = "labelAddress";
            this.labelAddress.Size = new System.Drawing.Size(38, 13);
            this.labelAddress.TabIndex = 21;
            this.labelAddress.Text = "Адрес";
            // 
            // labelCellPhone
            // 
            this.labelCellPhone.AutoSize = true;
            this.labelCellPhone.Location = new System.Drawing.Point(62, 426);
            this.labelCellPhone.Name = "labelCellPhone";
            this.labelCellPhone.Size = new System.Drawing.Size(112, 13);
            this.labelCellPhone.TabIndex = 22;
            this.labelCellPhone.Text = "Мобильный телефон";
            // 
            // labelHomePhone
            // 
            this.labelHomePhone.AutoSize = true;
            this.labelHomePhone.Location = new System.Drawing.Point(62, 491);
            this.labelHomePhone.Name = "labelHomePhone";
            this.labelHomePhone.Size = new System.Drawing.Size(108, 13);
            this.labelHomePhone.TabIndex = 23;
            this.labelHomePhone.Text = "Домашний телефон";
            // 
            // labelDateOfStart
            // 
            this.labelDateOfStart.AutoSize = true;
            this.labelDateOfStart.Location = new System.Drawing.Point(62, 533);
            this.labelDateOfStart.Name = "labelDateOfStart";
            this.labelDateOfStart.Size = new System.Drawing.Size(135, 13);
            this.labelDateOfStart.TabIndex = 24;
            this.labelDateOfStart.Text = "Дата принятия на работу";
            // 
            // groupBoxCompanyAndPost
            // 
            this.groupBoxCompanyAndPost.Controls.Add(this.buttonRemoveCompanyAndPost);
            this.groupBoxCompanyAndPost.Controls.Add(this.dataGridViewCompanyAndPost);
            this.groupBoxCompanyAndPost.Controls.Add(this.buttonAddCompanyAndPost);
            this.groupBoxCompanyAndPost.Location = new System.Drawing.Point(40, 599);
            this.groupBoxCompanyAndPost.Name = "groupBoxCompanyAndPost";
            this.groupBoxCompanyAndPost.Size = new System.Drawing.Size(612, 151);
            this.groupBoxCompanyAndPost.TabIndex = 27;
            this.groupBoxCompanyAndPost.TabStop = false;
            this.groupBoxCompanyAndPost.Text = "Компании и должности";
            // 
            // buttonRemoveCompanyAndPost
            // 
            this.buttonRemoveCompanyAndPost.Location = new System.Drawing.Point(315, 118);
            this.buttonRemoveCompanyAndPost.Name = "buttonRemoveCompanyAndPost";
            this.buttonRemoveCompanyAndPost.Size = new System.Drawing.Size(289, 23);
            this.buttonRemoveCompanyAndPost.TabIndex = 14;
            this.buttonRemoveCompanyAndPost.Text = "Удалить компанию и должность";
            this.buttonRemoveCompanyAndPost.UseVisualStyleBackColor = true;
            this.buttonRemoveCompanyAndPost.Click += new System.EventHandler(this.buttonRemoveCompanyAndPost_Click);
            // 
            // buttonAddPicture
            // 
            this.buttonAddPicture.Location = new System.Drawing.Point(318, 304);
            this.buttonAddPicture.Name = "buttonAddPicture";
            this.buttonAddPicture.Size = new System.Drawing.Size(75, 23);
            this.buttonAddPicture.TabIndex = 28;
            this.buttonAddPicture.Text = "Добавить";
            this.buttonAddPicture.UseVisualStyleBackColor = true;
            // 
            // buttonRemovePicture
            // 
            this.buttonRemovePicture.Location = new System.Drawing.Point(414, 304);
            this.buttonRemovePicture.Name = "buttonRemovePicture";
            this.buttonRemovePicture.Size = new System.Drawing.Size(75, 23);
            this.buttonRemovePicture.TabIndex = 29;
            this.buttonRemovePicture.Text = "Удалить";
            this.buttonRemovePicture.UseVisualStyleBackColor = true;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(75, 790);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(90, 23);
            this.buttonAdd.TabIndex = 14;
            this.buttonAdd.Text = "Добавить";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // textBoxCellPhone
            // 
            this.textBoxCellPhone.Location = new System.Drawing.Point(65, 442);
            this.textBoxCellPhone.Name = "textBoxCellPhone";
            this.textBoxCellPhone.Size = new System.Drawing.Size(100, 20);
            this.textBoxCellPhone.TabIndex = 30;
            // 
            // textBoxHomePhone
            // 
            this.textBoxHomePhone.Location = new System.Drawing.Point(65, 507);
            this.textBoxHomePhone.Name = "textBoxHomePhone";
            this.textBoxHomePhone.Size = new System.Drawing.Size(100, 20);
            this.textBoxHomePhone.TabIndex = 31;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(101, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 13);
            this.label1.TabIndex = 32;
            this.label1.Text = "*";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(76, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 13);
            this.label2.TabIndex = 33;
            this.label2.Text = "*";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(78, 152);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 13);
            this.label3.TabIndex = 34;
            this.label3.Text = "*";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(134, 247);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "*";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(98, 299);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(11, 13);
            this.label5.TabIndex = 36;
            this.label5.Text = "*";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(171, 422);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(11, 13);
            this.label6.TabIndex = 37;
            this.label6.Text = "*";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(195, 531);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(11, 13);
            this.label7.TabIndex = 38;
            this.label7.Text = "*";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(169, 595);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(11, 13);
            this.label8.TabIndex = 39;
            this.label8.Text = "*";
            // 
            // FormWorker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 836);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxHomePhone);
            this.Controls.Add(this.textBoxCellPhone);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.buttonRemovePicture);
            this.Controls.Add(this.buttonAddPicture);
            this.Controls.Add(this.groupBoxCompanyAndPost);
            this.Controls.Add(this.labelDateOfStart);
            this.Controls.Add(this.labelHomePhone);
            this.Controls.Add(this.labelCellPhone);
            this.Controls.Add(this.labelAddress);
            this.Controls.Add(this.labelBirthDay);
            this.Controls.Add(this.labelSex);
            this.Controls.Add(this.labelMidName);
            this.Controls.Add(this.labelFirstName);
            this.Controls.Add(this.labelLastName);
            this.Controls.Add(this.pictureBoxPicture);
            this.Controls.Add(this.dateTimePickerDateOfStart);
            this.Controls.Add(this.textBoxAddress);
            this.Controls.Add(this.groupBoxSex);
            this.Controls.Add(this.dateTimePickerBirthDay);
            this.Controls.Add(this.textBoxMidName);
            this.Controls.Add(this.textBoxFirstName);
            this.Controls.Add(this.textBoxLastName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormWorker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Работник";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormWorker_FormClosing);
            this.Load += new System.EventHandler(this.FormWorker_Load);
            this.groupBoxSex.ResumeLayout(false);
            this.groupBoxSex.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCompanyAndPost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPicture)).EndInit();
            this.groupBoxCompanyAndPost.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxLastName;
        private System.Windows.Forms.TextBox textBoxFirstName;
        private System.Windows.Forms.TextBox textBoxMidName;
        private System.Windows.Forms.DateTimePicker dateTimePickerBirthDay;
        private System.Windows.Forms.RadioButton radioButtonMale;
        private System.Windows.Forms.RadioButton radioButtonFemale;
        private System.Windows.Forms.GroupBox groupBoxSex;
        private System.Windows.Forms.TextBox textBoxAddress;
        private System.Windows.Forms.DateTimePicker dateTimePickerDateOfStart;
        private System.Windows.Forms.DataGridView dataGridViewCompanyAndPost;
        private System.Windows.Forms.Button buttonAddCompanyAndPost;
        private System.Windows.Forms.PictureBox pictureBoxPicture;
        private System.Windows.Forms.Label labelLastName;
        private System.Windows.Forms.Label labelFirstName;
        private System.Windows.Forms.Label labelMidName;
        private System.Windows.Forms.Label labelSex;
        private System.Windows.Forms.Label labelBirthDay;
        private System.Windows.Forms.Label labelAddress;
        private System.Windows.Forms.Label labelCellPhone;
        private System.Windows.Forms.Label labelHomePhone;
        private System.Windows.Forms.Label labelDateOfStart;
        private System.Windows.Forms.GroupBox groupBoxCompanyAndPost;
        private System.Windows.Forms.Button buttonAddPicture;
        private System.Windows.Forms.Button buttonRemovePicture;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonRemoveCompanyAndPost;
        private System.Windows.Forms.TextBox textBoxCellPhone;
        private System.Windows.Forms.TextBox textBoxHomePhone;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCompany;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPost;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSalary;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnHalfSalary;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDate;
    }
}