using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface BirdDao
    {
        List<Bird> listAllBirds();
        Bird getBird(int id);
        void deleteBird(int id);
        void editBird(Bird bird);
        Bird createBird(Bird bird, string name, string description, string picture);
        Bird getBirdByZip(string zipcode);
        Bird getRandomBird();
        
    }
}
