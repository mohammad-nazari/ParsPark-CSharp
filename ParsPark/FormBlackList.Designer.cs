namespace ParsPark
{
	partial class FormBlackList
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBlackList));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			this.dgvCars = new System.Windows.Forms.DataGridView();
			this.BlackListRow = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.BlackListLicense = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.BlackListDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.BlackListEdit = new System.Windows.Forms.DataGridViewLinkColumn();
			this.BlackListDelete = new System.Windows.Forms.DataGridViewLinkColumn();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnAddCar = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgvCars)).BeginInit();
			this.SuspendLayout();
			// 
			// dgvCars
			// 
			this.dgvCars.AllowUserToAddRows = false;
			this.dgvCars.AllowUserToDeleteRows = false;
			this.dgvCars.AllowUserToResizeColumns = false;
			this.dgvCars.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvCars.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			resources.ApplyResources(this.dgvCars, "dgvCars");
			this.dgvCars.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgvCars.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvCars.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.dgvCars.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCars.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BlackListRow,
            this.BlackListLicense,
            this.BlackListDescription,
            this.BlackListEdit,
            this.BlackListDelete});
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvCars.DefaultCellStyle = dataGridViewCellStyle3;
			this.dgvCars.MultiSelect = false;
			this.dgvCars.Name = "dgvCars";
			this.dgvCars.ReadOnly = true;
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
			dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvCars.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.dgvCars.RowHeadersVisible = false;
			this.dgvCars.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.dgvCars.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvCars.RowTemplate.ReadOnly = true;
			this.dgvCars.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvCars.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCars_CellContentClick);
			// 
			// BlackListRow
			// 
			this.BlackListRow.FillWeight = 10F;
			resources.ApplyResources(this.BlackListRow, "BlackListRow");
			this.BlackListRow.Name = "BlackListRow";
			this.BlackListRow.ReadOnly = true;
			// 
			// BlackListLicense
			// 
			this.BlackListLicense.FillWeight = 20F;
			resources.ApplyResources(this.BlackListLicense, "BlackListLicense");
			this.BlackListLicense.Name = "BlackListLicense";
			this.BlackListLicense.ReadOnly = true;
			// 
			// BlackListDescription
			// 
			this.BlackListDescription.FillWeight = 50F;
			resources.ApplyResources(this.BlackListDescription, "BlackListDescription");
			this.BlackListDescription.Name = "BlackListDescription";
			this.BlackListDescription.ReadOnly = true;
			// 
			// BlackListEdit
			// 
			this.BlackListEdit.FillWeight = 10F;
			resources.ApplyResources(this.BlackListEdit, "BlackListEdit");
			this.BlackListEdit.Name = "BlackListEdit";
			this.BlackListEdit.ReadOnly = true;
			this.BlackListEdit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.BlackListEdit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// BlackListDelete
			// 
			this.BlackListDelete.FillWeight = 10F;
			resources.ApplyResources(this.BlackListDelete, "BlackListDelete");
			this.BlackListDelete.Name = "BlackListDelete";
			this.BlackListDelete.ReadOnly = true;
			this.BlackListDelete.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.BlackListDelete.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// btnClose
			// 
			resources.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Name = "btnClose";
			this.btnClose.UseVisualStyleBackColor = true;
			// 
			// btnAddCar
			// 
			resources.ApplyResources(this.btnAddCar, "btnAddCar");
			this.btnAddCar.Name = "btnAddCar";
			this.btnAddCar.UseVisualStyleBackColor = true;
			this.btnAddCar.Click += new System.EventHandler(this.btnAddCar_Click);
			// 
			// FormBlackList
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			resources.ApplyResources(this, "$this");
			this.CancelButton = this.btnClose;
			this.Controls.Add(this.dgvCars);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnAddCar);
			this.KeyPreview = true;
			this.Name = "FormBlackList";
			this.TextAlign = System.Windows.Forms.VisualStyles.HorizontalAlign.Center;
			this.Theme = MetroFramework.MetroThemeStyle.Dark;
			this.Load += new System.EventHandler(this.FormBlackList_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvCars)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dgvCars;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnAddCar;
		private System.Windows.Forms.DataGridViewTextBoxColumn BlackListRow;
		private System.Windows.Forms.DataGridViewTextBoxColumn BlackListLicense;
		private System.Windows.Forms.DataGridViewTextBoxColumn BlackListDescription;
		private System.Windows.Forms.DataGridViewLinkColumn BlackListEdit;
		private System.Windows.Forms.DataGridViewLinkColumn BlackListDelete;
	}
}