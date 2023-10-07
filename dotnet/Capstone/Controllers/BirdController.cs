using Capstone.DAO;
using Capstone.Exceptions;
using Capstone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BirdController : ControllerBase
    {
        private readonly BirdDao birdDao;
        public BirdController(BirdDao birdDao)
        {
            this.birdDao = birdDao;
            
        }


        [HttpGet("/")]
        public ActionResult<object> OpenEndpoint()
        {
            return Ok(new { message = "Welcome to Birds Finder", description = "This is a place to document your bird sightings." });
        }


        [HttpGet("/birds")]
        public List<Bird> ListAllBirds()
        {
            List<Bird> birdList = birdDao.listAllBirds();

            if (birdList == null)
            {
                Console.WriteLine("No bird");
            }
           
            return birdList;
            
        }

        [HttpGet("/birds/{id}")]
        public Bird getBird(int id)
        {
            Bird bird = birdDao.getBird(id);
           // const string ErrorMessage = "No bird matches this id.";

            if (bird == null)
            {
                Console.WriteLine("No bird matches this id");
                //return StatusCode(404, ErrorMessage);
            }
      
            return bird;
            
        }

        [HttpGet("/randomBird")]
        public Bird getRandomBird()
        {
            Bird randomBird = birdDao.getRandomBird();

            if (randomBird == null)
            {
                Console.WriteLine("No bird matches this id");
                //return StatusCode(404, ErrorMessage);
            } 

            return randomBird;
        }

        [HttpPost("/birds")]
        public IActionResult createBird(Bird newBird)
        {
            const string errorMessage = "An error occurred and a bird was not created.";
            
            IActionResult result;
            try
            {
                Bird bird = birdDao.createBird(newBird);

                result = Created("", newBird);
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

        [HttpPut("/birds/{id}")]
        public IActionResult editBird(Bird updatedBird, int id)
        {
            const string errorMessage = "An error occurred and bird could not be modified.";
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

        [HttpDelete("/birds/{id}")]
        public IActionResult deleteBird(int id)
        {
            
            const string errorMessage = "An error occurred and bird could not be deleted.";
            IActionResult result;
            try
            {
                birdDao.deleteBird(id);
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

    }
}
