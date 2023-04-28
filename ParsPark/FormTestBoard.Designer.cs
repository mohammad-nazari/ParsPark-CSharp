namespace ParsPark
{
	partial class FormTestBoard
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTestBoard));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.mtxtCost = new System.Windows.Forms.MaskedTextBox();
			this.btnSendCost = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label1.Location = new System.Drawing.Point(120, 60);
			this.label1.Name = "label1";
			this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label1.Size = new System.Drawing.Size(166, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "لطفا نتیجه را در تابلو مشاهده کنید:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label2.Location = new System.Drawing.Point(168, 88);
			this.label2.Name = "label2";
			this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label2.Size = new System.Drawing.Size(118, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "عدد دلخواه را وارد کنید : ";
			// 
			// mtxtCost
			// 
			this.mtxtCost.Location = new System.Drawing.Point(93, 85);
			this.mtxtCost.Name = "mtxtCost";
			this.mtxtCost.Size = new System.Drawing.Size(60, 21);
			this.mtxtCost.TabIndex = 0;
			// 
			// btnSendCost
			// 
			this.btnSendCost.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnSendCost.Location = new System.Drawing.Point(12, 83);
			this.btnSendCost.Name = "btnSendCost";
			this.btnSendCost.Size = new System.Drawing.Size(75, 23);
			this.btnSendCost.TabIndex = 1;
			this.btnSendCost.Text = "ارسال";
			this.btnSendCost.UseVisualStyleBackColor = true;
			this.btnSendCost.Click += new System.EventHandler(this.btnSendCost_Click);
			// 
			// FormTestBoard
			// 
			this.AcceptButton = this.btnSendCost;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoSize = true;
			this.CancelButton = this.btnSendCost;
			this.ClientSize = new System.Drawing.Size(298, 114);
			this.Controls.Add(this.btnSendCost);
			this.Controls.Add(this.mtxtCost);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "FormTestBoard";
			this.Padding = new System.Windows.Forms.Padding(20, 60, 20, 5);
			this.RightToLeftLayout = true;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "تست تابلو ها";
			this.TextAlign = System.Windows.Forms.VisualStyles.HorizontalAlign.Center;
			this.Theme = MetroFramework.MetroThemeStyle.Dark;
			this.Load += new System.EventHandler(this.FormTestBoard_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnSendCost;
		public System.Windows.Forms.MaskedTextBox mtxtCost;
	}
}