using System;
using System.Drawing;
using CameraLib;
using ToolsLib;

namespace ParsPark
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ParsParkWebService" in both code and config file together.
	public class ParsParkWebService : IParsParkWebService
	{
		public string TestServer(string request)
		{
			// Run ANPR
			return "It is OK ... " + request;
		}
		public string DetectLp(string request)
		{
			// Run ANPR
			if (request != null)
			{
				if (Tools.AllowUsingAnpr())
				{
					try
					{
						Image img = ImageProcess.Base64ToImage(request);

						Camera cameraObject = new Camera { PictureObject = (Bitmap)img };
						cameraObject.InitializeCamera();

						// Start ANPR
						cameraObject.ProgressPictureFromCamera();

						return MySerialize.SerializeObject(cameraObject.License);
					}
					catch
					{
						return null;
					}
				}
			}
			return null;
		}
	}
}
