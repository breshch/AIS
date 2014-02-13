namespace WarehouseForms.Forms.Adding
{
    partial class FormTypeOfPost
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
            this.textBoxTypeOfPost = new System.Windows.Forms.TextBox();
            this.listBoxTypeOfPosts = new System.Windows.Forms.ListBox();
            this.labelTypeOfPost = new System.Windows.Forms.Label();
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
            // textBoxTypeOfPost
            // 
            this.textBoxTypeOfPost.Location = new System.Drawing.Point(12, 140);
            this.textBoxTypeOfPost.Name = "textBoxTypeOfPost";
            this.textBoxTypeOfPost.Size = new System.Drawing.Size(260, 20);
            this.textBoxTypeOfPost.TabIndex = 5;
            // 
            // listBoxTypeOfPosts
            // 
            this.listBoxTypeOfPosts.FormattingEnabled = true;
            this.listBoxTypeOfPosts.Location = new System.Drawing.Point(12, 12);
            this.listBoxTypeOfPosts.Name = "listBoxTypeOfPosts";
            this.listBoxTypeOfPosts.Size = new System.Drawing.Size(260, 95);
            this.listBoxTypeOfPosts.TabIndex = 4;
            // 
            // labelTypeOfPost
            // 
            this.labelTypeOfPost.AutoSize = true;
            this.labelTypeOfPost.Location = new System.Drawing.Point(12, 124);
            this.labelTypeOfPost.Name = "labelTypeOfPost";
            this.labelTypeOfPost.Size = new System.Drawing.Size(84, 13);
            this.labelTypeOfPost.TabIndex = 8;
            this.labelTypeOfPost.Text = "Тип должности";
            // 
            // FormTypeOfPost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 224);
            this.Controls.Add(this.labelTypeOfPost);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.textBoxTypeOfPost);
            this.Controls.Add(this.listBoxTypeOfPosts);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormTypeOfPost";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Тип должностей";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTypeOfPost_FormClosing);
            this.Load += new System.EventHandler(this.FormTypeOfPost_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.TextBox textBoxTypeOfPost;
        private System.Windows.Forms.ListBox listBoxTypeOfPosts;
        private System.Windows.Forms.Label labelTypeOfPost;
    }
}