namespace ParsPark
{
	partial class FormEditUser
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEditUser));
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
			resources.ApplyResources(this.pbUser, "pbUser");
			this.pbUser.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pbUser.Name = "pbUser";
			this.pbUser.TabStop = false;
			this.pbUser.Click += new System.EventHandler(this.pbUser_Click);
			this.pbUser.MouseHover += new System.EventHandler(this.pbUser_MouseHover);
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label1.Name = "label1";
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label2.Name = "label2";
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label3.Name = "label3";
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label4.Name = "label4";
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label5.Name = "label5";
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label6.Name = "label6";
			// 
			// label7
			// 
			resources.ApplyResources(this.label7, "label7");
			this.label7.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label7.Name = "label7";
			// 
			// tbFname
			// 
			resources.ApplyResources(this.tbFname, "tbFname");
			this.tbFname.Name = "tbFname";
			// 
			// tbPasswordR
			// 
			resources.ApplyResources(this.tbPasswordR, "tbPasswordR");
			this.tbPasswordR.Name = "tbPasswordR";
			// 
			// tbPassword
			// 
			resources.ApplyResources(this.tbPassword, "tbPassword");
			this.tbPassword.Name = "tbPassword";
			// 
			// tbUsername
			// 
			resources.ApplyResources(this.tbUsername, "tbUsername");
			this.tbUsername.Name = "tbUsername";
			this.tbUsername.ReadOnly = true;
			// 
			// tbLname
			// 
			resources.ApplyResources(this.tbLname, "tbLname");
			this.tbLname.Name = "tbLname";
			// 
			// tbAddress
			// 
			resources.ApplyResources(this.tbAddress, "tbAddress");
			this.tbAddress.Name = "tbAddress";
			// 
			// btnOk
			// 
			resources.ApplyResources(this.btnOk, "btnOk");
			this.btnOk.Name = "btnOk";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// label8
			// 
			resources.ApplyResources(this.label8, "label8");
			this.label8.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label8.Name = "label8";
			// 
			// rbEmploee
			// 
			resources.ApplyResources(this.rbEmploee, "rbEmploee");
			this.rbEmploee.Checked = true;
			this.rbEmploee.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.rbEmploee.Name = "rbEmploee";
			this.rbEmploee.TabStop = true;
			this.rbEmploee.UseVisualStyleBackColor = true;
			// 
			// rbAdmin
			// 
			resources.ApplyResources(this.rbAdmin, "rbAdmin");
			this.rbAdmin.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.rbAdmin.Name = "rbAdmin";
			this.rbAdmin.TabStop = true;
			this.rbAdmin.UseVisualStyleBackColor = true;
			// 
			// mtbPhone
			// 
			resources.ApplyResources(this.mtbPhone, "mtbPhone");
			this.mtbPhone.Name = "mtbPhone";
			// 
			// ofdPicture
			// 
			this.ofdPicture.FileName = "openFileDialog1";
			resources.ApplyResources(this.ofdPicture, "ofdPicture");
			// 
			// FormEditUser
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			resources.ApplyResources(this, "$this");
			this.CancelButton = this.btnCancel;
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
			this.Name = "FormEditUser";
			this.TextAlign = System.Windows.Forms.VisualStyles.HorizontalAlign.Center;
			this.Theme = MetroFramework.MetroThemeStyle.Dark;
			this.Load += new System.EventHandler(this.FormEditUser_Load);
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