namespace DatingApp.Entites
{
	public class AppUser
	{
		public int id { get; set; }

		public string UserName { get; set; }

		public byte[] PasswordHah { get; set; }

		public byte[] PasswordSalt { get; set; }

	}
}
