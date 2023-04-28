using DataBaseLib;
using ToolsLib;

namespace ParsPark
{
	class MySqlDataBaseClass
	{
		public MySqlDataBaseClass()
		{
			this.ParsPark.Database.Connection.ConnectionString = GlobalVariables.ConnectionString;
		}

		public string ConnectionString { get; set; }

		public parsparkoEntities ParsPark { get; set; } = new parsparkoEntities("");
	}
}
