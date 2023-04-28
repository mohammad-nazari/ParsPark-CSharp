using SettingsLib;

namespace ToolsLib
{
	public enum SoftwareType
	{
		Nothing,
		Enter,
		Exit,
		Both,
        Server,
        Client
	}
	/// <summary>
	/// For global changeable variables
	/// </summary>
	public static class GlobalVariables
	{
		public static string ConnectionString { get; set; } = "";

		public static AllSettings AllSettingsObject { get; set; } = new AllSettings();

		public static AllSettings AllOldSettingsObject { get; set; } = new AllSettings();

		public static SoftwareType SoftType { get; set; }
	}
}
