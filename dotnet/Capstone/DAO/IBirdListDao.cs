using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface IBirdListDao
    {
        BirdList getList(int id);
        List<BirdList> getAllLists();
        BirdList createList(BirdList list, string username);
        void editList(BirdList list, int id);
        void deleteList(int id);
        int getListIdByListName(string name);


    }
}
