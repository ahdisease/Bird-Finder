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
    public class BirdNoteController : ControllerBase
    {
        private readonly IBirdNoteDao birdSightingDao;
        public BirdNoteController(IBirdNoteDao birdSightingDao)
        {
            this.birdSightingDao = birdSightingDao;

        }

        [HttpGet("/bird/{birdId}/notes")]
        public List<BirdNote> getNotes(int birdId)
        {
            List<BirdNote> sightingList = birdSightingDao.getSightings(birdId);

            if (sightingList == null)
            {
                Console.WriteLine("No bird");
            }

            return sightingList;

        }

        [HttpGet("/note")]
        public BirdNote getNote(int id)
        {
            BirdNote sighting = birdSightingDao.getBirdSighting(id);

            if (sighting == null)
            {
                Console.WriteLine("No bird");
            }

            return sighting;

        }

        [HttpPost("/newNote")]
        public IActionResult addNote([FromBody] BirdNote newBirdSighting)
        {
            const string errorMessage = "An error occurred and a bird was not created.";

            IActionResult result;
            try
            {
                BirdNote birdSighting = birdSightingDao.addSighting(newBirdSighting, newBirdSighting.BirdId);

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
        [HttpPut("/editNote")]
        public IActionResult editNote(BirdNote sighting, int id)
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
        public IActionResult deleteNote(int id)
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
