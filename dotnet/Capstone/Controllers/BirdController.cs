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
        private readonly IBirdListDao birdListDao;
        public BirdController(BirdDao birdDao, IBirdListDao birdListDao)
        {
            this.birdDao = birdDao;
            this.birdListDao = birdListDao;
            
        }


        [HttpGet("/")]
        public ActionResult<object> OpenEndpoint()
        {
            return Ok(new { message = "Welcome to Birds Finder", description = "This is a place to document your bird sightings." });
        }


        [HttpGet("/birds")]
        public List<Bird> ListAllBirds()
        {
            List<Bird> birdList = birdDao.getBirds();

            if (birdList == null)
            {
                Console.WriteLine("No bird");
            }
           
            return birdList;
            
        }

        [HttpGet("/bird/{id}")]
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

        [HttpPost("/lists/{listId}/addBird")]
        public IActionResult createBird([FromBody] Bird newBird, int listId)
        {
            const string errorMessage = "An error occurred and a bird sighting was not created.";
            
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

        [HttpPut("/updateBird/{id}")]
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

        [HttpDelete("/deleteBird/{id}")]
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

 


        [HttpGet("/lists/{listId}/birds")]
        public List<Bird> getBirdsInList(int listId)
        {
            List<Bird> birdList = birdDao.getBirdsInList(listId);
            if (birdList == null)
            {
                Console.WriteLine("No bird in this list");
            }

            return birdList;
        }


        [HttpGet("/birds/{zipCode}")]
        public List<Bird> getBirdByZip(string zipCode)
        {
            List<Bird> birdList = birdDao.getBirdByZip(zipCode);
            if (birdList == null)
            {
                Console.WriteLine("No bird in this zip code");
            }

            return birdList;
        }

    }
}
