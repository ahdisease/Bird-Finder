﻿using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface IBirdListDao
    {
        BirdList getList(int id);
        List<BirdList> getAllLists(string username);
        BirdList createList(BirdList list, string username);
        void editList(BirdList list, int id, string username);
        void deleteList(int listId, string username);
        int getListIdByListName(string name);


    }
}
