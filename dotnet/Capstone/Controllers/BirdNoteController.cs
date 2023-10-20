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

        [HttpPost("/newNote/{birdId}")]
        public IActionResult addNote([FromBody] BirdNote newBirdSighting)
        {
            const string errorMessage = "An error occurred and a bird was not created.";

            IActionResult result;

            DateTime today = DateTime.Now;
            
            try
            {
                BirdNote birdSighting = birdSightingDao.addSighting(newBirdSighting, newBirdSighting.BirdId);
                /*
                if(birdSighting.DateSpotted > today) 
                {
                    Console.WriteLine("Cannot select future date");
                }
                */

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
        public IActionResult editNote(BirdNote sighting)
        {
            
            const string errorMessage = "An error occurred and bird could not be modified.";
            IActionResult result;

            int currentDate = sighting.DateSpotted.CompareTo(DateTime.Now);

            if (currentDate >= 0)
            {
                return BadRequest(new { message = "Cannot enter future date" });
            }
            try
            {
                birdSightingDao.editSighting(sighting, sighting.NoteId);
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
