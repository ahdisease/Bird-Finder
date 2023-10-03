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
        void createBird(Bird bird);
        Bird getBirdByZip(string zipcode);
        Bird getRandomBird();
        
    }
}
