namespace ParsPark
{
	partial class FormSubscriptions
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSubscriptions));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
			this.dgvSubscriptions = new System.Windows.Forms.DataGridView();
			this.SubscriptionRow = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SubscriptionCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SubscriptionFname = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SubscriptionLname = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SubscriptionLicense = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SubscriptionOrg = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SubscriptionStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SubscriptionEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SubscriptionDetails = new System.Windows.Forms.DataGridViewLinkColumn();
			this.SubscriptionCancel = new System.Windows.Forms.DataGridViewLinkColumn();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnAddSubscription = new System.Windows.Forms.Button();
			this.rbExpired = new System.Windows.Forms.RadioButton();
			this.rbCredit = new System.Windows.Forms.RadioButton();
			this.rbAll = new System.Windows.Forms.RadioButton();
			((System.ComponentModel.ISupportInitialize)(this.dgvSubscriptions)).BeginInit();
			this.SuspendLayout();
			// 
			// dgvSubscriptions
			// 
			this.dgvSubscriptions.AllowUserToAddRows = false;
			this.dgvSubscriptions.AllowUserToDeleteRows = false;
			this.dgvSubscriptions.AllowUserToResizeColumns = false;
			this.dgvSubscriptions.AllowUserToResizeRows = false;
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSubscriptions.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
			resources.ApplyResources(this.dgvSubscriptions, "dgvSubscriptions");
			this.dgvSubscriptions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle6.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
			dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSubscriptions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
			this.dgvSubscriptions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvSubscriptions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SubscriptionRow,
            this.SubscriptionCode,
            this.SubscriptionFname,
            this.SubscriptionLname,
            this.SubscriptionLicense,
            this.SubscriptionOrg,
            this.SubscriptionStart,
            this.SubscriptionEnd,
            this.SubscriptionDetails,
            this.SubscriptionCancel});
			dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle7.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
			dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSubscriptions.DefaultCellStyle = dataGridViewCellStyle7;
			this.dgvSubscriptions.MultiSelect = false;
			this.dgvSubscriptions.Name = "dgvSubscriptions";
			this.dgvSubscriptions.ReadOnly = true;
			dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle8.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
			dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSubscriptions.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
			this.dgvSubscriptions.RowHeadersVisible = false;
			this.dgvSubscriptions.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.dgvSubscriptions.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSubscriptions.RowTemplate.ReadOnly = true;
			this.dgvSubscriptions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvSubscriptions.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSubscriptions_CellContentClick);
			// 
			// SubscriptionRow
			// 
			this.SubscriptionRow.FillWeight = 5F;
			resources.ApplyResources(this.SubscriptionRow, "SubscriptionRow");
			this.SubscriptionRow.Name = "SubscriptionRow";
			this.SubscriptionRow.ReadOnly = true;
			// 
			// SubscriptionCode
			// 
			this.SubscriptionCode.FillWeight = 10F;
			resources.ApplyResources(this.SubscriptionCode, "SubscriptionCode");
			this.SubscriptionCode.Name = "SubscriptionCode";
			this.SubscriptionCode.ReadOnly = true;
			// 
			// SubscriptionFname
			// 
			this.SubscriptionFname.FillWeight = 10F;
			resources.ApplyResources(this.SubscriptionFname, "SubscriptionFname");
			this.SubscriptionFname.Name = "SubscriptionFname";
			this.SubscriptionFname.ReadOnly = true;
			// 
			// SubscriptionLname
			// 
			this.SubscriptionLname.FillWeight = 10F;
			resources.ApplyResources(this.SubscriptionLname, "SubscriptionLname");
			this.SubscriptionLname.Name = "SubscriptionLname";
			this.SubscriptionLname.ReadOnly = true;
			// 
			// SubscriptionLicense
			// 
			this.SubscriptionLicense.FillWeight = 10F;
			resources.ApplyResources(this.SubscriptionLicense, "SubscriptionLicense");
			this.SubscriptionLicense.Name = "SubscriptionLicense";
			this.SubscriptionLicense.ReadOnly = true;
			// 
			// SubscriptionOrg
			// 
			this.SubscriptionOrg.FillWeight = 20F;
			resources.ApplyResources(this.SubscriptionOrg, "SubscriptionOrg");
			this.SubscriptionOrg.Name = "SubscriptionOrg";
			this.SubscriptionOrg.ReadOnly = true;
			// 
			// SubscriptionStart
			// 
			this.SubscriptionStart.FillWeight = 10F;
			resources.ApplyResources(this.SubscriptionStart, "SubscriptionStart");
			this.SubscriptionStart.Name = "SubscriptionStart";
			this.SubscriptionStart.ReadOnly = true;
			// 
			// SubscriptionEnd
			// 
			this.SubscriptionEnd.FillWeight = 10F;
			resources.ApplyResources(this.SubscriptionEnd, "SubscriptionEnd");
			this.SubscriptionEnd.Name = "SubscriptionEnd";
			this.SubscriptionEnd.ReadOnly = true;
			// 
			// SubscriptionDetails
			// 
			this.SubscriptionDetails.FillWeight = 5F;
			resources.ApplyResources(this.SubscriptionDetails, "SubscriptionDetails");
			this.SubscriptionDetails.Name = "SubscriptionDetails";
			this.SubscriptionDetails.ReadOnly = true;
			this.SubscriptionDetails.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.SubscriptionDetails.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// SubscriptionCancel
			// 
			this.SubscriptionCancel.FillWeight = 10F;
			resources.ApplyResources(this.SubscriptionCancel, "SubscriptionCancel");
			this.SubscriptionCancel.Name = "SubscriptionCancel";
			this.SubscriptionCancel.ReadOnly = true;
			this.SubscriptionCancel.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.SubscriptionCancel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// btnClose
			// 
			resources.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Name = "btnClose";
			this.btnClose.UseVisualStyleBackColor = true;
			// 
			// btnAddSubscription
			// 
			resources.ApplyResources(this.btnAddSubscription, "btnAddSubscription");
			this.btnAddSubscription.Name = "btnAddSubscription";
			this.btnAddSubscription.UseVisualStyleBackColor = true;
			this.btnAddSubscription.Click += new System.EventHandler(this.btnAddSubscription_Click);
			// 
			// rbExpired
			// 
			resources.ApplyResources(this.rbExpired, "rbExpired");
			this.rbExpired.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.rbExpired.Name = "rbExpired";
			this.rbExpired.UseVisualStyleBackColor = true;
			this.rbExpired.CheckedChanged += new System.EventHandler(this.rbExpired_CheckedChanged);
			// 
			// rbCredit
			// 
			resources.ApplyResources(this.rbCredit, "rbCredit");
			this.rbCredit.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.rbCredit.Name = "rbCredit";
			this.rbCredit.UseVisualStyleBackColor = true;
			this.rbCredit.CheckedChanged += new System.EventHandler(this.rbCredit_CheckedChanged);
			// 
			// rbAll
			// 
			resources.ApplyResources(this.rbAll, "rbAll");
			this.rbAll.Checked = true;
			this.rbAll.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.rbAll.Name = "rbAll";
			this.rbAll.TabStop = true;
			this.rbAll.UseVisualStyleBackColor = true;
			this.rbAll.CheckedChanged += new System.EventHandler(this.rbAll_CheckedChanged);
			// 
			// FormSubscriptions
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.rbAll);
			this.Controls.Add(this.rbCredit);
			this.Controls.Add(this.rbExpired);
			this.Controls.Add(this.dgvSubscriptions);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnAddSubscription);
			this.Name = "FormSubscriptions";
			this.TextAlign = System.Windows.Forms.VisualStyles.HorizontalAlign.Center;
			this.Theme = MetroFramework.MetroThemeStyle.Dark;
			this.Load += new System.EventHandler(this.FormSubscriptions_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvSubscriptions)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnAddSubscription;
		private System.Windows.Forms.DataGridView dgvSubscriptions;
		private System.Windows.Forms.RadioButton rbExpired;
		private System.Windows.Forms.RadioButton rbCredit;
		private System.Windows.Forms.RadioButton rbAll;
		private System.Windows.Forms.DataGridViewTextBoxColumn SubscriptionRow;
		private System.Windows.Forms.DataGridViewTextBoxColumn SubscriptionCode;
		private System.Windows.Forms.DataGridViewTextBoxColumn SubscriptionFname;
		private System.Windows.Forms.DataGridViewTextBoxColumn SubscriptionLname;
		private System.Windows.Forms.DataGridViewTextBoxColumn SubscriptionLicense;
		private System.Windows.Forms.DataGridViewTextBoxColumn SubscriptionOrg;
		private System.Windows.Forms.DataGridViewTextBoxColumn SubscriptionStart;
		private System.Windows.Forms.DataGridViewTextBoxColumn SubscriptionEnd;
		private System.Windows.Forms.DataGridViewLinkColumn SubscriptionDetails;
		private System.Windows.Forms.DataGridViewLinkColumn SubscriptionCancel;
	}
}