using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface IBirdDao
    {
        List<Bird> getBirdsInList(int listId);
        Bird getBird(int id);
        void deleteBird(int id);
        void editBird(Bird bird, int id);
        Bird createBird(Bird bird, int listId);
        List<Bird> getBirdByZip(string zipCode);
        Bird getRandomBird();
        List<Bird> getBirds();

    }
}
