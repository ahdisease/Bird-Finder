using Capstone.Exceptions;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public class BirdSqlDao : BirdDao
    {
        private readonly string connectionString;
        public BirdSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public void createBird(Bird bird)
        {
            throw new System.NotImplementedException();
        }

        public void deleteBird(int id)
        {
            throw new System.NotImplementedException();
        }

        public void editBird(Bird bird)
        {
            throw new System.NotImplementedException();
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
