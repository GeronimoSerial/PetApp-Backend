
using DataAccessLayer.Entities;

namespace BusinessAccessLayer.Services
{
    public interface iTokenService
    {
        Task<string> CreateToken(User User);
    }
}
