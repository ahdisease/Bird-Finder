﻿using Capstone.Exceptions;
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
       

        public BirdSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
            
        }


        public Bird createBird(Bird bird)
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

                    cmd.Parameters.AddWithValue("@listId", bird.listId);
                    cmd.Parameters.AddWithValue("@name", bird.name);
                    cmd.Parameters.AddWithValue("@picture", bird.imgUrl);
                    cmd.Parameters.AddWithValue("@zip_code", bird.zipCode);

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
            string sql = "UPDATE bird SET list_id = @list_id, name = @name, picture = @picture, zip_code = @zip_code WHERE id = @id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@listId", bird.listId);
                    cmd.Parameters.AddWithValue("@name", bird.name);
                    cmd.Parameters.AddWithValue("@picture", bird.imgUrl);
                    cmd.Parameters.AddWithValue("@zip_code", bird.zipCode);

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
            bird.id = Convert.ToInt32(reader["id"]); 
            bird.listId = Convert.ToInt32(reader["list_id"]);
            bird.name = Convert.ToString(reader["name"]);
            bird.imgUrl = Convert.ToString(reader["picture"]);
            bird.zipCode = Convert.ToString(reader["zip_code"]);

            return bird;
        }
    }
}
