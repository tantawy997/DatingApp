﻿using System.ComponentModel.DataAnnotations;

namespace DAtingApp.DTOs
{
	public class RegisterDTO
	{
		[Required]
		public string UserName { get; set; }

		[Required]
		[StringLength(16, MinimumLength = 4)]
		public string Password { get; set; }
		[Required]
		public string KnownAs { get; set; }
		[Required]

		public DateTime DateOfBirth { get; set; }
		[Required]

		public string Gender { get; set; }
		[Required]

		public string City { get; set; }
		[Required]

		public string Country { get; set; }

	}
}
