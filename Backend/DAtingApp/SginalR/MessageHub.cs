using AutoMapper;
using DAtingApp.Data.repositories;
using DAtingApp.DTOs;
using DAtingApp.Entites;
using DAtingApp.extensions;
using DAtingApp.interfaces.repositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DAtingApp.SginalR
{
	[Authorize]
	public class MessageHub: Hub
	{
		private readonly IMessagesRepo _MessagesRepo;
		private readonly IUserRepo _UserRepo;
		private readonly IMapper _Mapper;
		private readonly IHubContext<PresenseHub> _PresenceHub;

		public MessageHub(IMessagesRepo messagesRepo,IUserRepo userRepo,IMapper mapper,
			IHubContext<PresenseHub> PresenceHub)
		{
			_MessagesRepo = messagesRepo;
			_UserRepo = userRepo;
			_Mapper = mapper;
			_PresenceHub = PresenceHub;
		}

		public override async Task OnConnectedAsync()
		{
			var httpContext = Context.GetHttpContext();
			var OtherUser = httpContext.Request.Query["user"];
			var GroupName = GetGroupName(Context.User.GetUserName(),OtherUser);
			await Groups.AddToGroupAsync(Context.ConnectionId, GroupName);

			var group= await AddToGroup(GroupName);
			await Clients.Group(GroupName).SendAsync("UpdatedGroup",group);

			var message = await _MessagesRepo.GetMessagesThread(Context.User.GetUserName(),OtherUser);

			await Clients.Caller.SendAsync("receiveMessageThread",message);
		}

		public async Task SendMessage(CreateMessageDTO createMessageDTO)
		{
			var username = Context.User.GetUserName();
			if (createMessageDTO.RecipientUserName.ToLower() == username)
			{
				throw new HubException("You can not send messages to yourself");
			}

			var sender = await _UserRepo.GetUserByUserNameAsync(username);
			var Recipient = await _UserRepo.GetUserByUserNameAsync(createMessageDTO.RecipientUserName);

			if (Recipient == null) throw new HubException("not found");

			var message = new Message
			{
				Sender = sender,
				Recipient = Recipient,
				RecipientUserName = Recipient.UserName,
				SenderUserName = sender.UserName,
				Content = createMessageDTO.Content

			};
			var groupName = GetGroupName(sender.UserName,Recipient.UserName);
			var group = await _MessagesRepo.GetMessageGroup(groupName);

			if(group.Connections.Any(c=> c.UserName == Recipient.UserName))
			{
				message.MessageReadDate= DateTime.UtcNow;
			}
			else
			{
				var Connections = await PresenceTracker.GetConnectionsForUser(Recipient.UserName);
				if(Connections != null)
				{
					await _PresenceHub.Clients.Clients(Connections).SendAsync("NewMessageRecieved",
						new { username = sender.UserName, knownAs = sender.KnownAs });
				}
			}
			_MessagesRepo.AddMessage(message);

			if (await _MessagesRepo.SaveAllAsync())
			{
				//var group = GetGroupName(sender.UserName,Recipient.UserName);

				await Clients.Group(groupName).SendAsync("NewMessage", _Mapper.Map<MessageDTO>(message));
			}

		}
		public override async Task OnDisconnectedAsync(Exception exception)
		{
			var group= await RemoveFromMessageGroup();
			await Clients.Group(group.GroupName).SendAsync("UpdatedGroup");

			await base.OnDisconnectedAsync(exception);
		}
		private string GetGroupName(string culler,string other)
		{
			var GroupName = string.CompareOrdinal(culler, other) < 0;
			return (GroupName) ? $"{culler}-{other}" : $"{other}-{culler}";
		}

		private async Task<Group> AddToGroup(string GroupName)
		{
			var group = await _MessagesRepo.GetMessageGroup(GroupName);
			var connection = new Connection(Context.ConnectionId, Context.User.GetUserName());
			
			if(group == null)
			{
				group = new Group(GroupName);
				_MessagesRepo.AddGroup(group);
			}

			group.Connections.Add(connection);

			 if(await _MessagesRepo.SaveAllAsync()) return group;

			throw new HubException("failed to add to group");

		}
		private async Task<Group> RemoveFromMessageGroup()
		{
			var group = await _MessagesRepo.GetGroupForConnection(Context.ConnectionId);

			var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

			_MessagesRepo.RemoveConnection(connection);
			if (await _MessagesRepo.SaveAllAsync()) return group;

			throw new HubException("failed to remove connection");
		}
	}
	
}
