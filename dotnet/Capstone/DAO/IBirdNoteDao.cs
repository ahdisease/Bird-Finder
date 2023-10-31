using Capstone.Models;
using System;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface IBirdNoteDao
    {
        List<BirdNote> getSightings(int birdId);
        void deleteSighting(int id, string username);
        void editSighting(BirdNote sighting, int id, string username);
        BirdNote addSighting(BirdNote birdSighting, int birdId);
        BirdNote getBirdSighting(int id);
    }
}
