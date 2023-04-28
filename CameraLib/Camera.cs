using System;
using System.Configuration;
using System.Net;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ToolsLib;

// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local

namespace CameraLib
{
	public struct Rect
	{
		public int Left;
		public int Top;
		public int Right;
		public int Bottom;
	};

	public class Camera : WebClient
	{
		#region ANPR
		const int WM_USER = 0x0400;
		const int WM_NEW_FRAME = WM_USER + 100;
		const int WM_SCENE_CHANGED = WM_USER + 101;
		const int WM_PLATE_DETECTED = WM_USER + 102;
		const int WM_PLATE_NOT_DETECTED = WM_USER + 103; //when a car is in the field of camera but its plate is not recognized
		const int WM_END_OF_VIDEO = WM_USER + 104; //when video file finished or camera closed

		//Frame width, height, number of channels and step size of the frame (usually = width x number of channels)
		public int FrameW, FrameH, FrameCh, FrameStep;
		public int missed_count = 0;
		//int frame_counter = 0, process_counter = 0, plate_counter = 0;
		//int Grabbing; //indicates whether we are grabbing or not: 0 --> not grabbing, 1 regular grabbing, 2 VLC grabbing 
		//Bitmap frame; //bitmap of playing frames on picture control (in video mode)
		//Bitmap frame_missed; //bitmap of last frame containing car but not detected
		//int nChange;
		//UserRect sel_rect, sel_rect2;
		//System.Drawing.Graphics picg;

		// Open App.Config of executable
		Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

		//KeyValueConfigurationCollection appSettings;

		//[DllImport("user32.dll")]
		//public static extern IntPtr FindWindow(string lpClassName, String lpWindowName);
		//[DllImport("user32.dll")]
		//public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

		[System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
		public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);


		[System.Runtime.InteropServices.DllImport("rtspcameratest.dll")]
		private static extern int GetPicture(string CameraURL, string FileAddress);



		[System.Runtime.InteropServices.DllImport("ANPR.dll")]
		private static extern Int64 LP_GetHID();
		//1
		//تابع زیر در هر برنامه حتما باید یکبار و تنها یکبار فراخوانی شود. این تابع شبکه های عصبی مورد استفاده را بارگذاری می کند
		[System.Runtime.InteropServices.DllImport("ANPR.dll")]
		private static extern unsafe int LP_Initialize(char* security_code, byte log_level = 1);

		//2
		//این تابع مسیر فایل تصویری را دریافت کرده و نتیجه را بر می گرداند: رشته، میزان اطمینان به رشته حاصله و مستطیل پلاک
		[System.Runtime.InteropServices.DllImport("ANPR.dll")]
		private static extern unsafe int LP_Recognize(byte* fn, char* result, float* cnf, Rect* prc);

		//2-2
		//این تابع مانند تابع بالایی است با این تفاوت که اندیس مستطیل مورد علاقه را هم می گیرد.
		[System.Runtime.InteropServices.DllImport("ANPR.dll")]
		private static extern unsafe int LP_RecognizeROI(byte roi_idx, byte* fn, char* result, float* cnf, Rect* prc);


		//3
		//تابع زیر برای بافری است که از دوربین یا فایل گرفته اید و نوعا یک جریان فشرده مثل jpg است.
		[System.Runtime.InteropServices.DllImport("ANPR.dll")]
		private static extern unsafe int LP_Recognize_Stream(IntPtr buffer, int size, char* result, float* cnf, Rect* prc);

		//4
		//تابع زیر برای زمانی است که بایتهای تصویر به صورت فشرده نشده در آرایه ای قرار دارند
		//مثلا اشاره گر ابتدای یک بیت مپ
		//مثال آن در همین برنامه دیده می شود
		[System.Runtime.InteropServices.DllImport("ANPR.dll")]
		private static extern unsafe int LP_Recognize_Buffer(IntPtr bytes, int W, int H, int step, char* result, float* cnf, Rect* prc);

		//5
		//خروجی تابع 2 یک رشته فارسی یونیکد است، اگر خروجی انگلیسی «اسکی» را لازم دارید از این تابع استفاده کنید 
		[System.Runtime.InteropServices.DllImport("ANPR.dll")]
		private static extern unsafe void LP_GetAsciiResults(char* result_fa, byte* result_en);//Get ascii results in English

		//6
		//خروجی تابع 2 یک رشته فارسی یونیکد است، اگر خروجی انگلیسی «یونیکد» را لازم دارید از این تابع استفاده کنید 
		[System.Runtime.InteropServices.DllImport("ANPR.dll")]
		private static extern unsafe void LP_GetEnResults(char* result_fa, char* result_en);//Get unicode results in English

		[System.Runtime.InteropServices.DllImport("ANPR.dll")]
		private static extern unsafe void LP_FindChars(IntPtr bytes, int W, int H, int channels, Rect roi, char* result, float* pcnf);

		//7
		/*این تابع برای تنظیم پارامترها، هنگام کار با ویدیو و دوربین است.
         * nEqualResults: Number of equal string within recent valid results. For example if = 3, final result
         * will be approved only if at least 3 strings are equal.
         * اگر پارامتر اول مثلا 3 باشد، حتما باید رشته پلاک در سه فریم متوالی یکسان باشد تا به عنوان پلاک معتبر تایید شود
         * 
         * nChars: Min number of characters that must be in a string to be considered as a valid result (DEF = 7)
         * این پارامتر، تعداد حداقل حروف رشته پلاک است که به عنوان خروجی برگردانده شود. مثلا اگر 8 بگذارید، 
         * تنها جوابهایی برگردانده می شود که 8 حرف داشته باشند و 3 بار تکرار شده باشند. 
         * 
         * diff_thresh
         * میزان تفاوتی که بین فریم جاری و فریم پس زمینه باید برقرار باشد تا به عنوان فریم حاوی خودرو تلقی شود
         * به عبارتی بیانگر حساسیت برنامه به ورود خودرو است.
         * مقادیر کوچک سبب می شود هر تغییری در صحنه به عنوان ورود خودرو تلقی شده
         * و پردازش شروع شود، مقادیر خیلی بزرگ ممکن است ورود خودرو را تشخیص ندهد. نوعا بین 5 تا 15 مناسب است
         * */
		[System.Runtime.InteropServices.DllImport("ANPR.dll")]
		private static extern int LP_SetParams(byte nEqualResults, byte nChars, byte diff_thresh, byte nSkipFramesAfterSuccess, short resize_thresh);

		//////////////////////////////////////////////////////////////////////////
		//new functions 
		[System.Runtime.InteropServices.DllImport("ANPR.dll")]
		private static extern void LP_AddROI(Rect roi);

		[System.Runtime.InteropServices.DllImport("ANPR.dll")]
		private static extern void LP_ClearROIs();


		[System.Runtime.InteropServices.DllImport("ANPR.dll")]
		private static extern int StartGrabbing(byte stream, byte[] URL, int interval_ms, IntPtr hwnd, int TakeShots);

		[System.Runtime.InteropServices.DllImport("ANPR.dll")]
		private static extern int StopGrabbing(byte stream);

		[System.Runtime.InteropServices.DllImport("ANPR.dll")]
		private static extern int StartGrabbingVLC(byte stream, byte[] URL, int interval_ms, IntPtr hwnd);

		[System.Runtime.InteropServices.DllImport("ANPR.dll")]
		private static extern int StopGrabbingVLC(byte stream);


		[System.Runtime.InteropServices.DllImport("ANPR.dll")]
		private static extern int PauseResume(byte stream, byte pause);

		[System.Runtime.InteropServices.DllImport("ANPR.dll")]
		private static extern unsafe int GetFrameInfo(byte stream, int* W, int* H, int* channels, int* step);

		[System.Runtime.InteropServices.DllImport("ANPR.dll")]
		private static extern IntPtr GetFrame(byte stream);

		//Start Processing of Camera Frames
		[System.Runtime.InteropServices.DllImport("ANPR.dll")]
		private static extern void StartAutoProcess(byte stream);

		//Stop Processing of Camera Frames
		[System.Runtime.InteropServices.DllImport("ANPR.dll")]
		private static extern void StopAutoProcess(byte stream);

		//str must be allocated before
		[System.Runtime.InteropServices.DllImport("ANPR.dll")]
		private static extern unsafe int GetLastResultsW(byte stream, char* str, Rect* rc, float* cnf);

		//Recognize Last Frame Grabbed from camera or video file
		[System.Runtime.InteropServices.DllImport("ANPR.dll")]
		private static extern unsafe int RecognizeActiveFrame(byte stream, char* str, Rect* rc, float* cnf);

		//Calling this function will run a thread that monitors camera frames and if somethings like a Car or a person 
		//appears in the scene it will announce it through WM_SCENE_CHANGED message (0x400 + 101)
		//Threshold is for the difference between frame and background to announce scene change (typically between 5 and 40)
		//larger values will detect large changes like incoming cars, but small values (e.g. 0-9) will detect small variations due to wind or lightening change
		//default value is 10
		[System.Runtime.InteropServices.DllImport("ANPR.dll")]
		private static extern void DetectSceneChange(byte stream, byte thresh);
		#endregion ANPR

		public string ErrorMessage { get; set; } = "";

		public Bitmap PictureObject { get; set; }

		public string LpResultFa { get; set; }

		public string LpResultEn { get; set; }

		public string PictureAddress { get; set; }

		public string SourceUrl { get; set; } = "http://admin:admin@192.168.1.160:80/snap.jpg?usr=admin&pwd=admin";

		public string ImageAddress { get; set; } = "Image.png";

		public LicensePlate License { get; set; } = new LicensePlate();

		public void InitializeCamera()
		{
			unsafe
			{
				//MessageBox.Show("HID = " + LP_GetHID().ToString());
				string security_code = "www.farsiocr.ir 09361392929";
				char[] security_code_p = new char[40];
				for (int i = 0; i < security_code.Length; i++)
					security_code_p[i] = security_code[i];
				fixed (char* p = &security_code_p[0])
				{
					LP_Initialize(p);
				}
			}
			//it is not required to call LP_SetParams with default params, but if you want to change them, you must call it
			LP_SetParams(1, 16, 10, 5, 1080);
		}

		protected override WebRequest GetWebRequest(Uri address)
		{
			WebRequest wr = base.GetWebRequest(address);
			if (wr != null)
			{
				wr.Timeout = 5000; // timeout in milliseconds (ms)
				return wr;
			}
			return null;
		}

		public void GetPicture()
		{
			if (PingIpAddress(SourceUrl))
			{
				PictureObject = (Bitmap)Image.FromFile("Car.png");
				if (GetPicture(SourceUrl, ImageAddress) > 0)
				{
					Stream stream = File.Open(ImageAddress, FileMode.Open);
					PictureObject = (Bitmap)Image.FromStream(stream);
					stream.Close();
				}
			}
		}

		public static bool ExecuteWithTimeLimit(TimeSpan timeSpan, string SourceUrl, string ImageAddress)
		{
			try
			{
				Task<int> task = Task<int>.Factory.StartNew(() => GetPicture(SourceUrl, ImageAddress));
				task.Wait(timeSpan);
				return task.IsCompleted && task.Result > 0;
			}
			catch
			{
				return false;
			}
		}

		public void ProgressPictureFromCamera()
		{
			ResetResults();
			SetROI();
			Recognize_Buffer();
			LpResultFa = LpResultFa ?? "";

			License.InitializeResult(LpResultFa);
		}

		public void ProgressPictureFromFile()
		{
			ResetResults();
			SetROI();
			Recognize();
			LpResultFa = LpResultFa ?? "";

			License.InitializeResult(LpResultFa);
		}

		private void SetROI()
		{
			LP_SetParams(Convert.ToByte(1), Convert.ToByte(16), Convert.ToByte(10), Convert.ToByte(5), Convert.ToInt16(1080));

			LP_ClearROIs();
		}

		private void Recognize()
		{
			byte[] str = new byte[256];
			char[] result = new char[30];
			char[] result_en = new char[30];
			float cnf = 0;

			unsafe
			{
				fixed (byte* p = &str[0])
				{
					for (int i = 0; i < PictureAddress.Length; i++)
						str[i] = Convert.ToByte(PictureAddress[i]);

					fixed (char* res = &result[0])
					{
						Rect rc;
						LP_Recognize(p, &res[0], &cnf, &rc);

						int i = 0;
						while (res[i] != 0)
							LpResultFa += res[i++];
						fixed (char* res_en = &result_en[0])
						{
							LP_GetEnResults(res, res_en);
							i = 0;
							while (res_en[i] != 0)
								LpResultEn += res_en[i++];
						}
					}

				}
			}
		}

		private void Recognize_Buffer()
		{
			char[] result = new char[30];
			char[] result_en = new char[30];
			float cnf = 0;

			unsafe
			{
				BitmapData data1 = PictureObject.LockBits(new Rectangle(0, 0, PictureObject.Width, PictureObject.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

				IntPtr buffer = data1.Scan0;
				fixed (char* res = &result[0])
				{
					Rect rc;
					LP_Recognize_Buffer(buffer, data1.Width, data1.Height, data1.Stride, &res[0], &cnf, &rc);

					int i = 0;
					while (res[i] != 0)
						LpResultFa += res[i++];

					fixed (char* res_en = &result_en[0])
					{
						LP_GetEnResults(res, res_en);
						i = 0;
						while (res_en[i] != 0)
							LpResultEn += res_en[i++];
					}
				}
			}
		}

		private void ResetResults()
		{
			LpResultFa = "";
			LpResultEn = "";
		}

		private bool PingIpAddress(string IpAddress)
		{
			string pattern = @"\b(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\b";
			MatchCollection mc = Regex.Matches(IpAddress, pattern);
			if (mc.Count > 0)
			{
				string ip = mc[0].Value;

				Ping ping = new Ping();
				PingReply reply = ping.Send(ip, 2 * 1000);

				if (reply != null && reply.Status == IPStatus.Success)
				{
					return true;
				}
			}
			return false;
		}
	}
}
