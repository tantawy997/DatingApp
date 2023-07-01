﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.Data;
using DAtingApp.DTOs;
using DAtingApp.Entites;
using DAtingApp.helpers;
using DAtingApp.interfaces.repositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DAtingApp.Data.repositories
{
	public class MessageRepo : IMessagesRepo
	{
		private readonly DataContext _Context;
		private readonly IMapper _Mapper;

		public MessageRepo(DataContext context,IMapper mapper) 
		{
			_Context = context;
			_Mapper = mapper;
		}
		public void AddMessage(Message message)
		{
			_Context.Messages.Add(message);
		}

		public void DeleteMessage(Message message)
		{
			_Context.Messages.Remove(message);
		}

		public async Task<Message> GetMessageAsync(Guid messageId)
		{
			return	await _Context.Messages.FindAsync(messageId);
		}

		public async Task<PageList<MessageDTO>> GetMessagesForUser(MessageParams messageParams)
		{
			var query = _Context.Messages
				.OrderBy(m => m.MessageSentDate)
				.AsQueryable();

			query = messageParams.Container switch
			{
				"Inbox" => query.Where(m => m.RecipientUserName == messageParams.UserName && 
				m.RecipientDeleted == false),
				"Outbox" => query.Where(m => m.SenderUserName == messageParams.UserName &&
				m.SenderDeleted == false),
				_ => query.Where(m=>m.RecipientUserName == messageParams.UserName && m.MessageReadDate == null
				&& m.RecipientDeleted == false)
			};

			var messages = query.ProjectTo<MessageDTO>(_Mapper.ConfigurationProvider);

			return await PageList<MessageDTO>
				.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
		}

		public async Task<IEnumerable<MessageDTO>> GetMessagesThread(string CurrentUserName, string RecipientUserName)
		{
			var messages = await _Context.Messages
				.Include(u => u.Sender).ThenInclude(u => u.photos)
				.Include(u =>u.Recipient).ThenInclude(u => u.photos)

				.Where(u => u.Recipient.UserName == CurrentUserName && u.RecipientDeleted == false
				&& u.Sender.UserName == RecipientUserName 
				||
				u.Recipient.UserName == RecipientUserName && u.SenderDeleted == false &&
				u.Sender.UserName == CurrentUserName 
				).OrderByDescending(m=>m.MessageSentDate).ToListAsync();

			var unreadMessages = messages.Where(u => u.MessageReadDate == null
			&& u.RecipientUserName == CurrentUserName).ToList();

			if (unreadMessages.Any())
			{
				foreach (var message in unreadMessages)
				{
					message.MessageReadDate = DateTime.UtcNow;
				}
			}

			await _Context.SaveChangesAsync();

			return _Mapper.Map<IEnumerable<MessageDTO>>(messages);

		}

		public async Task<bool> SaveAllAsync()
		{
			return await _Context.SaveChangesAsync() > 0;
		}
	}
}
