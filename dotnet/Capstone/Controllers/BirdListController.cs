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
    public class BirdListController: ControllerBase
    {
        private readonly IBirdListDao birdListDao;

        public BirdListController(IBirdListDao birdListDao)
        {
            this.birdListDao = birdListDao;
        }



        [HttpGet("/lists/{listId}")]
        public IActionResult getList(int listId)
        {
            const string errorMessage = "No bird matches this id.";
            IActionResult result;

            try
            {
                BirdList birdList = birdListDao.getList(listId);

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
                result = NotFound(new { message = e.Message });
            }


            return result;

        }


        [HttpGet("/lists/")]
        public List<BirdList> getAllLists()
        {
            IIdentity user = User.Identity;

            List<BirdList> birdList = birdListDao.getAllLists(user.Name);

            if (birdList == null)
            {
                Console.WriteLine("No lists");
            }

            return birdList;
        }


        [HttpPut("/editList")]
        public IActionResult editList(BirdList list)
        {
            const string errorMessage = "An error occurred and list could not be modified.";
            
            IActionResult result;

            try
            {
                IIdentity user = User.Identity;
                birdListDao.editList(list, list.ListId, user.Name);
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


        [HttpDelete("/deleteList/{listId}")]
        public IActionResult deleteList(int listId)
        {
            const string errorMessage = "An error occurred and list could not be deleted.";

            IActionResult result;

            try
            {
                IIdentity user = User.Identity;
                birdListDao.deleteList(listId, user.Name); 

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
        public IActionResult createList([FromBody] BirdList newList)
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
