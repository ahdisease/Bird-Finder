using Capstone.Exceptions;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Xml.Linq;

namespace Capstone.DAO
{
    public class BirdSqlDao : BirdDao
    {
        private readonly string connectionString;
        private readonly IBirdListDao birdListDao;
       

        public BirdSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
            this.birdListDao = new BirdListSqlDao(connectionString);
            
        }

        public List<Bird> getBirdsInList(int listId)
        {
            List<Bird> birdList = new List<Bird>();

            string sql = "SELECT id, list_id, name, picture, zip_code FROM bird WHERE list_id = @list_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@list_id", listId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        Bird bird = MapRowToBird(reader);
                        birdList.Add(bird);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return birdList;
        }

        public Bird createBird(Bird bird, int listId)
        {
            Bird newBird = null;

            String sql = "INSERT INTO bird(list_id, name, picture, zip_code) " +
                "VALUES(@list_id, @name, @picture, @zip_code)";

            int newBirdId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@list_id", listId);
                    cmd.Parameters.AddWithValue("@name", bird.BirdName);
                    cmd.Parameters.AddWithValue("@picture", bird.ImgUrl);
                    cmd.Parameters.AddWithValue("@zip_code", bird.ZipCode);

                    newBirdId = Convert.ToInt32(cmd.ExecuteScalar());

                }
                newBird = getBird(newBirdId);

            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return newBird;
        }

        public void deleteBird(int id)
        {
            string sql = "DELETE FROM bird WHERE id = @id";

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
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
        }


        public void editBird(Bird bird, int id)
        {
            string sql = "UPDATE bird SET name = @name, picture = @picture, zip_code = @zip_code WHERE id = @id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    //cmd.Parameters.AddWithValue("@listId", bird.listId);
                    cmd.Parameters.AddWithValue("@name", bird.BirdName);
                    cmd.Parameters.AddWithValue("@picture", bird.ImgUrl);
                    cmd.Parameters.AddWithValue("@zip_code", bird.ZipCode);

                    SqlDataReader reader = cmd.ExecuteReader();
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

        }

        public Bird getBird(int id)
        {
            Bird bird = null;
            string sql = "SELECT id, list_id, name, picture, zip_code FROM bird WHERE id = @id ";

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
                        bird = MapRowToBird(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return bird;
        }

        public List<Bird> getBirdByZip(string zipCode)
        {
            List<Bird> birdList = new List<Bird>();

            string sql = "SELECT id, list_id, name, picture, zip_code FROM bird WHERE zip_code = @zip_code";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@zip_code", zipCode);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Bird bird = MapRowToBird(reader);
                        birdList.Add(bird);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return birdList;
        }



        public Bird getRandomBird()
        {
            Bird randomBird = null;

            string sql = "SELECT TOP 1 * FROM bird ORDER BY NEWID()";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        randomBird = MapRowToBird(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return randomBird;
        }

        public List<Bird> getBirds()
        {
            List<Bird> birdList = new List<Bird>();

            string sql = "SELECT id, list_id, name, picture, zip_code FROM bird";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Bird bird = MapRowToBird(reader);
                        birdList.Add(bird);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return birdList;
        }

        private Bird MapRowToBird(SqlDataReader reader)
        {
            Bird bird = new Bird();
            bird.BirdId = Convert.ToInt32(reader["id"]); 
            bird.ListId = Convert.ToInt32(reader["list_id"]);
            bird.BirdName = Convert.ToString(reader["name"]);
            bird.ImgUrl = Convert.ToString(reader["picture"]);
            bird.ZipCode = Convert.ToString(reader["zip_code"]);

            return bird;
        }
    }
}
