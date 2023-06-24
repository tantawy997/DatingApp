using AutoMapper;
using DatingApp.Data;
using DatingApp.Entites;
using DAtingApp.DTOs;
using DAtingApp.extensions;

namespace DAtingApp.helpers
{
	public class AutoMapperProfiles :Profile
	{
		public AutoMapperProfiles()
		{

			CreateMap<AppUser, MemberDTO>()
				.ForMember(dest => dest.photoUrl,
				src => src.MapFrom(a => a.photos.FirstOrDefault(x => x.IsMain).Url))
				.ForMember(dest => dest.Age, src => src.MapFrom(a => a.DateOfBirth.CalculateAge()))

				.ReverseMap();

			CreateMap<Photo, PhotoDTO>()

				.ReverseMap();


			CreateMap<UpdateUserDto, AppUser>().ReverseMap();

		}




	}
}
