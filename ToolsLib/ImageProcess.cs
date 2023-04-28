using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

namespace ToolsLib
{
	public class ImageProcess
	{
		public static Image ByteToImage(byte[] blob)
		{
			MemoryStream mStream = new MemoryStream();
			byte[] pData = blob;
			mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
			Bitmap bm = new Bitmap(mStream, false);
			mStream.Dispose();
			return bm;
		}

		public static Image ByteArrayToImage(Byte[] byteArrayIn)
		{
			MemoryStream mStream = new MemoryStream(byteArrayIn, 0, byteArrayIn.Length) { Position = 0 };
			// this is important
			return Image.FromStream(mStream);
		}

		public static Byte[] ImageToByteArray(Image imageIn)
		{
			ImageConverter imgCon = new ImageConverter();
			return (byte[])imgCon.ConvertTo(imageIn, typeof(byte[]));
		}
		public static string ImageToBase64(Image image, ImageFormat format)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				// Convert Image to byte[]
				image.Save(ms, format);
				byte[] imageBytes = ms.ToArray();

				// Convert byte[] to Base64 String
				string base64String = Convert.ToBase64String(imageBytes);
				return base64String;
			}
		}

		public static Image Base64ToImage(string base64String)
		{
			// Convert Base64 String to byte[]
			byte[] imageBytes = Convert.FromBase64String(base64String);
			MemoryStream ms = new MemoryStream(imageBytes, 0,
			  imageBytes.Length);

			// Convert byte[] to Image
			ms.Write(imageBytes, 0, imageBytes.Length);
			Image image = Image.FromStream(ms, true);
			return image;
		}

		private static Bitmap DrawText(string TextValue, Font FontName, Color FColor, Color BgColor, int WidthLen, int HeightLen)
		{
			// first, create a dummy bitmap just to get a graphics object
			Bitmap img = new Bitmap(1, 1);
			Graphics drawing = Graphics.FromImage(img);

			//free up the dummy image and old graphics object
			img.Dispose();
			drawing.Dispose();

			//create a new image of the right size
			//img = new Bitmap((int)textSize.Width, (int)textSize.Height);
			img = new Bitmap(WidthLen, HeightLen);

			drawing = Graphics.FromImage(img);

			//paint the background
			drawing.Clear(BgColor);

			//create a brush for the text
			Brush textBrush = new SolidBrush(FColor);

			drawing.DrawString(TextValue, FontName, textBrush, -5, -15, StringFormat.GenericDefault);

			drawing.Save();

			textBrush.Dispose();
			drawing.Dispose();

			return img;
		}

		private static Bitmap GrayScale(Bitmap Bmp)
		{
			for (int y = 0; y < Bmp.Height; y++)
				for (int x = 0; x < Bmp.Width; x++)
				{
					var c = Bmp.GetPixel(x, y);
					int a = c.A;
					int r = c.R;
					int g = c.G;
					int b = c.B;
					int avg = (r + g + b) / 3;
					avg = avg < 128 ? 0 : 255;     // Converting gray pixels to either pure black or pure white
					Bmp.SetPixel(x, y, Color.FromArgb(a, avg, avg, avg));
				}
			return Bmp;
		}

		public static string GetImageHexString(string TextValue, Font FontName, Color FColor, Color BgColor, int Width, int Height)
		{
			// Convert input text to image
			PictureBox image = new PictureBox
			{
				SizeMode = PictureBoxSizeMode.StretchImage,
				BackColor = Color.White,
				Name = "picbVal",
				Size = new Size(Width, Height),
				Image = GrayScale(DrawText(TextValue, FontName, Color.Black, Color.White, Width, Height))
			};

			Bitmap bitmap = new Bitmap(Width, Height);
			image.DrawToBitmap(bitmap, image.ClientRectangle);

			// get image text height and width
			int height = bitmap.Height;
			int width = bitmap.Width;

			// initialize led status stream
			List<string> imageDataList = new List<string>();

			for (int i = 0; i < (height > Height ? Height : height); i += 2)
			{
				string tempData = string.Empty;
				for (int j = 0; j < (width > Width ? Width : width); j += 2)
				{
					var color = bitmap.GetPixel(j, i);
					var color2 = bitmap.GetPixel(j + 1, i);
					var color3 = bitmap.GetPixel(j, i + 1);
					var color4 = bitmap.GetPixel(j + 1, i + 1);

					int ii = (color.Name == @"ffffffff" ? 1 : 0);
					ii = (color2.Name == @"ffffffff" ? ii + 1 : ii);
					ii = (color3.Name == @"ffffffff" ? ii + 1 : ii);
					ii = (color4.Name == @"ffffffff" ? ii + 1 : ii);

					tempData += (ii > 2 ? "1" : "0");
				}
				imageDataList.Add(BinaryStringToHexString(tempData, 8));
			}

			imageDataList.Reverse();

			return string.Join(",", imageDataList);
		}

		public static string BinaryStringToHexString(string binary, int Len)
		{
			StringBuilder result = new StringBuilder(binary.Length / Len + 1);

			int mod4Len = binary.Length % Len;
			if (mod4Len != 0)
			{
				// pad to length multiple of Len
				binary = binary.PadLeft(((binary.Length / Len) + 1) * Len, '0');
			}

			for (int i = 0; i < binary.Length; i += Len)
			{
				string eightBits = binary.Substring(i, Len);
				result.AppendFormat("{0:X2}", Convert.ToByte(eightBits, 2));
			}

			return result.ToString();
		}
	}
}
