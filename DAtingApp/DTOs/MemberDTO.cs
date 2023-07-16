using DatingApp.Entites;

namespace DAtingApp.DTOs
{
	public class MemberDTO
	{
		public int UserId { get; set; }

		public string UserName { get; set; }
		public int Age { get; set; }
		public string KnownAs { get; set; }

		public DateTime Created { get; set; } 

		public DateTime LastActive { get; set; } 

		public string Gender { get; set; }

		public string Introduction { get; set; }

		public string LookingFor { get; set; }

		public string Interests { get; set; }

		public string City { get; set; }

		public string Country { get; set; }

		public string photoUrl { get; set; }

		public ICollection<PhotoDTO> photos { get; set; } 


	}
}
