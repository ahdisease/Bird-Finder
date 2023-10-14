using Capstone.DAO;
using Capstone.Exceptions;
using Capstone.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BirdListController: ControllerBase
    {
        private readonly IBirdListDao birdListDao;

        public BirdListController(IBirdListDao birdListDao)
        {
            this.birdListDao = birdListDao;
        }



        [HttpGet("/lists/{id}")]
        public BirdList getList(int id)
        {
            BirdList birdList = birdListDao.getList(id);

            if (birdList == null)
            {
                Console.WriteLine("No bird matches this id");
            }

            return birdList;

        }


        [HttpGet("/lists")]
        public List<BirdList> getAllLists()
        {
                
            List<BirdList> birdList = birdListDao.getAllLists();

            if (birdList == null)
            {
                Console.WriteLine("No lists");
            }

            return birdList;
        }


        [HttpPut("/editList/{id}")]
        public IActionResult editList(BirdList list, int id)
        {
            const string errorMessage = "An error occurred and list could not be modified.";
            
            IActionResult result;

            try
            {
                birdListDao.editList(list, id);
                result = Ok();
            }
            catch(ArgumentException e)
            {
                result = BadRequest(new {message = e.Message});
            }
            catch
            {
                result = BadRequest(new { message = errorMessage });
            }
            return result;

        }


        [HttpDelete("/deleteList/{id}")]
        public IActionResult deleteList(int id)
        {
            const string errorMessage = "An error occurred and list could not be deleted.";

            IActionResult result;

            try
            {
                birdListDao.deleteList(id);
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
                result = BadRequest(new {message = errorMessage});
            }

            return result;
        }


        [HttpPost("/createList")]
        public IActionResult createList([FromBody] BirdList newList, string username)
        {
            IIdentity user = User.Identity;

            const string errorMessage = "An error occurred and a bird was not created.";

            IActionResult result;

            try
            {
                BirdList birdList = birdListDao.createList(newList, user.Name);
                result = Created("", "");
            }
            catch(ArgumentException e)
            {
                result = BadRequest(new { message = e.Message });
            }
            catch
            {
                result = BadRequest(new {message = errorMessage});  
            }

            return result;
        }





    }


}
