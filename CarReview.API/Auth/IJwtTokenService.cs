
namespace CarReview.API.Auth
{
    public interface IJwtTokenService
    {
        Task<string> CreateAccessToken(string userName, string userId, IEnumerable<string> userRoles);
    }
}