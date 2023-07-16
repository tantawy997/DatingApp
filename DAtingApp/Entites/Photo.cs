using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.Entites
{
	[Table("Photos")]
	public class Photo
	{
		[Key]
		public int PhotoId { get; set; }
		public string Url { get; set; }
		public bool IsMain { get; set; }
		public string PublicId { get; set; }

		[ForeignKey("AppUser")]
		public int UserId { get; set; }

		public AppUser AppUser { get; set; }
	}

}