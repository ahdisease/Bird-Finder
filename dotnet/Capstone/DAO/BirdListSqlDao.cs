using Capstone.Exceptions;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public class BirdListSqlDao : IBirdListDao
    {
        private readonly string connectionString;
        private readonly IUserDao userDao;


        public BirdListSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
            this.userDao = new UserSqlDao(connectionString);

        }

        public BirdList createList(BirdList list, string username)
        {
            BirdList newBirdList = null;

            string sql = "INSERT INTO list(user_id, name) VALUES((SELECT user_id FROM users WHERE username = @username), @name )";

            int newListId = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@name", list.name);

                    newListId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                newBirdList = getList(newListId);
            }
            catch(SqlException ex)
            {
                throw new DaoException("SQL exception error", ex);
            }
            return newBirdList;
        }

        public void deleteList(int id)
        {
            string sql = "DELETE FROM list WHERE id = @id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                }
            }
            catch(SqlException ex)
            {
                throw new DaoException("SQL exception occured", ex);
            }
        }

        public void editList(BirdList list, int id)
        {
            string sql = "UPDATE list SET name = @name WHERE id = @id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("name", list.name);

                    SqlDataReader reader = cmd.ExecuteReader();

                }
            }
            catch(SqlException ex)
            {
                throw new DaoException("SQL exception occured", ex);
            }


        }

        public List<BirdList> getAllLists()
        {
            List<BirdList> birdLists = new List<BirdList>();
            string sql = "SELECT id, user_id, name FROM list";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        BirdList birdList = MapRowToBirdList(reader);
                        birdLists.Add(birdList);
                    }

                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return birdLists;
        }

        public BirdList getList(int id)
        {
            BirdList birdList = null;

            string sql = "SELECT id, user_id, name FROM list WHERE id = @id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        birdList = MapRowToBirdList(reader);
                    }

                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }


            return birdList;
        }

        public int getListIdByListName(string name)
        {
            int listId = 0;

            string sql = "SELECT id FROM list WHERE name = @name";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        listId = Convert.ToInt32(reader["id"]);
                    }

                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }


            return listId;
        }

        private BirdList MapRowToBirdList(SqlDataReader reader)
        {
            BirdList birdList = new BirdList();

            birdList.id = Convert.ToInt32(reader["id"]);
            birdList.userId = Convert.ToInt32(reader["user_id"]);
            birdList.name = Convert.ToString(reader["name"]);
            
            return birdList;
        }

    }
}
