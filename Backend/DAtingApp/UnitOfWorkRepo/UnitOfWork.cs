using AutoMapper;
using DatingApp.Data;
using DAtingApp.Data.repositories;
using DAtingApp.interfaces.repositoryInterfaces;

namespace DAtingApp.UnitOfWorkRepo
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly DataContext _Context;
		private readonly IMapper _Mapper;

		public UnitOfWork(DataContext context,IMapper mapper)
		{
			_Context = context;
			_Mapper = mapper;
		}
		public IUserLikeRepo _UserLikeRepo => new UserLikeRepo(_Context);

		public IUserRepo _UserRepo => new UserRepo(_Context, _Mapper);

		public IMessagesRepo _MessagesRepo => new MessageRepo(_Context,_Mapper);


		public async Task<bool> Completes()
		{
			return await _Context.SaveChangesAsync() > 0;
		}

		public bool HasChanges()
		{
			return _Context.ChangeTracker.HasChanges();
		}
	}
}
