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
            return Ok(new { message = "Welcome to Birds!", description = "This is an open endpoint. To reach any other endpoint in this API, you must be authenticated." });
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
    }
}
