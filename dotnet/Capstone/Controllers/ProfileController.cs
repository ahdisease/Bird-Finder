using Microsoft.AspNetCore.Mvc;
using Capstone.DAO;
using Capstone.Exceptions;
using Capstone.Models;
using Microsoft.AspNetCore.Identity;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUserDao _userDao;
        //private readonly IProfileDAO _profileDao;

        public ProfileController(IUserDao userDao)
        {
            _userDao = userDao;
            //_profileDao = profileDao;
        }

        [HttpGet("/profile")]
        public IActionResult GetUserProfile(IdentityUser username)
        {
            

            // Default generic error message
            const string errorMessage = "An unexpected error occurred retrieving user";
            IActionResult result = BadRequest(new { message = errorMessage });

            User user;
            try
            {
                user = _userDao.GetUserByUsername(username.UserName);
            } catch (DaoException)
            {
                return result;
            }

            if (user != null)
            {
                // Create a ReturnUser object to return to the client
                ReturnUser returnUser = new ReturnUser() { UserId = user.UserId, Username = user.Username, Role = user.Role, Profile = user.Profile };

                result = Created("/login", returnUser);
            }

            return result;
        }

        [HttpPost("/profile")]

        [HttpPut("/profile")]
        public IActionResult UpdateUserProfile()
        {

            return Unauthorized();
        }
    }
}
