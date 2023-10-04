using Capstone.Models;

namespace Capstone.DAO
{
    public interface IUserProfileDao
    {
        public UserProfile GetUserProfileByUsername(string username);

        public void UpdateUserProfile(UserProfile profile, string username);
    }
}
