using Microsoft.AspNetCore.Mvc;
using Capstone.DAO;
using Capstone.Exceptions;
using Capstone.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Principal;

namespace Capstone.Controllers
{
    [Authorize]
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
        [Route("/profile")]
        public IActionResult GetUserProfile()
        {
            IIdentity user = this.User.Identity;

            // Default generic error message
            const string errorMessage = "An unexpected error occurred retrieving profile";
            IActionResult result;
            try
            {
                UserProfile profile = _profileDao.GetUserProfileByUsername(user.Name);
                if (profile != null)
                {
                    result = Ok(profile);
                }
                else
                {
                    result = BadRequest(errorMessage);
                }
            }
            catch (DaoException e)
            {
                result = BadRequest(new { message = e.Message });
            }
            catch (ArgumentException e)
            {
                result = BadRequest(new { message = e.Message });
            }

            return result;
        }

        [HttpPost]
        [Route("/createProfile")]
        public IActionResult CreateUserProfile([FromBody] UserProfile profile)
        {
            IIdentity user = this.User.Identity;

            // Default generic error message
            string errorMessage = "An unexpected error occurred updating profile";
            IActionResult result;

            if (profile.ProfileActive)
            {
                return BadRequest(new { message = "Profile already created. Use PUT method to update." });
            }

            try
            {
                _profileDao.ActivateUserProfile(profile, user.Name);
                profile = _profileDao.UpdateUserProfile(profile, user.Name);
                result = Created("",profile);
            } 
            catch (DaoException e)
            {
                result = BadRequest(new { message = e.Message });
            } 
            catch (ArgumentException e)
            {
                result = BadRequest(new { message = e.Message });
            }
            catch
            {
                result = BadRequest(new { message = errorMessage });
            }

            return result;
        }

        [HttpPut]
        [Route("/editProfile")]
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
            } catch (DaoException e)
            {
                result = BadRequest(new { message = e.Message });
            }
            catch (ArgumentException e)
            {
                result = BadRequest(new { message = e.Message });
            }
            catch
            {
                result = BadRequest(new { message = errorMessage });
            }

            return result;
        }

        [HttpDelete]
        [Route("/deleteProfile")]
        public IActionResult DeleteUserProfile()
        {
            try
            {
                _profileDao.DeleteUserProfile(this.User.Identity.Name);
                return NoContent();
            }
            catch 
            {
                return NotFound(new { message = "Unable to locate profile." });
            } 
        }
    }
}
