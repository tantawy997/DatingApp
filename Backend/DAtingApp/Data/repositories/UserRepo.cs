using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.Data;
using DatingApp.Entites;
using DAtingApp.DTOs;
using DAtingApp.interfaces.repositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DAtingApp.Data.repositories
{
	public class UserRepo : IUserRepo
	{
		private readonly DataContext _Context;
		private readonly IMapper _Mapper;

		public UserRepo(DataContext Context,IMapper mapper) 
		{
			_Context = Context;
			_Mapper = mapper;
		}

		public async Task<MemberDTO> GetMemberAsync(string UserName)
		{
			return await _Context.Users.Where(user => user.UserName == UserName)
				.ProjectTo<MemberDTO>(_Mapper.ConfigurationProvider)
				.SingleOrDefaultAsync();
		}

		public async Task<IEnumerable<MemberDTO>> GetMembersAsync()
		{
			return await _Context.Users.ProjectTo<MemberDTO>(_Mapper.ConfigurationProvider)
				.ToListAsync();
		}

		public async Task<AppUser> GetUserByIdAsync(Guid id)
		{
			return await _Context.Users
				.Include(user =>user.photos)
				.FirstOrDefaultAsync(u => u.UserId == id);
		}

		public async Task<AppUser> GetUserByUserNameAsync(string UserName)
		{
			return await _Context.Users
				.Include(user => user.photos)
				.FirstOrDefaultAsync(u => u.UserName == UserName);

		}

		public async Task<IEnumerable<AppUser>> GetUsersAsync()
		{
			return await _Context.Users
				.Include(user => user.photos)
				.ToListAsync();
		}

		public async Task<bool> SaveAllAsync()
		{
			return await _Context.SaveChangesAsync() > 0;
		}

		public void Update(AppUser user)
		{
			_Context.Entry(user).State = EntityState.Modified;
		}
	}
}
