using System.ComponentModel.DataAnnotations;

namespace DAtingApp.Entites
{
	public class Group
	{
		public Group() { }

		public Group(string groupName)
		{
			GroupName = groupName;
		}

		[Key]
		public string GroupName { get; set; }

		public List<Connection> Connections { get; set; } = new List<Connection>();

	}
}
