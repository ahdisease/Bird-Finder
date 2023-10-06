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
        public void addSighting( BirdSighting newBirdSighting)
        {

            IIdentity user = User.Identity;
            
            try
            {

                BirdSighting birdSighting = birdSightingDao.addSighting(newBirdSighting, user.Name);
                
                if (birdSighting == null)
                {
                    Console.WriteLine(StatusCode(404));
                }
            }
            catch (DaoException)
            {
                Console.WriteLine(StatusCode(500));
            }

        }






    }
}
