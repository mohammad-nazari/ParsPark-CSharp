namespace ParsPark
{
	partial class FormEditBlackList
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEditBlackList));
			this.btnClose = new System.Windows.Forms.Button();
			this.btnSubmit = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.mtxtLPNumberThird = new System.Windows.Forms.MaskedTextBox();
			this.mtxtLPNumberSecond = new System.Windows.Forms.MaskedTextBox();
			this.mtxtLPNumberAlpha = new System.Windows.Forms.MaskedTextBox();
			this.mtxtLPNumberFirst = new System.Windows.Forms.MaskedTextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.tbDescription = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnClose
			// 
			resources.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
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
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Controls.Add(this.mtxtLPNumberThird);
			this.panel1.Controls.Add(this.mtxtLPNumberSecond);
			this.panel1.Controls.Add(this.mtxtLPNumberAlpha);
			this.panel1.Controls.Add(this.mtxtLPNumberFirst);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.tbDescription);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Name = "panel1";
			// 
			// mtxtLPNumberThird
			// 
			resources.ApplyResources(this.mtxtLPNumberThird, "mtxtLPNumberThird");
			this.mtxtLPNumberThird.Cursor = System.Windows.Forms.Cursors.Default;
			this.mtxtLPNumberThird.Name = "mtxtLPNumberThird";
			// 
			// mtxtLPNumberSecond
			// 
			resources.ApplyResources(this.mtxtLPNumberSecond, "mtxtLPNumberSecond");
			this.mtxtLPNumberSecond.Cursor = System.Windows.Forms.Cursors.Default;
			this.mtxtLPNumberSecond.Name = "mtxtLPNumberSecond";
			// 
			// mtxtLPNumberAlpha
			// 
			resources.ApplyResources(this.mtxtLPNumberAlpha, "mtxtLPNumberAlpha");
			this.mtxtLPNumberAlpha.Cursor = System.Windows.Forms.Cursors.Default;
			this.mtxtLPNumberAlpha.Name = "mtxtLPNumberAlpha";
			// 
			// mtxtLPNumberFirst
			// 
			resources.ApplyResources(this.mtxtLPNumberFirst, "mtxtLPNumberFirst");
			this.mtxtLPNumberFirst.Cursor = System.Windows.Forms.Cursors.Default;
			this.mtxtLPNumberFirst.Name = "mtxtLPNumberFirst";
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label4.Name = "label4";
			// 
			// tbDescription
			// 
			resources.ApplyResources(this.tbDescription, "tbDescription");
			this.tbDescription.Name = "tbDescription";
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label1.Name = "label1";
			// 
			// FormEditBlackList
			// 
			this.AcceptButton = this.btnSubmit;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			resources.ApplyResources(this, "$this");
			this.CancelButton = this.btnClose;
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnSubmit);
			this.Controls.Add(this.panel1);
			this.Name = "FormEditBlackList";
			this.TextAlign = System.Windows.Forms.VisualStyles.HorizontalAlign.Center;
			this.Theme = MetroFramework.MetroThemeStyle.Dark;
			this.Load += new System.EventHandler(this.FormEditBlackList_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnSubmit;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox tbDescription;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.MaskedTextBox mtxtLPNumberThird;
		private System.Windows.Forms.MaskedTextBox mtxtLPNumberSecond;
		private System.Windows.Forms.MaskedTextBox mtxtLPNumberAlpha;
		private System.Windows.Forms.MaskedTextBox mtxtLPNumberFirst;
	}
}