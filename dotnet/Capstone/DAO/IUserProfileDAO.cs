using Capstone.Models;

namespace Capstone.DAO
{
    public interface IUserProfileDao
    {
        public UserProfile UpdateUserProfile(UserProfile profile, string username);
    }
}
