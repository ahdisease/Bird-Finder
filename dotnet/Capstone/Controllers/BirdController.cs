using Capstone.DAO;
using Capstone.Exceptions;
using Capstone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class BirdController : ControllerBase
    {
        private readonly IBirdDao birdDao;
        
        public BirdController(IBirdDao birdDao)
        {
            this.birdDao = birdDao;
            
            
        }

        [AllowAnonymous]
        [HttpGet("/")]
        public ActionResult<object> OpenEndpoint()
        {
            return Ok(new { message = "Welcome to Birds Finder", description = "This is a place to document your bird sightings." });
        }


        [HttpGet("/birds")]
        public IActionResult ListAllBirds()
        {

            string errorMessage = "No birds.";

            IActionResult result;
            
            try
            {
                List<Bird> birdList = birdDao.getBirds();

                if (birdList != null)
                {
                    result = Ok(birdList);
                }
                else
                {
                    result = NotFound(new { message = errorMessage });

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
            catch
            {
                result = BadRequest(new { message = errorMessage });
            }


            return result;
            
        }

        [HttpGet("/bird/{id}")]
        public IActionResult getBird(int id)
        {
            
            string errorMessage = "No bird matches this id.";
            IActionResult result;
            try
            {
                Bird bird = birdDao.getBird(id);

                if (bird != null)
                {
                    result = Ok(bird);
                }
                else
                {
                    result = NotFound(new { message = errorMessage });
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
            catch
            {
                result = BadRequest(new { message = errorMessage });
            }


            return result;
            
        }

        [AllowAnonymous]
        [HttpGet("/randomBird")]
        public IActionResult getRandomBird()
        {
            string errorMessage = "No birds available.";
            IActionResult result;

            try
            {
                Bird randomBird = birdDao.getRandomBird();

                if (randomBird != null)
                {
                    result = Ok(randomBird);
                }
                else
                {
                    result = NotFound(new { message = errorMessage });
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
            catch
            {
                result = BadRequest(new { message = errorMessage });
            }

            return result;
        }

        [HttpPost("/lists/{listId}/addBird")]
        public IActionResult createBird([FromBody] Bird newBird, int listId)
        {
            string errorMessage = "An error occurred and a bird sighting was not created.";
            
            IActionResult result;
            try
            {
                Bird bird = birdDao.createBird(newBird, listId);

                result = Created("", "");
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

        [HttpPut("/updateBird")]
        public IActionResult editBird(Bird updatedBird, int id)
        {
            string errorMessage = "An error occurred and bird could not be modified.";
            IActionResult result;
            try
            {
                birdDao.editBird(updatedBird, id);
                result = Ok();
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

        [HttpDelete("/deleteBird/{id}")]
        public IActionResult deleteBird(int id)
        {
            IIdentity user = User.Identity;

            string errorMessage = "An error occurred and bird could not be deleted.";
            IActionResult result;
            try
            {
                birdDao.deleteBird(id,user.Name);
                result = NoContent();
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

 


        [HttpGet("/lists/{listId}/birds")]
        public IActionResult getBirdsInList(int listId)
        {
            string errorMessage = "An error occurred and there are no birds in this list.";
            IActionResult result;
            try
            {
                List<Bird> birdList = birdDao.getBirdsInList(listId);
                if (birdList != null)
                {
                    result = Ok(birdList);
                }
                else
                {
                    result = NotFound(new { message = errorMessage });
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
            catch
            {
                result = BadRequest(new { message = errorMessage });
            }



            return result;
        }

        [AllowAnonymous]
        [HttpGet("/birds/{zipCode}")]
        public IActionResult getBirdByZip(string zipCode)
        {
            string errorMessage = "An error occurred and there are no birds in this zipcode.";
            IActionResult result;
            try
            {
                List<Bird> birdList = birdDao.getBirdByZip(zipCode);
                if (birdList != null)
                {
                    result = Ok(birdList);
                }
                else
                {
                    result = NotFound(new { message = errorMessage });
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
            catch
            {
                result = BadRequest(new { message = errorMessage });
            }



            return result;
        }

    }
}
