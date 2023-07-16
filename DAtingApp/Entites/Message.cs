using DatingApp.Entites;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.ComponentModel.DataAnnotations;

namespace DAtingApp.Entites
{
	public class Message
	{
		[Key]
		public int Id { get; set; }

		public int SenderId { get; set; }

		public string SenderUserName { get; set; }

		public AppUser Sender { get; set; }

		public int RecipientId { get; set; }

		public string RecipientUserName { get; set; }


		public AppUser Recipient { get; set; }

		public string Content { get; set; }

		public DateTime? MessageReadDate { get; set; }

		public DateTime MessageSentDate { get; set; } = DateTime.UtcNow;

		public bool SenderDeleted { get; set; }

		public bool RecipientDeleted { get; set;}

	}
}
