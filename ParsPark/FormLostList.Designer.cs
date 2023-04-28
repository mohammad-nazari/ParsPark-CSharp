namespace ParsPark
{
	partial class FormLostList
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLostList));
			this.dgvReport = new System.Windows.Forms.DataGridView();
			this.ReportSelect = new GridViewCheckBoxColumn();
			this.ReportRow = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ReportCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ReportDriverName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ReportDriverNID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ReportDriverDID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ReportCarLicense = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ReportCardType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ReportFine = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ReportLostDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ReportDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnSearch = new System.Windows.Forms.Button();
			this.cbToDate = new System.Windows.Forms.CheckBox();
			this.cbFromDate = new System.Windows.Forms.CheckBox();
			this.bsbSaveReport = new ComponentOwl.BetterSplitButton.BetterSplitButton();
			this.cmsSaveReport = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiSaveExcel = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiSavePDF = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiSaveXML = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiSaveCSV = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiSaveHTML = new System.Windows.Forms.ToolStripMenuItem();
			this.ppdlgReport = new System.Windows.Forms.PrintPreviewDialog();
			this.sfdSaveReport = new System.Windows.Forms.SaveFileDialog();
			this.pdocReport = new System.Drawing.Printing.PrintDocument();
			this.pdlgReport = new System.Windows.Forms.PrintDialog();
			this.lblRecordsCost = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblRecordsCount = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.gridViewCheckBoxColumn1 = new GridViewCheckBoxColumn();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dtpEndTime = new Atf.UI.DateTimeSelector();
			this.dtpStartTime = new Atf.UI.DateTimeSelector();
			this.btnNew = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
			this.cmsSaveReport.SuspendLayout();
			this.SuspendLayout();
			// 
			// dgvReport
			// 
			this.dgvReport.AllowUserToAddRows = false;
			this.dgvReport.AllowUserToDeleteRows = false;
			this.dgvReport.AllowUserToResizeColumns = false;
			this.dgvReport.AllowUserToResizeRows = false;
			this.dgvReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgvReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvReport.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ReportSelect,
            this.ReportRow,
            this.ReportCode,
            this.ReportDriverName,
            this.ReportDriverNID,
            this.ReportDriverDID,
            this.ReportCarLicense,
            this.ReportCardType,
            this.ReportFine,
            this.ReportLostDate,
            this.ReportDescription});
			this.dgvReport.Location = new System.Drawing.Point(22, 62);
			this.dgvReport.Margin = new System.Windows.Forms.Padding(2);
			this.dgvReport.Name = "dgvReport";
			this.dgvReport.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.dgvReport.RowHeadersVisible = false;
			this.dgvReport.RowTemplate.Height = 24;
			this.dgvReport.Size = new System.Drawing.Size(1220, 538);
			this.dgvReport.TabIndex = 0;
			this.dgvReport.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReport_CellContentClick);
			// 
			// ReportSelect
			// 
			this.ReportSelect.FillWeight = 5F;
			this.ReportSelect.HeaderText = "";
			this.ReportSelect.Name = "ReportSelect";
			this.ReportSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.ReportSelect.ToolTipText = "انتخاب ردیف";
			// 
			// ReportRow
			// 
			this.ReportRow.FillWeight = 5F;
			this.ReportRow.HeaderText = "ردیف";
			this.ReportRow.Name = "ReportRow";
			this.ReportRow.ToolTipText = "ردیف";
			// 
			// ReportCode
			// 
			this.ReportCode.FillWeight = 10F;
			this.ReportCode.HeaderText = "شماره سریال";
			this.ReportCode.Name = "ReportCode";
			this.ReportCode.ToolTipText = "شماره سریال";
			// 
			// ReportDriverName
			// 
			this.ReportDriverName.FillWeight = 10F;
			this.ReportDriverName.HeaderText = "نام راننده";
			this.ReportDriverName.Name = "ReportDriverName";
			this.ReportDriverName.ToolTipText = "نام راننده";
			// 
			// ReportDriverNID
			// 
			this.ReportDriverNID.FillWeight = 10F;
			this.ReportDriverNID.HeaderText = "شماره ملی";
			this.ReportDriverNID.Name = "ReportDriverNID";
			this.ReportDriverNID.ToolTipText = "شماره ملی";
			// 
			// ReportDriverDID
			// 
			this.ReportDriverDID.FillWeight = 10F;
			this.ReportDriverDID.HeaderText = "شماره گواهی نامه";
			this.ReportDriverDID.Name = "ReportDriverDID";
			this.ReportDriverDID.ToolTipText = "شماره گواهی نامه";
			// 
			// ReportCarLicense
			// 
			this.ReportCarLicense.FillWeight = 10F;
			this.ReportCarLicense.HeaderText = "شماره پلاک";
			this.ReportCarLicense.Name = "ReportCarLicense";
			this.ReportCarLicense.ToolTipText = "شماره پلاک";
			// 
			// ReportCardType
			// 
			this.ReportCardType.FillWeight = 5F;
			this.ReportCardType.HeaderText = "نوع کارت";
			this.ReportCardType.Name = "ReportCardType";
			this.ReportCardType.ToolTipText = "نوع کارت";
			// 
			// ReportFine
			// 
			this.ReportFine.FillWeight = 10F;
			this.ReportFine.HeaderText = "جریمه (تومان)";
			this.ReportFine.Name = "ReportFine";
			this.ReportFine.ToolTipText = "جریمه (تومان)";
			// 
			// ReportLostDate
			// 
			this.ReportLostDate.FillWeight = 10F;
			this.ReportLostDate.HeaderText = "تاریخ گم شدن";
			this.ReportLostDate.Name = "ReportLostDate";
			this.ReportLostDate.ToolTipText = "تاریخ گم شدن";
			// 
			// ReportDescription
			// 
			this.ReportDescription.FillWeight = 20F;
			this.ReportDescription.HeaderText = "توضیحات";
			this.ReportDescription.Name = "ReportDescription";
			this.ReportDescription.ToolTipText = "توضیحات";
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(22, 629);
			this.btnClose.Margin = new System.Windows.Forms.Padding(2);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(206, 55);
			this.btnClose.TabIndex = 10;
			this.btnClose.Text = "بستن";
			this.btnClose.UseVisualStyleBackColor = true;
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Location = new System.Drawing.Point(642, 629);
			this.btnSearch.Margin = new System.Windows.Forms.Padding(2);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(200, 55);
			this.btnSearch.TabIndex = 8;
			this.btnSearch.Text = "جستجو";
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// cbToDate
			// 
			this.cbToDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cbToDate.AutoSize = true;
			this.cbToDate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
			this.cbToDate.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.cbToDate.Location = new System.Drawing.Point(1170, 666);
			this.cbToDate.Name = "cbToDate";
			this.cbToDate.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.cbToDate.Size = new System.Drawing.Size(72, 17);
			this.cbToDate.TabIndex = 4;
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
			this.cbFromDate.Location = new System.Drawing.Point(1169, 634);
			this.cbFromDate.Name = "cbFromDate";
			this.cbFromDate.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.cbFromDate.Size = new System.Drawing.Size(73, 17);
			this.cbFromDate.TabIndex = 0;
			this.cbFromDate.Text = "از تاریخ : ";
			this.cbFromDate.UseVisualStyleBackColor = true;
			this.cbFromDate.CheckedChanged += new System.EventHandler(this.cbFromDate_CheckedChanged);
			// 
			// bsbSaveReport
			// 
			this.bsbSaveReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bsbSaveReport.ContextMenuStrip = this.cmsSaveReport;
			this.bsbSaveReport.Location = new System.Drawing.Point(437, 629);
			this.bsbSaveReport.Name = "bsbSaveReport";
			this.bsbSaveReport.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.bsbSaveReport.Size = new System.Drawing.Size(200, 55);
			this.bsbSaveReport.TabIndex = 9;
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
			// pdlgReport
			// 
			this.pdlgReport.UseEXDialog = true;
			// 
			// lblRecordsCost
			// 
			this.lblRecordsCost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lblRecordsCost.Font = new System.Drawing.Font("Tahoma", 12F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))));
			this.lblRecordsCost.ForeColor = System.Drawing.Color.Green;
			this.lblRecordsCost.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lblRecordsCost.Location = new System.Drawing.Point(405, 607);
			this.lblRecordsCost.Name = "lblRecordsCost";
			this.lblRecordsCost.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lblRecordsCost.Size = new System.Drawing.Size(105, 19);
			this.lblRecordsCost.TabIndex = 43;
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
			this.label2.Location = new System.Drawing.Point(516, 611);
			this.label2.Name = "label2";
			this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label2.Size = new System.Drawing.Size(124, 14);
			this.label2.TabIndex = 42;
			this.label2.Text = "مجموع پول دریافتی:";
			// 
			// lblRecordsCount
			// 
			this.lblRecordsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lblRecordsCount.Font = new System.Drawing.Font("Tahoma", 12F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))));
			this.lblRecordsCount.ForeColor = System.Drawing.Color.Green;
			this.lblRecordsCount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lblRecordsCount.Location = new System.Drawing.Point(903, 602);
			this.lblRecordsCount.Name = "lblRecordsCount";
			this.lblRecordsCount.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lblRecordsCount.Size = new System.Drawing.Size(109, 19);
			this.lblRecordsCount.TabIndex = 41;
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
			this.label3.Location = new System.Drawing.Point(1018, 607);
			this.label3.Name = "label3";
			this.label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label3.Size = new System.Drawing.Size(139, 14);
			this.label3.TabIndex = 12;
			this.label3.Text = "نعداد رکورد یافت شده:";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
			this.label1.ForeColor = System.Drawing.Color.Green;
			this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label1.Location = new System.Drawing.Point(847, 602);
			this.label1.Name = "label1";
			this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label1.Size = new System.Drawing.Size(50, 19);
			this.label1.TabIndex = 41;
			this.label1.Text = "رکورد";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
			this.label4.ForeColor = System.Drawing.Color.Green;
			this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label4.Location = new System.Drawing.Point(349, 608);
			this.label4.Name = "label4";
			this.label4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label4.Size = new System.Drawing.Size(50, 19);
			this.label4.TabIndex = 43;
			this.label4.Text = "تومان";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// gridViewCheckBoxColumn1
			// 
			this.gridViewCheckBoxColumn1.FillWeight = 5F;
			this.gridViewCheckBoxColumn1.HeaderText = "";
			this.gridViewCheckBoxColumn1.Name = "gridViewCheckBoxColumn1";
			this.gridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.gridViewCheckBoxColumn1.ToolTipText = "انتخاب ردیف";
			this.gridViewCheckBoxColumn1.Width = 59;
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.FillWeight = 5F;
			this.dataGridViewTextBoxColumn1.HeaderText = "ردیف";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ToolTipText = "ردیف";
			this.dataGridViewTextBoxColumn1.Width = 59;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.FillWeight = 10F;
			this.dataGridViewTextBoxColumn2.HeaderText = "شماره سریال";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ToolTipText = "شماره سریال";
			this.dataGridViewTextBoxColumn2.Width = 117;
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.FillWeight = 10F;
			this.dataGridViewTextBoxColumn3.HeaderText = "نام راننده";
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ToolTipText = "نام راننده";
			this.dataGridViewTextBoxColumn3.Width = 118;
			// 
			// dataGridViewTextBoxColumn4
			// 
			this.dataGridViewTextBoxColumn4.FillWeight = 10F;
			this.dataGridViewTextBoxColumn4.HeaderText = "شماره ملی";
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ToolTipText = "شماره ملی";
			this.dataGridViewTextBoxColumn4.Width = 117;
			// 
			// dataGridViewTextBoxColumn5
			// 
			this.dataGridViewTextBoxColumn5.FillWeight = 10F;
			this.dataGridViewTextBoxColumn5.HeaderText = "شماره گواهی نامه";
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.ToolTipText = "شماره گواهی نامه";
			this.dataGridViewTextBoxColumn5.Width = 118;
			// 
			// dataGridViewTextBoxColumn6
			// 
			this.dataGridViewTextBoxColumn6.FillWeight = 10F;
			this.dataGridViewTextBoxColumn6.HeaderText = "شماره پلاک";
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			this.dataGridViewTextBoxColumn6.ToolTipText = "شماره پلاک";
			this.dataGridViewTextBoxColumn6.Width = 118;
			// 
			// dataGridViewTextBoxColumn7
			// 
			this.dataGridViewTextBoxColumn7.FillWeight = 5F;
			this.dataGridViewTextBoxColumn7.HeaderText = "نوع کارت";
			this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
			this.dataGridViewTextBoxColumn7.ToolTipText = "نوع کارت";
			this.dataGridViewTextBoxColumn7.Width = 59;
			// 
			// dataGridViewTextBoxColumn8
			// 
			this.dataGridViewTextBoxColumn8.FillWeight = 10F;
			this.dataGridViewTextBoxColumn8.HeaderText = "جریمه (تومان)";
			this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
			this.dataGridViewTextBoxColumn8.ToolTipText = "جریمه (تومان)";
			this.dataGridViewTextBoxColumn8.Width = 117;
			// 
			// dataGridViewTextBoxColumn9
			// 
			this.dataGridViewTextBoxColumn9.FillWeight = 10F;
			this.dataGridViewTextBoxColumn9.HeaderText = "تاریخ گم شدن";
			this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
			this.dataGridViewTextBoxColumn9.ToolTipText = "تاریخ گم شدن";
			this.dataGridViewTextBoxColumn9.Width = 118;
			// 
			// dataGridViewTextBoxColumn10
			// 
			this.dataGridViewTextBoxColumn10.FillWeight = 20F;
			this.dataGridViewTextBoxColumn10.HeaderText = "توضیحات";
			this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
			this.dataGridViewTextBoxColumn10.ToolTipText = "توضیحات";
			this.dataGridViewTextBoxColumn10.Width = 235;
			// 
			// dtpEndTime
			// 
			this.dtpEndTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.dtpEndTime.CustomFormat = "dd / MM / yyyy ساعت mm:HH";
			this.dtpEndTime.Enabled = false;
			this.dtpEndTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.dtpEndTime.Format = Atf.UI.DateTimeSelectorFormat.Custom;
			this.dtpEndTime.Location = new System.Drawing.Point(847, 660);
			this.dtpEndTime.Name = "dtpEndTime";
			this.dtpEndTime.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.dtpEndTime.Size = new System.Drawing.Size(307, 24);
			this.dtpEndTime.TabIndex = 5;
			this.dtpEndTime.UsePersianFormat = true;
			// 
			// dtpStartTime
			// 
			this.dtpStartTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.dtpStartTime.CustomFormat = "dd / MM / yyyy ساعت mm:HH";
			this.dtpStartTime.Enabled = false;
			this.dtpStartTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.dtpStartTime.Format = Atf.UI.DateTimeSelectorFormat.Custom;
			this.dtpStartTime.Location = new System.Drawing.Point(847, 629);
			this.dtpStartTime.Name = "dtpStartTime";
			this.dtpStartTime.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.dtpStartTime.Size = new System.Drawing.Size(307, 24);
			this.dtpStartTime.TabIndex = 1;
			this.dtpStartTime.UsePersianFormat = true;
			// 
			// btnNew
			// 
			this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNew.Location = new System.Drawing.Point(232, 629);
			this.btnNew.Margin = new System.Windows.Forms.Padding(2);
			this.btnNew.Name = "btnNew";
			this.btnNew.Size = new System.Drawing.Size(200, 55);
			this.btnNew.TabIndex = 8;
			this.btnNew.Text = "ثبت کارت جدید";
			this.btnNew.UseVisualStyleBackColor = true;
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			// 
			// FormLostList
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(1264, 691);
			this.Controls.Add(this.dgvReport);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.lblRecordsCost);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.bsbSaveReport);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblRecordsCount);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cbToDate);
			this.Controls.Add(this.cbFromDate);
			this.Controls.Add(this.dtpEndTime);
			this.Controls.Add(this.dtpStartTime);
			this.Controls.Add(this.btnNew);
			this.Controls.Add(this.btnSearch);
			this.Controls.Add(this.btnClose);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MinimumSize = new System.Drawing.Size(1280, 730);
			this.Name = "FormLostList";
			this.Padding = new System.Windows.Forms.Padding(20, 60, 20, 5);
			this.RightToLeftLayout = true;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "فهرست کارتهای گم شده";
			this.TextAlign = System.Windows.Forms.VisualStyles.HorizontalAlign.Center;
			this.Theme = MetroFramework.MetroThemeStyle.Dark;
			this.Load += new System.EventHandler(this.FormLostList_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
			this.cmsSaveReport.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.CheckBox cbToDate;
		private System.Windows.Forms.CheckBox cbFromDate;
		private Atf.UI.DateTimeSelector dtpEndTime;
		private Atf.UI.DateTimeSelector dtpStartTime;
		private System.Windows.Forms.DataGridView dgvReport;
		private ComponentOwl.BetterSplitButton.BetterSplitButton bsbSaveReport;
		private System.Windows.Forms.PrintPreviewDialog ppdlgReport;
		private System.Windows.Forms.ContextMenuStrip cmsSaveReport;
		private System.Windows.Forms.ToolStripMenuItem tsmiSaveExcel;
		private System.Windows.Forms.ToolStripMenuItem tsmiSavePDF;
		private System.Windows.Forms.ToolStripMenuItem tsmiSaveXML;
		private System.Windows.Forms.ToolStripMenuItem tsmiSaveCSV;
		private System.Windows.Forms.ToolStripMenuItem tsmiSaveHTML;
		private System.Windows.Forms.SaveFileDialog sfdSaveReport;
		private System.Drawing.Printing.PrintDocument pdocReport;
		private System.Windows.Forms.PrintDialog pdlgReport;
		private System.Windows.Forms.Label lblRecordsCost;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblRecordsCount;
		private System.Windows.Forms.Label label3;
		private GridViewCheckBoxColumn ReportSelect;
		private System.Windows.Forms.DataGridViewTextBoxColumn ReportRow;
		private System.Windows.Forms.DataGridViewTextBoxColumn ReportCode;
		private System.Windows.Forms.DataGridViewTextBoxColumn ReportDriverName;
		private System.Windows.Forms.DataGridViewTextBoxColumn ReportDriverNID;
		private System.Windows.Forms.DataGridViewTextBoxColumn ReportDriverDID;
		private System.Windows.Forms.DataGridViewTextBoxColumn ReportCarLicense;
		private System.Windows.Forms.DataGridViewTextBoxColumn ReportCardType;
		private System.Windows.Forms.DataGridViewTextBoxColumn ReportFine;
		private System.Windows.Forms.DataGridViewTextBoxColumn ReportLostDate;
		private System.Windows.Forms.DataGridViewTextBoxColumn ReportDescription;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label4;
		private GridViewCheckBoxColumn gridViewCheckBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
		private System.Windows.Forms.Button btnNew;
	}
}