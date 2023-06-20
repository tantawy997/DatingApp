using DAtingApp.extensions;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.Entites
{
	public class AppUser
	{
		[Key]
		public Guid UserId { get; set; }

		public string UserName { get; set; }

		public byte[] PasswordHah { get; set; }

		public byte[] PasswordSalt { get; set; }

		public DateTime DateOfBirth { get; set; }
		public string KnownAs { get; set; }

		public DateTime Created { get; set; } = DateTime.UtcNow;

		public DateTime LastActive { get; set; } = DateTime.UtcNow;

		public  string Gender { get; set; }

		public string Introduction { get; set; }

		public string LookingFor { get; set; }

		public string Interests { get; set;}

		public string City { get; set; }

		public string Country { get; set; }

		public ICollection<Photo> photos { get; set; } = new HashSet<Photo>();


		//public int GetAge()
		//{
		//	return DateOfBirth.CalculateAge();
		//}
	}
}
