using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SerialPortLib;
using ToolsLib;
using MetroFramework;
using MetroFramework.Forms;

namespace ParsPark
{
	public partial class FormDrivers : MetroForm
	{
		private readonly CityPay _serialComPortCityPay = new CityPay();
		public FormDrivers()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			{
				_serialComPortCityPay.BaudRate = 38400;
				_serialComPortCityPay.PortName = "COM8";
				if (_serialComPortCityPay.InitializeComport("کارت شهروندی") == false)
				{
					MessageBox.Show(_serialComPortCityPay.Errors, @"گزارش", MessageBoxButtons.OK, MessageBoxIcon.Error,
						MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
				}
				else
				{
					_serialComPortCityPay.SendPriceToCityPay(1200);
				}
			}

			pictureBox1.Image = DrawText(textBox1.Text, textBox1.Font, Color.Black, Color.Green);
		}

		private Image DrawText(String text, Font font, Color textColor, Color backColor)
		{
			//first, create a dummy bitmap just to get a graphics object
			Image img = new Bitmap(1, 1);
			Graphics drawing = Graphics.FromImage(img);

			//measure the string to see how big the image needs to be
			SizeF textSize = drawing.MeasureString(text, font);

			//free up the dummy image and old graphics object
			img.Dispose();
			drawing.Dispose();

			//create a new image of the right size
			img = new Bitmap((int)textSize.Width, (int)textSize.Height);

			drawing = Graphics.FromImage(img);

			//paint the background
			drawing.Clear(backColor);

			//create a brush for the text
			Brush textBrush = new SolidBrush(textColor);

			drawing.DrawString(text, font, textBrush, 0, 0);

			drawing.Save();

			textBrush.Dispose();
			drawing.Dispose();

			return img;

		}

		private void FormDrivers_Load(object sender, EventArgs e)
		{

		}
	}
}
