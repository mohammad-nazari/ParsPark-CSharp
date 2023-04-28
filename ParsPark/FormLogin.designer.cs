namespace ParsPark
{
	partial class FormLogin
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
			if(disposing && (components != null))
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
			this.btnSignIn = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.txtPassWord = new System.Windows.Forms.TextBox();
			this.txtUserName = new System.Windows.Forms.TextBox();
			this.lblPassWord = new System.Windows.Forms.Label();
			this.lblUserName = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtDBUser = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtDBPassword = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtDBName = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtDBSever = new System.Windows.Forms.TextBox();
			this.pnlSettings = new System.Windows.Forms.Panel();
			this.chbActiveODR = new System.Windows.Forms.CheckBox();
			this.nudPort = new System.Windows.Forms.NumericUpDown();
			this.btnSettings = new System.Windows.Forms.Button();
			this.ilIcons = new System.Windows.Forms.ImageList(this.components);
			this.lblSettings = new System.Windows.Forms.Label();
			this.pnlSettings.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
			this.SuspendLayout();
			// 
			// btnSignIn
			// 
			this.btnSignIn.Location = new System.Drawing.Point(159, 296);
			this.btnSignIn.Name = "btnSignIn";
			this.btnSignIn.Size = new System.Drawing.Size(137, 23);
			this.btnSignIn.TabIndex = 2;
			this.btnSignIn.Text = "ورود";
			this.btnSignIn.UseVisualStyleBackColor = true;
			this.btnSignIn.Click += new System.EventHandler(this.btnSignIn_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(23, 296);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(129, 23);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "خروج";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// txtPassWord
			// 
			this.txtPassWord.Location = new System.Drawing.Point(24, 89);
			this.txtPassWord.Name = "txtPassWord";
			this.txtPassWord.PasswordChar = '*';
			this.txtPassWord.Size = new System.Drawing.Size(181, 21);
			this.txtPassWord.TabIndex = 1;
			this.txtPassWord.Text = "admin";
			// 
			// txtUserName
			// 
			this.txtUserName.Location = new System.Drawing.Point(23, 63);
			this.txtUserName.Name = "txtUserName";
			this.txtUserName.Size = new System.Drawing.Size(182, 21);
			this.txtUserName.TabIndex = 0;
			this.txtUserName.Text = "admin";
			// 
			// lblPassWord
			// 
			this.lblPassWord.AutoSize = true;
			this.lblPassWord.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.lblPassWord.Location = new System.Drawing.Point(251, 93);
			this.lblPassWord.Name = "lblPassWord";
			this.lblPassWord.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lblPassWord.Size = new System.Drawing.Size(49, 13);
			this.lblPassWord.TabIndex = 6;
			this.lblPassWord.Text = "گذرواژه : ";
			// 
			// lblUserName
			// 
			this.lblUserName.AutoSize = true;
			this.lblUserName.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.lblUserName.Location = new System.Drawing.Point(237, 67);
			this.lblUserName.Name = "lblUserName";
			this.lblUserName.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lblUserName.Size = new System.Drawing.Size(63, 13);
			this.lblUserName.TabIndex = 8;
			this.lblUserName.Text = "نام کاربری : ";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label1.Location = new System.Drawing.Point(211, 31);
			this.label1.Name = "label1";
			this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label1.Size = new System.Drawing.Size(60, 13);
			this.label1.TabIndex = 10;
			this.label1.Text = "نام کاربری: ";
			// 
			// txtDBUser
			// 
			this.txtDBUser.Location = new System.Drawing.Point(3, 28);
			this.txtDBUser.Name = "txtDBUser";
			this.txtDBUser.Size = new System.Drawing.Size(181, 21);
			this.txtDBUser.TabIndex = 1;
			this.txtDBUser.Text = "root";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label2.Location = new System.Drawing.Point(225, 55);
			this.label2.Name = "label2";
			this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label2.Size = new System.Drawing.Size(46, 13);
			this.label2.TabIndex = 12;
			this.label2.Text = "گذرواژه: ";
			// 
			// txtDBPassword
			// 
			this.txtDBPassword.Location = new System.Drawing.Point(3, 52);
			this.txtDBPassword.Name = "txtDBPassword";
			this.txtDBPassword.PasswordChar = '*';
			this.txtDBPassword.Size = new System.Drawing.Size(181, 21);
			this.txtDBPassword.TabIndex = 2;
			this.txtDBPassword.Text = "root";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label3.Location = new System.Drawing.Point(201, 80);
			this.label3.Name = "label3";
			this.label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label3.Size = new System.Drawing.Size(70, 13);
			this.label3.TabIndex = 14;
			this.label3.Text = "پورت اتصالی: ";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label4.Location = new System.Drawing.Point(200, 104);
			this.label4.Name = "label4";
			this.label4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label4.Size = new System.Drawing.Size(71, 13);
			this.label4.TabIndex = 16;
			this.label4.Text = "نام پایگاه داده:";
			// 
			// txtDBName
			// 
			this.txtDBName.Location = new System.Drawing.Point(3, 101);
			this.txtDBName.Name = "txtDBName";
			this.txtDBName.Size = new System.Drawing.Size(181, 21);
			this.txtDBName.TabIndex = 4;
			this.txtDBName.Text = "parspark";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label5.Location = new System.Drawing.Point(204, 6);
			this.label5.Name = "label5";
			this.label5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label5.Size = new System.Drawing.Size(67, 13);
			this.label5.TabIndex = 18;
			this.label5.Text = "آدرس سرور: ";
			// 
			// txtDBSever
			// 
			this.txtDBSever.Location = new System.Drawing.Point(3, 3);
			this.txtDBSever.Name = "txtDBSever";
			this.txtDBSever.Size = new System.Drawing.Size(181, 21);
			this.txtDBSever.TabIndex = 0;
			this.txtDBSever.Text = "localhost";
			// 
			// pnlSettings
			// 
			this.pnlSettings.Controls.Add(this.chbActiveODR);
			this.pnlSettings.Controls.Add(this.nudPort);
			this.pnlSettings.Controls.Add(this.txtDBSever);
			this.pnlSettings.Controls.Add(this.label5);
			this.pnlSettings.Controls.Add(this.txtDBUser);
			this.pnlSettings.Controls.Add(this.label1);
			this.pnlSettings.Controls.Add(this.label4);
			this.pnlSettings.Controls.Add(this.txtDBPassword);
			this.pnlSettings.Controls.Add(this.txtDBName);
			this.pnlSettings.Controls.Add(this.label2);
			this.pnlSettings.Controls.Add(this.label3);
			this.pnlSettings.Location = new System.Drawing.Point(23, 140);
			this.pnlSettings.Margin = new System.Windows.Forms.Padding(2);
			this.pnlSettings.Name = "pnlSettings";
			this.pnlSettings.Size = new System.Drawing.Size(274, 151);
			this.pnlSettings.TabIndex = 19;
			// 
			// chbActiveODR
			// 
			this.chbActiveODR.AutoSize = true;
			this.chbActiveODR.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.chbActiveODR.Location = new System.Drawing.Point(144, 127);
			this.chbActiveODR.Name = "chbActiveODR";
			this.chbActiveODR.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chbActiveODR.Size = new System.Drawing.Size(127, 17);
			this.chbActiveODR.TabIndex = 19;
			this.chbActiveODR.Text = "فعال سازی پلاک خوان";
			this.chbActiveODR.UseVisualStyleBackColor = true;
			// 
			// nudPort
			// 
			this.nudPort.Location = new System.Drawing.Point(3, 77);
			this.nudPort.Margin = new System.Windows.Forms.Padding(2);
			this.nudPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.nudPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudPort.Name = "nudPort";
			this.nudPort.Size = new System.Drawing.Size(180, 21);
			this.nudPort.TabIndex = 3;
			this.nudPort.Value = new decimal(new int[] {
            3306,
            0,
            0,
            0});
			// 
			// btnSettings
			// 
			this.btnSettings.AutoSize = true;
			this.btnSettings.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnSettings.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnSettings.ImageKey = "left.png";
			this.btnSettings.ImageList = this.ilIcons;
			this.btnSettings.Location = new System.Drawing.Point(278, 119);
			this.btnSettings.Margin = new System.Windows.Forms.Padding(2);
			this.btnSettings.Name = "btnSettings";
			this.btnSettings.Size = new System.Drawing.Size(22, 22);
			this.btnSettings.TabIndex = 20;
			this.btnSettings.UseVisualStyleBackColor = true;
			this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
			// 
			// ilIcons
			// 
			this.ilIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilIcons.ImageStream")));
			this.ilIcons.TransparentColor = System.Drawing.Color.Transparent;
			this.ilIcons.Images.SetKeyName(0, "down.png");
			this.ilIcons.Images.SetKeyName(1, "left.png");
			this.ilIcons.Images.SetKeyName(2, "right.png");
			// 
			// lblSettings
			// 
			this.lblSettings.AutoSize = true;
			this.lblSettings.Cursor = System.Windows.Forms.Cursors.Hand;
			this.lblSettings.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.lblSettings.Location = new System.Drawing.Point(232, 124);
			this.lblSettings.Name = "lblSettings";
			this.lblSettings.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lblSettings.Size = new System.Drawing.Size(45, 13);
			this.lblSettings.TabIndex = 21;
			this.lblSettings.Text = "تنظیمات";
			this.lblSettings.Click += new System.EventHandler(this.lblSettings_Click);
			// 
			// FormLogin
			// 
			this.AcceptButton = this.btnSignIn;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(319, 329);
			this.Controls.Add(this.lblSettings);
			this.Controls.Add(this.btnSettings);
			this.Controls.Add(this.pnlSettings);
			this.Controls.Add(this.lblUserName);
			this.Controls.Add(this.lblPassWord);
			this.Controls.Add(this.txtUserName);
			this.Controls.Add(this.txtPassWord);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSignIn);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "FormLogin";
			this.Padding = new System.Windows.Forms.Padding(20, 60, 20, 5);
			this.Text = "ورود";
			this.TextAlign = System.Windows.Forms.VisualStyles.HorizontalAlign.Center;
			this.Theme = MetroFramework.MetroThemeStyle.Dark;
			this.Load += new System.EventHandler(this.FormLogin_Load);
			this.pnlSettings.ResumeLayout(false);
			this.pnlSettings.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnSignIn;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TextBox txtPassWord;
		private System.Windows.Forms.TextBox txtUserName;
		private System.Windows.Forms.Label lblPassWord;
		private System.Windows.Forms.Label lblUserName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtDBUser;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtDBPassword;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtDBName;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtDBSever;
		private System.Windows.Forms.Panel pnlSettings;
		private System.Windows.Forms.Button btnSettings;
		private System.Windows.Forms.Label lblSettings;
		private System.Windows.Forms.ImageList ilIcons;
		private System.Windows.Forms.NumericUpDown nudPort;
		private System.Windows.Forms.CheckBox chbActiveODR;
	}
}