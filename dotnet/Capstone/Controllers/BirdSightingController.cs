using Capstone.DAO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Capstone.Models;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BirdSightingController : Controller
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






    }
}
