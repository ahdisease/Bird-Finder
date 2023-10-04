using Capstone.Models;

namespace Capstone.DAO
{
    public interface IUserProfileDao
    {
        public UserProfile GetUserProfileByUsername(string username);

        public UserProfile UpdateUserProfile(UserProfile profile, string username);

        public void DeleteUserProfile( string username);

        public void ActivateUserProfile(UserProfile profile, string username);
    }
}
