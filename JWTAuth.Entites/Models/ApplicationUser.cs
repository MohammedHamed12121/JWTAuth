using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuth.Entites.Models
{
	public class ApplicationUser
	{
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }

	public class RefreshToken  // this is a refreash token
	{
        //public int Id { get; set; }
        [Key]
        public string Token { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Expires { get; set; }
        public bool Enabled { get; set; }
        public string? Email { get; set; }
    }
}
