using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ToolsLib
{
	public enum FileFormatNames
	{
		WORD,
		EXCEL,
		PDF,
		XML,
		CSV,
		HTML
	}
	public class StringConvert
	{
		public static string ByteArrayToString(byte[] ba)
		{
			StringBuilder hex = new StringBuilder(ba.Length * 2);
			foreach (byte b in ba)
			{
				hex.AppendFormat("{0:x2}", b);
			}

			return hex.ToString();
		}

		public static byte[] StringToByteArray(string hexString)
		{
			int length = hexString.Length;
			int upperBound = length / 2;
			if (length % 2 == 0)
			{
				upperBound -= 1;
			}
			else
			{
				hexString = "0" + hexString;
			}
			byte[] bytes = new byte[upperBound + 1];
			for (int i = 0; i <= upperBound; i++)
			{
				bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
			}

			return bytes;
		}

		public static byte[] ImageToByteArray(Image imageIn)
		{
			ImageConverter imgCon = new ImageConverter();
			return (byte[])imgCon.ConvertTo(imageIn, typeof(byte[]));

			/*MemoryStream ms = new MemoryStream();
			imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
			return ms.ToArray();*/

			/*using(MemoryStream mStream = new MemoryStream())
			{
				imageIn.Save(mStream, imageIn.RawFormat);
				return mStream.ToArray();
			}*/
		}

		public static Image ByteArrayToImage(byte[] byteArrayIn)
		{
			/*using(MemoryStream mStream = new MemoryStream(byteArrayIn))
			{
				return Image.FromStream(mStream);
			}*/
			MemoryStream ms = new MemoryStream(byteArrayIn);
			Image returnImage = Image.FromStream(ms);
			return returnImage;
		}

		public static string ToTraceString<T>(IQueryable<T> query)
		{
			var internalQueryField = query.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(f => f.Name.Equals("_internalQuery"));

			if (internalQueryField != null)
			{
				var internalQuery = internalQueryField.GetValue(query);

				var objectQueryField = internalQuery.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(f => f.Name.Equals("_objectQuery"));

				if (objectQueryField != null)
				{
					var objectQuery = objectQueryField.GetValue(internalQuery) as System.Data.Entity.Core.Objects.ObjectQuery<T>;

					return ToTraceStringWithParameters(objectQuery);
				}
			}
			return null;
		}

		public static string ToTraceStringWithParameters<T>(System.Data.Entity.Core.Objects.ObjectQuery<T> query)
		{
			string traceString = query.ToTraceString() + Environment.NewLine;

			foreach (var parameter in query.Parameters)
			{
				traceString += parameter.Name + " [" + parameter.ParameterType.FullName + "] = " + parameter.Value + "\n";
			}

			return traceString;
		}

		public static Image DrawText(String text, Font font, Color textColor, Color backColor)
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

		public static string ConvertAsciiToUnicode(string AsciiString)
		{
			// Create two different encodings.
			Encoding ascii = Encoding.ASCII;
			Encoding unicode = Encoding.Unicode;

			// Convert the string into a byte[].
			byte[] asciiBytes = ascii.GetBytes(AsciiString);

			// Perform the conversion from one encoding to the other.
			byte[] unicodeBytes = Encoding.Convert(ascii, unicode, asciiBytes);

			// Convert the new byte[] into a char[] and then into a string.
			// This is a slightly different approach to converting to illustrate
			// the use of GetCharCount/GetChars.
			char[] unicodeChars = new char[unicode.GetCharCount(unicodeBytes, 0, unicodeBytes.Length)];
			unicode.GetChars(unicodeBytes, 0, unicodeBytes.Length, unicodeChars, 0);
			string unicodeString = new string(unicodeChars);

			return unicodeString;
		}

		public static string ConvertUnicodeToAscii(string UnicodeString)
		{
			// Create two different encodings.
			Encoding ascii = Encoding.ASCII;
			Encoding unicode = Encoding.Unicode;

			// Convert the string into a byte[].
			byte[] unicodeBytes = unicode.GetBytes(UnicodeString);

			// Perform the conversion from one encoding to the other.
			byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);

			// Convert the new byte[] into a char[] and then into a string.
			// This is a slightly different approach to converting to illustrate
			// the use of GetCharCount/GetChars.
			char[] asciiChars = new char[ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
			ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
			string asciiString = new string(asciiChars);

			return asciiString;
		}

		public static string AsciiToUnicodNumber(string Input)
		{
			string tmp = "";
			foreach (char c in Input)
			{
				switch (c)
				{
					case '0':
						{
							tmp += "۰";
							break;
						}
					case '1':
						{
							tmp += "۱";
							break;
						}
					case '2':
						{
							tmp += "۲";
							break;
						}
					case '3':
						{
							tmp += "۳";
							break;
						}
					case '4':
						{
							tmp += "۴";
							break;
						}
					case '5':
						{
							tmp += "۵";
							break;
						}
					case '6':
						{
							tmp += "۶";
							break;
						}
					case '7':
						{
							tmp += "۷";
							break;
						}
					case '8':
						{
							tmp += "۸";
							break;
						}
					case '9':
						{
							tmp += "۹";
							break;
						}
					default:
						{
							tmp += c;
							break;
						}
				}
			}
			return tmp;
		}

		public static string UnicodToAsciiNumber(string Input)
		{
			string tmp = "";
			foreach (char c in Input)
			{
				switch (c)
				{
					case '۰':
						{
							tmp += "0";
							break;
						}
					case '۱':
						{
							tmp += "1";
							break;
						}
					case '۲':
						{
							tmp += "2";
							break;
						}
					case '۳':
						{
							tmp += "3";
							break;
						}
					case '۴':
						{
							tmp += "4";
							break;
						}
					case '۵':
						{
							tmp += "5";
							break;
						}
					case '۶':
						{
							tmp += "6";
							break;
						}
					case '۷':
						{
							tmp += "7";
							break;
						}
					case '۸':
						{
							tmp += "8";
							break;
						}
					case '۹':
						{
							tmp += "9";
							break;
						}
					default:
						{
							tmp += c;
							break;
						}
				}
			}
			return tmp;
		}

		public static string UnicodToAsciiNumberString(string Input)
		{
			string tmp = "";
			switch (Input)
			{
				case "۰":
					{
						tmp += "0";
						break;
					}
				case "۱":
					{
						tmp += "1";
						break;
					}
				case "۲":
					{
						tmp += "2";
						break;
					}
				case "۳":
					{
						tmp += "3";
						break;
					}
				case "۴":
					{
						tmp += "4";
						break;
					}
				case "۵":
					{
						tmp += "5";
						break;
					}
				case "۶":
					{
						tmp += "6";
						break;
					}
				case "۷":
					{
						tmp += "7";
						break;
					}
				case "۸":
					{
						tmp += "8";
						break;
					}
				case "۹":
					{
						tmp += "9";
						break;
					}
				default:
					{
						tmp += Input;
						break;
					}
			}
			return tmp;
		}

		public static Stream GenerateStreamFromString(string s)
		{
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(s);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}
	}
}
