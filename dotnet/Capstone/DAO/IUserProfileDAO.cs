using Capstone.Models;

namespace Capstone.DAO
{
    public interface IUserProfileDAO
    {
        public UserProfile UpdateUserProfile(UserProfile profile, string username);
    }
}
