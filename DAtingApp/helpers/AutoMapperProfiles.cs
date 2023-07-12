using AutoMapper;
using DatingApp.Data;
using DatingApp.Entites;
using DAtingApp.DTOs;
using DAtingApp.Entites;
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

			CreateMap<RegisterDTO, AppUser>()
				.ReverseMap();

			CreateMap<Message, MessageDTO>()
				.ForMember(dest => dest.RecipientPhotoUrl,src => src
				.MapFrom(photo => photo.Recipient.photos.FirstOrDefault(p => p.IsMain).Url))
				.ForMember(dest => dest.SenderPhotoUrl, src => src
				.MapFrom(photo => photo.Sender.photos.FirstOrDefault(p => p.IsMain).Url))
				.ReverseMap();

				
				CreateMap<DateTime,DateTime>()
				.ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));

				CreateMap<DateTime?, DateTime?>()
				.ConvertUsing(d => d.HasValue? DateTime.SpecifyKind(d.Value,DateTimeKind.Utc):null);
		}




	}
}
