using AutoMapper;
using DAtingApp.DTOs;
using DAtingApp.Entites;
using DAtingApp.extensions;
using DAtingApp.helpers;
using DAtingApp.interfaces.repositoryInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAtingApp.Controllers
{
	//[Route("api/[controller]")]
	//[ApiController]
	public class MessageController : ApiControllerBase
	{
		private readonly IUserRepo _UserRepo;
		private readonly IMapper _Mapper;
		private readonly IMessagesRepo _MessagesRepo;

		public MessageController(IUserRepo userRepo, IMapper mapper, IMessagesRepo messagesRepo)
		{
			_UserRepo = userRepo;
			_Mapper = mapper;
			_MessagesRepo = messagesRepo;
		}

		[HttpPost]
		public async Task<ActionResult<MessageDTO>> CreateMessage(CreateMessageDTO createMessage)
		{

			var username = User.GetUserName();
			if (createMessage.RecipientUserName.ToLower() == username)
			{
				return BadRequest("You can not send messages to yourself");
			}

			var sender = await _UserRepo.GetUserByUserNameAsync(username);
			var Recipient = await _UserRepo.GetUserByUserNameAsync(createMessage.RecipientUserName);

			if (Recipient == null) return NotFound();

			var message = new Message
			{
				Sender = sender,
				Recipient = Recipient,
				RecipientUserName = Recipient.UserName,
				SenderUserName = sender.UserName,
				Content = createMessage.Content

			};

			_MessagesRepo.AddMessage(message);

			if (await _MessagesRepo.SaveAllAsync()) return Ok(_Mapper.Map<MessageDTO>(message));

			return BadRequest("failed to send message");

		}

		[HttpGet]
		public async Task<ActionResult<PageList<MessageDTO>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
		{
			messageParams.UserName = User.GetUserName();
			var messages = await _MessagesRepo.GetMessagesForUser(messageParams);

			Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize
				, messages.TotalCount, messages.TotalPages));

			return Ok(messages);
		}
		[HttpGet("thread/{username}")]
		public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessagesThread(string username)
		{
			var currentUserName = User.GetUserName();

			var messages = await _MessagesRepo.GetMessagesThread(currentUserName, username);

			return Ok(messages);
		}

		[HttpDelete("{id}")]

		public async Task<ActionResult> DeleteMessage(Guid id)
		{
			var username = User.GetUserName();

			var message = await _MessagesRepo.GetMessageAsync(id);

			if(message.SenderUserName != username && message.RecipientUserName != username)
			{
				return Unauthorized();
			}

			if(message.RecipientUserName == username) message.RecipientDeleted = true;
			if(message.SenderUserName == username) message.SenderDeleted = true;

			if(message.SenderDeleted && message.RecipientDeleted)
			{
				_MessagesRepo.DeleteMessage(message);
			}

			if (await _MessagesRepo.SaveAllAsync()) return Ok();

			return BadRequest("problem deleting the message");
		}
	}
}
