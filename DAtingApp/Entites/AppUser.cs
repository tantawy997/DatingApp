﻿using DAtingApp.Entites;
using DAtingApp.extensions;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.Entites
{
	public class AppUser : IdentityUser<Guid>
	{

		public DateTime DateOfBirth { get; set; }
		public string KnownAs { get; set; }

		public DateTime Created { get; set; } = DateTime.UtcNow;

		public DateTime LastActive { get; set; } = DateTime.UtcNow;

		public  string Gender { get; set; }

		public string Introduction { get; set; }

		public string LookingFor { get; set; }

		public string Interests { get; set;}

		public string City { get; set; }

		public string Country { get; set; }

		public ICollection<Photo> photos { get; set; } = new HashSet<Photo>();

		
		public List<UserLike> LikedByUsers { get; set; }

		public List<UserLike> LikedUsers { get; set; }

		public List<Message> MessagesSent { get; set; }
		
		public List<Message> MessagesReceived { get; set; }


		public ICollection<AppUserRole> UserRoles { get; set; }

	}
}