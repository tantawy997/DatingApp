using DAtingApp.DTOs;
using DAtingApp.Entites;
using DAtingApp.helpers;

namespace DAtingApp.interfaces.repositoryInterfaces
{
	public interface IMessagesRepo
	{
		void AddMessage(Message message);

		void DeleteMessage(Message message);

		Task<Message> GetMessageAsync(int messageId);

		Task<PageList<MessageDTO>> GetMessagesForUser(MessageParams messageParams);

		Task<IEnumerable<MessageDTO>> GetMessagesThread(string CurrentUserName, string RecipientUserName);
		//Task<bool> SaveAllAsync();

		void AddGroup(Group group);

		void RemoveConnection(Connection connection);

		Task<Connection> GetConnection(string ConnectionId);

		Task<Group> GetMessageGroup(string GroupName);


		Task<Group> GetGroupForConnection(string ConnectionId);

	}
}
