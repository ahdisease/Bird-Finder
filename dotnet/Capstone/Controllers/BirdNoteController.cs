using Capstone.DAO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Capstone.Models;
using Capstone.Exceptions;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class BirdNoteController : ControllerBase
    {
        private readonly IBirdNoteDao birdSightingDao;
        public BirdNoteController(IBirdNoteDao birdSightingDao)
        {
            this.birdSightingDao = birdSightingDao;

        }

        [HttpGet("/bird/{birdId}/notes")]
        public IActionResult getNotes(int birdId)
        {
            const string errorMessage = "No sightings.";

            IActionResult result;
            try
            {
                List<BirdNote> sightingList = birdSightingDao.getSightings(birdId);

                if (sightingList != null)
                {
                    result = Ok(sightingList);
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

        [HttpGet("/note")]
        public IActionResult getNote(int id)
        {
            const string errorMessage = "No sightings.";

            IActionResult result;
            try
            {
                BirdNote sighting = birdSightingDao.getBirdSighting(id);

                if (sighting != null)
                {
                    result = Ok(sighting);
                }
                else
                {
                    result= NotFound(new { message = errorMessage });
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

        [HttpPost("/newNote")]
        public IActionResult addNote([FromBody] BirdNote newBirdSighting)
        {
            const string errorMessage = "An error occurred and a bird was not created.";

            IActionResult result;

            int currentDate = newBirdSighting.DateSpotted.CompareTo(DateTime.Now);

            if (currentDate >= 0)
            {
                return BadRequest(new { message = "Cannot enter future date" });
            }

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
