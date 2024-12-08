using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JWTAuth.Entites.Models;

namespace JWTAuth.Entites.Responses
{
    public record GeneralResponse(bool Flag, string Message = null!);
    public record AuthResponse(bool Flag, string Message = null!, string AccessToken = "", string? RefreshToken = "");
}
