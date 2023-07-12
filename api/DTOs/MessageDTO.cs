using DatingApp.Entites;

namespace DAtingApp.DTOs
{
	public class MessageDTO
	{
		public Guid Id { get; set; }

		public Guid SenderId { get; set; }

		public string SenderUserName { get; set; }
		public string SenderPhotoUrl { get; set; }

		public Guid RecipientId { get; set; }
		public string RecipientUserName { get; set; }
		
		public string RecipientPhotoUrl { get; set; }

		public string Content { get; set; }

		public DateTime? MessageReadDate { get; set; }

		public DateTime MessageSentDate { get; set; } 
	}
}
