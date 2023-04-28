using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Printing;
//using Microsoft.Reporting.WinForms;
using System.Drawing.Imaging;
using Microsoft.Reporting.WebForms;

namespace PrinterLib
{
	public class Printer
	{
		private int _mCurrentPageIndex;
		private IList<Stream> _mStreams;

		public string LicensePlate { get; set; }

		public string Name { get; set; }

		public string EnterTime { get; set; }

		public string ExitTime { get; set; }
		public string Type { get; set; }

		public string Duration { get; set; }

		public string Cost { get; set; }

		public string Lyric { get; set; }

		public string Support { get; set; }

		// Routine to provide to the report renderer, in order to
		//    save an image for each page of the report.
		private Stream CreateStream(string name,
		  string fileNameExtension, Encoding encoding,
		  string mimeType, bool willSeek)
		{
			Stream stream = new MemoryStream();
			_mStreams.Add(stream);
			return stream;
		}

		// Export the given report as an EMF (Enhanced Metafile) file.
		private void Export(LocalReport report)
		{
			string deviceInfo =
			  @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>85mm</PageWidth>
                <PageHeight>110mm</PageHeight>
                <MarginTop>1mm</MarginTop>
                <MarginLeft>1mm</MarginLeft>
                <MarginRight>1mm</MarginRight>
                <MarginBottom>1mm</MarginBottom>
            </DeviceInfo>";
			_mStreams = new List<Stream>();
			try
			{
				Warning[] warnings;
				report.Render("Image", deviceInfo, CreateStream,
				   out warnings);
			}
			catch (Exception ex)
			{
				MessageBox.Show("خطا در پرینت قبض" + ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
			foreach (Stream stream in _mStreams)
				stream.Position = 0;
		}
		// Handler for PrintPageEvents
		private void PrintPage(object sender, PrintPageEventArgs ev)
		{
			Metafile pageImage = new
			   Metafile(_mStreams[_mCurrentPageIndex]);

			// Adjust rectangular area with printer margins.
			Rectangle adjustedRect = new Rectangle(
				ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
				ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
				ev.PageBounds.Width,
				ev.PageBounds.Height);

			// Draw a white background for the report
			ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

			// Draw the report content
			ev.Graphics.DrawImage(pageImage, adjustedRect);

			// Prepare for the next page. Make sure we haven't hit the end.
			_mCurrentPageIndex++;
			ev.HasMorePages = (_mCurrentPageIndex < _mStreams.Count);
		}

		private void Print()
		{
			if (_mStreams == null || _mStreams.Count == 0)
				return;
			PrintDocument printDoc = new PrintDocument();
			PaperSize customSize = new PaperSize("First custom size", 300, 400);
			Margins customMargin = new Margins(0, 0, 0, 0);

			printDoc.DefaultPageSettings.PaperSize = customSize;
			printDoc.DefaultPageSettings.Margins = customMargin;

			if (!printDoc.PrinterSettings.IsValid)
			{
				throw new Exception("Error: cannot find the default printer.");
			}
			else
			{
				printDoc.PrintPage += PrintPage;
				_mCurrentPageIndex = 0;
				printDoc.Print();
			}
		}
		// Create a local report for Report.rdlc, load the data,
		//    export the report to an .emf file, and print it.
		public void Run()
		{
			LocalReport report = new LocalReport { ReportPath = @"Receipt.rdlc" };
			ReportParameterCollection rp = new ReportParameterCollection
			{
				new ReportParameter("ReportLicensePlate", LicensePlate),
				new ReportParameter("ReportParkingName", Name),
				new ReportParameter("ReportEnterTime", EnterTime),
				new ReportParameter("ReportExitTime", ExitTime),
				new ReportParameter("ReportTypeName", Type),
				new ReportParameter("ReportDuration", Duration),
				new ReportParameter("ReportCost", Cost + " تومان"),
				new ReportParameter("ReportLyric", Lyric),
				new ReportParameter("ReportSupport", Support)
			};
			ReportParameterCollection reportParameters = rp;
			try
			{
				report.SetParameters(reportParameters);
			}
			catch
			{
				MessageBox.Show("خطا در پرینت قبض", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
			}
			Export(report);
			Print();
		}

		public void Dispose()
		{
			if (_mStreams != null)
			{
				foreach (Stream stream in _mStreams)
					stream.Close();
				_mStreams = null;
			}
		}
	}
}
