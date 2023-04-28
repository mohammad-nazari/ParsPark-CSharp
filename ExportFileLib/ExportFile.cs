using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Windows.Forms;
using System.IO;
using OpenExcel.OfficeOpenXml;
using Novacode;
using System.Data;
using Font = iTextSharp.text.Font;

namespace ExportFileLib
{
	public class ExportFile
	{
		public DataGridView DgvObject { get; set; } = new DataGridView();
		public string FileName { get; set; }
		public string ParkingName { get; set; }

		public string DateTimeInfo { get; set; }

		public string NumberOfRecords { get; set; }
		public string TotalCost { get; set; }

		public void ExportToPdf()
		{

			FontFactory.RegisterDirectories();
			Font font = FontFactory.GetFont("B Titr", BaseFont.IDENTITY_H, 12, Font.BOLD, BaseColor.BLACK);

			//Creating iTextSharp Table from the DataTable data
			PdfPTable pdfTable1 = new PdfPTable(2)
			{
				DefaultCell = {
					PaddingBottom = 3 ,
					BorderWidth = 0
				},
				WidthPercentage = 100,
				RunDirection = PdfWriter.RUN_DIRECTION_RTL,
			};

			Chunk beginning = new Chunk(ParkingName, font);
			Phrase p1 = new Phrase(beginning);
			Chunk startendDateTime = new Chunk(DateTimeInfo, font);
			Phrase p2 = new Phrase(startendDateTime);

			PdfPCell cell1 = new PdfPCell(p1)
			{
				RunDirection = PdfWriter.RUN_DIRECTION_RTL,
				Padding = 10,
				BorderWidth = 0
			};
			pdfTable1.AddCell(cell1);

			PdfPCell cell2 = new PdfPCell(p2)
			{
				RunDirection = PdfWriter.RUN_DIRECTION_RTL,
				Padding = 10,
				BorderWidth = 0
			};
			pdfTable1.AddCell(cell2);

			font = FontFactory.GetFont("B Nazanin", BaseFont.IDENTITY_H, 12, Font.NORMAL, BaseColor.WHITE);
			//Creating iTextSharp Table from the DataTable data
			PdfPTable pdfTable = new PdfPTable(DgvObject.ColumnCount);
			pdfTable.DefaultCell.Padding = 3;
			pdfTable.WidthPercentage = 100;
			pdfTable.DefaultCell.BorderWidth = 1;
			pdfTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;

			for (int j = 0; j < DgvObject.Columns.Count; j++)
			{
				PdfPCell cell = new PdfPCell(new Phrase(DgvObject.Columns[j].HeaderText, font))
				{
					BackgroundColor = BaseColor.BLACK,
					RunDirection = PdfWriter.RUN_DIRECTION_RTL,
					PaddingBottom = 7
				};
				pdfTable.AddCell(cell);
			}

			font = FontFactory.GetFont("B Nazanin", BaseFont.IDENTITY_H, 12, Font.NORMAL, BaseColor.BLACK);
			//Adding DataRow
			for (int i = 0; i < DgvObject.Rows.Count - 1; i++)
			{
				for (int j = 0; j < DgvObject.Columns.Count; j++)
				{
					PdfPCell cell =
						new PdfPCell(new Phrase(DgvObject.Rows[i].Cells[j].Value != null ? DgvObject.Rows[i].Cells[j].Value.ToString() : "", font))
						{
							BackgroundColor = (i % 2) == 1 ? BaseColor.WHITE : BaseColor.LIGHT_GRAY,
							RunDirection = PdfWriter.RUN_DIRECTION_RTL,
							PaddingBottom = 7
						};
					pdfTable.AddCell(cell);
				}
			}

			font = FontFactory.GetFont("B Titr", BaseFont.IDENTITY_H, 12, Font.BOLD, BaseColor.BLACK);
			//Creating iTextSharp Table from the DataTable data
			PdfPTable pdfTable2 = new PdfPTable(2);
			pdfTable2.DefaultCell.Padding = 5;
			pdfTable2.WidthPercentage = 100;
			pdfTable2.DefaultCell.BorderWidth = 0;
			pdfTable2.RunDirection = PdfWriter.RUN_DIRECTION_RTL;

			beginning = new Chunk(NumberOfRecords, font);
			p1 = new Phrase(beginning);
			startendDateTime = new Chunk(TotalCost, font);
			p2 = new Phrase(startendDateTime);

			cell1 = new PdfPCell(p1)
			{
				RunDirection = PdfWriter.RUN_DIRECTION_RTL,
				Padding = 10,
				BorderWidth = 0
			};
			pdfTable2.AddCell(cell1);

			cell2 = new PdfPCell(p2)
			{
				RunDirection = PdfWriter.RUN_DIRECTION_RTL,
				Padding = 10,
				BorderWidth = 0
			};
			pdfTable2.AddCell(cell2);

			//Exporting to PDF
			using (FileStream stream = new FileStream(FileName, FileMode.Create))
			{
				Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
				PdfWriter.GetInstance(pdfDoc, stream);
				pdfDoc.Open();
				pdfDoc.Add(pdfTable1);
				pdfDoc.Add(pdfTable);
				pdfDoc.Add(pdfTable2);
				pdfDoc.Close();
				stream.Close();
			}
		}

		public void ExportToWord()
		{
			using (DocX document = DocX.Create(FileName))
			{
				Novacode.Paragraph p = document.InsertParagraph(ParkingName);
				p.Direction = Direction.RightToLeft;
				p = document.InsertParagraph(DateTimeInfo);
				p.Direction = Direction.RightToLeft;

				// Add a Table to this document.
				Table t = document.AddTable(DgvObject.Rows.Count, DgvObject.ColumnCount);
				// Specify some properties for this Table.
				t.Alignment = Alignment.center;
				t.SetDirection(Direction.RightToLeft);
				t.Design = TableDesign.MediumGrid1Accent2;

				for (int j = 0; j < DgvObject.Columns.Count; j++)
				{
					t.Rows[0].Cells[j].Paragraphs[0].Append(DgvObject.Columns[j].HeaderText);
					t.Rows[0].Cells[j].TextDirection = TextDirection.right;
				}

				for (int i = 1; i < DgvObject.Rows.Count; i++)
				{
					for (int j = 0; j < DgvObject.Columns.Count; j++)
					{
						t.Rows[i].Cells[j].Paragraphs[0].Append(DgvObject.Rows[i - 1].Cells[j].Value != null ? DgvObject.Rows[i - 1].Cells[j].Value.ToString() : "");
						t.Rows[i].Cells[j].TextDirection = TextDirection.right;
					}
				}
				document.InsertTable(t);

				p = document.InsertParagraph(NumberOfRecords);
				p.Direction = Direction.RightToLeft;
				p = document.InsertParagraph(TotalCost);
				p.Direction = Direction.RightToLeft;

				document.Save();
			}// Release this document from memory. 
		}

		public void ExportToExcel()
		{
			if (DgvObject.Rows.Count > 0)
			{
				if (!string.IsNullOrEmpty(FileName))
				{
					using (ExcelDocument workBook = ExcelDocument.CreateWorkbook(FileName))
					{
						ExcelWorksheet workSheet = workBook.Workbook.Worksheets.Add("report");
						workBook.EnsureStylesDefined();

						workSheet.Cells[1, 1].Value = ParkingName;
						workSheet.Cells[1, 2].Value = DateTimeInfo;

						for (uint j = 0; j < DgvObject.Columns.Count; j++)
						{
							workSheet.Cells[3, j + 1].Value = DgvObject.Columns[(int)j].HeaderText;
						}

						for (uint i = 0; i < DgvObject.Rows.Count - 1; i++)
						{
							for (uint j = 0; j < DgvObject.Columns.Count; j++)
							{
								workSheet.Cells[i + 4, j + 1].Value = DgvObject.Rows[(int)i].Cells[(int)j].Value != null ? DgvObject.Rows[(int)i].Cells[(int)j].Value.ToString() : "";
							}
						}

						workSheet.Cells[(uint)DgvObject.Rows.Count + 4, 1].Value = NumberOfRecords;
						workSheet.Cells[(uint)DgvObject.Rows.Count + 4, 2].Value = TotalCost;
					}
				}
			}
		}

		public void ExportToCsv()
		{
			StreamWriter sw = new StreamWriter(FileName);
			for (int j = 0; j < DgvObject.Columns.Count; j++)
			{
				sw.Write("\"" + DgvObject.Columns[j].HeaderText + "\"");
				if (j != DgvObject.Columns.Count - 1)
				{
					sw.Write(",");
				}
			}

			sw.Write(sw.NewLine);

			for (int i = 0; i < DgvObject.Rows.Count - 1; i++)
			{
				for (int j = 0; j < DgvObject.Columns.Count; j++)
				{
					sw.Write(DgvObject.Rows[i].Cells[j].Value != null ? "\"" + DgvObject.Rows[i].Cells[j].Value + "\"" : "\"\"");
					if (j != DgvObject.Columns.Count - 1)
					{
						sw.Write(",");
					}
				}

				sw.Write(sw.NewLine);
			}
			sw.Flush();

			sw.Close();
		}

		public void ExportToXml()
		{
			DataTable dt = new DataTable();

			for (int j = 0; j < DgvObject.Columns.Count; j++)
			{
				dt.Columns.Add(DgvObject.Columns[j].HeaderText);
			}

			for (int i = 0; i < DgvObject.Rows.Count - 1; i++)
			{
				DataRow dRow = dt.NewRow();
				for (int j = 0; j < DgvObject.Columns.Count; j++)
				{
					dRow[j] = DgvObject.Rows[i].Cells[j].Value != null ? DgvObject.Rows[i].Cells[j].Value.ToString() : "";
				}
				dt.Rows.Add(dRow);
			}

			dt.TableName = "Report";
			dt.WriteXml(FileName, true);
		}

		public void ExportToHtml()
		{
			StringBuilder strB = new StringBuilder();
			//create html & table
			strB.AppendLine("<html><body><center>");

			strB.AppendLine(ParkingName);
			strB.AppendLine(DateTimeInfo);

			strB.AppendLine("<html><body><center><table border='1' cellpadding='0' cellspacing='0' style='direction: rtl;'>");
			strB.AppendLine("<tr>");
			//Create table header
			for (int j = 0; j < DgvObject.Columns.Count; j++)
			{
				strB.AppendLine("<td align='center' valign='middle'>" + DgvObject.Columns[j].HeaderText + "</td>");
			}
			//create table body
			strB.AppendLine("<tr>");
			for (int i = 0; i < DgvObject.Rows.Count; i++)
			{
				for (int j = 0; j < DgvObject.Columns.Count; j++)
				{
					var newString = DgvObject.Rows[i].Cells[j].Value != null ? DgvObject.Rows[i].Cells[j].Value.ToString() : "";
					strB.AppendLine("<td align='center' valign='middle'>" + newString + "</td>");
				}
				strB.AppendLine("</tr>");
			}
			//table footer & end of html file
			strB.AppendLine("</table>");

			strB.AppendLine("<div>" + NumberOfRecords + "</div>");
			strB.AppendLine("<div>" + TotalCost + "</div>");

			strB.AppendLine("</center></body></html>");

			File.WriteAllText(FileName, strB.ToString());
		}
	}
}
