using Microsoft.AspNetCore.Mvc;
using Capstone.DAO;
using Capstone.Exceptions;
using Capstone.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Security.Claims;
using System.Security.Principal;

namespace Capstone.Controllers
{
    [Authorize]
    [Route("/profile")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        //private readonly IUserDao _userDao;
        private readonly IUserProfileDao _profileDao;

        public UserProfileController(IUserProfileDao userProfileDAO)
        {
            _profileDao = userProfileDAO;
        }

        [HttpGet]
        public IActionResult GetUserProfile(IdentityUser user)
        {
            // Default generic error message
            const string errorMessage = "An unexpected error occurred retrieving profile";
            IActionResult result = BadRequest(new { message = errorMessage });

            UserProfile profile = _profileDao.GetUserProfileByUsername(this.User.Identity.Name);

            if (profile != null)
            {
                result = Ok(profile);
            }

            return result;
        }

        [HttpPost]
        public IActionResult CreateUserProfile([FromBody] UserProfile profile)
        {
            IIdentity user = this.User.Identity;

            // Default generic error message
            string errorMessage = "An unexpected error occurred updating profile";
            IActionResult result;

            try
            {
                _profileDao.UpdateUserProfile(profile, user.Name);
                result = Ok();
            } catch (DaoException)
            {
                result = BadRequest(new { message = errorMessage });
            } catch (ArgumentException)
            {
                errorMessage = "UserProfile object was not provided.";
                result = BadRequest(new { message = errorMessage });

            }

            return result;
        }

        [HttpPut]
        public IActionResult UpdateUserProfile([FromBody] UserProfile profile)
        {
            IIdentity user = this.User.Identity;
            
            // Default generic error message
            string errorMessage = "An unexpected error occurred updating profile";
            IActionResult result;

            try
            {
                _profileDao.UpdateUserProfile(profile, user.Name);
                result = Ok();
            }
            catch (DaoException)
            {
                result = BadRequest(new { message = errorMessage });
            }
            catch (ArgumentException)
            {
                errorMessage = "UserProfile object was not provided.";
                result = BadRequest(new { message = errorMessage });

            }

            return result;
        }
    }
}
