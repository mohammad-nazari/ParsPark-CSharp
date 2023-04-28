namespace ToolsLib
{
	/// <summary>
	/// This check to enable options of this version of software
	/// </summary>
	public enum SoftwareEdition
	{                   /*	ANPR	Camera		Barriers		Report		RFID Card	*/
		Starter,        /*										Yes			Yes			*/
		Standard,       /*						Yes				Yes			Yes			*/
		Professional,   /*			Yes							Yes			Yes			*/
		Enterprise,     /*			Yes			Yes				Yes			Yes			*/
		Ultimate        /*	Yes		Yes			Yes				Yes			Yes			*/
	}

	public enum UserType
	{
		admin = 0,
		manager = 1,
		emploee = 2
	}

	public enum LogType
	{
		sub = 0,
		pub = 1
	}

	public enum Mode
	{
		Test,
		Use
	}

	public enum RequestType
	{
		Test,
		Detailes,
		Picture
	}

	public enum LogStatus
	{
		Normal,
		Warning,
		Error
	}

	public enum CheckCardValidation
	{
		Yes,
		No
	}

	public static class Constants
	{
		public const SoftwareEdition Softwareedition = SoftwareEdition.Ultimate;
		public const Mode SoftwareMode = ToolsLib.Mode.Use;
		public const CheckCardValidation CardValidationCheck = CheckCardValidation.Yes;
		public static readonly string Selectlogcode = @"SELECT * FROM `logs` WHERE `code` = '%S' AND `exit` IS NULL";
		public static readonly string AlphaNum = @"0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
		public static readonly string RegistryKeyAddress = @"Software\Anar\ParsPark";
	}

}
