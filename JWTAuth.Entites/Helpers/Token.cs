using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JWTAuth.Entites.Models;

namespace JWTAuth.Entites.Helpers
{
	public class Token
	{
        public string? AccessToken { get; set; }
        public RefreshToken? RefreshToken { get; set; }
    }
}
