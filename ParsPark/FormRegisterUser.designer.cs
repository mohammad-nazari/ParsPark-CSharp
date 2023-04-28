namespace ParsPark
{
    partial class FormRegisterUser
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRegisterUser));
			this.pbUser = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.tbFname = new System.Windows.Forms.TextBox();
			this.tbPasswordR = new System.Windows.Forms.TextBox();
			this.tbPassword = new System.Windows.Forms.TextBox();
			this.tbUsername = new System.Windows.Forms.TextBox();
			this.tbLname = new System.Windows.Forms.TextBox();
			this.tbAddress = new System.Windows.Forms.TextBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.rbEmploee = new System.Windows.Forms.RadioButton();
			this.rbAdmin = new System.Windows.Forms.RadioButton();
			this.mtbPhone = new System.Windows.Forms.MaskedTextBox();
			this.ofdPicture = new System.Windows.Forms.OpenFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.pbUser)).BeginInit();
			this.SuspendLayout();
			// 
			// pbUser
			// 
			this.pbUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pbUser.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pbUser.Image = ((System.Drawing.Image)(resources.GetObject("pbUser.Image")));
			this.pbUser.InitialImage = ((System.Drawing.Image)(resources.GetObject("pbUser.InitialImage")));
			this.pbUser.Location = new System.Drawing.Point(29, 63);
			this.pbUser.Name = "pbUser";
			this.pbUser.Size = new System.Drawing.Size(154, 172);
			this.pbUser.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pbUser.TabIndex = 0;
			this.pbUser.TabStop = false;
			this.pbUser.Click += new System.EventHandler(this.pbUser_Click);
			this.pbUser.MouseHover += new System.EventHandler(this.pbUser_MouseHover);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label1.Location = new System.Drawing.Point(445, 66);
			this.label1.Name = "label1";
			this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label1.Size = new System.Drawing.Size(30, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "نام : ";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label2.Location = new System.Drawing.Point(394, 170);
			this.label2.Name = "label2";
			this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label2.Size = new System.Drawing.Size(81, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "پسورد (مجدد) : ";
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label3.Location = new System.Drawing.Point(429, 144);
			this.label3.Name = "label3";
			this.label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label3.Size = new System.Drawing.Size(46, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "پسورد : ";
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.AutoSize = true;
			this.label4.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label4.Location = new System.Drawing.Point(433, 244);
			this.label4.Name = "label4";
			this.label4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label4.Size = new System.Drawing.Size(42, 13);
			this.label4.TabIndex = 4;
			this.label4.Text = "آدرس : ";
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.AutoSize = true;
			this.label5.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label5.Location = new System.Drawing.Point(399, 196);
			this.label5.Name = "label5";
			this.label5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label5.Size = new System.Drawing.Size(76, 13);
			this.label5.TabIndex = 5;
			this.label5.Text = "شماره تماس : ";
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label6.AutoSize = true;
			this.label6.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label6.Location = new System.Drawing.Point(412, 118);
			this.label6.Name = "label6";
			this.label6.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label6.Size = new System.Drawing.Size(63, 13);
			this.label6.TabIndex = 6;
			this.label6.Text = "نام کاربری : ";
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label7.AutoSize = true;
			this.label7.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label7.Location = new System.Drawing.Point(403, 92);
			this.label7.Name = "label7";
			this.label7.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label7.Size = new System.Drawing.Size(72, 13);
			this.label7.TabIndex = 7;
			this.label7.Text = "نام خانوادگی :‌";
			// 
			// tbFname
			// 
			this.tbFname.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tbFname.Location = new System.Drawing.Point(189, 63);
			this.tbFname.Name = "tbFname";
			this.tbFname.Size = new System.Drawing.Size(200, 21);
			this.tbFname.TabIndex = 0;
			// 
			// tbPasswordR
			// 
			this.tbPasswordR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tbPasswordR.Location = new System.Drawing.Point(189, 167);
			this.tbPasswordR.Name = "tbPasswordR";
			this.tbPasswordR.PasswordChar = '*';
			this.tbPasswordR.Size = new System.Drawing.Size(200, 21);
			this.tbPasswordR.TabIndex = 4;
			// 
			// tbPassword
			// 
			this.tbPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tbPassword.Location = new System.Drawing.Point(189, 141);
			this.tbPassword.Name = "tbPassword";
			this.tbPassword.PasswordChar = '*';
			this.tbPassword.Size = new System.Drawing.Size(200, 21);
			this.tbPassword.TabIndex = 3;
			// 
			// tbUsername
			// 
			this.tbUsername.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tbUsername.Location = new System.Drawing.Point(189, 115);
			this.tbUsername.Name = "tbUsername";
			this.tbUsername.Size = new System.Drawing.Size(200, 21);
			this.tbUsername.TabIndex = 2;
			// 
			// tbLname
			// 
			this.tbLname.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tbLname.Location = new System.Drawing.Point(189, 89);
			this.tbLname.Name = "tbLname";
			this.tbLname.Size = new System.Drawing.Size(200, 21);
			this.tbLname.TabIndex = 1;
			// 
			// tbAddress
			// 
			this.tbAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tbAddress.Location = new System.Drawing.Point(29, 260);
			this.tbAddress.Multiline = true;
			this.tbAddress.Name = "tbAddress";
			this.tbAddress.Size = new System.Drawing.Size(446, 70);
			this.tbAddress.TabIndex = 8;
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.Location = new System.Drawing.Point(166, 336);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(309, 23);
			this.btnOk.TabIndex = 9;
			this.btnOk.Text = "ثبت";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(29, 336);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(125, 23);
			this.btnCancel.TabIndex = 10;
			this.btnCancel.Text = "بستن";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// label8
			// 
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label8.AutoSize = true;
			this.label8.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label8.Location = new System.Drawing.Point(420, 222);
			this.label8.Name = "label8";
			this.label8.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label8.Size = new System.Drawing.Size(55, 13);
			this.label8.TabIndex = 18;
			this.label8.Text = "نوع کاربر : ";
			// 
			// rbEmploee
			// 
			this.rbEmploee.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.rbEmploee.AutoSize = true;
			this.rbEmploee.Checked = true;
			this.rbEmploee.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.rbEmploee.Location = new System.Drawing.Point(189, 220);
			this.rbEmploee.Name = "rbEmploee";
			this.rbEmploee.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.rbEmploee.Size = new System.Drawing.Size(53, 17);
			this.rbEmploee.TabIndex = 7;
			this.rbEmploee.TabStop = true;
			this.rbEmploee.Text = "کارمند";
			this.rbEmploee.UseVisualStyleBackColor = true;
			// 
			// rbAdmin
			// 
			this.rbAdmin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.rbAdmin.AutoSize = true;
			this.rbAdmin.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.rbAdmin.Location = new System.Drawing.Point(277, 220);
			this.rbAdmin.Name = "rbAdmin";
			this.rbAdmin.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.rbAdmin.Size = new System.Drawing.Size(45, 17);
			this.rbAdmin.TabIndex = 6;
			this.rbAdmin.TabStop = true;
			this.rbAdmin.Text = "مدیر";
			this.rbAdmin.UseVisualStyleBackColor = true;
			// 
			// mtbPhone
			// 
			this.mtbPhone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.mtbPhone.Location = new System.Drawing.Point(189, 193);
			this.mtbPhone.Mask = "9000000000";
			this.mtbPhone.Name = "mtbPhone";
			this.mtbPhone.Size = new System.Drawing.Size(200, 21);
			this.mtbPhone.TabIndex = 5;
			// 
			// ofdPicture
			// 
			this.ofdPicture.FileName = "openFileDialog1";
			this.ofdPicture.Filter = "BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff|All Graphics Typ" +
    "es|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff";
			// 
			// FormRegisterUser
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoSize = true;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(498, 365);
			this.Controls.Add(this.mtbPhone);
			this.Controls.Add(this.rbAdmin);
			this.Controls.Add(this.rbEmploee);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.tbAddress);
			this.Controls.Add(this.tbLname);
			this.Controls.Add(this.tbUsername);
			this.Controls.Add(this.tbPassword);
			this.Controls.Add(this.tbPasswordR);
			this.Controls.Add(this.tbFname);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pbUser);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormRegisterUser";
			this.Padding = new System.Windows.Forms.Padding(20, 60, 20, 5);
			this.RightToLeftLayout = true;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ثبت کاربر جدید";
			this.TextAlign = System.Windows.Forms.VisualStyles.HorizontalAlign.Center;
			this.Theme = MetroFramework.MetroThemeStyle.Dark;
			this.Load += new System.EventHandler(this.FormRegisterUser_Load);
			((System.ComponentModel.ISupportInitialize)(this.pbUser)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbFname;
        private System.Windows.Forms.TextBox tbPasswordR;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbUsername;
        private System.Windows.Forms.TextBox tbLname;
        private System.Windows.Forms.TextBox tbAddress;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton rbEmploee;
        private System.Windows.Forms.RadioButton rbAdmin;
        private System.Windows.Forms.MaskedTextBox mtbPhone;
        private System.Windows.Forms.OpenFileDialog ofdPicture;
    }
}