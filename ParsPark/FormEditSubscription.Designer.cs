namespace ParsPark
{
	partial class FormEditSubscription
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEditSubscription));
			this.btnClose = new System.Windows.Forms.Button();
			this.btnSubmit = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.cbOrgans = new System.Windows.Forms.ComboBox();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.txtOrgName = new System.Windows.Forms.TextBox();
			this.txtOrgValue = new System.Windows.Forms.TextBox();
			this.pbCLP = new System.Windows.Forms.PictureBox();
			this.mtxtLPNo3 = new System.Windows.Forms.MaskedTextBox();
			this.mtxtLPNo2 = new System.Windows.Forms.MaskedTextBox();
			this.mtxtLPAlpha = new System.Windows.Forms.MaskedTextBox();
			this.mtxtLPNo1 = new System.Windows.Forms.MaskedTextBox();
			this.cbLP = new System.Windows.Forms.ComboBox();
			this.mtxtCost = new System.Windows.Forms.MaskedTextBox();
			this.mtxtPhone = new System.Windows.Forms.MaskedTextBox();
			this.dtsEnd = new Atf.UI.DateTimeSelector();
			this.dtsStart = new Atf.UI.DateTimeSelector();
			this.txtFname = new System.Windows.Forms.TextBox();
			this.txtLname = new System.Windows.Forms.TextBox();
			this.txtCode = new System.Windows.Forms.TextBox();
			this.rtxtAddress = new System.Windows.Forms.RichTextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.pbUser = new System.Windows.Forms.PictureBox();
			this.ofdPicture = new System.Windows.Forms.OpenFileDialog();
			this.timerCardReader = new System.Windows.Forms.Timer(this.components);
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbCLP)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbUser)).BeginInit();
			this.SuspendLayout();
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.Name = "btnClose";
			this.btnClose.UseVisualStyleBackColor = true;
			// 
			// btnSubmit
			// 
			resources.ApplyResources(this.btnSubmit, "btnSubmit");
			this.btnSubmit.Name = "btnSubmit";
			this.btnSubmit.UseVisualStyleBackColor = true;
			this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.cbOrgans);
			this.panel1.Controls.Add(this.label11);
			this.panel1.Controls.Add(this.label10);
			this.panel1.Controls.Add(this.txtOrgName);
			this.panel1.Controls.Add(this.txtOrgValue);
			this.panel1.Controls.Add(this.pbCLP);
			this.panel1.Controls.Add(this.mtxtLPNo3);
			this.panel1.Controls.Add(this.mtxtLPNo2);
			this.panel1.Controls.Add(this.mtxtLPAlpha);
			this.panel1.Controls.Add(this.mtxtLPNo1);
			this.panel1.Controls.Add(this.cbLP);
			this.panel1.Controls.Add(this.mtxtCost);
			this.panel1.Controls.Add(this.mtxtPhone);
			this.panel1.Controls.Add(this.dtsEnd);
			this.panel1.Controls.Add(this.dtsStart);
			this.panel1.Controls.Add(this.txtFname);
			this.panel1.Controls.Add(this.txtLname);
			this.panel1.Controls.Add(this.txtCode);
			this.panel1.Controls.Add(this.rtxtAddress);
			this.panel1.Controls.Add(this.label9);
			this.panel1.Controls.Add(this.label8);
			this.panel1.Controls.Add(this.label7);
			this.panel1.Controls.Add(this.label6);
			this.panel1.Controls.Add(this.label5);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.pbUser);
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			// 
			// cbOrgans
			// 
			this.cbOrgans.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbOrgans.FormattingEnabled = true;
			this.cbOrgans.Items.AddRange(new object[] {
            resources.GetString("cbOrgans.Items")});
			resources.ApplyResources(this.cbOrgans, "cbOrgans");
			this.cbOrgans.Name = "cbOrgans";
			this.cbOrgans.SelectedIndexChanged += new System.EventHandler(this.cbOrgans_SelectedIndexChanged);
			// 
			// label11
			// 
			resources.ApplyResources(this.label11, "label11");
			this.label11.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label11.Name = "label11";
			// 
			// label10
			// 
			resources.ApplyResources(this.label10, "label10");
			this.label10.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label10.Name = "label10";
			// 
			// txtOrgName
			// 
			resources.ApplyResources(this.txtOrgName, "txtOrgName");
			this.txtOrgName.Name = "txtOrgName";
			// 
			// txtOrgValue
			// 
			resources.ApplyResources(this.txtOrgValue, "txtOrgValue");
			this.txtOrgValue.Name = "txtOrgValue";
			// 
			// pbCLP
			// 
			this.pbCLP.Cursor = System.Windows.Forms.Cursors.Hand;
			resources.ApplyResources(this.pbCLP, "pbCLP");
			this.pbCLP.Name = "pbCLP";
			this.pbCLP.TabStop = false;
			this.pbCLP.Click += new System.EventHandler(this.pbCLP_Click);
			this.pbCLP.MouseHover += new System.EventHandler(this.pbCLP_MouseHover);
			// 
			// mtxtLPNo3
			// 
			this.mtxtLPNo3.Cursor = System.Windows.Forms.Cursors.Default;
			resources.ApplyResources(this.mtxtLPNo3, "mtxtLPNo3");
			this.mtxtLPNo3.Name = "mtxtLPNo3";
			// 
			// mtxtLPNo2
			// 
			this.mtxtLPNo2.Cursor = System.Windows.Forms.Cursors.Default;
			resources.ApplyResources(this.mtxtLPNo2, "mtxtLPNo2");
			this.mtxtLPNo2.Name = "mtxtLPNo2";
			// 
			// mtxtLPAlpha
			// 
			this.mtxtLPAlpha.Cursor = System.Windows.Forms.Cursors.Default;
			resources.ApplyResources(this.mtxtLPAlpha, "mtxtLPAlpha");
			this.mtxtLPAlpha.Name = "mtxtLPAlpha";
			// 
			// mtxtLPNo1
			// 
			this.mtxtLPNo1.Cursor = System.Windows.Forms.Cursors.Default;
			resources.ApplyResources(this.mtxtLPNo1, "mtxtLPNo1");
			this.mtxtLPNo1.Name = "mtxtLPNo1";
			// 
			// cbLP
			// 
			this.cbLP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbLP.FormattingEnabled = true;
			resources.ApplyResources(this.cbLP, "cbLP");
			this.cbLP.Name = "cbLP";
			this.cbLP.SelectedIndexChanged += new System.EventHandler(this.cbSelectLicense_SelectedIndexChanged);
			// 
			// mtxtCost
			// 
			resources.ApplyResources(this.mtxtCost, "mtxtCost");
			this.mtxtCost.Name = "mtxtCost";
			// 
			// mtxtPhone
			// 
			resources.ApplyResources(this.mtxtPhone, "mtxtPhone");
			this.mtxtPhone.Name = "mtxtPhone";
			// 
			// dtsEnd
			// 
			this.dtsEnd.CustomFormat = "dddd dd MMMM yyyy ساعت ss:mm:HH";
			resources.ApplyResources(this.dtsEnd, "dtsEnd");
			this.dtsEnd.Format = Atf.UI.DateTimeSelectorFormat.Custom;
			this.dtsEnd.Name = "dtsEnd";
			this.dtsEnd.UsePersianFormat = true;
			// 
			// dtsStart
			// 
			this.dtsStart.CustomFormat = "dddd dd MMMM yyyy ساعت ss:mm:HH";
			resources.ApplyResources(this.dtsStart, "dtsStart");
			this.dtsStart.Format = Atf.UI.DateTimeSelectorFormat.Custom;
			this.dtsStart.Name = "dtsStart";
			this.dtsStart.UsePersianFormat = true;
			// 
			// txtFname
			// 
			resources.ApplyResources(this.txtFname, "txtFname");
			this.txtFname.Name = "txtFname";
			// 
			// txtLname
			// 
			resources.ApplyResources(this.txtLname, "txtLname");
			this.txtLname.Name = "txtLname";
			// 
			// txtCode
			// 
			resources.ApplyResources(this.txtCode, "txtCode");
			this.txtCode.Name = "txtCode";
			this.txtCode.ReadOnly = true;
			// 
			// rtxtAddress
			// 
			resources.ApplyResources(this.rtxtAddress, "rtxtAddress");
			this.rtxtAddress.Name = "rtxtAddress";
			// 
			// label9
			// 
			resources.ApplyResources(this.label9, "label9");
			this.label9.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label9.Name = "label9";
			// 
			// label8
			// 
			resources.ApplyResources(this.label8, "label8");
			this.label8.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label8.Name = "label8";
			// 
			// label7
			// 
			resources.ApplyResources(this.label7, "label7");
			this.label7.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label7.Name = "label7";
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label6.Name = "label6";
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label5.Name = "label5";
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label4.Name = "label4";
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label3.Name = "label3";
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label2.Name = "label2";
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label1.Name = "label1";
			// 
			// pbUser
			// 
			this.pbUser.Cursor = System.Windows.Forms.Cursors.Hand;
			resources.ApplyResources(this.pbUser, "pbUser");
			this.pbUser.Name = "pbUser";
			this.pbUser.TabStop = false;
			this.pbUser.Click += new System.EventHandler(this.pictureBox1_Click);
			this.pbUser.MouseHover += new System.EventHandler(this.pbUser_MouseHover);
			// 
			// ofdPicture
			// 
			this.ofdPicture.FileName = "openFileDialog1";
			// 
			// timerCardReader
			// 
			this.timerCardReader.Tick += new System.EventHandler(this.timerCardReader_Tick);
			// 
			// FormEditSubscription
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnSubmit);
			this.Name = "FormEditSubscription";
			this.TextAlign = System.Windows.Forms.VisualStyles.HorizontalAlign.Center;
			this.Theme = MetroFramework.MetroThemeStyle.Dark;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEditSubscription_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormEditSubscription_FormClosed);
			this.Load += new System.EventHandler(this.FormEditSubscription_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbCLP)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbUser)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnSubmit;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.PictureBox pbUser;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtFname;
		private System.Windows.Forms.TextBox txtLname;
		private System.Windows.Forms.TextBox txtCode;
		private System.Windows.Forms.RichTextBox rtxtAddress;
		private System.Windows.Forms.OpenFileDialog ofdPicture;
		private Atf.UI.DateTimeSelector dtsEnd;
		private Atf.UI.DateTimeSelector dtsStart;
		private System.Windows.Forms.MaskedTextBox mtxtPhone;
		private System.Windows.Forms.MaskedTextBox mtxtCost;
		private System.Windows.Forms.ComboBox cbLP;
		private System.Windows.Forms.MaskedTextBox mtxtLPNo3;
		private System.Windows.Forms.MaskedTextBox mtxtLPNo2;
		private System.Windows.Forms.MaskedTextBox mtxtLPAlpha;
		private System.Windows.Forms.MaskedTextBox mtxtLPNo1;
		private System.Windows.Forms.Timer timerCardReader;
		private System.Windows.Forms.PictureBox pbCLP;
		private System.Windows.Forms.TextBox txtOrgName;
		private System.Windows.Forms.TextBox txtOrgValue;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.ComboBox cbOrgans;
	}
}