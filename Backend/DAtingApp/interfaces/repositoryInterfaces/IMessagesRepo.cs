using DAtingApp.DTOs;
using DAtingApp.Entites;
using DAtingApp.helpers;

namespace DAtingApp.interfaces.repositoryInterfaces
{
	public interface IMessagesRepo
	{
		void AddMessage(Message message);

		void DeleteMessage(Message message);

		Task<Message> GetMessageAsync(Guid messageId);

		Task<PageList<MessageDTO>> GetMessagesForUser(MessageParams messageParams);

		Task<IEnumerable<MessageDTO>> GetMessagesThread(string CurrentUserName, string RecipientUserName);
		Task<bool> SaveAllAsync();


	}
}
