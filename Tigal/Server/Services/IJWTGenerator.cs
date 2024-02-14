using Tigal.Shared.Models;

namespace Tigal.Server.Services
{
    public interface IJWTGenerator
    {
        string GetToken(Users user);
    }
}
