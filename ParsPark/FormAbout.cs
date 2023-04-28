using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Forms;

namespace ParsPark
{
	public partial class FormAbout : MetroForm
	{
		public FormAbout()
		{
			InitializeComponent();
		}

		private void FormAbout_Load(object sender, EventArgs e)
		{

		}

		private void llblSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(llblSite.Text);
		}
	}
}
