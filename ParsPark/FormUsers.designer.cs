namespace ParsPark
{
    partial class FormUsers
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUsers));
			this.dgvUsers = new System.Windows.Forms.DataGridView();
			this.UserRow = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.UserCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.UserFname = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.UserLname = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.UserUname = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.UserType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.UserImage = new System.Windows.Forms.DataGridViewImageColumn();
			this.UserEdit = new System.Windows.Forms.DataGridViewLinkColumn();
			this.UserDelete = new System.Windows.Forms.DataGridViewLinkColumn();
			this.btnAddUser = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
			this.SuspendLayout();
			// 
			// dgvUsers
			// 
			this.dgvUsers.AllowUserToAddRows = false;
			this.dgvUsers.AllowUserToDeleteRows = false;
			this.dgvUsers.AllowUserToResizeColumns = false;
			this.dgvUsers.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.dgvUsers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.dgvUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgvUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgvUsers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UserRow,
            this.UserCode,
            this.UserFname,
            this.UserLname,
            this.UserUname,
            this.UserType,
            this.UserImage,
            this.UserEdit,
            this.UserDelete});
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgvUsers.DefaultCellStyle = dataGridViewCellStyle3;
			this.dgvUsers.Location = new System.Drawing.Point(23, 63);
			this.dgvUsers.MultiSelect = false;
			this.dgvUsers.Name = "dgvUsers";
			this.dgvUsers.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
			dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvUsers.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.dgvUsers.RowHeadersVisible = false;
			this.dgvUsers.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.dgvUsers.RowsDefaultCellStyle = dataGridViewCellStyle5;
			this.dgvUsers.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.dgvUsers.RowTemplate.Height = 100;
			this.dgvUsers.RowTemplate.ReadOnly = true;
			this.dgvUsers.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvUsers.Size = new System.Drawing.Size(1218, 577);
			this.dgvUsers.TabIndex = 0;
			this.dgvUsers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUsers_CellContentClick);
			// 
			// UserRow
			// 
			this.UserRow.FillWeight = 5F;
			this.UserRow.HeaderText = "ردیف";
			this.UserRow.Name = "UserRow";
			this.UserRow.ReadOnly = true;
			this.UserRow.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			// 
			// UserCode
			// 
			this.UserCode.FillWeight = 10F;
			this.UserCode.HeaderText = "کد شناسایی";
			this.UserCode.Name = "UserCode";
			this.UserCode.ReadOnly = true;
			this.UserCode.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			// 
			// UserFname
			// 
			this.UserFname.FillWeight = 20F;
			this.UserFname.HeaderText = "نام";
			this.UserFname.Name = "UserFname";
			this.UserFname.ReadOnly = true;
			this.UserFname.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			// 
			// UserLname
			// 
			this.UserLname.FillWeight = 20F;
			this.UserLname.HeaderText = "نام خانوادگی";
			this.UserLname.Name = "UserLname";
			this.UserLname.ReadOnly = true;
			// 
			// UserUname
			// 
			this.UserUname.FillWeight = 10F;
			this.UserUname.HeaderText = "نام کاربری";
			this.UserUname.Name = "UserUname";
			this.UserUname.ReadOnly = true;
			this.UserUname.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			// 
			// UserType
			// 
			this.UserType.FillWeight = 5F;
			this.UserType.HeaderText = "نوع کاربر";
			this.UserType.Name = "UserType";
			this.UserType.ReadOnly = true;
			this.UserType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			// 
			// UserImage
			// 
			this.UserImage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.UserImage.FillWeight = 20F;
			this.UserImage.HeaderText = "تصویر";
			this.UserImage.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
			this.UserImage.Name = "UserImage";
			this.UserImage.ReadOnly = true;
			this.UserImage.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.UserImage.ToolTipText = "عکس کاربر";
			// 
			// UserEdit
			// 
			this.UserEdit.FillWeight = 5F;
			this.UserEdit.HeaderText = "ویرایش";
			this.UserEdit.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.UserEdit.Name = "UserEdit";
			this.UserEdit.ReadOnly = true;
			this.UserEdit.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.UserEdit.Text = "ویرایش";
			this.UserEdit.ToolTipText = "ویرایش مشخصات کاربر";
			this.UserEdit.UseColumnTextForLinkValue = true;
			// 
			// UserDelete
			// 
			this.UserDelete.FillWeight = 5F;
			this.UserDelete.HeaderText = "حذف";
			this.UserDelete.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.UserDelete.Name = "UserDelete";
			this.UserDelete.ReadOnly = true;
			this.UserDelete.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.UserDelete.Text = "حذف";
			this.UserDelete.ToolTipText = "حذف کاربر";
			this.UserDelete.UseColumnTextForLinkValue = true;
			// 
			// btnAddUser
			// 
			this.btnAddUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAddUser.Location = new System.Drawing.Point(760, 646);
			this.btnAddUser.Name = "btnAddUser";
			this.btnAddUser.Size = new System.Drawing.Size(481, 37);
			this.btnAddUser.TabIndex = 1;
			this.btnAddUser.Text = "ثبت کاربر جدید";
			this.btnAddUser.UseVisualStyleBackColor = true;
			this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(23, 646);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(169, 37);
			this.btnClose.TabIndex = 2;
			this.btnClose.Text = "بستن";
			this.btnClose.UseVisualStyleBackColor = true;
			// 
			// FormUsers
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoSize = true;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(1264, 691);
			this.Controls.Add(this.dgvUsers);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnAddUser);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(1280, 730);
			this.Name = "FormUsers";
			this.Padding = new System.Windows.Forms.Padding(20, 60, 20, 5);
			this.RightToLeftLayout = true;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "کاربران";
			this.TextAlign = System.Windows.Forms.VisualStyles.HorizontalAlign.Center;
			this.Theme = MetroFramework.MetroThemeStyle.Dark;
			this.Load += new System.EventHandler(this.FormUsers_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgvUsers;
		private System.Windows.Forms.DataGridViewTextBoxColumn UserRow;
		private System.Windows.Forms.DataGridViewTextBoxColumn UserCode;
		private System.Windows.Forms.DataGridViewTextBoxColumn UserFname;
		private System.Windows.Forms.DataGridViewTextBoxColumn UserLname;
		private System.Windows.Forms.DataGridViewTextBoxColumn UserUname;
		private System.Windows.Forms.DataGridViewTextBoxColumn UserType;
		private System.Windows.Forms.DataGridViewImageColumn UserImage;
		private System.Windows.Forms.DataGridViewLinkColumn UserEdit;
		private System.Windows.Forms.DataGridViewLinkColumn UserDelete;
	}
}