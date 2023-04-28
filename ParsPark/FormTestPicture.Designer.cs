namespace ParsPark
{
	partial class FormTestPicture
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTestPicture));
			this.pbCamare = new System.Windows.Forms.PictureBox();
			this.btnClose = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pbCamare)).BeginInit();
			this.SuspendLayout();
			// 
			// pbCamare
			// 
			this.pbCamare.Image = ((System.Drawing.Image)(resources.GetObject("pbCamare.Image")));
			this.pbCamare.InitialImage = ((System.Drawing.Image)(resources.GetObject("pbCamare.InitialImage")));
			this.pbCamare.Location = new System.Drawing.Point(23, 63);
			this.pbCamare.Name = "pbCamare";
			this.pbCamare.Size = new System.Drawing.Size(238, 208);
			this.pbCamare.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pbCamare.TabIndex = 0;
			this.pbCamare.TabStop = false;
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(23, 277);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(238, 23);
			this.btnClose.TabIndex = 1;
			this.btnClose.Text = "بستن";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// FormTestPicture
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoSize = true;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(284, 310);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.pbCamare);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "FormTestPicture";
			this.Padding = new System.Windows.Forms.Padding(20, 60, 20, 5);
			this.RightToLeftLayout = true;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "تست دوربین";
			this.TextAlign = System.Windows.Forms.VisualStyles.HorizontalAlign.Center;
			this.Theme = MetroFramework.MetroThemeStyle.Dark;
			this.TopMost = true;
			this.Load += new System.EventHandler(this.FormTestPicture_Load);
			((System.ComponentModel.ISupportInitialize)(this.pbCamare)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Button btnClose;
		public System.Windows.Forms.PictureBox pbCamare;
	}
}