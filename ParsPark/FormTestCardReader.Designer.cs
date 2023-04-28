namespace ParsPark
{
	partial class FormTestCardReader
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTestCardReader));
			this.btnClose = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.txtCardNumber = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.timerReadCard = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(23, 111);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(163, 23);
			this.btnClose.TabIndex = 0;
			this.btnClose.Text = "بستن";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label1.Location = new System.Drawing.Point(20, 60);
			this.label1.Name = "label1";
			this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label1.Size = new System.Drawing.Size(169, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "لطفا کارت را به دستگاه نزدیک کنید: ";
			// 
			// txtCardNumber
			// 
			this.txtCardNumber.Enabled = false;
			this.txtCardNumber.Location = new System.Drawing.Point(23, 85);
			this.txtCardNumber.Name = "txtCardNumber";
			this.txtCardNumber.ReadOnly = true;
			this.txtCardNumber.Size = new System.Drawing.Size(85, 21);
			this.txtCardNumber.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label2.Location = new System.Drawing.Point(118, 88);
			this.label2.Name = "label2";
			this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label2.Size = new System.Drawing.Size(71, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "شماره کارت : ";
			// 
			// timerReadCard
			// 
			this.timerReadCard.Enabled = true;
			this.timerReadCard.Tick += new System.EventHandler(this.timerReadCard_Tick);
			// 
			// FormTestCardReader
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoSize = true;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(212, 144);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtCardNumber);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnClose);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "FormTestCardReader";
			this.Padding = new System.Windows.Forms.Padding(20, 60, 20, 5);
			this.RightToLeftLayout = true;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "تست کارتخوان";
			this.TextAlign = System.Windows.Forms.VisualStyles.HorizontalAlign.Center;
			this.Theme = MetroFramework.MetroThemeStyle.Dark;
			this.Load += new System.EventHandler(this.FormTestCardReader_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtCardNumber;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Timer timerReadCard;
	}
}