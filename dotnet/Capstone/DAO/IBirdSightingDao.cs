using Capstone.Models;
using System;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface IBirdSightingDao
    {
        List<BirdSighting> getSightings(int birdId);
        void deleteSighting(int  id);
        void editSighting(BirdSighting sighting, int id);
        BirdSighting addSighting(BirdSighting birdSighting, int userId, int birdId, DateTime dateSighted);
    }
}
