﻿using System.ComponentModel.DataAnnotations;

namespace DAtingApp.DTOs
{
	public class RegisterDTO
	{
		[Required]
		public string UserName { get; set; }

		[Required]
		public string Password { get; set; }

	}
}
