using System.Windows.Forms;

namespace ToolsLib
{
	class AddRow
	{
		public void AddRowData(TableLayoutPanel T ,int ColumnIndex, string InputString)
		{
			T.Controls.Add(new Label()
			{
				Text = "OK"
			}, ColumnIndex, T.RowCount - 1);
		}
	}
}
