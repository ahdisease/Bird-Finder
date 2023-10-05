using Capstone.Exceptions;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Policy;
using System.Xml.Linq;

namespace Capstone.DAO
{
    public class BirdSqlDao : BirdDao
    {
        private readonly string connectionString;
       

        public BirdSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
            
        }


        public Bird createBird(Bird bird, string name, string description, string picture)
        {
            Bird newBird = null;

            String sql = "INSERT INTO bird(name, description, picture) " +
                "VALUES(@name, @description, @picture)";

            int newBirdId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@picture", picture);

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
            string sql = "UPDATE bird SET name = @name, description = @description, picture = @picture WHERE id = @id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", bird.name);
                    cmd.Parameters.AddWithValue("@description", bird.description);
                    cmd.Parameters.AddWithValue("@picture", bird.picture);

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
            string sql = "SELECT id, name, description, picture FROM bird WHERE id = @id ";

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

        public Bird getBirdByZip(string zipcode)
        {
            throw new System.NotImplementedException();
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

        public List<Bird> listAllBirds()
        {
            List<Bird> birdList = new List<Bird>();

            string sql = "SELECT id, name, description, picture FROM bird";

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
            bird.id = Convert.ToInt32(reader["id"]);
            bird.name = Convert.ToString(reader["name"]);
            bird.description = Convert.ToString(reader["description"]);
            bird.picture = Convert.ToString(reader["picture"]);
         
            return bird;
        }
    }
}
