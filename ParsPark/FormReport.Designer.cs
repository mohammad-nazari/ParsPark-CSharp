namespace ParsPark
{
	partial class FormReport
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReport));
			this.dgvReport = new System.Windows.Forms.DataGridView();
			this.SelectRow = new GridViewCheckBoxColumn();
			this.Row = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.EnLicense = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ExLicense = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.EnterTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ExitTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Duration = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Cost = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Details = new System.Windows.Forms.DataGridViewLinkColumn();
			this.Print = new System.Windows.Forms.DataGridViewLinkColumn();
			this.panel2 = new System.Windows.Forms.Panel();
			this.chlbUsers = new System.Windows.Forms.CheckedListBox();
			this.lblRecordsCost = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.bsbSaveReport = new ComponentOwl.BetterSplitButton.BetterSplitButton();
			this.cmsSaveReport = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiSaveExcel = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiSavePDF = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiSaveXML = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiSaveCSV = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiSaveHTML = new System.Windows.Forms.ToolStripMenuItem();
			this.cbToDate = new System.Windows.Forms.CheckBox();
			this.cbFromDate = new System.Windows.Forms.CheckBox();
			this.lblRecordsCount = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.rbCarAll = new System.Windows.Forms.RadioButton();
			this.rbCarBlackList = new System.Windows.Forms.RadioButton();
			this.rbCarOk = new System.Windows.Forms.RadioButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chlbOrgs = new System.Windows.Forms.CheckedListBox();
			this.rbTypeAll = new System.Windows.Forms.RadioButton();
			this.rbTypeSubscribe = new System.Windows.Forms.RadioButton();
			this.rbTypePublic = new System.Windows.Forms.RadioButton();
			this.dtpEndTime = new Atf.UI.DateTimeSelector();
			this.dtpStartTime = new Atf.UI.DateTimeSelector();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnPrintReceipt = new System.Windows.Forms.Button();
			this.btnPrint = new System.Windows.Forms.Button();
			this.btnSearch = new System.Windows.Forms.Button();
			this.gridViewCheckBoxColumn1 = new GridViewCheckBoxColumn();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewLinkColumn1 = new System.Windows.Forms.DataGridViewLinkColumn();
			this.dataGridViewLinkColumn2 = new System.Windows.Forms.DataGridViewLinkColumn();
			this.sfdSaveReport = new System.Windows.Forms.SaveFileDialog();
			this.pdocReport = new System.Drawing.Printing.PrintDocument();
			this.pdlgReport = new System.Windows.Forms.PrintDialog();
			this.ppdlgReport = new System.Windows.Forms.PrintPreviewDialog();
			((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
			this.panel2.SuspendLayout();
			this.cmsSaveReport.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dgvReport
			// 
			this.dgvReport.AllowUserToAddRows = false;
			this.dgvReport.AllowUserToDeleteRows = false;
			this.dgvReport.AllowUserToResizeColumns = false;
			this.dgvReport.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvReport.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.dgvReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgvReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgvReport.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.dgvReport.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvReport.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SelectRow,
            this.Row,
            this.Code,
            this.EnLicense,
            this.ExLicense,
            this.EnterTime,
            this.ExitTime,
            this.Duration,
            this.Cost,
            this.Type,
            this.Details,
            this.Print});
			this.dgvReport.Location = new System.Drawing.Point(23, 63);
			this.dgvReport.MultiSelect = false;
			this.dgvReport.Name = "dgvReport";
			this.dgvReport.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.dgvReport.RowHeadersVisible = false;
			this.dgvReport.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvReport.RowsDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvReport.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.dgvReport.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvReport.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dgvReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvReport.Size = new System.Drawing.Size(1218, 496);
			this.dgvReport.TabIndex = 0;
			this.dgvReport.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReport_CellContentClick);
			// 
			// SelectRow
			// 
			this.SelectRow.FillWeight = 5F;
			this.SelectRow.HeaderText = "";
			this.SelectRow.Name = "SelectRow";
			this.SelectRow.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.SelectRow.ToolTipText = "انتخاب ردیف";
			// 
			// Row
			// 
			this.Row.FillWeight = 5F;
			this.Row.HeaderText = "ردیف";
			this.Row.Name = "Row";
			this.Row.ReadOnly = true;
			this.Row.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Row.ToolTipText = "ردیف";
			// 
			// Code
			// 
			this.Code.FillWeight = 10F;
			this.Code.HeaderText = "کد کارت";
			this.Code.Name = "Code";
			this.Code.ReadOnly = true;
			this.Code.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Code.ToolTipText = "کد کارت";
			// 
			// EnLicense
			// 
			this.EnLicense.FillWeight = 10F;
			this.EnLicense.HeaderText = "پلاک ورودی";
			this.EnLicense.Name = "EnLicense";
			this.EnLicense.ReadOnly = true;
			this.EnLicense.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.EnLicense.ToolTipText = "شماره پلاک ماشین در ورود";
			// 
			// ExLicense
			// 
			this.ExLicense.FillWeight = 10F;
			this.ExLicense.HeaderText = "پلاک خروجی";
			this.ExLicense.Name = "ExLicense";
			this.ExLicense.ReadOnly = true;
			this.ExLicense.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.ExLicense.ToolTipText = "شماره پلاک ماشین در خروج";
			// 
			// EnterTime
			// 
			this.EnterTime.FillWeight = 10F;
			this.EnterTime.HeaderText = "زمان ورود";
			this.EnterTime.Name = "EnterTime";
			this.EnterTime.ReadOnly = true;
			this.EnterTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.EnterTime.ToolTipText = "زمان ورود به پارکینگ";
			// 
			// ExitTime
			// 
			this.ExitTime.FillWeight = 10F;
			this.ExitTime.HeaderText = "زمان خروج";
			this.ExitTime.Name = "ExitTime";
			this.ExitTime.ReadOnly = true;
			this.ExitTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.ExitTime.ToolTipText = "زمان خروج از پارکینگ";
			// 
			// Duration
			// 
			this.Duration.FillWeight = 10F;
			this.Duration.HeaderText = "مدت زمان";
			this.Duration.Name = "Duration";
			this.Duration.ReadOnly = true;
			this.Duration.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Duration.ToolTipText = "مدت زمان حضور در پارکینگ";
			// 
			// Cost
			// 
			this.Cost.FillWeight = 5F;
			this.Cost.HeaderText = "هزینه";
			this.Cost.Name = "Cost";
			this.Cost.ReadOnly = true;
			this.Cost.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Cost.ToolTipText = "هزینه پارکینگ";
			// 
			// Type
			// 
			this.Type.FillWeight = 15F;
			this.Type.HeaderText = "نوع";
			this.Type.Name = "Type";
			// 
			// Details
			// 
			this.Details.FillWeight = 5F;
			this.Details.HeaderText = "جزئیات";
			this.Details.Name = "Details";
			this.Details.ReadOnly = true;
			this.Details.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Details.Text = "جزئیات";
			this.Details.ToolTipText = "جزئیات بیشتر";
			// 
			// Print
			// 
			this.Print.FillWeight = 5F;
			this.Print.HeaderText = "پرینت";
			this.Print.Name = "Print";
			this.Print.ReadOnly = true;
			this.Print.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Print.Text = "پرینت";
			this.Print.ToolTipText = "پرینت قبض";
			// 
			// panel2
			// 
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel2.Controls.Add(this.chlbUsers);
			this.panel2.Controls.Add(this.lblRecordsCost);
			this.panel2.Controls.Add(this.label2);
			this.panel2.Controls.Add(this.bsbSaveReport);
			this.panel2.Controls.Add(this.cbToDate);
			this.panel2.Controls.Add(this.cbFromDate);
			this.panel2.Controls.Add(this.lblRecordsCount);
			this.panel2.Controls.Add(this.label3);
			this.panel2.Controls.Add(this.groupBox2);
			this.panel2.Controls.Add(this.groupBox1);
			this.panel2.Controls.Add(this.dtpEndTime);
			this.panel2.Controls.Add(this.dtpStartTime);
			this.panel2.Controls.Add(this.btnClose);
			this.panel2.Controls.Add(this.btnPrintReceipt);
			this.panel2.Controls.Add(this.btnPrint);
			this.panel2.Controls.Add(this.btnSearch);
			this.panel2.Location = new System.Drawing.Point(23, 565);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(1218, 118);
			this.panel2.TabIndex = 3;
			// 
			// chlbUsers
			// 
			this.chlbUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.chlbUsers.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.chlbUsers.FormattingEnabled = true;
			this.chlbUsers.HorizontalScrollbar = true;
			this.chlbUsers.Location = new System.Drawing.Point(313, 17);
			this.chlbUsers.Name = "chlbUsers";
			this.chlbUsers.Size = new System.Drawing.Size(124, 84);
			this.chlbUsers.Sorted = true;
			this.chlbUsers.TabIndex = 13;
			// 
			// lblRecordsCost
			// 
			this.lblRecordsCost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lblRecordsCost.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.lblRecordsCost.Font = new System.Drawing.Font("Tahoma", 20F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
			this.lblRecordsCost.ForeColor = System.Drawing.Color.Green;
			this.lblRecordsCost.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lblRecordsCost.Location = new System.Drawing.Point(687, 25);
			this.lblRecordsCost.Name = "lblRecordsCost";
			this.lblRecordsCost.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lblRecordsCost.Size = new System.Drawing.Size(200, 35);
			this.lblRecordsCost.TabIndex = 39;
			this.lblRecordsCost.Text = "0";
			this.lblRecordsCost.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
			this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label2.Location = new System.Drawing.Point(760, 9);
			this.label2.Name = "label2";
			this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label2.Size = new System.Drawing.Size(124, 14);
			this.label2.TabIndex = 38;
			this.label2.Text = "مجموع پول دریافتی:";
			// 
			// bsbSaveReport
			// 
			this.bsbSaveReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bsbSaveReport.ContextMenuStrip = this.cmsSaveReport;
			this.bsbSaveReport.Location = new System.Drawing.Point(2, 32);
			this.bsbSaveReport.Name = "bsbSaveReport";
			this.bsbSaveReport.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.bsbSaveReport.Size = new System.Drawing.Size(156, 23);
			this.bsbSaveReport.TabIndex = 15;
			this.bsbSaveReport.Text = "ذخیره در Word";
			this.bsbSaveReport.Click += new System.EventHandler(this.bsbSaveReport_Click);
			// 
			// cmsSaveReport
			// 
			this.cmsSaveReport.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.cmsSaveReport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSaveExcel,
            this.tsmiSavePDF,
            this.tsmiSaveXML,
            this.tsmiSaveCSV,
            this.tsmiSaveHTML});
			this.cmsSaveReport.Name = "cmsSaveReport";
			this.cmsSaveReport.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.cmsSaveReport.Size = new System.Drawing.Size(152, 114);
			// 
			// tsmiSaveExcel
			// 
			this.tsmiSaveExcel.Name = "tsmiSaveExcel";
			this.tsmiSaveExcel.Size = new System.Drawing.Size(151, 22);
			this.tsmiSaveExcel.Text = "ذخیره در Excel";
			this.tsmiSaveExcel.Click += new System.EventHandler(this.tsmiSaveExcel_Click);
			// 
			// tsmiSavePDF
			// 
			this.tsmiSavePDF.Name = "tsmiSavePDF";
			this.tsmiSavePDF.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.tsmiSavePDF.Size = new System.Drawing.Size(151, 22);
			this.tsmiSavePDF.Text = "ذخیره در PDF";
			this.tsmiSavePDF.Click += new System.EventHandler(this.tsmiSavePDF_Click);
			// 
			// tsmiSaveXML
			// 
			this.tsmiSaveXML.Name = "tsmiSaveXML";
			this.tsmiSaveXML.Size = new System.Drawing.Size(151, 22);
			this.tsmiSaveXML.Text = "ذخیره در XML";
			this.tsmiSaveXML.Click += new System.EventHandler(this.tsmiSaveXML_Click);
			// 
			// tsmiSaveCSV
			// 
			this.tsmiSaveCSV.Name = "tsmiSaveCSV";
			this.tsmiSaveCSV.Size = new System.Drawing.Size(151, 22);
			this.tsmiSaveCSV.Text = "ذخیره در CSV";
			this.tsmiSaveCSV.Click += new System.EventHandler(this.tsmiSaveCSV_Click);
			// 
			// tsmiSaveHTML
			// 
			this.tsmiSaveHTML.Name = "tsmiSaveHTML";
			this.tsmiSaveHTML.Size = new System.Drawing.Size(151, 22);
			this.tsmiSaveHTML.Text = "ذخیره در HTML";
			this.tsmiSaveHTML.Click += new System.EventHandler(this.tsmiSaveHTML_Click);
			// 
			// cbToDate
			// 
			this.cbToDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cbToDate.AutoSize = true;
			this.cbToDate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
			this.cbToDate.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.cbToDate.Location = new System.Drawing.Point(1143, 68);
			this.cbToDate.Name = "cbToDate";
			this.cbToDate.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.cbToDate.Size = new System.Drawing.Size(72, 17);
			this.cbToDate.TabIndex = 6;
			this.cbToDate.Text = "تا تاریخ : ";
			this.cbToDate.UseVisualStyleBackColor = true;
			this.cbToDate.CheckedChanged += new System.EventHandler(this.cbToDate_CheckedChanged);
			// 
			// cbFromDate
			// 
			this.cbFromDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cbFromDate.AutoSize = true;
			this.cbFromDate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
			this.cbFromDate.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.cbFromDate.Location = new System.Drawing.Point(1142, 38);
			this.cbFromDate.Name = "cbFromDate";
			this.cbFromDate.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.cbFromDate.Size = new System.Drawing.Size(73, 17);
			this.cbFromDate.TabIndex = 5;
			this.cbFromDate.Text = "از تاریخ : ";
			this.cbFromDate.UseVisualStyleBackColor = true;
			this.cbFromDate.CheckedChanged += new System.EventHandler(this.cbFromDate_CheckedChanged);
			// 
			// lblRecordsCount
			// 
			this.lblRecordsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lblRecordsCount.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.lblRecordsCount.Font = new System.Drawing.Font("Tahoma", 20F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
			this.lblRecordsCount.ForeColor = System.Drawing.Color.Green;
			this.lblRecordsCount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lblRecordsCount.Location = new System.Drawing.Point(687, 76);
			this.lblRecordsCount.Name = "lblRecordsCount";
			this.lblRecordsCount.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lblRecordsCount.Size = new System.Drawing.Size(200, 35);
			this.lblRecordsCount.TabIndex = 29;
			this.lblRecordsCount.Text = "0";
			this.lblRecordsCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
			this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label3.Location = new System.Drawing.Point(790, 60);
			this.label3.Name = "label3";
			this.label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label3.Size = new System.Drawing.Size(94, 14);
			this.label3.TabIndex = 28;
			this.label3.Text = "نعداد رکورد ها:";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.rbCarAll);
			this.groupBox2.Controls.Add(this.rbCarBlackList);
			this.groupBox2.Controls.Add(this.rbCarOk);
			this.groupBox2.Location = new System.Drawing.Point(164, 5);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.groupBox2.Size = new System.Drawing.Size(143, 110);
			this.groupBox2.TabIndex = 27;
			this.groupBox2.TabStop = false;
			// 
			// rbCarAll
			// 
			this.rbCarAll.AutoSize = true;
			this.rbCarAll.Checked = true;
			this.rbCarAll.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.rbCarAll.Location = new System.Drawing.Point(91, 20);
			this.rbCarAll.Name = "rbCarAll";
			this.rbCarAll.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.rbCarAll.Size = new System.Drawing.Size(46, 17);
			this.rbCarAll.TabIndex = 0;
			this.rbCarAll.TabStop = true;
			this.rbCarAll.Text = "همه";
			this.rbCarAll.UseVisualStyleBackColor = true;
			// 
			// rbCarBlackList
			// 
			this.rbCarBlackList.AutoSize = true;
			this.rbCarBlackList.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.rbCarBlackList.Location = new System.Drawing.Point(7, 66);
			this.rbCarBlackList.Name = "rbCarBlackList";
			this.rbCarBlackList.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.rbCarBlackList.Size = new System.Drawing.Size(130, 17);
			this.rbCarBlackList.TabIndex = 2;
			this.rbCarBlackList.Text = "ماشینهای لیست سیاه";
			this.rbCarBlackList.UseVisualStyleBackColor = true;
			// 
			// rbCarOk
			// 
			this.rbCarOk.AutoSize = true;
			this.rbCarOk.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.rbCarOk.Location = new System.Drawing.Point(9, 43);
			this.rbCarOk.Name = "rbCarOk";
			this.rbCarOk.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.rbCarOk.Size = new System.Drawing.Size(128, 17);
			this.rbCarOk.TabIndex = 1;
			this.rbCarOk.Text = "ماشین های بدون مورد";
			this.rbCarOk.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.chlbOrgs);
			this.groupBox1.Controls.Add(this.rbTypeAll);
			this.groupBox1.Controls.Add(this.rbTypeSubscribe);
			this.groupBox1.Controls.Add(this.rbTypePublic);
			this.groupBox1.Location = new System.Drawing.Point(443, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.groupBox1.Size = new System.Drawing.Size(238, 112);
			this.groupBox1.TabIndex = 26;
			this.groupBox1.TabStop = false;
			// 
			// chlbOrgs
			// 
			this.chlbOrgs.Enabled = false;
			this.chlbOrgs.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.chlbOrgs.FormattingEnabled = true;
			this.chlbOrgs.Location = new System.Drawing.Point(6, 10);
			this.chlbOrgs.Name = "chlbOrgs";
			this.chlbOrgs.Size = new System.Drawing.Size(129, 84);
			this.chlbOrgs.Sorted = true;
			this.chlbOrgs.TabIndex = 3;
			this.chlbOrgs.SelectedIndexChanged += new System.EventHandler(this.clbOrgs_SelectedIndexChanged);
			// 
			// rbTypeAll
			// 
			this.rbTypeAll.AutoSize = true;
			this.rbTypeAll.Checked = true;
			this.rbTypeAll.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.rbTypeAll.Location = new System.Drawing.Point(186, 21);
			this.rbTypeAll.Name = "rbTypeAll";
			this.rbTypeAll.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.rbTypeAll.Size = new System.Drawing.Size(46, 17);
			this.rbTypeAll.TabIndex = 0;
			this.rbTypeAll.TabStop = true;
			this.rbTypeAll.Text = "همه";
			this.rbTypeAll.UseVisualStyleBackColor = true;
			// 
			// rbTypeSubscribe
			// 
			this.rbTypeSubscribe.AutoSize = true;
			this.rbTypeSubscribe.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.rbTypeSubscribe.Location = new System.Drawing.Point(141, 67);
			this.rbTypeSubscribe.Name = "rbTypeSubscribe";
			this.rbTypeSubscribe.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.rbTypeSubscribe.Size = new System.Drawing.Size(91, 17);
			this.rbTypeSubscribe.TabIndex = 2;
			this.rbTypeSubscribe.Text = "فقط مشترکین";
			this.rbTypeSubscribe.UseVisualStyleBackColor = true;
			this.rbTypeSubscribe.CheckedChanged += new System.EventHandler(this.rbTypeSubscribe_CheckedChanged);
			// 
			// rbTypePublic
			// 
			this.rbTypePublic.AutoSize = true;
			this.rbTypePublic.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.rbTypePublic.Location = new System.Drawing.Point(160, 44);
			this.rbTypePublic.Name = "rbTypePublic";
			this.rbTypePublic.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.rbTypePublic.Size = new System.Drawing.Size(72, 17);
			this.rbTypePublic.TabIndex = 1;
			this.rbTypePublic.Text = "فقط عادی";
			this.rbTypePublic.UseVisualStyleBackColor = true;
			// 
			// dtpEndTime
			// 
			this.dtpEndTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.dtpEndTime.CustomFormat = "dd / MM / yyyy ساعت mm:HH";
			this.dtpEndTime.Enabled = false;
			this.dtpEndTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.dtpEndTime.Format = Atf.UI.DateTimeSelectorFormat.Custom;
			this.dtpEndTime.Location = new System.Drawing.Point(893, 64);
			this.dtpEndTime.Name = "dtpEndTime";
			this.dtpEndTime.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.dtpEndTime.Size = new System.Drawing.Size(234, 24);
			this.dtpEndTime.TabIndex = 10;
			this.dtpEndTime.UsePersianFormat = true;
			// 
			// dtpStartTime
			// 
			this.dtpStartTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.dtpStartTime.CustomFormat = "dd / MM / yyyy ساعت mm:HH";
			this.dtpStartTime.Enabled = false;
			this.dtpStartTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.dtpStartTime.Format = Atf.UI.DateTimeSelectorFormat.Custom;
			this.dtpStartTime.Location = new System.Drawing.Point(893, 31);
			this.dtpStartTime.Name = "dtpStartTime";
			this.dtpStartTime.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.dtpStartTime.Size = new System.Drawing.Size(234, 24);
			this.dtpStartTime.TabIndex = 7;
			this.dtpStartTime.UsePersianFormat = true;
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnClose.Location = new System.Drawing.Point(2, 92);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(156, 23);
			this.btnClose.TabIndex = 18;
			this.btnClose.Text = "بستن";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnPrintReceipt
			// 
			this.btnPrintReceipt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPrintReceipt.Location = new System.Drawing.Point(2, 63);
			this.btnPrintReceipt.Name = "btnPrintReceipt";
			this.btnPrintReceipt.Size = new System.Drawing.Size(75, 23);
			this.btnPrintReceipt.TabIndex = 17;
			this.btnPrintReceipt.Text = "پرینت قبض";
			this.btnPrintReceipt.UseVisualStyleBackColor = true;
			this.btnPrintReceipt.Click += new System.EventHandler(this.btnPrintReceipt_Click);
			// 
			// btnPrint
			// 
			this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPrint.Location = new System.Drawing.Point(83, 63);
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(75, 23);
			this.btnPrint.TabIndex = 16;
			this.btnPrint.Text = "پرینت";
			this.btnPrint.UseVisualStyleBackColor = true;
			this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Location = new System.Drawing.Point(2, 5);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(156, 23);
			this.btnSearch.TabIndex = 14;
			this.btnSearch.Text = "جستجو";
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// gridViewCheckBoxColumn1
			// 
			this.gridViewCheckBoxColumn1.FalseValue = "0";
			this.gridViewCheckBoxColumn1.FillWeight = 5F;
			this.gridViewCheckBoxColumn1.HeaderText = "";
			this.gridViewCheckBoxColumn1.IndeterminateValue = "2";
			this.gridViewCheckBoxColumn1.Name = "gridViewCheckBoxColumn1";
			this.gridViewCheckBoxColumn1.ReadOnly = true;
			this.gridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.gridViewCheckBoxColumn1.ToolTipText = "انتخاب سطر";
			this.gridViewCheckBoxColumn1.TrueValue = "1";
			this.gridViewCheckBoxColumn1.Width = 61;
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.FillWeight = 10F;
			this.dataGridViewTextBoxColumn1.HeaderText = "شماره کد";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn1.ToolTipText = "شماره کد کارت";
			this.dataGridViewTextBoxColumn1.Width = 122;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.FillWeight = 10F;
			this.dataGridViewTextBoxColumn2.HeaderText = "شماره پلاک";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn2.ToolTipText = "شماره پلاک ماشین";
			this.dataGridViewTextBoxColumn2.Width = 121;
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.FillWeight = 15F;
			this.dataGridViewTextBoxColumn3.HeaderText = "زمان ورود";
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn3.ToolTipText = "زمان ورود به پارکینگ";
			this.dataGridViewTextBoxColumn3.Width = 183;
			// 
			// dataGridViewTextBoxColumn4
			// 
			this.dataGridViewTextBoxColumn4.FillWeight = 15F;
			this.dataGridViewTextBoxColumn4.HeaderText = "زمان خروج";
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn4.ToolTipText = "زمان خروج از پارکینگ";
			this.dataGridViewTextBoxColumn4.Width = 183;
			// 
			// dataGridViewTextBoxColumn5
			// 
			this.dataGridViewTextBoxColumn5.FillWeight = 10F;
			this.dataGridViewTextBoxColumn5.HeaderText = "مدت زمان";
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.ReadOnly = true;
			this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn5.ToolTipText = "مدت زمان حضور در پارکینگ";
			this.dataGridViewTextBoxColumn5.Width = 122;
			// 
			// dataGridViewTextBoxColumn6
			// 
			this.dataGridViewTextBoxColumn6.FillWeight = 10F;
			this.dataGridViewTextBoxColumn6.HeaderText = "هزینه";
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			this.dataGridViewTextBoxColumn6.ReadOnly = true;
			this.dataGridViewTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn6.ToolTipText = "هزینه پارکینگ";
			this.dataGridViewTextBoxColumn6.Width = 121;
			// 
			// dataGridViewTextBoxColumn7
			// 
			this.dataGridViewTextBoxColumn7.FillWeight = 10F;
			this.dataGridViewTextBoxColumn7.HeaderText = "نوع";
			this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
			this.dataGridViewTextBoxColumn7.ReadOnly = true;
			this.dataGridViewTextBoxColumn7.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn7.ToolTipText = "نوع کاربر";
			this.dataGridViewTextBoxColumn7.Width = 122;
			// 
			// dataGridViewLinkColumn1
			// 
			this.dataGridViewLinkColumn1.FillWeight = 5F;
			this.dataGridViewLinkColumn1.HeaderText = "جزئیات";
			this.dataGridViewLinkColumn1.Name = "dataGridViewLinkColumn1";
			this.dataGridViewLinkColumn1.ReadOnly = true;
			this.dataGridViewLinkColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewLinkColumn1.Text = "جزئیات";
			this.dataGridViewLinkColumn1.ToolTipText = "جزئیات بیشتر";
			this.dataGridViewLinkColumn1.Width = 61;
			// 
			// dataGridViewLinkColumn2
			// 
			this.dataGridViewLinkColumn2.FillWeight = 5F;
			this.dataGridViewLinkColumn2.HeaderText = "پرینت";
			this.dataGridViewLinkColumn2.Name = "dataGridViewLinkColumn2";
			this.dataGridViewLinkColumn2.ReadOnly = true;
			this.dataGridViewLinkColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewLinkColumn2.Text = "پرینت";
			this.dataGridViewLinkColumn2.ToolTipText = "پرینت قبض";
			this.dataGridViewLinkColumn2.Width = 61;
			// 
			// pdocReport
			// 
			this.pdocReport.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.pdocReport_PrintPage);
			// 
			// pdlgReport
			// 
			this.pdlgReport.UseEXDialog = true;
			// 
			// ppdlgReport
			// 
			this.ppdlgReport.AutoScrollMargin = new System.Drawing.Size(0, 0);
			this.ppdlgReport.AutoScrollMinSize = new System.Drawing.Size(0, 0);
			this.ppdlgReport.ClientSize = new System.Drawing.Size(400, 300);
			this.ppdlgReport.Enabled = true;
			this.ppdlgReport.Icon = ((System.Drawing.Icon)(resources.GetObject("ppdlgReport.Icon")));
			this.ppdlgReport.Name = "ppdlgReport";
			this.ppdlgReport.Visible = false;
			// 
			// FormReport
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoSize = true;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(1280, 730);
			this.Controls.Add(this.dgvReport);
			this.Controls.Add(this.panel2);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(1280, 730);
			this.Name = "FormReport";
			this.Padding = new System.Windows.Forms.Padding(20, 60, 20, 5);
			this.RightToLeftLayout = true;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "جستجو و گزارشگیری";
			this.TextAlign = System.Windows.Forms.VisualStyles.HorizontalAlign.Center;
			this.Theme = MetroFramework.MetroThemeStyle.Dark;
			this.Load += new System.EventHandler(this.FormReport_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.cmsSaveReport.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.DataGridView dgvReport;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button btnPrintReceipt;
		private System.Windows.Forms.Button btnPrint;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.Button btnClose;
		private Atf.UI.DateTimeSelector dtpEndTime;
		private Atf.UI.DateTimeSelector dtpStartTime;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton rbCarAll;
		private System.Windows.Forms.RadioButton rbCarBlackList;
		private System.Windows.Forms.RadioButton rbCarOk;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rbTypeAll;
		private System.Windows.Forms.RadioButton rbTypeSubscribe;
		private System.Windows.Forms.RadioButton rbTypePublic;
		private System.Windows.Forms.Label lblRecordsCount;
		private System.Windows.Forms.Label label3;
		private GridViewCheckBoxColumn gridViewCheckBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
		private System.Windows.Forms.DataGridViewLinkColumn dataGridViewLinkColumn1;
		private System.Windows.Forms.DataGridViewLinkColumn dataGridViewLinkColumn2;
		private System.Windows.Forms.CheckBox cbToDate;
		private System.Windows.Forms.CheckBox cbFromDate;
		private ComponentOwl.BetterSplitButton.BetterSplitButton bsbSaveReport;
		private System.Windows.Forms.ContextMenuStrip cmsSaveReport;
		private System.Windows.Forms.ToolStripMenuItem tsmiSaveExcel;
		private System.Windows.Forms.ToolStripMenuItem tsmiSavePDF;
		private System.Windows.Forms.ToolStripMenuItem tsmiSaveXML;
		private System.Windows.Forms.SaveFileDialog sfdSaveReport;
		private System.Windows.Forms.ToolStripMenuItem tsmiSaveCSV;
		private System.Windows.Forms.ToolStripMenuItem tsmiSaveHTML;
		private System.Drawing.Printing.PrintDocument pdocReport;
		private System.Windows.Forms.PrintDialog pdlgReport;
		private System.Windows.Forms.PrintPreviewDialog ppdlgReport;
		private System.Windows.Forms.Label lblRecordsCost;
		private System.Windows.Forms.Label label2;
		private GridViewCheckBoxColumn SelectRow;
		private System.Windows.Forms.DataGridViewTextBoxColumn Row;
		private System.Windows.Forms.DataGridViewTextBoxColumn Code;
		private System.Windows.Forms.DataGridViewTextBoxColumn EnLicense;
		private System.Windows.Forms.DataGridViewTextBoxColumn ExLicense;
		private System.Windows.Forms.DataGridViewTextBoxColumn EnterTime;
		private System.Windows.Forms.DataGridViewTextBoxColumn ExitTime;
		private System.Windows.Forms.DataGridViewTextBoxColumn Duration;
		private System.Windows.Forms.DataGridViewTextBoxColumn Cost;
		private System.Windows.Forms.DataGridViewTextBoxColumn Type;
		private System.Windows.Forms.DataGridViewLinkColumn Details;
		private System.Windows.Forms.DataGridViewLinkColumn Print;
		private System.Windows.Forms.CheckedListBox chlbUsers;
		private System.Windows.Forms.CheckedListBox chlbOrgs;
	}
}