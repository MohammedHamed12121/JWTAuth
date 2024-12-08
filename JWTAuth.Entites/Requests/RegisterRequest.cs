using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuth.Entites.Requests
{
	public class RegisterRequest
	{
		[Required]
		public string? Email { get; set; }
		[Required]
		public string? Password { get; set; }
		[Required]
		public string? Role { get; set; }
	}

	public class LoginRequest
	{
		[Required]
		public string? Email { get; set; }
		[Required]
		public string? Password { get; set; }
	}
}
