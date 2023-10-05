﻿using Capstone.DAO;
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
            return Ok(new { message = "Welcome to Birds Finder", description = "This is a place to document your bird sightings." });
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

        [HttpGet("/birds/{id}")]
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

        [HttpGet("/birds/random")]
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

        [HttpPost("/birds")]
        public void createBird(Bird newBird)
        {
            //const string ErrorMessage = "An error occurred and user was not created.";
            try
            {
                Bird bird = birdDao.createBird(newBird, newBird.name, newBird.description, newBird.picture);
                if (bird == null)
                {
                    Console.WriteLine(StatusCode(404));
                }
            }catch (DaoException )
            {
                Console.WriteLine(StatusCode(500));
            }

        }

        [HttpPut("/birds/{id}")]
        public void editBird(Bird updatedBird, int id)
        {
            birdDao.editBird(updatedBird, id);

        }

    }
}