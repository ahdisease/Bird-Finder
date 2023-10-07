using Capstone.DAO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Capstone.Models;
using Capstone.Exceptions;
using System.Security.Principal;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BirdSightingController : ControllerBase
    {
        private readonly IBirdSightingDao birdSightingDao;
        public BirdSightingController(IBirdSightingDao birdSightingDao)
        {
            this.birdSightingDao = birdSightingDao;

        }

        [HttpGet("/birds/{birdId}/notes")]
        public List<BirdSighting> ListAllBirds(int birdId)
        {
            List<BirdSighting> sightingList = birdSightingDao.getSightings(birdId);

            if (sightingList == null)
            {
                Console.WriteLine("No bird");
            }

            return sightingList;

        }

        [HttpPost("/newNote")]
        public IActionResult addSighting( BirdSighting newBirdSighting)
        {

            IIdentity user = User.Identity;
            

            const string errorMessage = "An error occurred and a bird was not created.";

            IActionResult result;
            try
            {
                BirdSighting birdSighting = birdSightingDao.addSighting(newBirdSighting, user.Name);

                result = Created("", newBirdSighting);
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
        [HttpPut("/editNote/{id}")]
        public IActionResult editSighting(BirdSighting sighting, int id)
        {
            
            const string errorMessage = "An error occurred and bird could not be modified.";
            IActionResult result;
            try
            {
                birdSightingDao.editSighting(sighting, id);
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
         
        [HttpDelete("/deleteNote/{id}")]
        public IActionResult deleteSighting(int id)
        {
            
            const string errorMessage = "An error occurred and bird could not be deleted.";
            IActionResult result;
            try
            {
                birdSightingDao.deleteSighting(id);
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
